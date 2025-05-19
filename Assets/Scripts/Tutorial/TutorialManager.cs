
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public BasicTutorial basicTutorial;

    public StatsSO statsSO;

    public GameObject startUI;

    [SerializeField]
    GameObject mission;


    void Start()
    {
        basicTutorial.EndEvent += BasicTutorialEnd;
        if (statsSO.Level.Value == 1)
        {
        
            startUI.SetActive(true);
            mission.SetActive(false);

            GameManager.Instance.Pause = true;
        }
        else
        {
            GameManager.Instance.Pause = false;
            mission.SetActive(true);
            Destroy(basicTutorial.gameObject);
            Destroy(startUI);
            Destroy(gameObject);

        }
      
       
    }

    public void TutorialStart()
    {

        Destroy(startUI);
        basicTutorial.gameObject.SetActive(true);
        basicTutorial.AttakUpTutorial();


    }

    public void Skip()
    {

        GameManager.Instance.Pause = false;
        statsSO.AddExp(1);
        Destroy(basicTutorial.gameObject);
        Destroy(startUI);
        Destroy(gameObject);
        mission.SetActive(true);


    }

    public void BasicTutorialEnd()
    {
        statsSO.AddExp(1);

        Skip();

    }





}


