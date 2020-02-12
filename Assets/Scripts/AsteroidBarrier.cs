using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AsteroidBarrier : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
    	if (collision.gameObject.tag == "Player") {
    		SceneManager.LoadScene("Scenes/DeathScene");
    		// Use: SceneManager.GetActiveScene().buildIndex + 1 
    		// For generic next level
    	}
    	
    }
}
