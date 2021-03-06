﻿using System.Collections;
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
            int deaths = PlayerPrefs.GetInt("DeathCount", -1);
            if (deaths == -1) {
                PlayerPrefs.SetInt("DeathCount", 1);
            } else {
                PlayerPrefs.SetInt("DeathCount", deaths + 1);
            }
    		gm.died = true;
            GameManager.Logger.LogLevelAction(200 + (gm.GetLevelBuildIndex() - 1), "Level " + (gm.GetLevelBuildIndex() - 1) + ": Player died on AsteroidBarrier");
    		SceneManager.LoadScene("Scenes/NonLevelScenes/DeathScene");
    		gm.SetGameScene(SceneManager.GetActiveScene().name);
    	}
    }
}
