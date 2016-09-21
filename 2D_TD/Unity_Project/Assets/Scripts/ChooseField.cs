using UnityEngine;
using System.Collections;

public class ChooseField : MonoBehaviour {

    public bool taken = false;
    public GameObject tower, towerBuf;

    void OnMouseEnter()
    {
        if (BuildTower.selected)
        {
            towerBuf.SetActive(true);
            towerBuf.transform.position = transform.position;
        }
    }

    void OnMouseExit()
    {
        if (BuildTower.selected)
        {
            towerBuf.SetActive(false);
        }
    }

    void OnMouseDown()
    {
        if (BuildTower.selected && !taken)
        {
            Instantiate(tower, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
            towerBuf.SetActive(false);
            BuildTower.selected = false;
            Shooting t = tower.GetComponent<Shooting>();
            t.field = gameObject;
            taken = true;
            money.amountOfGold -= 10;
        }
    }
}
