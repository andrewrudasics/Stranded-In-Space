using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    GameManager gm;
    // Start is called before the first frame update

    IEnumerator Start()
    {
        gm = GameManager.Instance; 
        string userId = GameManager.Logger.GetSavedUserId();
        if (userId.Equals("")) {
            userId = GameManager.Logger.GenerateUuid();
            GameManager.Logger.SetSavedUserId(userId);
            gm.SetUserId(userId);   
        }
        IEnumerator rout = GameManager.Logger.StartNewSession(userId);
        yield return StartCoroutine(rout);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	IEnumerator ExecuteThenLoad(IEnumerator co, int level)
	{
		yield return StartCoroutine(co);
		SceneManager.LoadScene(level);
	}

    public void PlayGame()
    {
        if (!gm.levelStarted) {
            gm.levelStarted = true;
            IEnumerator startLevel = GameManager.Logger.LogLevelStart(
            100 + (gm.GetLevelBuildIndex() - 1), "Starting level " + (gm.GetLevelBuildIndex() - 1));
			StartCoroutine(ExecuteThenLoad(startLevel, gm.GetLevelBuildIndex()));
        }
    }

    public void GoToLevelSelect()
    {
		SceneManager.LoadScene("Scenes/NonLevelScenes/LevelSelect");
	}

    public void ViewControls()
    {
        GameManager.Logger.LogActionWithNoLevel(1, "Viewed Controls");
		SceneManager.LoadScene("Scenes/NonLevelScenes/Controls");
    }
}
