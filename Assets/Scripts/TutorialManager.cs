using UnityEditor.Rendering;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public StartTutorial startTutorial;

    public StatsSO statsSO;

    public GameObject startUI;


    public int step = 0;

    public int endTutorial = 3;

    void Start()
    {
        if(statsSO.Level.Value == 1)
        {
            startUI.SetActive(true);
            Time.timeScale = 0;
            

        }
        else
        {
            Skip();
           
        }


        
    }

    public void TutorialStart()
    {
      
        Destroy(startUI);
        startTutorial.gameObject.SetActive(true);
        startTutorial.AttakUpTutorial();
        
       
    }

    public void Skip()
    {
        Time.timeScale = 1;
        Destroy(startTutorial);
        Destroy(startUI);
        Destroy(this);

    }

    void Update()
    {
        
        print(startTutorial.step);
       
        switch(startTutorial.step)
        {
            case 1:
                startTutorial.MultUpTutorial();
                break;
            case 2:
                startTutorial.AttakUpTutorial();
                break;
            case 3:
                Time.timeScale = 1;
                statsSO.AddExp(1);
                startTutorial.step++;
                break;
            case 4:
                Skip();
                break;

        }

    }




}
