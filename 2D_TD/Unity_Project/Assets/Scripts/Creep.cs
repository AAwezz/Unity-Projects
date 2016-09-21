using UnityEngine;
using System.Collections;

public class Creep : MonoBehaviour
{
    // 0.007f slow, 0.02f normal, 0.05 fast
    public float health = 1, speed = 0.02f, slow = 1;
    Vector3 flytRight = new Vector3(1, 0, 0);
    Vector3 flytDown = new Vector3(0, -1, 0);
    // Vector3 flytLeft = new Vector3(-1, 0, 0);
    Vector3 flytUp = new Vector3(0, 1, 0);
    public static bool isBossLevel = false, fireblastOff = false, freezeOff = false;
    public GameObject fireblast, freeze;

    void Start()
    {
        health = SpawnWave.levelCount;
    }

    void Update()
    {
        speed = 0.02f * slow;
        if (System.Math.Round(this.transform.position.x, 1) == -9 && System.Math.Round(this.transform.position.y, 1) != 1
            || System.Math.Round(this.transform.position.x, 1) == 2 && System.Math.Round(this.transform.position.y, 1) > -4.2)
        {
            this.transform.position += flytDown * speed;
        }
        else if (System.Math.Round(this.transform.position.x, 1) != -5 && System.Math.Round(this.transform.position.y, 1) == 1
            || System.Math.Round(this.transform.position.y, 1) == 5)
        {
            this.transform.position += flytRight * speed;
        }
        else if (System.Math.Round(this.transform.position.x, 1) == -5 && System.Math.Round(this.transform.position.y, 1) != 5)
        {
            this.transform.position += flytUp * speed;
        }
        else if (System.Math.Round(this.transform.position.x, 1) == 2 && System.Math.Round(this.transform.position.y, 1) <= -4.2)
        {
            onDeath();
            Life.amountOfLife -= 1;
        }
    }

    void OnMouseDown()
    {
        if (!isBossLevel)
        {
            if (ChooseSpell.lightningIsActive)
            {
                SpawnWave.setMana(50);
                ChooseSpell.useLightning(gameObject);
            }
            if (ChooseSpell.fireblastIsActive)
            {
                SpawnWave.setMana(50);
                fireblastOff = true;
                GameObject g = (GameObject)Instantiate(fireblast, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)), Quaternion.identity);
                g.SendMessage("explosion");
            }
            if (ChooseSpell.freezeIsActive)
            {
                SpawnWave.setMana(50);
                freezeOff = true;
                GameObject g = (GameObject)Instantiate(freeze, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)), Quaternion.identity);
                g.SendMessage("freezing");
            }
        }
    }

    void SetSpeedModifier(float slowness)
    {
        slow = slowness;
    }

    void setHealth(int i)
    {
        health = i * i / 2;
    }

    void getHealth(GameObject gObject)
    {
        gObject.SendMessage("explode", Mathf.Round(health * 0.8f));
    }

    void bossLevel(int hp)
    {
        health = hp;
        isBossLevel = true;
    }
    void setSpellsOn()
    {
        isBossLevel = false;
    }

    void ApplyDmg(int dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            onDeath();
            money.amountOfGold += SpawnWave.levelCount/10+1;
        }
    }

    public void onDeath()
    {
        Destroy(gameObject);
        SpawnWave.creepsAlive -= 1;
    }
}
