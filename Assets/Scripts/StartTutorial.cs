using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartTutorial : MonoBehaviour, ICanvasRaycastFilter
{
    public RectTransform holeRect;
    RectTransform attackButton; 

    public GameObject content;

    public RectTransform x10;

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

        if(attackButton == null)
            attackButton = content.GetComponentInChildren<Button>().GetComponent<RectTransform>();
            //attackButton = content.GetComponentsInChildren<Button>()[(int)StatType.AttackDamage].GetComponent<RectTransform>();

        UpgradeTutorial(attackButton);
    }

    public void MultUpTutorial()
    {
        tutorialDec.text = "x1, x10, x100버튼을 클릭하여 빠른 업그레이드가 가능합니다";
        UpgradeTutorial(x10);
    }

    public void UpgradeTutorial(RectTransform target)
    {
        gameObject.SetActive(true);

        target.GetComponent<Button>().onClick.AddListener(() => 
        {
            
            StartTutorialStep(++step);

        });
       
        
        holeRect.position = target.position;
       
       
        holeRect.sizeDelta = target.sizeDelta;

       

        
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
            case 3:
                EndEvent?.Invoke();
                break;
            case 4:
               
                break;

        }

    }
   



    


}

