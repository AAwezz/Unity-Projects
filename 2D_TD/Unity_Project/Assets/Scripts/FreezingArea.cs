using UnityEngine;
using System.Collections;

public class FreezingArea : MonoBehaviour {
    public static float FreezeTime = 3;
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "creep" && !Creep.isBossLevel)
        {
            other.SendMessage("SetSpeedModifier", 0.5);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "creep" && !Creep.isBossLevel)
        {
            other.SendMessage("SetSpeedModifier", 1);
        }
    }

    void freezing()
    {
        StartCoroutine(SpawnFreeze());
    }

    IEnumerator SpawnFreeze()
    {  
        yield return new WaitForSeconds(FreezeTime);
        if (SpawnWave.creeps != null)
        {
            for (int i = 0; i < SpawnWave.creeps.Length; i++)
            {
                if (SpawnWave.creeps[i] != null)
                {
                    SpawnWave.creeps[i].SendMessage("SetSpeedModifier", 1);
                }
            }
        }
        Destroy(gameObject);
    }
}
