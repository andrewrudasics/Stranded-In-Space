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
                gm.SetLevelIndex(nextSceneIndex);
                SceneManager.LoadScene(nextSceneIndex);
            }
    		// Use: SceneManager.GetActiveScene().buildIndex + 1 
    		// For generic next level
    	}
    	
    }
}
