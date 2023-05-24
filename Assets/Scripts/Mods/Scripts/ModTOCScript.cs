using System.Collections.Generic;
using MoonSharp.Interpreter;
using Unity.VisualScripting;
using UnityEngine;

namespace Mods
{

	public partial class Mod // Make ModTOCScript access private members in Mod (friend)
	{

		public class ModTOCScript : ModScript
		{
			public TOC Toc { get; private set; }

			public ModTOCScript(Mod mod)
				: base(mod, CoreModules.None)
			{
			}

			// Initialize mod TOC
			public void Initialize()
			{
				Toc = new TOC();
				if (!TryLuaRunner(() => Call(Globals[_mod.ModName], Toc)))
					_mod.TOCScript = null;
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
				
				public string ToTooltipString()
				{
					string tooltip = "";
					
					tooltip += "Title: " + title.Truncate(56) + "\n";
					tooltip += "Notes: " + notes + "\n";
					tooltip += "Version: " + version + "\n";
					tooltip += "Author: " + author + "\n";
					tooltip += "Files to load: " + filesToLoad.Count + "\n";

					return tooltip;
				}
			}

		}

	}

}
