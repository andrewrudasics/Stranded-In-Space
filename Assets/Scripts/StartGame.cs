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
        if (gm.GetUserId().Equals("")) {

            string userId = GameManager.Logger.GenerateUuid();
            GameManager.Logger.SetSavedUserId(userId);
            gm.SetUserId(userId);
            IEnumerator rout = GameManager.Logger.StartNewSession(userId);
            StartCoroutine(rout);
        }
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
    public void PlayGame()
    {

        IEnumerator startLevel = GameManager.Logger.LogLevelStart(
            100 + (gm.GetLevelBuildIndex() - 1), "Starting level " + (gm.GetLevelBuildIndex() - 1));
        StartCoroutine(startLevel);
        StartCoroutine(ExecuteAfterTime(1));
    	
    }

    public void ViewControls()
    {
        GameManager.Logger.LogActionWithNoLevel(1, "Viewed Controls");
        SceneManager.LoadScene("Scenes/NonLevelScenes/Controls");
    }
}
