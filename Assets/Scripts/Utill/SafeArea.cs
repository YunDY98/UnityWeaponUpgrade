using UnityEngine;

public class SafeArea : MonoBehaviour
{
    RectTransform panel;

    void Awake()
    {
        panel = GetComponent<RectTransform>();
        ApplySafeArea(panel);

    }

#if UNITY_EDITOR
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            ApplySafeArea(panel);
        }
    }
#endif

    // 주어진 패널(RectTransform)에 대해 화면의 안전 영역(Safe Area)에 맞춰 앵커를 자동으로 조정함
    void ApplySafeArea(RectTransform panel)
    {
        // 현재 기기의 Safe Area를 가져옴 (노치, 홈 바 등을 제외한 실제 표시 영역)
        Rect safeArea = Screen.safeArea;

        // Safe Area의 시작 위치 (0,0)
        Vector2 anchorMin = safeArea.position;

        // Safe Area의 끝 위치 (1,1)
        Vector2 anchorMax = safeArea.position + safeArea.size;

        // 화면 전체 크기에 비례한 0~1 사이의 정규화된 값으로 환산
        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        // 패널의 앵커 값을 Safe Area에 맞춰 조정
        panel.anchorMin = anchorMin;
        panel.anchorMax = anchorMax;
    }
}
