using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    
    void Awake()
    {

        Camera camera = Camera.main;

        Rect rect = camera.rect;

        float scaleHeight = ((float)Screen.width / Screen.height) / (9f / 16f);
        float scaleWidth = 1f / scaleHeight;

        if(scaleHeight < 1)
        {
            rect.height = scaleHeight;
            rect.y = (1f - scaleHeight) / 2f;
        }
        else
        {
            rect.width = scaleWidth;
            rect.x = (1f - scaleWidth) / 2f;
        }

        camera.rect = rect;
        
    }
}
