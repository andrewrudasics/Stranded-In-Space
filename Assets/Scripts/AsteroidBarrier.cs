using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AsteroidBarrier : MonoBehaviour
{
	GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
        Debug.Log("Current game state when Starts: " + gm.gameState);
        gm.SetGameState(GameState.Game);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
    	if (collision.gameObject.tag == "Player") {
            IEnumerator death = GameManager.Logger.LogActionWithNoLevel(4, "Level " + gm.GetLevelBuildIndex() + ": Player died on AsteroidBarrier");
            StartCoroutine(death);
            IEnumerator levelComplete = GameManager.Logger.LogLevelEnd("Died on level " + (gm.GetLevelBuildIndex() - 1));
            StartCoroutine(levelComplete);
    		SceneManager.LoadScene("Scenes/NonLevelScenes/DeathScene");
    		gm.SetGameScene(SceneManager.GetActiveScene().name);
    	}
    	
    }
}
