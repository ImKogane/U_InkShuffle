using System;
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
		private readonly string _basePath = Application.streamingAssetsPath + "/Mods/";
		private readonly string _extension = ".lua";
		
		private readonly SortedDictionary<string, Mod> _mods = new();
		
		public event Runner OnStartDiscovery;
		public event Runner OnEndDiscovery;

		public IEnumerable<KeyValuePair<string, Mod>> EnumerateAllMods()
		{
			return _mods.AsEnumerable();
		}
		
		public IEnumerable<KeyValuePair<string, Mod>> EnumerateEnabledMods()
		{
			return _mods.Where(e => e.Value.IsEnabled).AsEnumerable();
		}
		
		public IEnumerable<KeyValuePair<string, Mod>> EnumerateDisabledMods()
		{
			return _mods.Where(e => !e.Value.IsEnabled).AsEnumerable();
		}

		[CanBeNull]
		public Mod GetMod(string modName)
		{
			return _mods.ContainsKey(modName) ? _mods[modName] : null;
		}
		
		public bool TryDoFile(Script script, string modName, string fileName)
		{
			string fullPath = _basePath + modName + "/" + fileName + _extension;
			
			try
			{
				script.DoString(File.ReadAllText(fullPath));
				return true;
			}
			catch (Exception e)
			{
				Debug.LogWarning("Error while loading lua script at \"" + fullPath + "\", reason: " + e.Message);
				return false;
			}
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
			foreach (var element in EnumerateAllMods())
			{
				element.Value.SetModEnabled(false);
			}
			_mods.Clear();
			
			// --> iterate through the Mods directory
			DirectoryInfo dir = new DirectoryInfo(_basePath);
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
		
		public void PrintAllMods()
		{
			if (!EnumerateAllMods().Any())
			{
				print("======= NO MOD DISCOVERED =======");
			}
			else
			{
				print("======= DISCOVERED MODS: =======");
				foreach (var element in EnumerateAllMods())
				{
					print(element.Value.ToString());
				}
				print("==============================");
			}
		}

	}
	
}
