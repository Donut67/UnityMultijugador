using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerHandler : MonoBehaviour
{
    public bool test = false;
    private NetworkHelper networkHelper;
    private int[] seleccions = {-1, -1, -1, -1};
    private List<bool> ready = new List<bool>();

    private void Start()
    {
        DontDestroyOnLoad(this);
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
        ready.Insert(id, false);
        SendToClient(id, "JUGADOR," + id.ToString());
        GameObject.FindWithTag("Chat").GetComponent<ChatController>().AddChatToChatOutput("P" + id + " connected");
        string send = "SELECT,";
        for(int i = 0; i < 4; i++) {
            send += seleccions[i].ToString() + ",";
        }
        SendToClient(id, send);
    }

    private void ClientDisconnected(int arg0)
    {
    }

    private void ReceiveMessage(string message, int from)
    {
        // Do things here
        GameObject.FindWithTag("Chat").GetComponent<ChatController>().AddChatToChatOutput(message);
        if(message == "LLEST") {
            ready[from] = true;
            bool tots = true;
            foreach(bool pReady in ready){
                if(!pReady) {
                    tots = false;
                    break;
                }
            }
            GameObject.FindWithTag("Chat").GetComponent<ChatController>().AddChatToChatOutput(from + ": LLEST");
            if(tots) SendToAll("LLEST");
        }else{
            string[] resultat = message.Split(",");

            if (resultat[0] == "SELECT") {
                int qui  = from - 1;
                int quin = Int32.Parse(resultat[1]);
                int pos = -1;

                for(int i = 0; i < 4; i++) {
                    if(seleccions[i] == qui) {
                        pos = i;
                        break;
                    }
                }

                if(seleccions[quin] == -1) {
                    if(pos != -1) seleccions[pos] = -1;
                    seleccions[quin] = qui;
                }

                string send = "SELECT";
                for(int i = 0; i < 4; i++) {
                    send += "," + seleccions[i].ToString();
                }
                GameObject.FindWithTag("Chat").GetComponent<ChatController>().AddChatToChatOutput(send);
                SendToAll(send);
            }
        }
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
