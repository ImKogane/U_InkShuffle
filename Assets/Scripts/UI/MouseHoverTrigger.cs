using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseHoverTrigger : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image previewImage;

    private GameObject cardPreview;

    private void Start()
    {
        cardPreview = Camera.main.GetComponent<CardClick>().cardPreview;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GetComponent<PlaceCard>().SpawnCardAtRandomPosition();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // previewImage.gameObject.SetActive(true);

        cardPreview.gameObject.SetActive(true);
        cardPreview.GetComponent<GenerateCard>().GenerateCardImage(GetComponent<ScriptableObjectManager>().card, null);

        //previewImage.gameObject.GetComponent<GenerateCard>().GenerateCardImage(previewImage.gameObject.GetComponent<ScriptableObjectManager>().card);
        //previewImage.sprite = GetComponent<ScriptableObjectManager>().card._fullImage;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        cardPreview.gameObject.SetActive(false);
        // previewImage.gameObject.SetActive(false);
    }
}
