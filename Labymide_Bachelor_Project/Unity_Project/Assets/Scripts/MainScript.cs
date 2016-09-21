using UnityEngine;
using System.Collections;
using System.Diagnostics;

/*
 * This is our mainscript that starts up the wallarrays, 
 * sets up playernames and starts the game.
 */

public class MainScript : MonoBehaviour
{
    public static GameObject[] dieSixArray, dieFiveArray, dieFourArray, dieThreeArray, dieTwoArray, dieOneArray, allWallsArray, allHoldersArray, allFieldsArray;
    public static int playerTurn, dieSixArrayNumb, dieFiveArrayNumb, dieFourArrayNumb, dieThreeArrayNumb, dieTwoArrayNumb, dieOneArrayNumb = 0;
    public static bool[] wallOnHolder, fieldChecked;
    public GUIText playerNameTag, playerNameTag2, playerNameTag3, playerNameTag4;
    public GUITexture diePicture, diePicture2, diePicture3, diePicture4;
    public static string oldGameObjectName = "";

    void Start()
    {
        SettingPlayerNames();
        allFieldsArray = GameObject.FindGameObjectsWithTag("Field");
        fieldChecked = new bool[allFieldsArray.Length];
        ArraySetup();
        PlayerStartTurn(Network.connections.Length + 1);
    }

    void Update()
    {
        if (Holders.nextTurn && Network.isServer)
        {
            GetComponent<NetworkView>().RPC("PlayerTurn", RPCMode.AllBuffered, playerTurn);
            Holders.nextTurn = false;
        }
    }

    /* 
     * Decides which person starts the game
     */
    void PlayerStartTurn(int n)
    {
        if (Network.isServer)
        {
            playerTurn = Random.Range(0, n);
            do
            {
                ChangingPlayerTurn();
            } while (ServerHub.playerNames[playerTurn] == null); // This makes sure that the players can take any playerposition and the game still works
            GetComponent<NetworkView>().RPC("PlayerTurn", RPCMode.AllBuffered, playerTurn);
        }
    }

    /* 
     * A method to change playerTurn
     */
    public static void ChangingPlayerTurn()
    {
        if (playerTurn == 3)
        {
            playerTurn = 0;
        }
        else
        {
            playerTurn++;
        }
    }

    /* 
     * A RPC method that sets the player turn for all clients,
     * and changes the picture of the die next to the playernames
     */
    [RPC]
    void PlayerTurn(int n)
    {
        playerTurn = n;
        ChangingDiePicture();
    }

    /* 
     * This method changes the picture of the die
     * so that each player knows whos turn it is
     */
    void ChangingDiePicture()
    {
        switch (playerTurn)
        {
            case 0:
                diePicture.enabled = true;
                diePicture2.enabled = false;
                diePicture3.enabled = false;
                diePicture4.enabled = false;
                break;
            case 1:
                diePicture.enabled = false;
                diePicture2.enabled = true;
                diePicture3.enabled = false;
                diePicture4.enabled = false;
                break;
            case 2:
                diePicture.enabled = false;
                diePicture2.enabled = false;
                diePicture3.enabled = true;
                diePicture4.enabled = false;
                break;
            case 3:
                diePicture.enabled = false;
                diePicture2.enabled = false;
                diePicture3.enabled = false;
                diePicture4.enabled = true;
                break;
        }
    }

    /* 
     * This function sets the player names
     */
    void SettingPlayerNames()
    {
        playerNameTag.text = ServerHub.playerNames[0];
        playerNameTag2.text = ServerHub.playerNames[1];
        playerNameTag3.text = ServerHub.playerNames[2];
        playerNameTag4.text = ServerHub.playerNames[3];
        playerNameTag.color = Color.red;
        playerNameTag2.color = Color.yellow;
        playerNameTag3.color = Color.green;
        playerNameTag4.color = Color.blue;
    }

