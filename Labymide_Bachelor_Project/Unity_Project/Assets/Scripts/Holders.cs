using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * This is the script for all the holders so that you can
 * move a wall to a new holder position.
 */

public class Holders : MonoBehaviour
{
    public static GameObject[] avaliableHoldersArray, holderBufArray;
    public static int avaliableHoldersCount, holderCount;
    public static bool isAvaliable = true, wallMoved = true, haveWall = false, nextTurn = false, once = true;
    static string prevHoldersName = "";

    /* 
     * The OnMouseDown method tell which holder to move the choosen wall to,
     * it checks if anyone have won
     * and changes player turn.
     */
    void OnMouseDown()
    {
        if (!wallMoved && ServerHub.playerNames[MainScript.playerTurn] == NetworkManager.showName)
        {
            for (int i = 0; i < avaliableHoldersCount; i++)
            {
                if (this.gameObject == avaliableHoldersArray[i])
                {
                    MoveWall();
                    GetComponent<NetworkView>().RPC("ReconfigureWallArrays", RPCMode.AllBuffered, this.name, prevHoldersName);
                    if (Network.isServer)
                        CheckWinState(true);
                    if (Network.isClient)
                        GetComponent<NetworkView>().RPC("CheckWinState", RPCMode.Server, true);
                    do
                    {
                        MainScript.ChangingPlayerTurn();
                    } while (ServerHub.playerNames[MainScript.playerTurn] == null);
                    GetComponent<NetworkView>().RPC("NextTurn", RPCMode.AllBuffered, MainScript.playerTurn);
                }
            }
        }
    }

    /* 
     * This method moves the wall to the chosen position and change the wallOnHolder array.
     */
    void MoveWall()
    {
        for (int j = 0; j < MainScript.allHoldersArray.Length; j++)
        {
            if (Wall.wall != null && Wall.wall.transform.position.x == MainScript.allHoldersArray[j].transform.position.x && Wall.wall.transform.position.z == MainScript.allHoldersArray[j].transform.position.z)
            {
                GetComponent<NetworkView>().RPC("ChangeHolderBool", RPCMode.AllBuffered, j, false);
            }
        }
        GetComponent<NetworkView>().RPC("SetWallPosition", RPCMode.AllBuffered, new Vector3(this.transform.position.x, 0.625f, this.transform.position.z));
    }

