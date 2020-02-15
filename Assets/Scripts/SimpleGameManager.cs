/*
Logging ActionId Glossery
1 = clicked button
2 = player completed a level
3 = player finished all levels
4 = player died
5 = starting level
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

    private static GameManager _instance = null;
    private static CapstoneLogger loggerInstance = null;
    public event OnStateChangeHandler OnStateChange;
    public GameState gameState { get; private set; }
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

    public static CapstoneLogger Logger {
        get {
            if (loggerInstance == null) {  
                loggerInstance = new CapstoneLogger(
                202006, "strandedin", "670df58df5a2ec63b0a33e054418105a", 0);

            }  
            return loggerInstance;
        }
    }

    public void SetUserId(string userId) {
        this.userId = userId;
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
