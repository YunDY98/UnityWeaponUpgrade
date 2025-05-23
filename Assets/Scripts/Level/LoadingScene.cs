using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LodingScene : MonoBehaviour
{
    //로딩 바
    public Slider loadingBar;

    public bool loadingStart = true;

    public Button gameStart;
    AsyncOperation ao;

    void Start()
    {   
      
       StartCoroutine(TransitionNextScene(1));

    }
    
    IEnumerator TransitionNextScene(int num)
    {
        ao = SceneManager.LoadSceneAsync(num);
       
        // //로드되는 씬 안보이게 
        ao.allowSceneActivation = false;

        //로딩이 완료 될때까지 
        while(!ao.isDone)
        {
            //로딩 진행률 
            loadingBar.value = ao.progress;
          
           
            if(ao.progress >= 0.9f)
            {
                ao.allowSceneActivation = true;
                // loadingBar.gameObject.SetActive(false);
                // gameStart.gameObject.SetActive(true);

            }

            yield return null;
        }

        
    }

}
