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

    IEnumerator ExecuteAfterTime(float time) 
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(gm.GetLevelBuildIndex());
    }

    public void TryAgain() 
    {
        GameManager.Logger.LogLevelEnd("Player died on Level " + (gm.GetLevelBuildIndex() - 1));
        IEnumerator startLevel = GameManager.Logger.LogLevelStart(
            100 + (gm.GetLevelBuildIndex() - 1), "Starting level " + (gm.GetLevelBuildIndex() - 1));
        StartCoroutine(startLevel);
        StartCoroutine(ExecuteAfterTime(1));
    }

    public void GoToMainMenu()
    {
        GameManager.Logger.LogLevelEnd("Player died on Level " + (gm.GetLevelBuildIndex() - 1));
        SceneManager.LoadScene(gm.GetMainMenuBuildIndex());
    }
}
