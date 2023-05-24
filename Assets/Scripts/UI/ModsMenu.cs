using System;
using System.Collections.Generic;
using System.Linq;
using Mods;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DefaultNamespace
{
	public class ModsMenu : MonoBehaviour
	{
		public GameObject ModsListParent;
		public GameObject ModWidgetPrefab;
		public TextMeshProUGUI ModsCountText;
		
		// Tooltip
		public GameObject TooltipGO;
		public RectTransform TooltipBackgroundRectTransform;
		public TextMeshProUGUI TooltipText;
		private string _tooltipCurrentOwner;

		private void Start()
		{
			TooltipGO?.SetActive(false);
		}

		public void ShowTooltip(string modName, string text)
		{
			_tooltipCurrentOwner = modName;
			TooltipGO?.SetActive(true);
			TooltipText.SetText(text);
		}
		
		public void HideTooltip(string modName)
		{
			if (modName == _tooltipCurrentOwner)
			{
				TooltipGO?.SetActive(false);
				_tooltipCurrentOwner = "";
			}
		}

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
			foreach (var mod in ModsManager.Instance.EnumerateAllMods())
			{
				GameObject modWidgetGo = Instantiate(ModWidgetPrefab, ModsListParent?.transform);
				ModWidget modWidget = modWidgetGo?.GetComponent<ModWidget>();
				
				modWidget?.Initialize(this, mod);
			}
			
			// misc changes
			NotifyModChange();
		}

		private void Update()
		{
			RefreshTooltipPosition();
		}

		private void RefreshTooltipPosition()
		{
			if (TooltipGO != null && TooltipBackgroundRectTransform != null && TooltipText != null)
			{
				if (TooltipGO.activeInHierarchy)
				{
					TooltipBackgroundRectTransform.sizeDelta = new Vector2(TooltipText.preferredWidth + 40, TooltipText.preferredHeight + 40);
					Vector2 size = Vector2.Scale(TooltipBackgroundRectTransform.rect.size, TooltipBackgroundRectTransform.lossyScale); // RectTransform to screen space
					float width = (new Rect((Vector2) TooltipBackgroundRectTransform.position - (size * 0.5f), size)).width;
					TooltipGO.transform.position = Input.mousePosition + new Vector3((width / 2.0f) + 20, 0, 0);
				}
			}
		}
	}
}
