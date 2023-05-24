using System;
using System.Security;
using MoonSharp.Interpreter;
using UnityEngine;

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
				
				// Game Namespace TODO
				// _FullScript.Globals["GetCurrentRound"] = (Func<double>);
			}
		
		}
		
	}
	
}
