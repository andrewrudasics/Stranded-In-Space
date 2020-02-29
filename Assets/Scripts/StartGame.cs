using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    GameManager gm;
    // Start is called before the first frame update

    void Start()
    {
        gm = GameManager.Instance; 
        string userId = GameManager.Logger.GetSavedUserId();
        if (userId.Equals("")) {
            userId = GameManager.Logger.GenerateUuid();
            GameManager.Logger.SetSavedUserId(userId);
            gm.SetUserId(userId);   
        }
        IEnumerator rout = GameManager.Logger.StartNewSession(userId);
        StartCoroutine(rout);

    }

    // Update is called once per frame
    void Update()
    {
        
    } 


    IEnumerator ExecuteAfterTimeLevel(float time, int level) 
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(level);
    }

    IEnumerator ExecuteAfterTimeScene(float time, string scene) 
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(scene);
    }

    public void PlayGame()
    {
        if (!gm.levelStarted) {
            gm.levelStarted = true;
            IEnumerator startLevel = GameManager.Logger.LogLevelStart(
            100 + (gm.GetLevelBuildIndex() - 1), "Starting level " + (gm.GetLevelBuildIndex() - 1));
            StartCoroutine(startLevel);
            StartCoroutine(ExecuteAfterTimeLevel(2, gm.GetLevelBuildIndex()));
            
        }
    }

    public void GoToLevelSelect()
    {
        StartCoroutine(ExecuteAfterTimeScene(2, "Scenes/NonLevelScenes/LevelSelect"));
        

    }

    public void ViewControls()
    {
        GameManager.Logger.LogActionWithNoLevel(1, "Viewed Controls");

        StartCoroutine(ExecuteAfterTimeScene(1.5f, "Scenes/NonLevelScenes/Controls"));
        
    }
}
