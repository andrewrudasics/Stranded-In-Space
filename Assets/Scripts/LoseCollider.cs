using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseCollider : MonoBehaviour
{
	GameManager gm;

	// Start is called before the first frame update
	void Start()
    {
		gm = GameManager.Instance;
		gm.SetGameState(GameState.Game);
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			int deaths = PlayerPrefs.GetInt("DeathCount", -1);
            if (deaths == -1) {
                PlayerPrefs.SetInt("DeathCount", 1);
            } else {
                PlayerPrefs.SetInt("DeathCount", deaths + 1);
            }
			gm.died = true;
			GameManager.Logger.LogLevelAction(300 + (gm.GetLevelBuildIndex() - 1), "Level " + (gm.GetLevelBuildIndex() - 1) + ": Player died in space");
			SceneManager.LoadScene("Scenes/NonLevelScenes/DeathScene");
			gm.SetGameScene(SceneManager.GetActiveScene().name);
		}
		Destroy(collision.gameObject);
	}
}
