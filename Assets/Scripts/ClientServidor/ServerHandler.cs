using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerHandler : MonoBehaviour
{
    public bool test = false;
    private NetworkHelper networkHelper;
    private List<int> clientIdList = new List<int> { };

    private PlayerController[] players;
    private Rigidbody2D[] playersRbs = new Rigidbody2D[2];
    private string[] dataRecieved;
    private GameObject stones;

    private int playersConnected = 0;

    const string ID_NUM_JUGADOR = "0";
    const string ID_POS_JUGADOR = "1";
    const string ID_POS_CAIXA = "2";
    const string ID_INPUTS = "3";
    const string ID_BOTO_POLSAT = "4";
    const string ID_FINAL = "5";
    const string ID_JUGADOR_MORT = "6";

	private int enteredPlayers = 0;

    private void Start()
    {
        DontDestroyOnLoad(this);
        players = FindObjectsOfType<PlayerController>();
        if(players.Length > 0)
        {
            playersRbs[0] = players[0].GetComponent<Rigidbody2D>();
            playersRbs[1] = players[1].GetComponent<Rigidbody2D>();
        }
        InvokeRepeating("SendPlayerPositionAndSpeed", 0.2f, 0.2f);
    }

    public void SendButtonPressed(bool pressed)
    {
        string data = "";
        if (pressed) data = ID_BOTO_POLSAT + "/1";
        else data = ID_BOTO_POLSAT + "/0";

        SendToAll(data);
    }

    public void SendEndLevelDoorEntered(bool isPlayer1)
    {
        string data = "";
		enteredPlayers++;
        if (isPlayer1) data = ID_FINAL + "/1";
        else data = ID_FINAL + "/2";
		data = data + "/" + enteredPlayers.ToString();

        SendToAll(data);
    }
    private void SendPlayerPositionAndSpeed()
    {

		SendStonesPosition();
		if (players.Length == 0) {
            players = FindObjectsOfType<PlayerController>();
            if (players.Length > 0)
            {
                playersRbs[0] = players[0].GetComponent<Rigidbody2D>();
                playersRbs[1] = players[1].GetComponent<Rigidbody2D>();
            }
        }
        if (players.Length == 0) return;
        if (!players[0].isPlayer1)
        {
            PlayerController p = players[0];
            players[0] = players[1];
            players[1] = p;
        }

        for(int i = 0; i < players.Length; i++)
        {
            string data = ID_POS_JUGADOR + "/" + (i+1).ToString() + "/" + players[i].transform.position.x.ToString() + "/" + players[i].transform.position.y.ToString() + "/" + players[i].transform.position.z.ToString() + "/" + playersRbs[i].velocity.x.ToString() + "/" + playersRbs[i].velocity.y.ToString();
            SendToAll(data);
        }
    }

    public void SendPlayerDead(bool isPlayer1)
    {
        string data = "";

        if (isPlayer1) data = ID_JUGADOR_MORT + "/1";
        else data = ID_JUGADOR_MORT + "/2";

        SendToAll(data);
    }

    private void SendStonesPosition()
    {
        if(stones == null)stones = GameObject.FindWithTag("Stones");
        if (stones == null) return;
        int i = 0;
        foreach(Transform child in stones.transform)
        {
            string data = ID_POS_CAIXA + "/" + i.ToString() + "/" + child.position.x.ToString() + "/" + child.position.y.ToString() + "/" + child.position.z.ToString();
            i++;
        }
    }



    public bool StartServer(int localPort)
    {
        networkHelper = FindObjectOfType<NetworkHelper>();
        networkHelper.onHostAdded.AddListener(ServerStarted);
        networkHelper.onHostRemoved.AddListener(ServerStopped);
        networkHelper.onConnectClient.AddListener(ClientConnected);
        networkHelper.onDisconnectClient.AddListener(ClientDisconnected);
        networkHelper.onMessageReceivedFrom.AddListener(ReceiveMessage);
        return networkHelper.MakeServer(localPort);
    }

    private List<int> ConnectedClients => networkHelper.connectionIds;

    private void ServerStarted()
    {
        if (!test) SceneManager.LoadScene("Server");
    }

    private void ServerStopped()
    {
    }

    private void ClientConnected(int id)
    {
        playersConnected++;
        clientIdList.Add(id);
        SendToClient(id, ID_NUM_JUGADOR + "/" + playersConnected.ToString());
    }

    private void ClientDisconnected(int arg0)
    {
        clientIdList.Remove(arg0);
        playersConnected--;
    }

    private void ReceiveMessage(string message, int from)
    {
        // Do things here
        // Example: Print message on chat
        //GameObject.FindWithTag("Chat").GetComponent<ChatController>().AddChatToChatOutput(from + " -> " + message);

        // Example: relay all messages
        if (players.Length == 0) {
            players = FindObjectsOfType<PlayerController>();
            if (players.Length > 0)
            {
                playersRbs[0] = players[0].GetComponent<Rigidbody2D>();
                playersRbs[1] = players[1].GetComponent<Rigidbody2D>();
            }
        }
        if (!players[0].isPlayer1)
        {
            PlayerController p = players[0];
            players[0] = players[1];
            players[1] = p;
        }
        //SendToAllExcept(from + " -> " + message, from);
        PlayerController player = players[0];
        if(from == clientIdList[0])
        {
            player = players[0];
        }
        else if(clientIdList.Count == 2 && from == clientIdList[1])
        {
            player = players[1];
        }

        dataRecieved = message.Split('/');
        player.dir = int.Parse(dataRecieved[0]);
        if (dataRecieved[1] == "1")
        {
            player.jumpJustPressed = 1;
        }
        /*if (dataRecieved[3] == "0")
        {
            player.isAlive = false;
        }
        if (dataRecieved[4] == "1")
        {
            player.justGotHit = 1;
        }*/
        //string data1 = "0/" + player.transform.position.x.ToString() + "/" + player.transform.position.y.ToString() + "/" + player.transform.position.z.ToString() + "/0";
        //string data2 = "1/" + player.transform.position.x.ToString() + " / " + player.transform.position.y.ToString() + "/" + player.transform.position.z.ToString() + "/" + player.jumpJustPressed.ToString() + "/" + player.dir.ToString(); ;

        string data = ID_INPUTS + "/" + player.jumpJustPressed.ToString() + "/" + player.dir.ToString();

        //SendToClient(from, data1);
        //SendToAllExcept(data2, from);

        SendToAllExcept(data, from);
    }

    public void SendToClient(int id, string message)
    {
        networkHelper.SendToOne(id, message);
    }

    public void SendToAllExcept(string message, int exceptId)
    {
        networkHelper.SendToAllExcept(message, exceptId);
    }

    public void SendToAll(string message)
    {
        networkHelper.SendToAll(message);
    }

}
