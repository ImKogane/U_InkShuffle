using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SpriteLookup : MonoBehaviour
{
    public List<CardAttributes> cardScriptableObjects;
    public CardAttributes associatedScriptableObject;

    public void Start()
    {
        CardAttributes[] cards = Resources.LoadAll<CardAttributes>("");
        cardScriptableObjects = new List<CardAttributes>(cards);
    }
    public void Search()
    {
        Sprite currentSprite = GetComponent<Image>().sprite;

        foreach (CardAttributes c in cardScriptableObjects)
        {
            if(currentSprite == c._fullImage) 
            { 
                associatedScriptableObject = c;
            }
            else
            {
                continue;
            }
        }
    }
}
