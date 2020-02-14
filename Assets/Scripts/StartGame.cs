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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
    	SceneManager.LoadScene(gm.GetLevelBuildIndex());
    }

    public void ViewControls()
    {
        SceneManager.LoadScene("Scenes/NonLevelScenes/Controls");
    }
}
