using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEnabler : MonoBehaviour
{
	GameManager gm;
	
	public Button levelOne;
	public Button levelTwo;
	public Button levelThree;
	public Button levelFour;
	public Button levelFive;
	public Button levelSix;
	public Button levelSeven;
	public Button levelEight;
	public Button levelNine;
	public Button levelTen;
	public Button levelEleven;
    // Start is called before the first frame update
    void Start()
    {
    	gm = GameManager.Instance; 
    	List<Button> levels = new List<Button>();
    	levels.Add(levelOne);
    	levels.Add(levelTwo);
    	levels.Add(levelThree);
    	levels.Add(levelFour);
    	levels.Add(levelFive);
    	levels.Add(levelSix);
    	levels.Add(levelSeven);
    	levels.Add(levelEight);
    	levels.Add(levelNine);
    	levels.Add(levelTen);
    	levels.Add(levelEleven);
        gm = GameManager.Instance; 
        int open = gm.GetNumLevelsCompleted() + 1;
        if (open > levels.Count) {
        	open = levels.Count;
        }
        for (int i = 1; i < open; i++) {
        	levels[i].interactable = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