    /* 
     * This RPC method sets the position of the wall to where you have pressed on a avaliable holder
     * It also sets its wallOnHolder, to the given position, to true.
     */
    [RPC]
    void SetWallPosition(Vector3 pos)
    {
        if (Wall.wall != null)
        {
            Wall.wall.transform.position = pos;
            if (Wall.wall.transform.position.x % 2 == 0)
            {
                Wall.wall.transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else
            {
                Wall.wall.transform.eulerAngles = new Vector3(0, 90, 0);
            }
            for (int k = 0; k < MainScript.allHoldersArray.Length; k++)
            {
                if (Wall.wall.transform.position.x == MainScript.allHoldersArray[k].transform.position.x && Wall.wall.transform.position.z == MainScript.allHoldersArray[k].transform.position.z)
                {
                    GetComponent<NetworkView>().RPC("ChangeHolderBool", RPCMode.AllBuffered, k, true);
                }
            }
            MainScript.ClearHolderColor();
            MainScript.ClearWallColor();
            MainScript.ClearFieldsColor();
            if (Player.colorOn)
            {
                Player.ColorFields(Player.player);
            }
            Dice.dicedRolled = true;
            wallMoved = true;
        }
    }

    /* 
     * A RPC method that changes a certain boolean the wallOnHolder array.
     */
    [RPC]
    void ChangeHolderBool(int i, bool b)
    {
        MainScript.wallOnHolder[i] = b;
    }

    /* 
     * A RPC method that changes playerTurn
     */
    [RPC]
    void NextTurn(int i)
    {
        MainScript.playerTurn = i;
        nextTurn = true;
        Wall.wall = null;
        avaliableHoldersArray = new GameObject[7];
        avaliableHoldersCount = 0;
    }

    /* 
     * A RPC method checks if any player have won, after the wall have been moved.
     */
    [RPC]
    void CheckWinState(bool win)
    {
        WinState.WinningState((int)GameObject.Find("WinnerField").transform.position.x, (int)GameObject.Find("WinnerField").transform.position.z);
        WinState.check = win;
    }

    /* 
     * method configures our die array's and arrayNumb's.
     */
    [RPC]
    public void ReconfigureWallArrays(string newHolderName, string prevHolderName)
    {
        prevHoldersName = prevHolderName;
        switch (newHolderName)
        {
            case "HolderOne":
                if (prevHoldersName == "HolderOneTwo")
                {
                    MainScript.dieTwoArray = MainScript.RemoveWallFromArray(MainScript.dieTwoArray, MainScript.dieTwoArrayNumb, Wall.wall);
                    MainScript.dieTwoArrayNumb--;
                }
                break;
            // Depending on which holder the wall came from, and is going to, we need to delete it from its 
            // previous die array and add it to the new die array.
            case "HolderOneTwo":
                if (prevHoldersName == "HolderOne")
                {
                    MainScript.AddWallToArray(MainScript.dieTwoArray, MainScript.dieTwoArrayNumb, Wall.wall);
                    MainScript.dieTwoArrayNumb++;
                }
                else if (prevHoldersName == "HolderTwo")
                {
                    MainScript.AddWallToArray(MainScript.dieOneArray, MainScript.dieOneArrayNumb, Wall.wall);
                    MainScript.dieOneArrayNumb++;
                }
                else if (prevHoldersName == "HolderTwoThree")
                {
                    MainScript.dieThreeArray = MainScript.RemoveWallFromArray(MainScript.dieThreeArray, MainScript.dieThreeArrayNumb, Wall.wall);
                    MainScript.dieThreeArrayNumb--;
                    MainScript.AddWallToArray(MainScript.dieOneArray, MainScript.dieOneArrayNumb, Wall.wall);
                    MainScript.dieOneArrayNumb++;
                }
                break;
            case "HolderTwo":
                if (prevHoldersName == "HolderOneTwo")
                {
                    MainScript.dieOneArray = MainScript.RemoveWallFromArray(MainScript.dieOneArray, MainScript.dieOneArrayNumb, Wall.wall);
                    MainScript.dieOneArrayNumb--;
                }
                else if (prevHoldersName == "HolderTwoThree")
                {
                    MainScript.dieThreeArray = MainScript.RemoveWallFromArray(MainScript.dieThreeArray, MainScript.dieThreeArrayNumb, Wall.wall);
                    MainScript.dieThreeArrayNumb--;
                }
                break;
            case "HolderTwoThree":
                if (prevHoldersName == "HolderTwo")
                {
                    MainScript.AddWallToArray(MainScript.dieThreeArray, MainScript.dieThreeArrayNumb, Wall.wall);
                    MainScript.dieThreeArrayNumb++;
                }
                else if (prevHoldersName == "HolderThree")
                {
                    MainScript.AddWallToArray(MainScript.dieTwoArray, MainScript.dieTwoArrayNumb, Wall.wall);
                    MainScript.dieTwoArrayNumb++;
                }
                else if (prevHoldersName == "HolderThreeFour")
                {
                    MainScript.dieFourArray = MainScript.RemoveWallFromArray(MainScript.dieFourArray, MainScript.dieFourArrayNumb, Wall.wall);
                    MainScript.dieFourArrayNumb--;
                    MainScript.AddWallToArray(MainScript.dieTwoArray, MainScript.dieTwoArrayNumb, Wall.wall);
                    MainScript.dieTwoArrayNumb++;
                }
                else if (prevHoldersName == "HolderOneTwo")
                {
                    MainScript.dieOneArray = MainScript.RemoveWallFromArray(MainScript.dieOneArray, MainScript.dieOneArrayNumb, Wall.wall);
                    MainScript.dieOneArrayNumb--;
                    MainScript.AddWallToArray(MainScript.dieThreeArray, MainScript.dieThreeArrayNumb, Wall.wall);
                    MainScript.dieThreeArrayNumb++;
                }
                break;
            case "HolderThree":
                if (prevHoldersName == "HolderThreeFour")
                {
                    MainScript.dieFourArray = MainScript.RemoveWallFromArray(MainScript.dieFourArray, MainScript.dieFourArrayNumb, Wall.wall);
                    MainScript.dieFourArrayNumb--;
                }
                else if (prevHoldersName == "HolderTwoThree")
                {
                    MainScript.dieTwoArray = MainScript.RemoveWallFromArray(MainScript.dieTwoArray, MainScript.dieTwoArrayNumb, Wall.wall);
                    MainScript.dieTwoArrayNumb--;
                }
                break;
            case "HolderThreeFour":
                if (prevHoldersName == "HolderThree")
                {
                    MainScript.AddWallToArray(MainScript.dieFourArray, MainScript.dieFourArrayNumb, Wall.wall);
                    MainScript.dieFourArrayNumb++;
                }
                else if (prevHoldersName == "HolderFour")
                {
                    MainScript.AddWallToArray(MainScript.dieThreeArray, MainScript.dieThreeArrayNumb, Wall.wall);
                    MainScript.dieThreeArrayNumb++;
                }
                else if (prevHoldersName == "HolderFourFive")
                {
                    MainScript.dieFiveArray = MainScript.RemoveWallFromArray(MainScript.dieFiveArray, MainScript.dieFiveArrayNumb, Wall.wall);
                    MainScript.dieFiveArrayNumb--;
                    MainScript.AddWallToArray(MainScript.dieThreeArray, MainScript.dieThreeArrayNumb, Wall.wall);
                    MainScript.dieThreeArrayNumb++;
                }
                else if (prevHoldersName == "HolderTwoThree")
                {
                    MainScript.dieTwoArray = MainScript.RemoveWallFromArray(MainScript.dieTwoArray, MainScript.dieTwoArrayNumb, Wall.wall);
                    MainScript.dieTwoArrayNumb--;
                    MainScript.AddWallToArray(MainScript.dieFourArray, MainScript.dieFourArrayNumb, Wall.wall);
                    MainScript.dieFourArrayNumb++;
                }
                break;
            case "HolderFour":
                if (prevHoldersName == "HolderThreeFour")
                {
                    MainScript.dieThreeArray = MainScript.RemoveWallFromArray(MainScript.dieThreeArray, MainScript.dieThreeArrayNumb, Wall.wall);
                    MainScript.dieThreeArrayNumb--;
                }
                else if (prevHoldersName == "HolderFourFive")
                {
                    MainScript.dieFiveArray = MainScript.RemoveWallFromArray(MainScript.dieFiveArray, MainScript.dieFiveArrayNumb, Wall.wall);
                    MainScript.dieFiveArrayNumb--;
                }
                break;
            case "HolderFourFive":
                if (prevHoldersName == "HolderFour")
                {
                    MainScript.AddWallToArray(MainScript.dieFiveArray, MainScript.dieFiveArrayNumb, Wall.wall);
                    MainScript.dieFiveArrayNumb++;
                }
                else if (prevHoldersName == "HolderFive")
                {
                    MainScript.AddWallToArray(MainScript.dieFourArray, MainScript.dieFourArrayNumb, Wall.wall);
                    MainScript.dieFourArrayNumb++;
                }
                else if (prevHoldersName == "HolderFiveSix")
                {
                    MainScript.dieSixArray = MainScript.RemoveWallFromArray(MainScript.dieSixArray, MainScript.dieSixArrayNumb, Wall.wall);
                    MainScript.dieSixArrayNumb--;
                    MainScript.AddWallToArray(MainScript.dieFourArray, MainScript.dieFourArrayNumb, Wall.wall);
                    MainScript.dieFourArrayNumb++;
                }
                else if (prevHoldersName == "HolderThreeFour")
                {
                    MainScript.dieThreeArray = MainScript.RemoveWallFromArray(MainScript.dieThreeArray, MainScript.dieThreeArrayNumb, Wall.wall);
                    MainScript.dieThreeArrayNumb--;
                    MainScript.AddWallToArray(MainScript.dieFiveArray, MainScript.dieFiveArrayNumb, Wall.wall);
                    MainScript.dieFiveArrayNumb++;
                }
                break;
            case "HolderFive":
                if (prevHoldersName == "HolderFourFive")
                {
                    MainScript.dieFourArray = MainScript.RemoveWallFromArray(MainScript.dieFourArray, MainScript.dieFourArrayNumb, Wall.wall);
                    MainScript.dieFourArrayNumb--;
                }
                else if (prevHoldersName == "HolderFiveSix")
                {
                    MainScript.dieSixArray = MainScript.RemoveWallFromArray(MainScript.dieSixArray, MainScript.dieSixArrayNumb, Wall.wall);
                    MainScript.dieSixArrayNumb--;
                }
                break;
            case "HolderFiveSix":
                if (prevHoldersName == "HolderFive")
                {
                    MainScript.AddWallToArray(MainScript.dieSixArray, MainScript.dieSixArrayNumb, Wall.wall);
                    MainScript.dieSixArrayNumb++;
                }
                else if (prevHoldersName == "HolderSix")
                {
                    MainScript.AddWallToArray(MainScript.dieFiveArray, MainScript.dieFiveArrayNumb, Wall.wall);
                    MainScript.dieFiveArrayNumb++;
                }
                else if (prevHoldersName == "HolderFourFive")
                {
                    MainScript.dieFourArray = MainScript.RemoveWallFromArray(MainScript.dieFourArray, MainScript.dieFourArrayNumb, Wall.wall);
                    MainScript.dieFourArrayNumb--;
                    MainScript.AddWallToArray(MainScript.dieSixArray, MainScript.dieSixArrayNumb, Wall.wall);
                    MainScript.dieSixArrayNumb++;
                }
                break;
            case "HolderSix":
                if (prevHoldersName == "HolderFiveSix")
                {
                    MainScript.dieFiveArray = MainScript.RemoveWallFromArray(MainScript.dieFiveArray, MainScript.dieFiveArrayNumb, Wall.wall);
                    MainScript.dieFiveArrayNumb--;
                }
                break;
        }
    }

    /* 
     * These last methods finds the avaliable holders where you can move the wall too.
     */
    public static void PlaceForWalls(int x, int z)
    {
        holderBufArray = new GameObject[7];
        avaliableHoldersArray = new GameObject[7];
        avaliableHoldersCount = 0;
        holderCount = 0;
        if (Mathf.Abs(x) % 2 == 0)
        {
            HorizontalHolders(x, z);
        }
        else
        {
            VerticalHolders(x, z);
        }
        AvaliableHolders(x, z);
    }

    static void AvaliableHolders(int x, int z)
    {
        for (int i = 0; i < holderCount; i++)
        {
            if (x == holderBufArray[i].transform.position.x && z == holderBufArray[i].transform.position.z)
            {
                prevHoldersName = holderBufArray[i].name;
            }
        }
        for (int i = 0; i < holderCount; i++)
        {
            switch (Dice.sideOnDice)
            {
                case 1:
                    if (holderBufArray[i].name == "HolderOne" || (holderBufArray[i].name == "HolderOneTwo" && prevHoldersName != "HolderOneTwo"))
                    {
                        avaliableHoldersArray[avaliableHoldersCount] = holderBufArray[i];
                        avaliableHoldersCount++;
                    }
                    break;
                case 2:
                    if (holderBufArray[i].name == "HolderTwo" || holderBufArray[i].name == "HolderOneTwo" || (holderBufArray[i].name == "HolderTwoThree" && prevHoldersName != "HolderTwoThree"))
                    {
                        avaliableHoldersArray[avaliableHoldersCount] = holderBufArray[i];
                        avaliableHoldersCount++;
                    }
                    break;
                case 3:
                    if (holderBufArray[i].name == "HolderThree" || holderBufArray[i].name == "HolderTwoThree" || (holderBufArray[i].name == "HolderThreeFour" && prevHoldersName != "HolderThreeFour"))
                    {
                        avaliableHoldersArray[avaliableHoldersCount] = holderBufArray[i];
                        avaliableHoldersCount++;
                    }
                    break;
                case 4:
                    if (holderBufArray[i].name == "HolderFour" || holderBufArray[i].name == "HolderThreeFour" || (holderBufArray[i].name == "HolderFourFive" && prevHoldersName != "HolderFourFive"))
                    {
                        avaliableHoldersArray[avaliableHoldersCount] = holderBufArray[i];
                        avaliableHoldersCount++;
                    }
                    break;
                case 5:
                    if (holderBufArray[i].name == "HolderFive" || holderBufArray[i].name == "HolderFourFive" || (holderBufArray[i].name == "HolderFiveSix" && prevHoldersName != "HolderFiveSix"))
                    {
                        avaliableHoldersArray[avaliableHoldersCount] = holderBufArray[i];
                        avaliableHoldersCount++;
                    }
                    break;
                case 6:
                    if (holderBufArray[i].name == "HolderSix" || holderBufArray[i].name == "HolderFiveSix")
                    {
                        avaliableHoldersArray[avaliableHoldersCount] = holderBufArray[i];
                        avaliableHoldersCount++;
                    }
                    break;
            }
        }
        for (int i = 0; i < avaliableHoldersCount; i++)
        {
            avaliableHoldersArray[i].GetComponent<Renderer>().material.color = Color.cyan;
        }
    }

    static void VerticalHolders(int x, int z)
    {
        for (int i = 0; i < MainScript.allHoldersArray.Length; i++)
        {
            if (x == MainScript.allHoldersArray[i].transform.position.x && z - 2 == MainScript.allHoldersArray[i].transform.position.z ||
                x - 1 == MainScript.allHoldersArray[i].transform.position.x && z + 1 == MainScript.allHoldersArray[i].transform.position.z ||
                x - 1 == MainScript.allHoldersArray[i].transform.position.x && z - 1 == MainScript.allHoldersArray[i].transform.position.z ||
                x + 1 == MainScript.allHoldersArray[i].transform.position.x && z + 1 == MainScript.allHoldersArray[i].transform.position.z ||
                x + 1 == MainScript.allHoldersArray[i].transform.position.x && z - 1 == MainScript.allHoldersArray[i].transform.position.z ||
                x == MainScript.allHoldersArray[i].transform.position.x && z + 2 == MainScript.allHoldersArray[i].transform.position.z ||
                x == MainScript.allHoldersArray[i].transform.position.x && z == MainScript.allHoldersArray[i].transform.position.z)
            {
                holderBufArray[holderCount] = MainScript.allHoldersArray[i];
                holderCount++;
            }
        }
    }

    static void HorizontalHolders(int x, int z)
    {
        for (int i = 0; i < MainScript.allHoldersArray.Length; i++)
        {
            if (x - 2 == MainScript.allHoldersArray[i].transform.position.x && z == MainScript.allHoldersArray[i].transform.position.z ||
                x - 1 == MainScript.allHoldersArray[i].transform.position.x && z + 1 == MainScript.allHoldersArray[i].transform.position.z ||
                x - 1 == MainScript.allHoldersArray[i].transform.position.x && z - 1 == MainScript.allHoldersArray[i].transform.position.z ||
                x + 1 == MainScript.allHoldersArray[i].transform.position.x && z + 1 == MainScript.allHoldersArray[i].transform.position.z ||
                x + 1 == MainScript.allHoldersArray[i].transform.position.x && z - 1 == MainScript.allHoldersArray[i].transform.position.z ||
                x + 2 == MainScript.allHoldersArray[i].transform.position.x && z == MainScript.allHoldersArray[i].transform.position.z ||
                x == MainScript.allHoldersArray[i].transform.position.x && z == MainScript.allHoldersArray[i].transform.position.z)
            {
                holderBufArray[holderCount] = MainScript.allHoldersArray[i];
                holderCount++;
            }
        }
    }
}