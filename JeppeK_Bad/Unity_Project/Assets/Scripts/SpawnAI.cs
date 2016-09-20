using UnityEngine;
using System.Collections;

public class SpawnAI : MonoBehaviour {
    public GameObject AI;
    public int startEnemies = 10;
    public float enemyIncreaseTimer = 10.0f;

    int x, z, numberOfAIsStart = 0, desiredNumberOfAIs = 0;
    private float timeElapsed = 0;
    private GameObject player;
    private Vector3 playerPos;
	// Use this for initialization
	void Start () {

        player = GameObject.FindGameObjectWithTag("Player");

        desiredNumberOfAIs = startEnemies;

        while (numberOfAIsStart != desiredNumberOfAIs)
        {
            StartCoroutine("SpawnOneAICou");
        }
	}
	
	// Update is called once per frame
	void Update () {
        playerPos = player.transform.position;

        int numberOfAis = GameMasterPublicVariables.spawnedAI - GameMasterPublicVariables.killedAI;

        timeElapsed += Time.deltaTime;

        if (timeElapsed > enemyIncreaseTimer) {
            desiredNumberOfAIs += 1;
            timeElapsed = 0;
        }

        if (numberOfAis < desiredNumberOfAIs)
        {
            StartCoroutine("SpawnOneAICou");
        }
	}

    void SpawnOneAI()
    {
        x = Random.Range(-45, 45);
        z = Random.Range(25, -65);
        if (x > playerPos.x + 15 || x < playerPos.x - 15 && z > playerPos.z + 15 || z < playerPos.z - 15)
        {
            AI.transform.position = new Vector3(x, 1, z);
            Instantiate(AI);
            numberOfAIsStart++;
            GameMasterPublicVariables.spawnedAI += 1;
        }
    }

    IEnumerator SpawnOneAICou()
    {
        SpawnOneAI();
        yield return null;
    }
}
