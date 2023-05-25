using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseHoverTrigger : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image previewImage;


    public void OnPointerClick(PointerEventData eventData)
    {
        GetComponent<PlaceCard>().SpawnCardAtRandomPosition();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        previewImage.gameObject.SetActive(true);
        previewImage.sprite = GetComponent<ScriptableObjectManager>().card._fullImage;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        previewImage.gameObject.SetActive(false);
    }
}
