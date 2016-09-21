using UnityEngine;

/*
 * This script is used to be able to choose which wall should be moved.
 */
public class Wall : MonoBehaviour
{
    public static GameObject wall;
    string lastName;

    /*
     * This method makes it possible to press the colored walls avaliable to move.
     */
    void OnMouseDown()
    {
        if (!Holders.wallMoved && ServerHub.playerNames[MainScript.playerTurn] == NetworkManager.showName)
        {
            switch (Dice.sideOnDice)
            {
                case 1: ClickingOnWall(MainScript.dieOneArray, MainScript.dieOneArrayNumb);
                    break;
                case 2: ClickingOnWall(MainScript.dieTwoArray, MainScript.dieTwoArrayNumb);
                    break;
                case 3: ClickingOnWall(MainScript.dieThreeArray, MainScript.dieThreeArrayNumb);
                    break;
                case 4: ClickingOnWall(MainScript.dieFourArray, MainScript.dieFourArrayNumb);
                    break;
                case 5: ClickingOnWall(MainScript.dieFiveArray, MainScript.dieFiveArrayNumb);
                    break;
                case 6: ClickingOnWall(MainScript.dieSixArray, MainScript.dieSixArrayNumb);
                    break;
            }
        }
    }

    /*
     * These two methods colors the selected wall red.
     */
    void ClickingOnWall(GameObject[] Array, int numbOfEleInArray)
    {
        for (int j = 0; j < numbOfEleInArray; j++)
        {
            if (this.name == Array[j].name)
            {
                ColorMoveWall(Array, numbOfEleInArray);
            }
        }
    }

    void ColorMoveWall(GameObject[] Array, int numbOfEleInArray)
    {
        MainScript.ClearHolderColor();
        if (this.GetComponent<Renderer>().material.color == Color.red)
        {
            MainScript.AvaliableWalls();
            Holders.avaliableHoldersArray = new GameObject[7];
            GetComponent<NetworkView>().RPC("ResetWallGameObject", RPCMode.AllBuffered, lastName);
        }
        else
        {
            for (int i = 0; i < numbOfEleInArray; i++)
            {
                Array[i].GetComponent<Renderer>().material.color = Color.cyan;
                if (this.name == Array[i].name)
                {
                    this.GetComponent<Renderer>().material.color = Color.red;
                    GetComponent<NetworkView>().RPC("WallGameObject", RPCMode.AllBuffered, this.name);
                    lastName = this.name;
                    Holders.PlaceForWalls((int)this.transform.position.x, (int)this.transform.position.z);
                }
            }
        }
    }

    /*
     * These two RPC calls lets the other players see which
     * wall is moved.
     */
    [RPC]
    void WallGameObject(string name)
    {
        wall = GameObject.Find(name);
    }

    [RPC]
    void ResetWallGameObject(string name)
    {
        lastName = name;
        wall = null;
    }
}