using Unity.VisualScripting;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public StartTutorial startTutorial;

    public StatsSO statsSO;

    public GameObject startUI;

    void Start()
    {
        startTutorial.EndEvent += StartTutorialEnd;

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
        Destroy(startTutorial.gameObject);
        Destroy(startUI);
        Destroy(gameObject);

    }

    public void StartTutorialEnd()
    {
        statsSO.AddExp(1);
        Skip();

    }






}


