using System;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

public class StartTutorial : MonoBehaviour, ICanvasRaycastFilter
{
    public RectTransform holeRect;
    Button[] tutoBtn =  new Button[Enum.GetValues(typeof(Tutorial)).Length]; 

    public GameObject content;

    public Button x10;

    public TextMeshProUGUI tutorialDec;

    public event Action EndEvent;

    public int step = 0;
    
  

    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
       
        if(RectTransformUtility.RectangleContainsScreenPoint(holeRect, sp, eventCamera))
        {
            return false; 
        }
        
        return true;
    }
    

    public void AttakUpTutorial()
    {

        tutorialDec.text = "공격력 업그레이드";
        int type = (int)Tutorial.Attack;
        if(tutoBtn[type] == null)
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
        gameObject.SetActive(true);


        target.TryGetComponent<LongClick>(out var longClick);

        if(longClick != null)
            longClick.enabled = false;   
        
        UnityEngine.Events.UnityAction oneTimeListener = null;

        oneTimeListener = () =>
        {
            StartTutorialStep(++step);
            if(longClick != null)
                longClick.enabled = true;
            target.onClick.RemoveListener(oneTimeListener);
           
        };
        target.onClick.AddListener(oneTimeListener);

        var rect = target.GetComponent<RectTransform>();
       
        
        holeRect.position = rect.position;
       
       
        holeRect.sizeDelta = rect.sizeDelta;

       

        
    }


    void StartTutorialStep(int step)
    {
        
       
       
        switch(step)
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

