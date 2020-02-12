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
    		SceneManager.LoadScene("Scenes/DeathScene");
    		gm.SetGameScene(SceneManager.GetActiveScene().name);
    		Debug.Log("Current game state when death: " + gm.gameState);
    	}
    	
    }
}
