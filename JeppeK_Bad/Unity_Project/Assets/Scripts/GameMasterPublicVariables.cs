using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameMasterPublicVariables : MonoBehaviour {

    public static bool EnemyHalfSpeed = false;
    public Text killCount;
    public static int killedAI;
    public static int spawnedAI = 0;

	// Use this for initialization
	void Start () {
        killedAI = 0;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void Update () {
        killCount.text = "Kills: " + killedAI;
	}

    public void restartLevel()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}
