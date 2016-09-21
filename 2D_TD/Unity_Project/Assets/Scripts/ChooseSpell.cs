using UnityEngine;
using System.Collections;

public class ChooseSpell : MonoBehaviour {

    public static bool lightningIsActive=false,fireblastIsActive=false,freezeIsActive=false;
    public static int chainLeft = 0, start=0, lightningDmg=5;
    public GameObject fireblastArea,fire,freezeArea,freeze,lightnings;

    void Update()
    {
        if (Creep.fireblastOff)
        {
            fireblastArea.SetActive(false);
            fireblastIsActive = !fireblastIsActive;
            Creep.fireblastOff = false;
        }
        else if (Creep.freezeOff)
        {
            freezeArea.SetActive(false);
            freezeIsActive = !freezeIsActive;
            Creep.freezeOff = false;
        }
    }

    void OnMouseDown()
    {
        if (!Creep.isBossLevel && SpawnWave.manaNumb - 50 >= 0)
        {
            if (gameObject == lightnings)
            {
                lightningIsActive = !lightningIsActive;
                if (lightningIsActive)
                {
                    fireblastIsActive = false;
                    freezeIsActive = false;
                    print("Lightning is active");
                }
                else
                {
                    print("Lightning is deactiveted");
                }
            }
            else if (gameObject == fire)
            {
                fireblastIsActive = !fireblastIsActive;
                if (fireblastIsActive)
                {
                    lightningIsActive = false;
                    freezeIsActive = false;
                    fireblastArea.SetActive(true);
                    print("Fireblast is active");
                }
                else
                {
                    fireblastArea.SetActive(false);
                    print("Fireblast is deactiveted");
                }
            }
            else if (gameObject == freeze)
            {
                freezeIsActive = !freezeIsActive;
                if (freezeIsActive)
                {
                    lightningIsActive = false;
                    fireblastIsActive = false;
                    freezeArea.SetActive(true);
                    print("Freeze is active");
                }
                else
                {
                    freezeArea.SetActive(false);
                    print("Freeze is deactiveted");
                }
            }
        }
    }

    public static void useLightning(GameObject gObject){
        for(int i=0;i<SpawnWave.creeps.Length;i++){
            if (SpawnWave.creeps[i] == gObject)
            {
                SpawnWave.creeps[i].SendMessage("ApplyDmg", lightningDmg);
                chainLeft = 4;
                start = i;
                lightningIsActive = !lightningIsActive;
            }
            if (i > start && chainLeft>0)
            {
                if (SpawnWave.creeps[i] != null)
                {
                    chainLeft -= 1;
                    SpawnWave.creeps[i].SendMessage("ApplyDmg", lightningDmg);
                }
            }
        }
    }
}
