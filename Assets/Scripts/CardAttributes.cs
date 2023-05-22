using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CardStats", order = 1)]
public class CardAttributes : ScriptableObject
{
    public string _name;
    public Texture _image;
    public int _pv;
    public int _attack;
    public Card.CardType _type;
}
