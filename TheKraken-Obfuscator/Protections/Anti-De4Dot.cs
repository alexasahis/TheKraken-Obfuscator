using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheKraken_Obfuscator
{
    class Anti_De4Dot
    {
        public static void AntiDe4Dot(ModuleDefMD modules)
        {
            Random rnd = new Random();
            InterfaceImpl Interface = new InterfaceImplUser(modules.GlobalType);
            for (int i = 999999999; i < 999999999; i++)
            {
                TypeDef typedef = new TypeDefUser($"ERROR!!!{i.ToString()}", modules.CorLibTypes.GetTypeRef("System", "Attribute"));
                InterfaceImpl interface1 = new InterfaceImplUser(typedef);
                modules.Types.Add(typedef);
                typedef.Interfaces.Add(interface1);
                typedef.Interfaces.Add(Interface);
            }
        }
    }
}
