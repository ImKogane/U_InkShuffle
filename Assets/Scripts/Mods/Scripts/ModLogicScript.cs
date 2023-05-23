﻿using System;
using MoonSharp.Interpreter;
using UnityEngine;

namespace Mods
{

	public partial class Mod // Make ModLogicScript access private members in Mod (friend)
	{
		
		public class ModLogicScript : Script
		{
			private Mod _mod;
			private Table _modTable; // Mod Namespace

			public ModLogicScript(Mod mod)
				: base(CoreModules.Preset_SoftSandbox)
			{
				_mod = mod;
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
