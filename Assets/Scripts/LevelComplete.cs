using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelComplete : MonoBehaviour
{
	GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance; 

        GameManager.Logger.LogLevelEnd("Completed level " + (gm.GetLevelBuildIndex() -2));
        gm.levelStarted = true;
        IEnumerator startLevel = GameManager.Logger.LogLevelStart(
        100 + (gm.GetLevelBuildIndex() - 1), "Starting level " + (gm.GetLevelBuildIndex() - 1));
        StartCoroutine(startLevel);
        StartCoroutine(ExecuteAfterTime(1.5f));

    }
    IEnumerator ExecuteAfterTime(float time) 
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(gm.GetLevelBuildIndex());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
