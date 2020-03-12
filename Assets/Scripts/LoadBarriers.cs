using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadBarriers : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> barriers;

    void Start()
    {
        int deaths = PlayerPrefs.GetInt("DeathCount", -1);
        int levelsComplete = PlayerPrefs.GetInt("LevelsCompletedCount", -1);
        float deathsToCompleted = ((float)deaths) / levelsComplete;

        foreach (GameObject g in barriers) {
        	if (deathsToCompleted < 1) {
        		g.SetActive(true);
        	}
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
