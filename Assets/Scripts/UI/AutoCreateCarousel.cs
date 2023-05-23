using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AutoCreateCarousel : MonoBehaviour
{
    public GameObject carouselContainer;
    public GameObject slidePrefab;
    public float slideSpacing = 10f;

    public List<Sprite> slideImages;

    private void Start()
    {
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
        slideImages.Clear();
        GameObject tempGameObject = GameObject.FindGameObjectWithTag("PlayerManager");
        PlayerBoard tempDeck = tempGameObject.GetComponent<PlayerBoard>();
        List<CardAttributes> tempList = tempDeck.cardsInHand;
        foreach (CardAttributes c in tempList)
        {
            slideImages.Add(c._fullImage);
        }

        for (int i = carouselContainer.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(carouselContainer.transform.GetChild(i).gameObject);
        }

        float slideWidth = Screen.width * 0.8f;
        float slideHeight = Screen.height * 0.6f;

        foreach (Sprite slideImage in slideImages)
        {
            GameObject slideObject = Instantiate(slidePrefab, carouselContainer.transform);
            RectTransform slideRectTransform = slideObject.GetComponent<RectTransform>();
            slideRectTransform.sizeDelta = new Vector2(slideWidth, slideHeight);
            Image slideImageComponent = slideObject.GetComponent<Image>();
            slideImageComponent.sprite = slideImage;
        }
    }
}
