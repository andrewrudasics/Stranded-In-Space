﻿using System.Collections;
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

		foreach (Transform child in transform)
		{
			if (deathsToCompleted < 0.6)
			{
				child.gameObject.SetActive(true);
				GameManager.Logger.LogActionWithNoLevel(10, "Loaded Hard Barrier");
			}
			else
			{
				child.gameObject.SetActive(false);
			}

		}
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}