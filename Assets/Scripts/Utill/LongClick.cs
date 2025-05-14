using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LongClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    Coroutine repeatCor;
    Coroutine startCor;



    UpgradeUI upgradeUI;


    readonly WaitForSeconds longClick = new(0.5f);
    readonly WaitForNextFrameUnit repeatTime = new();

    void Start()
    {
        upgradeUI = GetComponentInParent<UpgradeUI>();
    }



    public void OnPointerDown(PointerEventData eventData)
    {
        if (upgradeUI.cost.text == "Max")
            return;
        Button btn = GetComponent<Button>();
        startCor = StartCoroutine(StartRepeat(btn));

        AudioManager.Instance.PlaySfx(Sfx.Click);

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        AudioManager.Instance.OffClickSound();

        if (startCor != null)
        {
            StopCoroutine(startCor);

        }


        if (repeatCor != null)
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

        AudioManager.Instance.longClickSound = AudioManager.Instance.PlaySfx(Sfx.Click);
        AudioManager.Instance.longClickSound.loop = true;
        AudioManager.Instance.longClickSound.pitch = 0.9f;

        while (true)
        {
            if (upgradeUI.cost.text == "Max")
            {
                AudioManager.Instance.OffClickSound();

                break;
            }

            btn.onClick.Invoke();
            yield return repeatTime;
        }

    }

    void OnDisable()
    {
        AudioManager.Instance.OffClickSound();

    }

    



}
