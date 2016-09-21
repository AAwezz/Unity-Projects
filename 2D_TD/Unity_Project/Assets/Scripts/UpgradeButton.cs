using UnityEngine;
using System.Collections;

public class UpgradeButton : MonoBehaviour {

    public GameObject thisTower, thisRange, thisSell;
    int price = 0;

    void OnMouseDown()
    {
        Shooting s  = thisTower.GetComponent<Shooting>();
        SellButton sellButton = thisSell.GetComponent<SellButton>();
        price = 10 * s.towerlevel+10*s.towerlevel/2;
        if (money.amountOfGold >= price && s.towerlevel <= 10)
        {
            money.amountOfGold -= price;
            sellButton.moneyback += price;
            thisTower.SendMessage("UpgradeTower");
            thisRange.SendMessage("changeRange");
        }
    }
}
