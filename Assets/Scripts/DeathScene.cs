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

	IEnumerator ExecuteThenLoad(IEnumerator co)
	{
		yield return StartCoroutine(co);
		SceneManager.LoadScene(gm.GetLevelBuildIndex());
	}

	public void TryAgain() 
    {
        
        if (!gm.levelStarted) {
            gm.levelStarted = true;
            IEnumerator startLevel = GameManager.Logger.LogLevelStart(
            100 + (gm.GetLevelBuildIndex() - 1), "Starting level " + (gm.GetLevelBuildIndex() - 1));
			StartCoroutine(ExecuteThenLoad(startLevel));
        }
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(gm.GetMainMenuBuildIndex());
    }
}
