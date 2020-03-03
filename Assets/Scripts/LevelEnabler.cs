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
	public Button levelTwelve;
	public Button levelThirteen;
	public Button levelFourteen;
	public Button levelFifteen;
	public Button levelSixteen;
	public Button levelSeventeen;
	public Button levelEighteen;
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
    	levels.Add(levelTwelve);
    	levels.Add(levelThirteen);
    	levels.Add(levelFourteen);
    	levels.Add(levelFifteen);
    	levels.Add(levelSixteen);
    	levels.Add(levelSeventeen);
    	levels.Add(levelEighteen);
        gm = GameManager.Instance; 
        int open = gm.GetNumLevelsCompleted() + 1;
        if (open > levels.Count) {
        	open = levels.Count;
        }
        for (int i = 1; i < open; i++) {
        	levels[i].interactable = true;
        	PlayerPrefs.SetInt("" + i, 1);
        }

        for (int i = 1; i < levels.Count + 1; i++) {
        	if (PlayerPrefs.GetInt("" + i) == 1) {
        		levels[i].interactable = true;
        	} 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
