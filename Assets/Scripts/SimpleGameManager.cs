using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { NullState, MainMenu, Game}
public delegate void OnStateChangeHandler();

public class GameManager {

	private string scene = " ";
    private int levelBuildIndex = 2;
    private int mainMenuBuildIndex = 0;

    private static GameManager _instance = null;
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
