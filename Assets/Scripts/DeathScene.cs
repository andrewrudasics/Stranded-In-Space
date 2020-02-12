using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScene : MonoBehaviour
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

    public void TryAgain() 
    {
    	SceneManager.LoadScene(gm.GetLevelBuildIndex());
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(gm.GetMainMenuBuildIndex());
    }
}
