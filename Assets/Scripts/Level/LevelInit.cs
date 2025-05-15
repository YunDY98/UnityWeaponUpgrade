using UnityEngine;


public class LevelInit : MonoBehaviour
{
    [SerializeField]
    RectTransform targetUI;

    
    Transform map;

    void Awake()
    {

        map = GetComponentInChildren<Move>().transform;
       
    }

    void Start()
    {
        Init();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
            Init();
        
    }
    // void Init()
    // {
    //     // UI의 Top 위치 구함 (world 기준)
    //     Vector3 uiTop = targetUI.TransformPoint(new Vector3(0, targetUI.rect.height /2f, 0));

    //     // UI Top을 화면 좌표로 변환
    //     Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(null, uiTop);

    //     // 다시 월드 좌표로 변환 (this의 깊이 기준)
    //     float zDepth = Camera.main.WorldToScreenPoint(transform.position).z;
    //     Vector3 worldTarget = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, zDepth));

    //     // 맵의 밑변 위치 구함
    //     float bottomOffset = map.GetComponent<Renderer>().bounds.size.y /2f;

    //     // 맵을 기준으로 밑면이 UI 위에 닿도록 위치 이동
    //     transform.position = new Vector3(0, worldTarget.y +bottomOffset, 0);
    // }

    void Init()
{
    // UI의 왼쪽 상단 위치 구함 (world 기준)
    Vector3 uiTopLeft = targetUI.TransformPoint(new Vector3(-targetUI.rect.width / 2f, targetUI.rect.height / 2f, 0));

   

    // UI 왼쪽 상단을 화면 좌표로 변환
    Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(null, uiTopLeft);

    print(screenPos.x + " xxxxx");


    // 다시 월드 좌표로 변환 (this의 깊이 기준)
    float zDepth = Camera.main.WorldToScreenPoint(transform.position).z;
    Vector3 worldTarget = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, zDepth));

     print(worldTarget.x + "t xxxxx");

    // 맵의 왼쪽 하단 위치 구함
    float mapWidth = map.GetComponent<Renderer>().bounds.size.x;
    float bottomOffset = map.GetComponent<Renderer>().bounds.size.y / 2f;
    float leftOffset = mapWidth / 2f;

    // 맵을 UI의 왼쪽과 맞추고, 밑면이 UI 위에 닿도록 위치 이동
    transform.position = new Vector3(
        worldTarget.x + leftOffset,  // X 위치: UI 왼쪽에 맵 왼쪽을 맞춤
        worldTarget.y + bottomOffset, // Y 위치: 기존과 동일
        0
    );

   
    

}
   
}
