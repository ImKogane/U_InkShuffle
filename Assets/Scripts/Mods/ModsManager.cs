using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Utilities;
using MoonSharp.Interpreter;
using UnityEngine;

namespace Mods
{
	
	public class ModsManager : LazySingleton<ModsManager>
	{
		private readonly SortedDictionary<string, Mod> _mods = new();
		
		public event Runner OnStartDiscovery;
		public event Runner OnEndDiscovery;

		public IEnumerable<Mod> EnumerateAllMods()
		{
			return _mods.Values.AsEnumerable();
		}
		
		// Code candy
		public IEnumerable<Mod> EnumerateEnabledMods(bool isEnabled = true)
		{
			return EnumerateAllMods().Where(mod => mod.IsEnabled == isEnabled);
		}

		[CanBeNull]
		public Mod GetMod(string modName)
		{
			return _mods.ContainsKey(modName) ? _mods[modName] : null;
		}

		protected override void SingletonAwake()
		{
			// ** Setup ** //

			Script.GlobalOptions.RethrowExceptionNested = true;
        
			Script.DefaultOptions.DebugPrint = Debug.Log;
        
			// Automatically register all [MoonSharpUserData] types
			UserData.RegisterAssembly();

			// we automatically discover the mods when we launch the game
			DiscoverMods();
		}

		/**
		 * Disable all current mods and start a new discovery.
		 * Reloads all mods entirely from disk, including their toc files.
		 * This means that this function can also detect newly installed mods.
		 */
		public void DiscoverMods()
		{
			OnStartDiscovery?.Invoke();
			
			print("======= STARTING MOD DISCOVERY... =======");

			// --> save the current enabled states so that we can reapply them
			Dictionary<string, bool> wasEnabled = _mods.ToDictionary(entry => entry.Key, 
																	entry => entry.Value.IsEnabled);
			
			// --> disable all current mods
			foreach (var mod in EnumerateAllMods())
				mod.SetModEnabled(false);
			_mods.Clear();
			
			// --> iterate through the Mods directory
			DirectoryInfo dir = new DirectoryInfo(ModScript.basePath);
			if (dir.Exists)
			{
				foreach (DirectoryInfo addonDir in dir.EnumerateDirectories())
				{
					if (!addonDir.Exists)
						continue;
					
					// --> if a potential mod directory is found, try to discover its TOC file
					string modName = addonDir.Name;
					var mod = new Mod(modName);
					if (mod.IsDiscovered)
					{
						// --> if the TOC file is valid, add this mod as a discovered mod
						_mods[modName] = mod;
						
						// --> apply back the saved enabled states, for mods that are still here
						if (wasEnabled.ContainsKey(modName))
							mod.SetModEnabled(wasEnabled[modName]); // keep the state before we reloaded
						else
							mod.SetModEnabled(true); // new mod, enable it by default
					}
				}
			}
			
			print("======= DISCOVERED " + EnumerateAllMods().Count() + " MOD(S) =======");
			
			OnEndDiscovery?.Invoke();
		}
	}
	
}
