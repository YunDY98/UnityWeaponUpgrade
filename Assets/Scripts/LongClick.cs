using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LongClick : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{

    bool isPress = false;
    bool isLongPress = false;
    float pressTime = 0f;
    Coroutine repeatCor;
    Coroutine startCor;
    
    
    readonly WaitForSeconds longClick = new(0.5f);
    readonly WaitForNextFrameUnit repeatTime = new();


    public void OnPointerDown(PointerEventData eventData)
    {
        Button btn = GetComponent<Button>();
        startCor = StartCoroutine(StartRepeat(btn));
           
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPress = false;
        isLongPress = false;
        pressTime = 0f; 
        if(startCor != null)
            StopCoroutine(startCor);
        
        if(repeatCor != null)
            StopCoroutine(repeatCor);

        DataManager.Instance.SaveData();
      

    }
    IEnumerator StartRepeat(Button btn)
    {
        yield return longClick;
        repeatCor = StartCoroutine(RepeatButton(btn));

    }

    IEnumerator RepeatButton(Button btn)
    {
        while(true)
        {
            btn.onClick.Invoke();
            yield return repeatTime;
        }
        
    }

}
