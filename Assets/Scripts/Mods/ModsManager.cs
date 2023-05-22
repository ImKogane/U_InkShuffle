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
		private readonly string BasePath = Application.streamingAssetsPath + "/Mods/";
		private readonly string Extension = ".lua";
		
		private readonly SortedDictionary<string, Mod> Mods = new();

		public event Runner OnStartDiscovery;
		public event Runner OnEndDiscovery;

		public IEnumerable<KeyValuePair<string, Mod>> EnumerateAllMods()
		{
			return Mods.AsEnumerable();
		}
		
		public IEnumerable<KeyValuePair<string, Mod>> EnumerateEnabledMods()
		{
			return Mods.Where(e => e.Value.IsFullyLoaded).AsEnumerable();
		}
		
		public IEnumerable<KeyValuePair<string, Mod>> EnumerateDisabledMods()
		{
			return Mods.Where(e => !e.Value.IsFullyLoaded).AsEnumerable();
		}

		[CanBeNull]
		public Mod GetMod(string ModName)
		{
			return Mods.ContainsKey(ModName) ? Mods[ModName] : null;
		}
		
		public int GetModsCount()
		{
			return Mods.Count;
		}
		
		public bool TryDoFile(Script script, string modName, string fileName)
		{
			string fullPath = BasePath + modName + "/" + fileName + Extension;
			
			try
			{
				script.DoString(File.ReadAllText(fullPath));
				return true;
			}
			catch (InterpreterException e)
			{
				Debug.LogWarning("Error while loading lua script at : " + e.DecoratedMessage);
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
			Dictionary<string, bool> WasEnabled = Mods.ToDictionary(entry => entry.Key, 
																	entry => entry.Value.IsFullyLoaded);
			
			// --> disable all current mods
			foreach (var element in EnumerateAllMods())
			{
				element.Value.SetModEnabled(false);
			}
			Mods.Clear();
			
			// --> iterate through the Mods directory
			DirectoryInfo dir = new DirectoryInfo(BasePath);
			if (dir.Exists)
			{
				foreach (DirectoryInfo addonDir in dir.EnumerateDirectories())
				{
					if (!addonDir.Exists)
						continue;
					
					// --> if a potential mod directory is found, try to discover its TOC file
					string ModName = addonDir.Name;
					var Mod = new Mod(ModName);
					if (Mod.IsValid)
					{
						// --> if the TOC file is valid, add this mod as a discovered mod
						Mods[ModName] = Mod;
						
						// --> apply back the saved enabled states, for mods that are still here
						if (WasEnabled.ContainsKey(ModName))
							Mod.SetModEnabled(WasEnabled[ModName]); // keep the state before we reloaded
						else
							Mod.SetModEnabled(true); // new mod, enable it by default
					}
				}
			}
			
			print("======= DISCOVERED " + GetModsCount() + " MOD(S) =======");
			
			OnEndDiscovery?.Invoke();
		}
		
		public void PrintAllMods()
		{
			if (GetModsCount() <= 0)
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
