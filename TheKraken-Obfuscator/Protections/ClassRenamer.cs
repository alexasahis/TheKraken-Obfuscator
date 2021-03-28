using dnlib.DotNet;

namespace TheKraken_Obfuscator
{
    public class ClassRenamer
    {
        public static void _ClassRenamer(ModuleDefMD moduleDef)
        {
            foreach (var types in moduleDef.Types)
            {
                types.Name = Program.RandomString(types.Name.Length + 200);
            }
        }
    }
}
