using dnlib.DotNet;
using System;

namespace TheKraken_Obfuscator
{
	public class JunkClasses
	{
		private static Random RDG = new Random();

		public static void JunkClassesAdder(ModuleDefMD moduleDef)
		{
			for (int i = 0; i < RDG.Next(30, 50); i++)
			{
				TypeDefUser newtype = new TypeDefUser(Program.RandomString(20));
				moduleDef.Types.Add(newtype);
			}
		}
	}

}
