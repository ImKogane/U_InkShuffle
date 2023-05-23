using System;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using UnityEngine;

namespace Mods
{

	public partial class Mod // Make ModTOCScript access private members in Mod (friend)
	{

		public class ModTOCScript : Script
		{
			public TOC Toc { get; private set; }
			
			private Mod _mod;

			public ModTOCScript(Mod mod)
				: base(CoreModules.None)
			{
				_mod = mod;
			}

			// Initialize mod TOC
			public void Initialize()
			{
				try
				{
					Toc = new TOC();
					Call(Globals[_mod.ModName], Toc);
				}
				catch (Exception e)
				{
					Debug.LogWarning("Error while loading TOC file for Mod \"" + _mod.ModName + "\", reason: " + e.Message);
					_mod.TOCScript = null;
				}
			}

			[MoonSharpUserData]
			public class TOC
			{
				public string title;
				public string notes;
				public string version;
				public string author;
				public List<string> filesToLoad = new();
				
				public override string ToString()
				{
					return "[Title: \"" + title + "\", Version \"" + version + "\", Author: \"" + author + "\"]";
				}
			}

		}

	}

}
