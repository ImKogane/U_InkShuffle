using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AutoCreateCarousel : MonoBehaviour
{
    public GameObject carouselContainer;
    public GameObject slidePrefab;
    public float slideSpacing = 10f;

    private List<Sprite> slideImages;

    private void Start()
    {
        UpdateCarousel();
        foreach (Sprite slideImage in slideImages)
        {
            GameObject slideObject = Instantiate(slidePrefab, carouselContainer.transform);
            Image slideImageComponent = slideObject.GetComponent<Image>();
            slideImageComponent.sprite = slideImage;
        }

        HorizontalLayoutGroup layoutGroup = carouselContainer.AddComponent<HorizontalLayoutGroup>();
        layoutGroup.spacing = slideSpacing;
        layoutGroup.childScaleWidth = true; 
        layoutGroup.childScaleHeight = true;
        ContentSizeFitter sizeFitter = carouselContainer.AddComponent<ContentSizeFitter>();
        sizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        RectTransform carouselRectTransform = carouselContainer.GetComponent<RectTransform>();
        carouselRectTransform.anchoredPosition = Vector2.zero;
    }

    public void UpdateCarousel()
    {
        GameObject tempGameObject = GameObject.FindGameObjectWithTag("PlayerManager");
        PlayerBoard tempDeck = tempGameObject.GetComponent<PlayerBoard>();
        List<CardAttributes> tempList = tempDeck.cardsInHand;
        foreach (CardAttributes c in tempList)
        {
            slideImages.Add(c._fullImage);
        }
    }

}
