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

    private string folderPath = "Assets/ScriptableObjects";

    public void Search()
    {
        FillCardList();
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

    private void FillCardList()
    {
        string[] filePaths = AssetDatabase.FindAssets("t:ScriptableObject", new string[] { folderPath });

        foreach (string filePath in filePaths)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(filePath);

            ScriptableObject scriptableObject = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);

            cardScriptableObjects.Add((CardAttributes)scriptableObject);
        }
    }
}
