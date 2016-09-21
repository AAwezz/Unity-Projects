using UnityEngine;
using System.Collections;

public class SpawnWave : MonoBehaviour {

    public static int creepsAlive = 0, numberOfCreeps = 10, levelCount = 0,manaNumb=100,maxMana=100,skillPoints=0,manaRegen=10;
    public GameObject sphere;
    public GUIText lvl,mana,skill;
    public static GameObject[] creeps;

    void Start()
    {
        lvl.text = "0";
        mana.text = manaNumb+"/"+maxMana;
    }

    void Update()
    {
        mana.text = manaNumb + "/" + maxMana;
        skill.text = ""+skillPoints;
    }

    public static void setMana(int newMana)
    {
        manaNumb = manaNumb-newMana;
    }

    void OnMouseDown()
    {
        if (creepsAlive == 0)
        {
            levelCount += 1;
            StartCoroutine(Spawn());
            if (manaNumb + manaRegen <= maxMana)
            {
                manaNumb += manaRegen;
            }
            else
            {
                manaNumb = maxMana;
            }
            skillPoints += 1;
            lvl.text = ""+levelCount;
        }
    }

    IEnumerator Spawn()
    {
        if (levelCount % 10 == 0)
        {
            numberOfCreeps = 1;
        }
        else
        {
            numberOfCreeps = 10;
        }
        creeps = new GameObject[numberOfCreeps];
        for (int i = 0; i < numberOfCreeps; i++)
        {
            creepsAlive += 1;
            creeps[i] = (GameObject)Instantiate(sphere);
            creeps[i].SendMessage("setSpellsOn");
            if (levelCount % 10 == 0)
            {
                creeps[i].SendMessage("bossLevel", levelCount * 6);
            }
            else if (levelCount % 7 == 0 && levelCount > 10 && levelCount % 10 != 0 && levelCount % 5 != 0)
            {
                creeps[i].SendMessage("SetSpeedModifier", 2);
            }
            else if (levelCount % 5 == 0 && levelCount > 10 && levelCount % 10 != 0)
            {
                creeps[i].SendMessage("setHealth", levelCount * 1.5f);
            }
            yield return new WaitForSeconds(0.5f);
        } 
    }
}
