using System;
using MoonSharp.Interpreter;
using UnityEngine;

namespace Mods
{

	public partial class Mod // Make ModLogicScript access private members in Mod (friend)
	{
		
		public class ModLogicScript : Script
		{
			private Mod m_Mod;

			public ModLogicScript(Mod Mod)
				: base(CoreModules.Preset_SoftSandbox)
			{
				m_Mod = Mod;
				SetupGlobals();
			}
		
			// Setup global environment
			private void SetupGlobals()
			{
				// Mod Namespace
				m_Mod.ModTable = new Table(this);
				Globals["ModName"] = m_Mod.ModName;
				Globals["ModTable"] = m_Mod.ModTable;
				
				// Game Namespace TODO
				// _FullScript.Globals["GetCurrentRound"] = (Func<double>);
			}
		
			public void OnEnable()
			{
				try
				{
					Call(Globals["OnModEnabled"]);
				}
				catch (Exception e)
				{
					// ignored
				}
			}
			
			public void OnDisable()
			{
				try
				{
					Call(Globals["OnModDisabled"]);
				}
				catch (Exception e)
				{
					// ignored
				}
			}
		
		}
		
	}
	
}
