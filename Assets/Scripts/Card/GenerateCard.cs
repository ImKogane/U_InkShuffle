using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GenerateCard : MonoBehaviour
{

    [Header("Card elements")]
    [SerializeField] private GameObject cardBorder;
    [SerializeField] public GameObject cardBackground;
    [SerializeField] private TextMeshProUGUI lifePointText;
    [SerializeField] private TextMeshProUGUI atkPointText;
    [SerializeField] private TextMeshProUGUI nameText;

    public string path;

    private Dictionary<string, Sprite> _loadedSprites = new();

    public void GenerateCardImage(CardAttributes cardValues, Card card)
    {
        // optimisation, instead of generating a CardAttributes from a card, get either one or the other
        if (cardValues != null)
        {
            nameText.text = cardValues._name;
            atkPointText.text = cardValues._attack.ToString();
            lifePointText.text = cardValues._pv.ToString();

            if(cardBackground != null ) cardBackground.GetComponent<Image>().sprite = GetSprite(cardValues._imagePath);

            path = cardValues._imagePath;
        }
        else if (card != null)
        {
            nameText.text = card.Name;
            atkPointText.text = card.Attack.ToString();
            lifePointText.text = card.PV.ToString();

            if(cardBackground != null ) cardBackground.GetComponent<Image>().sprite = GetSprite(card._imagePath);

            path = card._imagePath;
        }
    }

    private Texture2D LoadImageFromFile(string path)
    {
        // Lire les données binaires de l'image depuis le chemin spécifié
        byte[] imageData = System.IO.File.ReadAllBytes(path);

        // Créer une nouvelle texture
        Texture2D texture = new Texture2D(2, 2);

        // Charger les données binaires de l'image dans la texture
        texture.LoadImage(imageData);

        return texture;
    }
    private Sprite SpriteTransform(Texture2D imageTexture)
    {
        // Convertir l'image en sprite
        Sprite sprite = Sprite.Create(imageTexture, new Rect(0, 0, imageTexture.width, imageTexture.height), Vector2.zero);

        // Assigner le sprite à un composant SpriteRenderer ou à un autre objet nécessitant un sprite
        return sprite;
    }

    private Sprite GetSprite(string path)
    {
        // sprite load from disk optimization (keep in memory)
        if (_loadedSprites.ContainsKey(path))
        {
            return _loadedSprites[path];
        }
        else
        {
            _loadedSprites[path] = SpriteTransform(LoadImageFromFile(path));
            return _loadedSprites[path];
        }
    }
}
