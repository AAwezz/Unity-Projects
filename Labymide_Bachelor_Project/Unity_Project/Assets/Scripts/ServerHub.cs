using UnityEngine;
using System.Collections;

/*
 * The serverHub lets the user choose a position and the host can initiate the game if there is one client joined.
 */

public class ServerHub : MonoBehaviour
{

    public GUIText playerNameTag, playerNameTag2, playerNameTag3, playerNameTag4;
    int numb = 0, count = 0,nameCount=0;
    public static string[] playerNames = new string[4];
    string rPCName="";
    bool[] buttonsBool = new bool[4];
    bool showButtons = true, seatTaken = false;

    void Start()
    {
        if (Network.isServer)
            GetComponent<NetworkView>().RPC("DeactivateGUIText", RPCMode.AllBuffered, true);
    }

    /*
     * This RPC call is used to disable all the gui text at the start.
     */
    [RPC]
    void DeactivateGUIText(bool boolBuff)
    {
        buttonsBool[0] = boolBuff;
        buttonsBool[1] = boolBuff;
        buttonsBool[2] = boolBuff;
        buttonsBool[3] = boolBuff;
        playerNameTag.enabled = false;
        playerNameTag2.enabled = false;
        playerNameTag3.enabled = false;
        playerNameTag4.enabled = false;
    }

    /*
     * This RPC call starts the game for all the players.
     */
    [RPC]
    void StartGame(string gameName)
    {
        Application.LoadLevel(gameName);
    }

    [RPC]
    void CloseServer(bool buttonsBool)
    {
        showButtons = buttonsBool;
    }
    /*
    * This method receives presse from the buttons
    * and it also scales the screen.
    */
    void OnGUI()
    {
        var svMat = NetworkManager.ScalingScreen();

        if (Network.isServer)
        {
            if (GUI.Button(new Rect(80, 210, 150, 30), "Start Game"))
            {
                if (Network.connections.Length > 0 && count == Network.connections.Length+1)
                {
                    GetComponent<NetworkView>().RPC("CloseServer", RPCMode.AllBuffered, showButtons);
                    GetComponent<NetworkView>().RPC("StartGame", RPCMode.All, "Labymide");
                }
            }
        }
        if (GUI.Button(new Rect(80, 250, 150, 30), "Quit Game"))
        {
            GetComponent<NetworkView>().RPC("QuitingGame", RPCMode.AllBuffered, numb, seatTaken);
            seatTaken = false;
            Network.Disconnect();
            Application.LoadLevel("startPage");
        }
        if (showButtons)
        {
            if (buttonsBool[0] == true)
            {
                if (GUI.Button(new Rect(300, 210, 150, 30), "Take red's seat"))
                {
                    DisplayingGUIText(1);
                    numb = 0;
                    seatTaken = true;
                    ChangingName();
                    GetComponent<NetworkView>().RPC("ActivatingGUIText", RPCMode.AllBuffered, 0, rPCName);
                    showButtons = false;
                }
            }
            if (buttonsBool[1] == true)
            {
                if (GUI.Button(new Rect(300, 250, 150, 30), "Take yellow's seat"))
                {
                    DisplayingGUIText(2);
                    numb = 1;
                    seatTaken = true; 
                    ChangingName();
                    GetComponent<NetworkView>().RPC("ActivatingGUIText", RPCMode.AllBuffered, 1, rPCName);
                    showButtons = false;
                }
            }
            if (buttonsBool[2] == true)
            {
                if (GUI.Button(new Rect(300, 290, 150, 30), "Take green's seat"))
                {
                    DisplayingGUIText(3);
                    numb = 2;
                    seatTaken = true;
                    ChangingName();
                    GetComponent<NetworkView>().RPC("ActivatingGUIText", RPCMode.AllBuffered, 2, rPCName);
                    showButtons = false;
                }
            }
            if (buttonsBool[3] == true)
            {
                if (GUI.Button(new Rect(300, 330, 150, 30), "Take blue's seat"))
                {
                    DisplayingGUIText(4);
                    numb = 3;
                    seatTaken = true;
                    ChangingName();
                    GetComponent<NetworkView>().RPC("ActivatingGUIText", RPCMode.AllBuffered, 3, rPCName);
                    showButtons = false;
                }
            }
        }
        GUI.matrix = svMat;
    }

    private void ChangingName()
    {
        rPCName = NetworkManager.showName;
        if (rPCName == playerNames[0] || rPCName == playerNames[1] ||
            rPCName == playerNames[2] || rPCName == playerNames[3])
        {
            nameCount++;
            rPCName = rPCName + nameCount;
            NetworkManager.showName = rPCName;
            ChangingName();
        }
    }

    /*
     * This RPC call tells the other players if another player has left the serverhub.
     */
    [RPC]
    void QuitingGame(int i, bool b)
    {
        if (b)
        {
            count--;
            switch (i)
            {
                case 0:
                    playerNameTag.text = "Waiting for player...";
                    playerNameTag.color = Color.white;
                    playerNames[0] = null;
                    playerNameTag.enabled = false;
                    break;
                case 1:
                    playerNameTag2.text = "Waiting for player...";
                    playerNameTag2.color = Color.white;
                    playerNames[1] = null;
                    playerNameTag2.enabled = false;
                    break;
                case 2:
                    playerNameTag3.text = "Waiting for player...";
                    playerNameTag3.color = Color.white;
                    playerNames[2] = null;
                    playerNameTag3.enabled = false;
                    break;
                case 3:
                    playerNameTag4.text = "Waiting for player...";
                    playerNameTag4.color = Color.white;
                    playerNames[3] = null;
                    playerNameTag4.enabled = false;
                    break;
            }
            buttonsBool[i] = true;
        }
    }

    /*
     * This method displays the GUI text.
     */
    void DisplayingGUIText(int i)
    {
        playerNameTag.enabled = true;
        playerNameTag2.enabled = true;
        playerNameTag3.enabled = true;
        playerNameTag4.enabled = true;
        if (playerNameTag.text == "" && i != 1)
        {
            playerNameTag.text = "Waiting for player...";
            playerNameTag.color = Color.red;
        }
        if (playerNameTag2.text == "" && i != 2)
        {
            playerNameTag2.text = "Waiting for player...";
            playerNameTag2.color = Color.yellow;
        }
        if (playerNameTag3.text == "" && i != 3)
        {
            playerNameTag3.text = "Waiting for player...";
            playerNameTag3.color = Color.green;
        }
        if (playerNameTag4.text == "" && i != 4)
        {
            playerNameTag4.text = "Waiting for player...";
            playerNameTag4.color = Color.blue;
        }
    }

    /*
     * This RPC call lets the choosen seat to be shown to all that it is now taken.
     */
    [RPC]
    void ActivatingGUIText(int i, string name)
    {
        count++;
        buttonsBool[i] = false;
        playerNames[i] = name;
            switch (i)
            {
                case 0:
                    playerNameTag.enabled = true;
                    playerNameTag.text = playerNames[i];
                    playerNameTag.color = Color.red;
                    break;
                case 1:
                    playerNameTag2.enabled = true;
                    playerNameTag2.text = playerNames[i];
                    playerNameTag2.color = Color.yellow;
                    break;
                case 2:
                    playerNameTag3.enabled = true;
                    playerNameTag3.text = playerNames[i];
                    playerNameTag3.color = Color.green;
                    break;
                case 3:
                    playerNameTag4.enabled = true;
                    playerNameTag4.text = playerNames[i];
                    playerNameTag4.color = Color.blue;
                    break;
            }
    }
}
