using System.Collections;
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

    void OnEnable()
    {
        GameManager.Instance.Stop = true;
        
    }

    void Start()
    {
        StartCoroutine(LoadInit());
    }

    IEnumerator LoadInit()
    {
        
        load.value = 0;
        while (load.value < 0.99f)
        {
            load.value = (float)currentLoadCnt / totalLoadCnt;
           
            yield return null;

        }
        GameManager.Instance.Stop = false;
        gameObject.SetActive(false);

    }


    void Update()
    {
       

    }


}
