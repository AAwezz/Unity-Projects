using UnityEngine;
using System.Collections;

public class Board : MonoBehaviour {

    public GameObject freezeArea, freezeAreaBuffer, fireblastArea, fireblastAreaBuffer;

    void OnMouseDown()
    {
        if (ChooseSpell.freezeIsActive){
            SpawnWave.setMana(50);
            freezeAreaBuffer.SetActive(false);
            ChooseSpell.freezeIsActive = false;
            GameObject g = (GameObject)Instantiate(freezeArea, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)), Quaternion.identity);
            g.SendMessage("freezing");
        } 
        if(ChooseSpell.fireblastIsActive)
        {
            SpawnWave.setMana(50);
            fireblastAreaBuffer.SetActive(false);
            ChooseSpell.fireblastIsActive = false;
            GameObject g = (GameObject)Instantiate(fireblastArea, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)), Quaternion.identity);
            g.SendMessage("explosion");
        }
    }
}
