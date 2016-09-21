using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

/*
 * This script lets you create or join a gamehub.
 */

public class NetworkManager : MonoBehaviour {

    const string typeName = "wasd12345";
    public string gameName, playerName;
    public static string showName;

    HostData[] hostList;

    public static float originalWidth = 640.0f;
    public static float originalHeight = 400.0f;
    public static Vector3 scale;

    /*
     * This method displays the avaliable gamehubs.
     */
    void RefreshHostList()
    {
        MasterServer.RequestHostList(typeName);
    }

  /*  void Awake()
    {
        MasterServer.ipAddress = "192.168.1.99";
        MasterServer.port = 23466;
        Network.natFacilitatorIP = "192.168.1.99";
        Network.natFacilitatorPort = 5000;
    } */

    void OnMasterServerEvent(MasterServerEvent msEvent)
    {
        if (msEvent == MasterServerEvent.HostListReceived)
            hostList = MasterServer.PollHostList();
    }

    /*
     * This method lets you join a created gamehub.
     */
    void JoinServer(HostData hostData)
    {
            Network.Connect(hostData);
            Application.LoadLevel("ServerHub");
    }

    /*
     * This method and OnServerInitialized() lets you create a game and join it.
     */
    void StartServer()
    {
        Network.InitializeServer(3, 1024, !Network.HavePublicAddress());
        MasterServer.RegisterHost(typeName, gameName);
        Network.maxConnections = 3;
    }

    void OnServerInitialized()
    {
        Application.LoadLevel("ServerHub");
    }

    /*
     * This method receives presse from the buttons
     * and it also scales the screen.
     */
    void OnGUI()
    {
        var svMat = ScalingScreen();

        if (!Network.isClient && !Network.isServer)
        {
            GUI.Label(new Rect(250, 50, 150, 30), "<size=15>Enter player name</size>");
            playerName = GUI.TextField(new Rect(250, 80, 150, 20), playerName,25);
            playerName = Regex.Replace(playerName, @"[^a-zA-Z0-9-_]", ""); // This is the only allowed characters in the playerName
            showName = playerName;
            GUI.Label (new Rect(80, 150, 150, 30), "<size=15>Enter server name</size>");
            gameName = GUI.TextField(new Rect(80, 180, 150, 20), gameName,18);
            gameName = Regex.Replace(gameName, @"[^a-zA-Z0-9-_]", ""); // This is the only allowed characters in the gameName
                if (GUI.Button(new Rect(80, 210, 150, 30), "Create server") && gameName !="" && playerName !="")
                {
                    StartServer();
                }
            if (GUI.Button(new Rect(420, 150, 150, 30), "Find A Game"))
                RefreshHostList();
            if (hostList != null)
            {
                for (int i = 0; i < hostList.Length; i++)
                {
                    if (GUI.Button(new Rect(420, 185 + (35 * i), 150, 30), hostList[i].gameName) && playerName != "")
                    {
                        JoinServer(hostList[i]);
                    }
                }
            }
        }

        GUI.matrix = svMat;
    }

    public static Matrix4x4 ScalingScreen()
    {
        scale.x = Screen.width / originalWidth;
        scale.y = Screen.height / originalHeight;
        scale.z = 1;
        var svMat = GUI.matrix;
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, scale);
        return svMat;
    }
}
