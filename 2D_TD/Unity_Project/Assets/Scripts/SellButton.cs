using UnityEngine;
using System.Collections;

public class SellButton : MonoBehaviour {

    public GameObject thisTower;
    public int moneyback;

    void OnMouseDown()
    {
        Shooting field = thisTower.GetComponent<Shooting>();
        ChooseField cField = field.field.GetComponent<ChooseField>();
        cField.taken = false;
        money.amountOfGold += (moneyback+10)/2;
        Destroy(thisTower);
    }
}
