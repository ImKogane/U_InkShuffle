using Mods;
using MoonSharp.Interpreter;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CardAttributes", order = 1)]
public class CardAttributes : ScriptableObject
{
    public string _name;
    public Material _cleanImage;
    public Sprite _fullImage;
    public string _imagePath;
    public int _pv;
    public int _attack;
    public Card.CardType _type;
    public Card.Rarity _rarity;

    // card generator from mod info
    public static CardAttributes TryCreateNewCard(Table cardTable)
    {
        var cardSO = ScriptableObject.CreateInstance<CardAttributes>();

        // TODO finish this protected func?
        T TryGet<T>(string key, DataType dataType)
        {
            DynValue elem = cardTable.Get(key);
            
            if (elem.Type == dataType)
                return (T) elem.ToObject(typeof(T));
            
            return default;
        }
        
        cardSO._name = cardTable.Get("name").String;
        
        cardSO._imagePath = ModScript.basePath + ((ModScript)cardTable.OwnerScript).GetOwningMod().ModName + "/" + cardTable.Get("imagePath").String + ModScript.imageExtension;
        
        cardSO._pv = (int) cardTable.Get("hp").Number;
        cardSO._attack = (int) cardTable.Get("attack").Number;

        switch (cardTable.Get("type").String)
        {
            case "SPECIAL":
                cardSO._type = Card.CardType.Special;
                break;
            case "NORMAL":
            default:
                cardSO._type = Card.CardType.Normal;
                break;
        }
        
        switch (cardTable.Get("rarity").String)
        {
            case "RARE":
                cardSO._rarity = Card.Rarity.Rare;
                break;
            case "EPIC":
                cardSO._rarity = Card.Rarity.Epic;
                break;
            case "COMMON":
            default:
                cardSO._rarity = Card.Rarity.Common;
                break;
        }
        
        return cardSO;
    }
    
}
