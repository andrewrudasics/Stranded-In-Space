﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseCollider : MonoBehaviour
{
	GameManager gm;

	// Start is called before the first frame update
	void Start()
    {
		gm = GameManager.Instance;
		gm.SetGameState(GameState.Game);
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			print("player");
			SceneManager.LoadScene("Scenes/DeathScene");
			gm.SetGameScene(SceneManager.GetActiveScene().name);
		}
		Destroy(collision.gameObject);
	}
}