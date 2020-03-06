/*
Logging ActionId Glossery
    1xx = Starting level xx (LogStartLevel)
    2xx = Died by asteroid on level xx (LogLevelAction)
    3xx = Died off screen on level xx (LogLevelAction)
    4xx = Player jumped in level xx (LogLevelAction)
    1 = Player viewed controls (NoLevelAction)
    2 = Player finished all levels (NoLevelAction)
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cse481.logging;

public enum GameState { NullState, MainMenu, Game}
public delegate void OnStateChangeHandler();

public class GameManager {

	private string scene = " ";
    private int levelBuildIndex = 2;
    private int mainMenuBuildIndex = 0;
    private string userId = "";

    public bool levelStarted = false;
    public bool died = false;
    public static bool pullBack = false;

    /* Set to 5 on release
     *
     */
    private static int pullBackIndex = 5;

    private HashSet<int> levelsComplete = new HashSet<int>();

    private static GameManager _instance = null;
    private static CapstoneLogger loggerInstance = null;
    public event OnStateChangeHandler OnStateChange;
    public GameState gameState { get; private set; }
    public WaitTimer timer;


    protected GameManager() {}


    // Singleton pattern implementation
    public static GameManager Instance {
        get {
            if (_instance == null) {  
                _instance = new GameManager();
            }  
            return _instance;
        }
    }



    // Last variable (int) is used to determine if it is logging debug data, test data, deployment data, 
    // switch for publishing
    public static CapstoneLogger Logger {
        get {
            if (loggerInstance == null) {  
                int savedIndex = PlayerPrefs.GetInt("LoggingIndex", -1);
                if (savedIndex != pullBackIndex && savedIndex != pullBackIndex + 1) {
                    System.Random rnd = new System.Random();
                    savedIndex = rnd.Next(pullBackIndex, pullBackIndex + 2);
                    PlayerPrefs.SetInt("LoggingIndex", savedIndex);
					Debug.Log("rnd pullBack");
                }

				Debug.Log(savedIndex);

				if (savedIndex == pullBackIndex) {
                    pullBack = true;
                }
                loggerInstance = new CapstoneLogger(
                202006, "strandedin", "670df58df5a2ec63b0a33e054418105a", savedIndex);
                //new WaitTimer().WaitForSeconds(2f);
            }  
            return loggerInstance;
        }
    }

    public void SetUserId(string userId) {
        this.userId = userId;
    }

    public void AddCompletedLevelIndex(int index)
    {
        levelsComplete.Add(index);
    }

    public int GetNumLevelsCompleted()
    {
        return levelsComplete.Count;
    }

    public string GetUserId() {
        return userId;
    }

    public void SetGameState(GameState gameState) {
        this.gameState = gameState;
        if(OnStateChange!=null) {
            OnStateChange();
        }
    }

    public void SetGameScene(string scene) {
    	this.scene = scene;
    }

    public void SetLevelIndex(int index) {
        this.levelBuildIndex = index;
    }

    public int GetLevelBuildIndex() {
        return levelBuildIndex;
    }

    public int GetMainMenuBuildIndex() {
        return mainMenuBuildIndex;
    }
}
