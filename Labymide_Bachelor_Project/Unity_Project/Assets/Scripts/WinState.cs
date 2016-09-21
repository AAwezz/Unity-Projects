using UnityEngine;
using System.Collections;

/*
 * This script is used to check if any of the players have won after
 * each time a wall have been moved.
 */

public class WinState : MonoBehaviour
{

    public static bool[] winner = new bool[4];
    public static bool check = false;
    static GameObject Red, Yellow, Green, Blue;
    public GUITexture winFlagRed, winFlagYellow, winFlagGreen, winFlagBlue;
    public GUIText winnerText;

    void Start()
    {
        GetComponent<NetworkView>().RPC("ShowWinningFlag", RPCMode.AllBuffered, winner);
        Red = GameObject.Find("RedStart");
        Yellow = GameObject.Find("YellowStart");
        Green = GameObject.Find("GreenStart");
        Blue = GameObject.Find("BlueStart");
        winnerText.enabled = false;
    }

    void Update()
    {
        if (check)
        {
            GetComponent<NetworkView>().RPC("ShowWinningFlag", RPCMode.AllBuffered, winner);
            if (winner[0] && !winner[1] && !winner[2] && !winner[3] && ServerHub.playerNames[0] != "")
            {
                GetComponent<NetworkView>().RPC("EndGame", RPCMode.AllBuffered, false, true, 0);
            }
            if (!winner[0] && winner[1] && !winner[2] && !winner[3] && ServerHub.playerNames[1] != "")
            {
                GetComponent<NetworkView>().RPC("EndGame", RPCMode.AllBuffered, false, true, 1);
            }
            if (!winner[0] && !winner[1] && winner[2] && !winner[3] && ServerHub.playerNames[2] != "")
            {
                GetComponent<NetworkView>().RPC("EndGame", RPCMode.AllBuffered, false, true, 2);
            }
            if (!winner[0] && !winner[1] && !winner[2] && winner[3] && ServerHub.playerNames[3] != "")
            {
                GetComponent<NetworkView>().RPC("EndGame", RPCMode.AllBuffered, false, true, 3);
            }
            winner[0] = false;
            winner[1] = false;
            winner[2] = false;
            winner[3] = false;

            for (int i = 0; i < MainScript.fieldChecked.Length; i++)
            {
                MainScript.fieldChecked[i] = false;
            }
            check = false;
        }
    }

    /*
     * This RPC call disables all the players from rolling the dice and shows the winner.
     */
    [RPC]
    void EndGame(bool f, bool t, int winNumb)
    {
        Dice.dicedRolled = f;
        winnerText.text = ServerHub.playerNames[winNumb] + " Wins The Game";
        switch (winNumb)
        {
            case 0:
                winnerText.color = Color.red;
                Player.ColorFields(GameObject.Find("RedStart"));
                break;
            case 1:
                winnerText.color = Color.yellow;
                Player.ColorFields(GameObject.Find("YellowStart"));
                break;
            case 2:
                winnerText.color = Color.green;
                Player.ColorFields(GameObject.Find("GreenStart"));
                break;
            case 3:
                winnerText.color = Color.blue;
                Player.ColorFields(GameObject.Find("BlueStart"));
                break;
        }
        winnerText.enabled = t;
    }

    /*
     * This RPC call shows which players have a path to the win field.
     */
    [RPC]
    void ShowWinningFlag(bool[] win)
    {
        if (win[0])
            winFlagRed.enabled = true;
        else
            winFlagRed.enabled = false;
        if (win[1])
            winFlagYellow.enabled = true;
        else
            winFlagYellow.enabled = false;
        if (win[2])
            winFlagGreen.enabled = true;
        else
            winFlagGreen.enabled = false;
        if (win[3])
            winFlagBlue.enabled = true;
        else
            winFlagBlue.enabled = false;
    }

