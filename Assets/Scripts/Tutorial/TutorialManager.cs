
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public StartTutorial startTutorial;

    public StatsSO statsSO;

    public GameObject startUI;

    [SerializeField]
    GameObject mission;


    void Start()
    {
        startTutorial.EndEvent += StartTutorialEnd;
        if (statsSO.Level.Value == 1)
        {
        
            startUI.SetActive(true);
            mission.SetActive(false);

            Time.timeScale = 0;
         
            //GameManager.Instance.Stop = true;
        }
        else
        {
            GameManager.Instance.Stop = false;
            mission.SetActive(true);
            Destroy(startTutorial.gameObject);
            Destroy(startUI);
            Destroy(gameObject);

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
        
        GameManager.Instance.Stop = false;
        statsSO.AddExp(1);
        Destroy(startTutorial.gameObject);
        Destroy(startUI);
        Destroy(gameObject);
        mission.SetActive(true);


    }

    public void StartTutorialEnd()
    {
        statsSO.AddExp(1);

        Skip();

    }


    void OnDisable()
    {
       
       
        Time.timeScale = 1;
        
        
    }





}


