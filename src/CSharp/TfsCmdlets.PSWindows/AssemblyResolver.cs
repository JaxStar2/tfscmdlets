using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TfsCmdlets
{
    public static class AssemblyResolver
    {
        private static readonly Dictionary<string, Assembly> RedirectedAssemblies = new Dictionary<string, Assembly>();

        public static void Register()
        {
            try
            {
                var binPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var doc = LoadAppConfigData(binPath);

                foreach (XmlElement e in doc.GetElementsByTagName("assemblyIdentity"))
                {
                    var name = e.GetAttribute("name");
                    RedirectedAssemblies[name] =
                        Assembly.LoadFrom(Path.Combine(binPath, $"{name}.dll"));
                }

                AppDomain.CurrentDomain.AssemblyResolve += (sender, e) =>
                {
                    Debug.WriteLine("Assembly: (e.Name)");
                    var assemblyName = (e.Name.Split(','))[0];

                    if (!RedirectedAssemblies.ContainsKey(assemblyName)) return null;

                    Debug.WriteLine("Assembly: Redirected!");
                    return RedirectedAssemblies[assemblyName];
                };

            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        private static XmlDocument LoadAppConfigData(string binPath)
        {
            var doc = new XmlDocument();
            doc.Load(Path.Combine(binPath, "TfsCmdlets.PSWindows.dll.config"));

            var nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("asm", "urn:schemas-microsoft-com:asm.v1");

            return doc;
        }
    }
}
