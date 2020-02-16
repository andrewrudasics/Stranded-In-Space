using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScene : MonoBehaviour
{
	GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;   
        gm.SetLevelIndex(2);
        IEnumerator playerWin  = GameManager.Logger.LogActionWithNoLevel(3, "Player completed all levels");
        StartCoroutine(playerWin);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
