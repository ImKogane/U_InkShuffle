using JetBrains.Annotations;
using MoonSharp.Interpreter;
using UnityEngine;

namespace Mods
{
	public partial class Mod
	{
		public string ModName { get; } // ID
		
		public ModTOCScript TOCScript { get; private set; } // ONLY ModName.lua script
		public ModLogicScript LogicScript { get; private set; } // EVERY script the mod wanted us to load (except the TOC)
		
		public bool IsDiscovered => TOCScript != null;
		public bool IsEnabled => IsDiscovered && LogicScript != null;

		public Mod(string modName)
		{
			ModName = modName; // Auto-property can be made get-only
			
			TOCScript = new ModTOCScript(this);

			if (!TOCScript.TryDoFile(modName)) // Read TOC file
				TOCScript = null;
		}

		/// <summary>
		/// Enable/Disable the mod.<br/><br/>
		/// NOTE #1: Enabling a disabled mod will reload the Logic files from the disk!<br/><br/>
		/// NOTE #2: The TOC file and its infos will be untouched.
		/// (to also reload TOC files, see ModsManager.DiscoverMods)
		/// </summary>
		public void SetModEnabled(bool enabled)
		{
			if (!IsDiscovered)
				return;
			
			if (enabled == IsEnabled)
				return;
			
			if (enabled && !IsEnabled)
			{
				Debug.Log("------- Enable: " + ModName + " -------");
				
				LogicScript = new ModLogicScript(this);

				// load all mod files
				foreach (var fileName in TOCScript.Toc.filesToLoad)
				{
					if (!LogicScript.TryDoFile(fileName)) // Read Logic Files
					{
						LogicScript = null;
						break;
					}
				}

				TryCall("OnModEnabled");
			}
			else if (!enabled && IsEnabled)
			{
				Debug.Log("------- Disable: " + ModName + " -------");
				
				TryCall("OnModDisabled");
				LogicScript = null;
			}
		}
		
		public void ReloadModLogic()
		{
			SetModEnabled(false);
			SetModEnabled(true);
		}
		
		public override string ToString()
		{
			return "[ModName: " + ModName + ", Enabled: \"" + IsEnabled + "\", ModInfo: " + TOCScript.Toc.ToString() + "]";
		}
		
		/// <summary>
		/// Try to call a lua global function in the mod.
		/// </summary>
		/// <param name="function">The Lua global function name.</param>
		/// <param name="args">Additional arguments to send to the Lua function.</param>
		/// <returns>
		/// <b>True</b> if the call succeeded.<br/>
		/// <b>False</b> if the function doesn't exist / an error was thrown.
		/// </returns>
		public bool TryCall(string function, params object[] args)
		{
			return LogicScript?.TryLuaRunner(() => LogicScript.Call(LogicScript.Globals[function], args)) ?? false;
		}
		
		/// <summary>
		/// Try to call a lua global function in the mod, <b>with the return value</b>.
		/// </summary>
		/// <param name="function">The Lua global function name.</param>
		/// <param name="args">Additional arguments to send to the Lua function.</param>
		/// <returns>
		/// <b>DynValue</b> the return value of the lua function. Can be Nil.<br/>
		/// <b>null</b> if the function doesn't exist / an error was thrown.
		/// </returns>
		[CanBeNull]
		public DynValue TryCallR(string function, params object[] args)
		{
			return LogicScript?.TryLuaSupplier(() => LogicScript.Call(LogicScript.Globals[function], args));
		}
		
		/// <summary>
		/// Try to get a lua global variable in the mod.
		/// </summary>
		/// <param name="variable">The Lua global variable name.</param>
		/// <returns>
		/// <b>DynValue</b> the value of the lua variable. Can be Nil.<br/>
		/// <b>null</b> if an error was thrown.
		/// </returns>
		[CanBeNull]
		public DynValue TryGet(string variable)
		{
			return LogicScript?.TryLuaSupplier(() => LogicScript.Globals.Get(variable));
		}
	}
}
