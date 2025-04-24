using UnityEngine;
using UnityEngine.EventSystems;

public class TouchRing : MonoBehaviour,IPointerDownHandler
{
    [SerializeField]
    EffectManager effect;
    public void OnPointerDown(PointerEventData eventData)
    {

        effect.TouchRing(eventData.position);
    }
}
