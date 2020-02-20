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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ExecuteAfterTime(int time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(gm.GetLevelBuildIndex());
    }

    public void TryAgain() 
    {
        IEnumerator startLevel = GameManager.Logger.LogLevelStart(
            5, "Starting level " + (gm.GetLevelBuildIndex() - 1));
        StartCoroutine(startLevel);
    	StartCoroutine(ExecuteAfterTime(2));
    }

    public void GoToMainMenu()
    {
        GameManager.Logger.LogLevelEnd(
            "Level " + (gm.GetLevelBuildIndex() - 1) + " quit after death");
        SceneManager.LoadScene(gm.GetMainMenuBuildIndex());
    }
}