    /*
     * This method tries to make a path from the winnerField to all four player startfields.
     */
    public static void WinningState(int x, int z)
    {
        for (int i = 0; i < MainScript.allHoldersArray.Length; i++)
        {
            if (MainScript.allHoldersArray[i].transform.position.x == x - 1 && MainScript.allHoldersArray[i].transform.position.z == z)
            {
                if (!MainScript.wallOnHolder[i])
                {
                    for (int j = 0; j < MainScript.allFieldsArray.Length; j++)
                    {
                        if (x - 2 == Red.transform.position.x && z == Red.transform.position.z && ServerHub.playerNames[0] != null)
                            winner[0] = true;
                        if (x - 2 == Yellow.transform.position.x && z == Yellow.transform.position.z && ServerHub.playerNames[1] != null)
                            winner[1] = true;
                        if (x - 2 == Green.transform.position.x && z == Green.transform.position.z && ServerHub.playerNames[2] != null)
                            winner[2] = true;
                        if (x - 2 == Blue.transform.position.x && z == Blue.transform.position.z && ServerHub.playerNames[3] != null)
                            winner[3] = true;
                        if (MainScript.allFieldsArray[j].transform.position.x == x - 2 && MainScript.allFieldsArray[j].transform.position.z == z && MainScript.fieldChecked[j] != true)
                        {
                            MainScript.fieldChecked[j] = true;
                            WinningState((int)MainScript.allFieldsArray[j].transform.position.x, (int)MainScript.allFieldsArray[j].transform.position.z);
                        }
                    }
                }
            }
            else if (MainScript.allHoldersArray[i].transform.position.x == x && MainScript.allHoldersArray[i].transform.position.z == z - 1)
            {
                if (!MainScript.wallOnHolder[i])
                {
                    for (int j = 0; j < MainScript.allFieldsArray.Length; j++)
                    {
                        if (x == Red.transform.position.x && z - 2 == Red.transform.position.z && ServerHub.playerNames[0] != null)
                            winner[0] = true;
                        if (x == Yellow.transform.position.x && z - 2 == Yellow.transform.position.z && ServerHub.playerNames[1] != null)
                            winner[1] = true;
                        if (x == Green.transform.position.x && z - 2 == Green.transform.position.z && ServerHub.playerNames[2] != null)
                            winner[2] = true;
                        if (x == Blue.transform.position.x && z - 2 == Blue.transform.position.z && ServerHub.playerNames[3] != null)
                            winner[3] = true;
                        if (MainScript.allFieldsArray[j].transform.position.x == x && MainScript.allFieldsArray[j].transform.position.z == z - 2 && MainScript.fieldChecked[j] != true)
                        {
                            MainScript.fieldChecked[j] = true;
                            WinningState((int)MainScript.allFieldsArray[j].transform.position.x, (int)MainScript.allFieldsArray[j].transform.position.z);
                        }
                    }
                }
            }
            else if (MainScript.allHoldersArray[i].transform.position.x == x && MainScript.allHoldersArray[i].transform.position.z == z + 1)
            {
                if (!MainScript.wallOnHolder[i])
                {
                    for (int j = 0; j < MainScript.allFieldsArray.Length; j++)
                    {
                        if (x == Red.transform.position.x && z + 2 == Red.transform.position.z && ServerHub.playerNames[0] != null)
                            winner[0] = true;
                        if (x == Yellow.transform.position.x && z + 2 == Yellow.transform.position.z && ServerHub.playerNames[1] != null)
                            winner[1] = true;
                        if (x == Green.transform.position.x && z + 2 == Green.transform.position.z && ServerHub.playerNames[2] != null)
                            winner[2] = true;
                        if (x == Blue.transform.position.x && z + 2 == Blue.transform.position.z && ServerHub.playerNames[3] != null)
                            winner[3] = true;
                        if (MainScript.allFieldsArray[j].transform.position.x == x && MainScript.allFieldsArray[j].transform.position.z == z + 2 && MainScript.fieldChecked[j] != true)
                        {
                            MainScript.fieldChecked[j] = true;
                            WinningState((int)MainScript.allFieldsArray[j].transform.position.x, (int)MainScript.allFieldsArray[j].transform.position.z);
                        }
                    }
                }
            }
            else if (MainScript.allHoldersArray[i].transform.position.x == x + 1 && MainScript.allHoldersArray[i].transform.position.z == z)
            {
                if (!MainScript.wallOnHolder[i])
                {
                    for (int j = 0; j < MainScript.allFieldsArray.Length; j++)
                    {
                        if (x + 2 == Red.transform.position.x && z == Red.transform.position.z && ServerHub.playerNames[0] != null)
                            winner[0] = true;
                        if (x + 2 == Yellow.transform.position.x && z == Yellow.transform.position.z && ServerHub.playerNames[1] != null)
                            winner[1] = true;
                        if (x + 2 == Green.transform.position.x && z == Green.transform.position.z && ServerHub.playerNames[2] != null)
                            winner[2] = true;
                        if (x + 2 == Blue.transform.position.x && z == Blue.transform.position.z && ServerHub.playerNames[3] != null)
                            winner[3] = true;
                        if (MainScript.allFieldsArray[j].transform.position.x == x + 2 && MainScript.allFieldsArray[j].transform.position.z == z && MainScript.fieldChecked[j] != true)
                        {
                            MainScript.fieldChecked[j] = true;
                            WinningState((int)MainScript.allFieldsArray[j].transform.position.x, (int)MainScript.allFieldsArray[j].transform.position.z);
                        }
                    }
                }
            }
        }
    }
}
