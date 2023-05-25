using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mods;
using MoonSharp.Interpreter;
using UnityEngine;

public class ModLinker : MonoBehaviour
{
    public PlayerBoard IABoard;
    public PlayerBoard PlayerBoard;

    private List<CardAttributes> _generatedDeck = new();

    // do the init in awake, before the board Start
    private void Awake()
    {
        return; // TODO remove when image path is fixed
        
        foreach (var mod in ModsManager.Instance.EnumerateEnabledMods())
        {
            DynValue cardsTable = mod.TryCallR("RegisterCards");
            if (cardsTable == null || cardsTable.Type != DataType.Table)
                continue;
            
            foreach (var cardTable in cardsTable.Table.Values)
            {
                if (cardTable == null || cardTable.Type != DataType.Table)
                    continue;
                
                var cardSO = CardAttributes.TryCreateNewCard(cardTable.Table);
                if (cardSO != null)
                    _generatedDeck.Add(cardSO);
            }
            
        }

        IABoard.cardsDeck = _generatedDeck.ToList();
        PlayerBoard.cardsDeck = _generatedDeck.ToList();
    }
    
}
