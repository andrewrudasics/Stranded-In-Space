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

    private void OnCollisionEnter2D(Collision2D collision) 
    {
    	if (collision.gameObject.tag == "Player") {
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
    		if (SceneManager.sceneCountInBuildSettings > nextSceneIndex)
            {
                GameManager.Logger.LogLevelEnd("Completed level " + (nextSceneIndex - 2));
                gm.SetLevelIndex(nextSceneIndex);
                if (SceneUtility.GetBuildIndexByScenePath("Scenes/NonLevelScenes/WinScene") == nextSceneIndex) {
                    SceneManager.LoadScene(nextSceneIndex);
                } else {
                    SceneManager.LoadScene("Scenes/NonLevelScenes/LevelCompleted");
                }
            }
    	}
    	
    }
}
