using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CountdownTimer : MonoBehaviour {

    public Text timerTxt;
    float timer, timeBonus, timeLeft, time;
    public float timeForKill, startTime, shotgunSpawnTime, akSpawnTime;
    public GameObject shotgun, ak;
	// Use this for initialization
	void Start () {
        timer = startTime;
        timeLeft = startTime;
	}
	
	// Update is called once per frame
	void Update () {
        TimerCountdown();
		time += Time.deltaTime;
		if (ak == null || shotgun == null) {
			return;
		}
		if (time > shotgunSpawnTime) {
			shotgun.SetActive (true);
		}
		if (time > akSpawnTime) {
			ak.SetActive (true);
		}
	}

    void TimerCountdown()
    {
        timer -= Time.deltaTime;
        timeBonus = GameMasterPublicVariables.killedAI * timeForKill;
        timeLeft = timer + timeBonus;
        timerTxt.text = "Time left: " + timeLeft.ToString("f2");
    }
}
