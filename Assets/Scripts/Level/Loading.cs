using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{

    private static Loading _instance;
    public static Loading Instance
    {
        get { return _instance; }
        set { _instance = value; }

    }
    [SerializeField]
    Slider load;

    public int totalLoadCnt = 0;
    public int currentLoadCnt = 0;
    public int spriteLoadCnt;

    void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;

            DontDestroyOnLoad(gameObject);
        }

    }

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
