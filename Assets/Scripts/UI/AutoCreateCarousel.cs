using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AutoCreateCarousel : MonoBehaviour
{
    public GameObject carouselContainer;
    public GameObject slidePrefab;


    public List<Sprite> slideImages;


    [SerializeField] private GameObject cardImgPrefab;
    public List<GameObject> cardImgList;

    private void Start()
    {
    }

    public void UpdateCarousel()
    {
        cardImgList.Clear();
        GameObject playerManager = GameObject.FindGameObjectWithTag("PlayerManager");
        PlayerBoard playerBoard = playerManager.GetComponent<PlayerBoard>();

        List<CardAttributes> tempList = playerBoard.cardsInHand;

        for (int i = carouselContainer.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(carouselContainer.transform.GetChild(i).gameObject);
        }

        foreach (CardAttributes card in tempList) 
        {
            GameObject tempCardImg = GameObject.Instantiate(cardImgPrefab, carouselContainer.transform);
            tempCardImg.gameObject.GetComponent<GenerateCard>().GenerateCardImage(card);
            cardImgList.Add(tempCardImg);

            tempCardImg.GetComponent<Image>().sprite = tempCardImg.GetComponent<GenerateCard>().cardBackground.GetComponent<Image>().sprite;

            if (card._imagePath == tempCardImg.GetComponent<GenerateCard>().path)
            {
                tempCardImg.GetComponent<ScriptableObjectManager>().card = card;
            }

        }

    }
}
