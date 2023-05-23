using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Mods;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ModWidget : MonoBehaviour
{
    private ModsMenu _modsMenu;
    private Mod _mod;
    
    public TextMeshProUGUI ModTitleText;
    public Toggle ModEnabledToggle;

    private bool HasValidMod()
    {
        return _mod is {IsDiscovered: true};
    }

    public void Initialize(ModsMenu modsMenu, Mod mod)
    {
        _modsMenu = modsMenu;
        _mod = mod;
        RefreshDisplay();
    }

    private void RefreshDisplay()
    {
        if (HasValidMod())
        {
            ModTitleText?.SetText(_mod.TOCScript.Toc.title);
            ModEnabledToggle?.SetIsOnWithoutNotify(_mod.IsEnabled);
        }
    }

    public void OnToggleValueChanged(bool state) // unity event
    {
        if (HasValidMod())
        {
            _mod.SetModEnabled(state);
            RefreshDisplay();
            _modsMenu.NotifyModChange();
        }
    }
}
