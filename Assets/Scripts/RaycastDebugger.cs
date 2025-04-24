using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class RaycastDebugger : MonoBehaviour
{
    public GraphicRaycaster raycaster;  // 현재 UI가 붙어 있는 Canvas의 GraphicRaycaster
    public EventSystem eventSystem;     // EventSystem 오브젝트

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PointerEventData pointerData = new PointerEventData(eventSystem);
            pointerData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(pointerData, results);

            Debug.Log($"[{results.Count}]개의 UI 요소가 클릭됨:");

            foreach (var result in results)
            {
                Debug.Log($"→ {result.gameObject.name} (Sorting Order: {result.sortingOrder})");
            }
        }
    }
}