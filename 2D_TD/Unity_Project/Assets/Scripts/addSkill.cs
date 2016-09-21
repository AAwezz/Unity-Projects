using UnityEngine;
using System.Collections;

public class addSkill : MonoBehaviour
{

    public GameObject fire, freeze, lightning, mana, regen, life;
    public GUIText fireTxt, freezeTxt, lightningTxt, manaTxt, regenTxt, lifeTxt;
    public static int fireInt=1, freezeInt=1, lightningInt=1, manaInt=1, regenInt=1, lifeInt=1;

    void OnMouseDown()
    {
        if (SpawnWave.skillPoints > 0)
        {
            SpawnWave.skillPoints -= 1;
            if (gameObject == fire)
            {
                fireInt += 1;
                fireTxt.text = "" + fireInt;
                Explosion.fireDmg += 0.5f;
            }
            else if (gameObject == freeze)
            {
                freezeInt += 1;
                freezeTxt.text = "" + freezeInt;
                FreezingArea.FreezeTime += 0.5f;
            }
            else if (gameObject == lightning)
            {
                lightningInt += 1;
                lightningTxt.text = "" + lightningInt;
                ChooseSpell.lightningDmg += 2;
            }
            else if (gameObject == mana)
            {
                manaInt += 1;
                manaTxt.text = "" + manaInt;
                SpawnWave.maxMana += 20;
            }
            else if (gameObject == regen)
            {
                regenInt += 1;
                regenTxt.text = "" + regenInt;
                SpawnWave.manaRegen += 5;
            }
            else if (gameObject == life)
            {
                lifeInt += 1;
                lifeTxt.text = ""+lifeInt;
                Life.amountOfLife += 1;
            }
        }
    }
}
