using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using OpCodes = dnlib.DotNet.Emit.OpCodes;

namespace TheKraken_Obfuscator
{
    public class StringEncryptor
    {
        private static MethodDefUser methodDefUser;
        public static string Key = Program.RandomString(35);

        public static void StartEncrypt(ModuleDefMD moduleDef)
        {
            Console.WriteLine(Key);
            DecryptClass(moduleDef);
            EncryptStrings(moduleDef);
        }

        public static void EncryptStrings(ModuleDefMD moduleDef)
        {
            foreach (var md in moduleDef.GetTypes())
            {
                foreach (var method in md.Methods)
                {
                    if (method.Body == null) continue;
                    for (int i = 0; i < method.Body.Instructions.Count(); i++)
                    {
                        if (method.Body.Instructions[i].OpCode == OpCodes.Ldstr)
                        {
                            string oldString = method.Body.Instructions[i].Operand.ToString();
                            string newString = EncryptString(oldString);
                            method.Body.Instructions[i].Operand = newString;
                            method.Body.Instructions.Insert(i + 1, new Instruction(OpCodes.Call, methodDefUser));
                            i += 1;
                        }
                    }
                }
            }
        }
        public static void DecryptClass(ModuleDefMD moduleDef)
        {
            methodDefUser = new MethodDefUser("StringDecryptor", MethodSig.CreateStatic(moduleDef.CorLibTypes.String, moduleDef.CorLibTypes.String), MethodImplAttributes.IL | MethodImplAttributes.Managed, MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.HideBySig | MethodAttributes.ReuseSlot)
            {
                Body = new CilBody()
            };
            moduleDef.GlobalType.Methods.Add(methodDefUser);

            methodDefUser.Body.Instructions.Add(OpCodes.Nop.ToInstruction());
            methodDefUser.Body.Instructions.Add(OpCodes.Call.ToInstruction(moduleDef.Import(typeof(System.Text.Encoding).GetMethod("get_UTF8", new Type[] { }))));
            methodDefUser.Body.Instructions.Add(OpCodes.Ldarg_0.ToInstruction());
            methodDefUser.Body.Instructions.Add(OpCodes.Call.ToInstruction(moduleDef.Import(typeof(System.Convert).GetMethod("FromBase64String", new Type[] { typeof(string) }))));
            methodDefUser.Body.Instructions.Add(OpCodes.Callvirt.ToInstruction(moduleDef.Import(typeof(System.Text.Encoding).GetMethod("GetString", new Type[] { typeof(byte[]) }))));
            methodDefUser.Body.Instructions.Add(OpCodes.Ret.ToInstruction());
        }
        public static string EncryptString(string String)
        {


            var string2 = Encoding.UTF8.GetBytes(String);
            var string3 = Convert.ToBase64String(string2);
            return string3;
        }
    }
}