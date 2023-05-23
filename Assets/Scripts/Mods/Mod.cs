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

			if (!ModsManager.Instance.TryDoFile(TOCScript, modName, modName)) // Read TOC file
			{
				TOCScript = null;
			}
			
			TOCScript?.Initialize();
		}

		public void ReloadModLogic()
		{
			SetModEnabled(false);
			SetModEnabled(true);
		}

		/**
		 * Enable/Disable the mod.
		 * NOTE: Enabling a disabled mod will reload the Logic files from the disk!
		 * The TOC file and its infos will be untouched.
		 * (to also reload TOC files, @see ModsManager.DiscoverMods)
		 */
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
					if (!ModsManager.Instance.TryDoFile(LogicScript, ModName, fileName)) // Read Logic Files
					{
						LogicScript = null;
						break;
					}
				}

				LogicScript?.OnEnable();
			}
			else if (!enabled && IsEnabled)
			{
				Debug.Log("------- Disable: " + ModName + " -------");
				
				LogicScript?.OnDisable();
				LogicScript = null;
			}
		}
		
		public override string ToString()
		{
			return "[ModName: " + ModName + ", Enabled: \"" + IsEnabled + "\", ModInfo: " + TOCScript.Toc.ToString() + "]";
		}
	}
}
