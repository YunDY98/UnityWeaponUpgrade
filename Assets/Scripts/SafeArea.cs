using UnityEngine;

public class SafeArea : MonoBehaviour
{
    [SerializeField]
    RectTransform panel;

    void Awake()
    {
        ApplySafeArea(panel);
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            ApplySafeArea(panel);
        }
    }
    void ApplySafeArea(RectTransform panel)
    {
        Rect safeArea = Screen.safeArea;

        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        panel.anchorMin = anchorMin;
        panel.anchorMax = anchorMax;
    }
}
