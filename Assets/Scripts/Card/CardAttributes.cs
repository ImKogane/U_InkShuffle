using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CardAttributes", order = 1)]
public class CardAttributes : ScriptableObject
{
    public string _name;
    public Material _cleanImage;
    public Sprite _fullImage;
    public int _pv;
    public int _attack;
    public Card.CardType _type;
    public Card.Rarity _rarity;
}