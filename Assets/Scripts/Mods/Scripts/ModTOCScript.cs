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
			private Mod m_Mod;
			private TOC m_TOC;

			public ModTOCScript(Mod Mod)
				: base(CoreModules.None)
			{
				m_Mod = Mod;
			}

			// Initialize mod TOC
			public void Initialize()
			{
				try
				{
					m_TOC = new TOC();
					Call(Globals[m_Mod.ModName], m_TOC);
				}
				catch (Exception e)
				{
					Debug.LogWarning("Error while loading TOC file for Mod \"" + m_Mod.ModName + "\", reason: " + e.Message);
					m_Mod.TOCScript = null;
				}
			}

			public TOC GetTOC() => m_TOC;

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
