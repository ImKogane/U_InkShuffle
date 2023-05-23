using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class createDeckUI : MonoBehaviour
{
    public GridLayoutGroup gridLayout;
    public GameObject imagePrefab;
    public CardAttributes[] cards;

    private void Start()
    {
        cards = Resources.LoadAll<CardAttributes>("");

        DisplaySprites();
        
    }

    private void DisplaySprites()
    {
        // Supprimer tous les enfants du GridLayoutGroup
        foreach (Transform child in gridLayout.transform)
        {
            Destroy(child.gameObject);
        }

        // Créer et afficher les images pour chaque sprite
        foreach (CardAttributes card in cards)
        {
            GameObject imageObject = Instantiate(imagePrefab, gridLayout.transform);
            Image imageComponent = imageObject.GetComponent<Image>();
            imageComponent.sprite = card._fullImage;
        }

        // Mettre à jour la disposition de la grille
        gridLayout.enabled = true;
    }
}