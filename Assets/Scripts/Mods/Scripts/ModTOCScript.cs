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
				Toc = new TOC(_mod.ModName);
				SetupGlobals();
			}

			// Setup global environment
			private void SetupGlobals()
			{
				Globals["toc"] = Toc;
			}

			[MoonSharpUserData]
			public class TOC
			{
				public string title;
				public string notes;
				public string version;
				public string author;
				public List<string> filesToLoad = new();

				private string _defaultModTitle;

				public TOC(string defaultModTitle)
				{
					_defaultModTitle = defaultModTitle;
				}
				
				[MoonSharpHidden]
				public string GetDisplayTitle()
				{
					return string.IsNullOrWhiteSpace(title) ? _defaultModTitle : title;
				}
				
				public override string ToString()
				{
					return "[Title: \"" + GetDisplayTitle() + "\", Version \"" + version + "\", Author: \"" + author + "\"]";
				}
				
				public string ToTooltipString()
				{
					string tooltip = "";

					tooltip += "Title: " + GetDisplayTitle().Truncate(56) + "\n";
					
					if (!string.IsNullOrWhiteSpace(notes))
						tooltip += "Notes: " + notes + "\n";
					
					if (!string.IsNullOrWhiteSpace(version))
						tooltip += "Version: " + version + "\n";
					
					if (!string.IsNullOrWhiteSpace(author))
						tooltip += "Author: " + author + "\n";
					
					tooltip += "Files to load: " + filesToLoad.Count + "\n";

					return tooltip;
				}
			}

		}

	}

}
