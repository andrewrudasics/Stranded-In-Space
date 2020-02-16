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

    public void TryAgain() 
    {
        IEnumerator startLevel = GameManager.Logger.LogLevelStart(
            5, "Starting level " + (gm.GetLevelBuildIndex() - 1));
        StartCoroutine(startLevel);
    	SceneManager.LoadScene(gm.GetLevelBuildIndex());
    }

    public void GoToMainMenu()
    {
        IEnumerator levelQuit = GameManager.Logger.LogLevelEnd(
            "Level " + (gm.GetLevelBuildIndex() - 1) + " quit after death");
        StartCoroutine(levelQuit);
        SceneManager.LoadScene(gm.GetMainMenuBuildIndex());
    }
}
