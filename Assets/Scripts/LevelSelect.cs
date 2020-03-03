using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
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

    public void GoToLevel(int x)
    {
    	gm.SetLevelIndex(x + 1);
    	if (!gm.levelStarted) {
            gm.levelStarted = true;
            IEnumerator startLevel = GameManager.Logger.LogLevelStart(
            100 + (gm.GetLevelBuildIndex() - 1), "Starting level " + (gm.GetLevelBuildIndex() - 1));
			StartCoroutine(ExecuteThenLoad(startLevel));
        }
    }
}
