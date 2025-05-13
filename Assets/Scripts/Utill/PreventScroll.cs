using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollPrevent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private ScrollRect scrollRect;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scrollRect = GetComponentInParent<ScrollRect>();
        
    }

   public void OnPointerEnter(PointerEventData eventData)
    {
        scrollRect.enabled = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       
        scrollRect.enabled = true;
    }
}
