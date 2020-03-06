using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Airlock : MonoBehaviour
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

    IEnumerator ExecuteThenLoad(IEnumerator co)
    {
        yield return StartCoroutine(co);
        SceneManager.LoadScene(gm.GetLevelBuildIndex());
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
    	if (collision.gameObject.tag == "Player") {
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            PlayerPrefs.SetInt("" + (nextSceneIndex - 2), 1);
    		if (SceneManager.sceneCountInBuildSettings > nextSceneIndex)
            {
                gm.AddCompletedLevelIndex(nextSceneIndex - 1);
                GameManager.Logger.LogLevelEnd("Completed level " + (nextSceneIndex - 2));
                gm.SetLevelIndex(nextSceneIndex);
                gm.levelStarted = false;
                if (SceneUtility.GetBuildIndexByScenePath("Scenes/NonLevelScenes/WinScene") == nextSceneIndex) {
                    SceneManager.LoadScene(nextSceneIndex);
                } else {
                    if (!gm.levelStarted) {
                        gm.levelStarted = true;
                        IEnumerator startLevel = GameManager.Logger.LogLevelStart(
                        100 + (gm.GetLevelBuildIndex() - 1), "Starting level " + (gm.GetLevelBuildIndex() - 1));
                        StartCoroutine(ExecuteThenLoad(startLevel));
                    }
                    //SceneManager.LoadScene(nextSceneIndex);//"Scenes/NonLevelScenes/LevelSelect");
                }
            }
    	}
    	
    }
}
