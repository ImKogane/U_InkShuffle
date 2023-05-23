using System;
using System.Collections.Generic;
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
		public GameObject TooltipGO;
		private RectTransform _tooltipBackground;
		private List<TextMeshProUGUI> _tooltipTexts;
		private string _tooltipCurrentOwner;
		private string _tooltipToUpdate;

		private void Start()
		{
			if (TooltipGO != null)
			{
				TooltipGO.SetActive(false);
				_tooltipTexts = TooltipGO.GetComponentsInChildren<TextMeshProUGUI>().ToList();
				_tooltipBackground = TooltipGO.GetComponentInChildren<Image>().gameObject.GetComponent<RectTransform>();
			}
		}

		public void ShowTooltip(string modName, string text)
		{
			_tooltipCurrentOwner = modName;
			TooltipGO?.SetActive(true);
			_tooltipToUpdate = text;
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
			foreach (var element in ModsManager.Instance.EnumerateAllMods())
			{
				GameObject modWidgetGo = Instantiate(ModWidgetPrefab, ModsListParent?.transform);
				ModWidget modWidget = modWidgetGo?.GetComponent<ModWidget>();
				
				modWidget?.Initialize(this, element.Value);
			}
			
			// misc changes
			NotifyModChange();
		}

		private void Update()
		{
			if (_tooltipToUpdate != "")
			{
				// bc it takes one frame for unity to update the text/width/position accordingly, I have to do it like this
				TooltipGO.transform.position = new Vector3(10000, 0, 0);
				_tooltipTexts.ForEach(t => t.SetText(_tooltipToUpdate));
				_tooltipToUpdate = "";
				return;
			}
			
			RefreshTooltipPosition();
		}

		private void RefreshTooltipPosition()
		{
			if (TooltipGO != null)
			{
				if (TooltipGO.activeInHierarchy)
				{
					Vector2 size = Vector2.Scale(_tooltipBackground.rect.size, _tooltipBackground.lossyScale); // RectTransform to screen space
					float width = (new Rect((Vector2) _tooltipBackground.position - (size * 0.5f), size)).width;
					TooltipGO.transform.position = Input.mousePosition + new Vector3((width / 2.0f) + 20, 0, 0);
				}
			}
		}
	}
}
