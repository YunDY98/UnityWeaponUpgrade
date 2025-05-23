using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BasicTutorial : MonoBehaviour, ICanvasRaycastFilter
{
    public RectTransform holeRect;
    Button[] tutoBtn = new Button[Enum.GetValues(typeof(Tutorial)).Length];

    public GameObject content;

    public Button x10;

    public TextMeshProUGUI tutorialDec;

    public event Action EndEvent;

    public int step = 0;




    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {

        if (RectTransformUtility.RectangleContainsScreenPoint(holeRect, sp, eventCamera))
        {
            //해당 영역에 있을경우 레이케스트 무시 
            return false;
        }

        return true;
    }


    public void AttakUpTutorial()
    {

        tutorialDec.text = "공격력 업그레이드";
        int type = (int)Tutorial.Attack;
        if (tutoBtn[type] == null)
        {
            tutoBtn[type] = content.GetComponentInChildren<Button>();

        }


        UpgradeTutorial(tutoBtn[type]);
    }

    public void MultUpTutorial()
    {
        tutorialDec.text = "x1, x10, x100버튼을 클릭하여 빠른 업그레이드가 가능합니다";
        UpgradeTutorial(x10);
    }

    public void UpgradeTutorial(Button target)
    {
        target.TryGetComponent<LongClick>(out var longClick);

        //튜토리얼중 롱클릭 off
        if (longClick != null)
            longClick.enabled = false;

        UnityEngine.Events.UnityAction oneTimeListener = null;

        oneTimeListener = () =>
        {
            //다음 튜토리얼로 이동
            BasicTutorialStep(++step);

            //튜토리얼 종료시 롱클릭 on
            if (longClick != null)
                longClick.enabled = true;

            //현재 튜토리얼이 끝나면 리스너 제거 
            target.onClick.RemoveListener(oneTimeListener);

        };
        target.onClick.AddListener(oneTimeListener);

        //현재 튜토리얼 영역에 맞춰 빨갠색으로 강조 
        var rect = target.GetComponent<RectTransform>();
        holeRect.position = rect.position;
        holeRect.sizeDelta = rect.sizeDelta;
    }


    void BasicTutorialStep(int step)
    {
        switch (step)
        {
            case 1:
                MultUpTutorial();
                break;
            case 2:
                AttakUpTutorial();
                break;
            default:
                EndEvent?.Invoke();
                break;

        }
    }




}


enum Tutorial
{
    Attack,
}

