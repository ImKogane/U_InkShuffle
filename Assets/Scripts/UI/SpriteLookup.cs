using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEditor;
using Unity.VisualScripting;

public class SpriteLookup : MonoBehaviour
{
    [SerializeField]
    public List<CardAttributes> cardScriptableObjects;

    public CardAttributes associatedScriptableObject;

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
