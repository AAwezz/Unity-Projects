using UnityEngine;
using System.Collections;

public class Shooting : MonoBehaviour
{
    public GameObject s, start, field;
    float timeLeft = 0.0f;
    public float range = 2.0f, interval = 1, dmg = 1, speed = 10;
    public int towerlevel = 1;

    void UpgradeTower()
    {
        range += 0.15f;
        interval -= 0.08f;
        towerlevel += 1;
        speed += 0.05f;
        dmg += 1;
    }

    GameObject findClosestTarget()
    {
        GameObject closest = null;

        for (int i = 0; i < SpawnWave.numberOfCreeps; i++)
        {
            if (SpawnWave.creeps != null)
            {
                if (SpawnWave.creeps[i] != null && Vector3.Distance(transform.position, SpawnWave.creeps[i].transform.position) <= range)
                {
                    closest = SpawnWave.creeps[i];
                    break;
                }
            }
        }
        return closest;
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0.0f)
        {
            GameObject target = findClosestTarget();
            if (target != null)
            {
                if (Vector3.Distance(transform.position, target.transform.position) <= range)
                {
                    GameObject g = (GameObject)Instantiate(s, transform.position, Quaternion.identity);
                    aShoot b = g.GetComponent<aShoot>();
                    b.speed = speed;
                    b.dmg = dmg;
                    b.setDestination(target.transform);
                    timeLeft = interval;
                }
            }
        }
    }
}
