using System;
using System.Security;
using MoonSharp.Interpreter;
using UnityEngine;
using Utilities;

namespace Mods
{

	public partial class Mod // Make ModLogicScript access private members in Mod (friend)
	{
		
		public class ModLogicScript : ModScript
		{
			private Table _modTable; // Mod Namespace

			public ModLogicScript(Mod mod)
				: base(mod, CoreModules.Preset_SoftSandbox)
			{
				SetupGlobals();
			}
		
			// Setup global environment
			private void SetupGlobals()
			{
				// Mod Namespace
				_modTable = new Table(this);
				Globals["ModName"] = _mod.ModName;
				Globals["ModTable"] = _modTable;

				var toc = _mod.TOCScript.Toc;
				Globals["GetTocInfo"] = (Producer<string, string>)(key =>
				{
					var field = toc.GetType().GetField(key);
					
					if (field != null && field.FieldType == typeof(string))
						return (string) field.GetValue(toc);

					return default;
				});
				
				Globals["GetGame"] = (Supplier<TurnBasedSystem>)(() => ModLinker.CurrentGame);
			}
		
		}
		
	}
	
}
