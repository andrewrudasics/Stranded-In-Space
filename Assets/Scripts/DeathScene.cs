using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScene : MonoBehaviour
{

	GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
    	gm = GameManager.Instance;   
        if (gm.died) {
            GameManager.Logger.LogLevelEnd("Player died on Level " + (gm.GetLevelBuildIndex() - 1));
        }
        gm.died = false;
        gm.levelStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ExecuteAfterTime(float time) 
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(gm.GetLevelBuildIndex());
    }

    public void TryAgain() 
    {
        
        if (!gm.levelStarted) {
            gm.levelStarted = true;
            IEnumerator startLevel = GameManager.Logger.LogLevelStart(
            100 + (gm.GetLevelBuildIndex() - 1), "Starting level " + (gm.GetLevelBuildIndex() - 1));
            StartCoroutine(startLevel);
            StartCoroutine(ExecuteAfterTime(2));
        }
    }

    public void GoToMainMenu()
    {
        //GameManager.Logger.LogLevelEnd("Player died on Level " + (gm.GetLevelBuildIndex() - 1));
        SceneManager.LoadScene(gm.GetMainMenuBuildIndex());
    }
}
