using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TfsCmdlets.CorrectnessTests
{ 
    [TestClass]
    public class CmdletTests
    {
        [TestMethod]
        public void ValueReturningCmdletsHaveOutputTypeAttribute()
        {
            var cmdletTypes = GetCmdlets().Where(t => t.Name.IsMatch(VALUR_RETURNING_VERBS));

            foreach (var t in cmdletTypes)
            {
                Console.WriteLine(t.FullName);

                var attr = t.GetCustomAttribute<OutputTypeAttribute>(); 
                Assert.IsNotNull(attr, $"{t.FullName} cmdlet is missing the [OutputType] attribute");
            }
        }

        [TestMethod]
        public void AllCmdletsHaveCmdletAttribute()
        {
            var cmdletTypes = GetCmdlets();

            foreach (var t in cmdletTypes)
            {
                Console.WriteLine(t.FullName);

                var attr = t.GetCustomAttribute<CmdletAttribute>();
                Assert.IsNotNull(attr, $"{t.FullName} cmdlet is missing the [Cmdlet] attribute");
            }
        }

        [TestMethod]
        public void PassthruCmdletsHavePassthruArgument()
        {
            var cmdletTypes = GetCmdlets().Where(t => t.Name.IsMatch(PASSTHRU_VERBS));

            foreach (var t in cmdletTypes)
            {
                Console.WriteLine(t.FullName);

                Assert.IsTrue(t.HasArgument("Passthru"), $"{t.FullName} cmdlet is missing the Passthru argument");
            }
        }

        [TestMethod]
        public void StateChangingCmdletsHaveConfirmImpact()
        {
            var cmdletTypes = GetCmdlets().Where(t => t.Name.IsMatch(STATE_CHANGING_VERBS));

            foreach (var t in cmdletTypes)
            {
                Console.WriteLine(t.FullName);

                var attr = t.GetCustomAttribute<CmdletAttribute>();
                Assert.IsTrue(((int)attr.ConfirmImpact) >= ((int)ConfirmImpact.Medium), $"{t.FullName} cmdlet must have ConfirmImpact of at least Medium");
            }
        }

        [TestMethod]
        public void DestructiveCmdletsHaveConfirmImpact()
        {
            var cmdletTypes = GetCmdlets().Where(t => t.Name.IsMatch(DESTRUCTIVE_VERBS));

            foreach (var t in cmdletTypes)
            {
                var attr = t.GetCustomAttribute<CmdletAttribute>();
                Assert.IsTrue(((int)attr.ConfirmImpact) >= ((int)ConfirmImpact.High), $"{t.FullName} cmdlet must have ConfirmImpact of at least High");
            }
        }


        private static Assembly Assembly { get; } = typeof(ServiceLocator).Assembly;

        private static IEnumerable<Type> GetCmdlets()
        {
            return Assembly.GetExportedTypes().Where(t => t.IsSubclassOf(typeof(Cmdlet)) && !t.IsAbstract);
        }

        private const string DESTRUCTIVE_VERBS = "Dismount|Remove|Stop";
        private const string STATE_CHANGING_VERBS = "Import|Mount|Move|New|Rename|Set|Start";
        private const string PASSTHRU_VERBS = "^Connect|Copy|^Move|New|Rename";
        private const string VALUR_RETURNING_VERBS = "Get|" + PASSTHRU_VERBS;

    }
}
