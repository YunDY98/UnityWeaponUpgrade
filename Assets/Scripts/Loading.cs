using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [SerializeField]
    Slider load;

    public static int totalLoadCnt = 0;
    public static int currentLoadCnt = 0;
    public static int spriteLoadCnt;

    void Start()
    {
        Time.timeScale = 0f;
    }

    void Update()
    {
        load.value = (float)currentLoadCnt / totalLoadCnt;
        
        if (load.value > 0.99f)
        {
            Time.timeScale = 1f;
            Destroy(gameObject);
            
        }

    }


}
