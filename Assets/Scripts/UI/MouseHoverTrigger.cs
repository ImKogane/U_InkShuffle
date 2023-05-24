using UnityEngine;
using UnityEngine.EventSystems;

public class MouseHoverTrigger : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        GetComponent<SpriteLookup>().Search();
        GetComponent<PlaceCard>().SpawnCardAtRandomPosition();
    }
}
