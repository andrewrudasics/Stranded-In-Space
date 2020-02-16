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
            string userId = GameManager.Logger.GetSavedUserId();
            if (userId == null) {
                userId = GameManager.Logger.GenerateUuid();
                GameManager.Logger.SetSavedUserId(userId);
            }
            IEnumerator rout = GameManager.Logger.StartNewSession(userId);
            StartCoroutine(rout);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    } 

    public void PlayGame()
    {
        IEnumerator startLevel = GameManager.Logger.LogLevelStart(
            5, "Starting level " + (gm.GetLevelBuildIndex() - 1));
        StartCoroutine(startLevel);
    	SceneManager.LoadScene(gm.GetLevelBuildIndex());
    }

    public void ViewControls()
    {
        IEnumerator logControlView = GameManager.Logger.LogActionWithNoLevel(1, "Viewed Controls");
        StartCoroutine(logControlView);
        SceneManager.LoadScene("Scenes/NonLevelScenes/Controls");
    }
}
