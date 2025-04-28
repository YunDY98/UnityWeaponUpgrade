using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LongClick : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    Coroutine repeatCor;
    Coroutine startCor;

    AudioSource longClickSound;
    
    
    readonly WaitForSeconds longClick = new(0.5f);
    readonly WaitForNextFrameUnit repeatTime = new();


    public void OnPointerDown(PointerEventData eventData)
    {
        Button btn = GetComponent<Button>();
        startCor = StartCoroutine(StartRepeat(btn));
        
        AudioManager.Instance.PlaySfx(Sfx.Click);
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OffSound();
    
        if(startCor != null)
        {
            StopCoroutine(startCor);

        }
            
        
        if(repeatCor != null)
        {
            StopCoroutine(repeatCor);
            
           
            
            

        }
           

        DataManager.Instance.SaveData();
      

    }
    IEnumerator StartRepeat(Button btn)
    {

       
        yield return longClick;
        repeatCor = StartCoroutine(RepeatButton(btn));
       

    }

    IEnumerator RepeatButton(Button btn)
    {
        
        longClickSound =  AudioManager.Instance.PlaySfx(Sfx.Click);
        longClickSound.loop = true;
        longClickSound.pitch = 0.9f;
    
        while(true)
        {
            btn.onClick.Invoke();
            yield return repeatTime;
        }
        
    }

    void OnDisable()
    {
       OffSound();
       
    }

    void OffSound()
    {
        
        if(longClickSound != null)
        {
            longClickSound.loop = false;
            longClickSound.Stop();

        }
           
        
        

    }



}
