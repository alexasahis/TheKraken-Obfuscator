using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace TheKraken_Obfuscator
{
    class Program
    {
        //-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
        //This Software Is Completely Free To Use For Any Reason. Please Note Though This Obfuscation Is Relatively
        //Weak And Is More For Teaching And Demonstration Purposes. For Example Strings Are Only Converted To Base64
        //And Can Easily Be Decompiled.
        //We Recommened Buying EtherProtector From EtherCloud For Good Cheap Obfuscation. Or Even Buy Other Stuff
        //That Has No Decompiler. I Will Make Kraken More Stronger If The Project Gets Some Attention to Give Me
        //A Reason To Update.
        //Check Out My Website: https://0x421.ga/ For Updates.
        //-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
        private static Random random = new Random(); //Part Of Random String Generation Thingy Just Ignore This :/

        static void Main(string[] args)
        {
            Console.WriteLine("                     --TheKraken Obfuscator--");
            Console.WriteLine();
            Console.WriteLine("                 -=-Created by 0x421 x EtherCloud-=-");
            Console.WriteLine("             Goto https://ethercloud.tk For EtherProtector!");
            string text = args[0]; //Allows You To Drag And Drop Your Executable To Make Obfuscation Easier For You.
            string path2 = text.Replace("\"", ""); //Removes "" From Argument. Make Sure Your Executable Does Not Have A Space In It.
            byte[] filebytes = File.ReadAllBytes(path2);
            AssemblyDef assembly = AssemblyDef.Load(filebytes);
            Console.WriteLine("Obfuscation Started!");
            var modules = ModuleDefMD.Load(filebytes);

            Anti_De4Dot.AntiDe4Dot(modules);
            JunkClasses.JunkClassesAdder(modules);
            ClassRenamer._ClassRenamer(modules);
            StringEncryptor.StartEncrypt(modules);
            WaterMark(modules);
            modules.Write($"{assembly.Name}-protected.exe");
            Console.WriteLine("Finished Obfuscation!");
            Thread.Sleep(5000);
        }

        public static String RandomString(int length) //Generates Random Strings.
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789诶比西迪伊艾弗吉艾尺艾杰开艾勒艾马艾娜哦屁吉吾艾儿艾";
            return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static void WaterMark(ModuleDef moduleDef)
        {
            MethodDefUser methodDefUser = new MethodDefUser("TheKraken", MethodSig.CreateStatic(moduleDef.CorLibTypes.String, moduleDef.CorLibTypes.String), MethodImplAttributes.IL | MethodImplAttributes.Managed, MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.HideBySig | MethodAttributes.ReuseSlot)
            {
                Body = new CilBody()
            };
            moduleDef.GlobalType.Methods.Add(methodDefUser);

            methodDefUser.Body.Instructions.Add(Instruction.Create(OpCodes.Ldstr, "TheKraken (Free GitHub Edition. https://github.com/0x421/TheKraken-Obfuscator) Created By 0x421. (Discord: 0x421#8108). Buy EtherProtector For Actual Good Obfuscation At https://ethercloud.tk"));
            methodDefUser.Body.Instructions.Add(new Instruction(OpCodes.Ret));
        }
    }
}
