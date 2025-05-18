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
        get => _instance;
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
    
        StartCoroutine(LoadInit());
    }

    IEnumerator LoadInit()
    {
        GameManager.Instance.Stop = true;
        
        load.value = 0;
        while (load.value < 0.99f)
        {
            load.value = (float)currentLoadCnt / totalLoadCnt;
            yield return null;
        }
        GameManager.Instance.Stop = false;
        gameObject.SetActive(false);
    }
}
