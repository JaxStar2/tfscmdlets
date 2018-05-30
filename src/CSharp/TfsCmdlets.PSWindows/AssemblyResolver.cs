using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Web.Hosting;
using System.Xml;

namespace TfsCmdlets
{
    public static class AssemblyResolver
    {
        private static readonly Dictionary<string, Assembly> RedirectedAssemblies = new Dictionary<string, Assembly>();
        private static readonly TraceSource TraceSource = new TraceSource("TfsCmdlets Assembly Resolver", SourceLevels.All);

        private static bool _isRegistered;

        public static void Register()
        {
            if (_isRegistered) return;

            try
            {
                var binPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                if (binPath == null)
                {
                    LogCritical("Assembly.GetExecutingAssembly().Location cannot be null. Disabling Assembly Resolver");
                    return;
                }

                var doc = LoadAppConfigData(binPath);

                foreach (XmlElement e in doc.GetElementsByTagName("assemblyIdentity"))
                {
                    var name = e.GetAttribute("name");

                    try
                    {
                        LogInfo($"Loading assembly [{name}] from '{binPath}\\{name}.dll'");
                        RedirectedAssemblies[name] = Assembly.LoadFrom(Path.Combine(binPath, $"{name}.dll"));
                    }
                    catch (Exception ex)
                    {
                        LogWarn($"Error loading assembly {name}: {ex.Message}; skipping");
                    }
                }

                AppDomain.CurrentDomain.AssemblyResolve += (sender, e) =>
                {
                    try
                    {
                        LogInfo($"Request for unresolved assembly {e.Name}");

                        var assemblyName = (e.Name.Split(','))[0];

                        if (!RedirectedAssemblies.ContainsKey(assemblyName))
                        {
                            LogWarn($"Unknown assembly {e.Name}; skipping");
                            return null;
                        }

                        var redirectedAssembly = RedirectedAssemblies[assemblyName];

                        LogInfo($"[{assemblyName}] resolved as '{redirectedAssembly.FullName}'");

                        return redirectedAssembly;

                    }
                    catch (Exception ex)
                    {
                        LogError($"Error during AssemblyResolve event call: {ex.ToString()}; ignoring request");
                        return null;
                    }
                };

                LogInfo("Finished registering Assembly Resolver. Listening for AssemblyResolve event calls...");

                _isRegistered = true;
            }
            catch (Exception ex)
            {
                LogError($"Error setting up Assembly Resolver: {ex.ToString()}");
            }
        }

        private static void LogCritical(string message, int id = -3)
        {
            TraceSource.TraceEvent(TraceEventType.Critical, id, message);
        }

        private static void LogError(string message, int id = -2)
        {
            TraceSource.TraceEvent(TraceEventType.Error, id, message);
        }

        private static void LogWarn(string message, int id = -1)
        {
            TraceSource.TraceEvent(TraceEventType.Warning, id, message);
        }

        private static void LogInfo(string message, int id = 0)
        {
            TraceSource.TraceEvent(TraceEventType.Information, id, message);
        }

        private static XmlDocument LoadAppConfigData(string binPath)
        {
            var doc = new XmlDocument();
            var configFile = Path.Combine(binPath, "TfsCmdlets.PSWindows.dll.config");

            LogInfo($"Loading assembly redirection information from file {configFile}");
            doc.Load(configFile);

            var nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("asm", "urn:schemas-microsoft-com:asm.v1");

            return doc;
        }
    }
}