using System;
using System.Linq;
using Mods;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
	public class ModsMenu : MonoBehaviour
	{
		public GameObject ModsListParent;
		public GameObject ModWidgetPrefab;
		public TextMeshProUGUI ModsCountText;

		private void OnEnable()
		{
			ModsManager.Instance.OnEndDiscovery += RefreshDisplay;
			RefreshDisplay();
		}

		private void OnDisable()
		{
			if (ModsManager.HasValidInstance()) // only while we dev
				ModsManager.Instance.OnEndDiscovery -= RefreshDisplay;
		}

		public void OnReloadButtonClick() // unity event
		{
			ModsManager.Instance.DiscoverMods();
		}

		public void NotifyModChange()
		{
			int totalCount = ModsManager.Instance.EnumerateAllMods().Count();
			int enabledCount = ModsManager.Instance.EnumerateEnabledMods().Count();
			
			ModsCountText?.SetText("(" + enabledCount + "/" + totalCount + ")");
		}
		
		public void RefreshDisplay()
		{
			// clear current widgets
			ModsListParent?.GetComponentsInChildren<Transform>().ToList().ForEach(t =>
			{
				if (t != ModsListParent.transform)
					Destroy(t.gameObject);
			});
			
			// create new ones for all discovered mods
			foreach (var element in ModsManager.Instance.EnumerateAllMods())
			{
				GameObject modWidgetGo = Instantiate(ModWidgetPrefab, ModsListParent?.transform);
				ModWidget modWidget = modWidgetGo?.GetComponent<ModWidget>();
				
				modWidget?.Initialize(this, element.Value);
			}
			
			// misc changes
			NotifyModChange();
		}
		
	}
}