    /* 
     * This method initialize six gameobject arrays which hold the walls.
     */
    void ArraySetup()
    {
        dieSixArray = new GameObject[12];
        dieFiveArray = new GameObject[36];
        dieFourArray = new GameObject[60];
        dieThreeArray = new GameObject[84];
        dieTwoArray = new GameObject[108];
        dieOneArray = new GameObject[132];
        float wallPosX;
        float wallPosZ;

        // Adding all holders and walls to their arrays by using tags.
        allHoldersArray = GameObject.FindGameObjectsWithTag("Holders");
        allWallsArray = GameObject.FindGameObjectsWithTag("Wall");

        for (int i = 0; i < fieldChecked.Length; i++)
        {
            fieldChecked[i] = false;
        }

        // wallOnHolder is a boolean array for holders. If its value is true, there is a wall ontop of that holder,
        // and if its false there is no wall ontop of the holder .
        wallOnHolder = new bool[allHoldersArray.Length];
        for (int i = 0; i < wallOnHolder.Length; i++)
        {
            wallOnHolder[i] = false;
        }
        for (int i = 0; i < allWallsArray.Length; i++)
        {
            wallPosX = allWallsArray[i].transform.position.x;
            wallPosZ = allWallsArray[i].transform.position.z;

            for (int j = 0; j < allHoldersArray.Length; j++)
            {
                if (allHoldersArray[j].transform.position.x == wallPosX && allHoldersArray[j].transform.position.z == wallPosZ)
                {
                    wallOnHolder[j] = true;
                }
            }

            // Here the walls get added into their destined array, by using the walls positions
            if (-3 < wallPosX && wallPosX < 3 && -3 < wallPosZ && wallPosZ < 3)
            {
                dieSixArray[dieSixArrayNumb] = allWallsArray[i];
                dieSixArrayNumb++;
            }
            else if (-5 < wallPosX && wallPosX < 5 && -5 < wallPosZ && wallPosZ < 5)
            {
                dieFiveArray[dieFiveArrayNumb] = allWallsArray[i];
                dieFiveArrayNumb++;
            }
            else if (-7 < wallPosX && wallPosX < 7 && -7 < wallPosZ && wallPosZ < 7)
            {
                dieFourArray[dieFourArrayNumb] = allWallsArray[i];
                dieFourArrayNumb++;
            }
            else if (-9 < wallPosX && wallPosX < 9 && -9 < wallPosZ && wallPosZ < 9)
            {
                dieThreeArray[dieThreeArrayNumb] = allWallsArray[i];
                dieThreeArrayNumb++;
            }
            else if (-11 < wallPosX && wallPosX < 11 && -11 < wallPosZ && wallPosZ < 11)
            {
                dieTwoArray[dieTwoArrayNumb] = allWallsArray[i];
                dieTwoArrayNumb++;
            }
            else /*if (-13 < wallPosX && wallPosX < 13 && -13 < wallPosZ && wallPosZ < 13)*/
            {
                dieOneArray[dieOneArrayNumb] = allWallsArray[i];
                dieOneArrayNumb++;
            }
        }
    }

    /* 
     * This method colors the walls you can move, depending on what number you have rolled with the dice. 
     */
    public static void AvaliableWalls()
    {
        switch (Dice.sideOnDice)
        {
            case 1:
                for (int i = 0; i < MainScript.dieOneArrayNumb; i++)
                {
                    MainScript.dieOneArray[i].GetComponent<Renderer>().material.color = Color.cyan;
                }
                break;
            case 2:
                for (int i = 0; i < MainScript.dieTwoArrayNumb; i++)
                {
                    MainScript.dieTwoArray[i].GetComponent<Renderer>().material.color = Color.cyan;
                }
                break;
            case 3:
                for (int i = 0; i < MainScript.dieThreeArrayNumb; i++)
                {
                    MainScript.dieThreeArray[i].GetComponent<Renderer>().material.color = Color.cyan;
                }
                break;
            case 4:
                for (int i = 0; i < MainScript.dieFourArrayNumb; i++)
                {
                    MainScript.dieFourArray[i].GetComponent<Renderer>().material.color = Color.cyan;
                }
                break;
            case 5:
                for (int i = 0; i < MainScript.dieFiveArrayNumb; i++)
                {
                    MainScript.dieFiveArray[i].GetComponent<Renderer>().material.color = Color.cyan;
                }
                break;
            case 6:
                for (int i = 0; i < MainScript.dieSixArrayNumb; i++)
                {
                    MainScript.dieSixArray[i].GetComponent<Renderer>().material.color = Color.cyan;
                }
                break;
        }
    }

    /* 
     * These three methods colors the gameobject white, so it gets its default color. 
     */
    public static void ClearWallColor()
    {
        for (int i = 0; i < allWallsArray.Length; i++)
        {
            allWallsArray[i].GetComponent<Renderer>().material.color = Color.white;
        }
    }
    public static void ClearHolderColor()
    {
        for (int i = 0; i < Holders.holderCount; i++)
        {
            Holders.holderBufArray[i].GetComponent<Renderer>().material.color = Color.white;
        }
    }
    public static void ClearFieldsColor()
    {
        for (int i = 0; i < allFieldsArray.Length; i++)
        {
            allFieldsArray[i].GetComponent<Renderer>().material.color = Color.gray;
        }
    }

    /* 
     * These two methods add and removes the wall that gets moved, from an array to another, depending on what number the dice have shown. 
     */
    public static GameObject[] RemoveWallFromArray(GameObject[] DieArray, int DieCount, GameObject Wall)
    {
        GameObject[] bufArray = new GameObject[DieArray.Length];
        int count = 0;
        for (int i = 0; i < DieCount; i++)
        {
            if (DieArray[i].name != Wall.name)
            {
                bufArray[count] = DieArray[i];
                count++;
            }
        }
        return bufArray;
    }

    public static GameObject[] AddWallToArray(GameObject[] DieArray, int DieCount, GameObject Wall)
    {
        DieArray[DieCount] = Wall;
        return DieArray;
    }
}
