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
    private char divider = '|';

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
        ready.Add(false);
        SendToClient(id, "JUGADOR" + divider + id.ToString());
        GameObject.FindWithTag("Chat").GetComponent<ChatController>().AddChatToChatOutput("P" + id + " connected");
        string send = "SELECT" + divider;
        for(int i = 0; i < 4; i++) {
            send += seleccions[i].ToString() + divider;
        }
        SendToClient(id, send);
    }

    private void ClientDisconnected(int arg0)
    {
    }

    private void ReceiveMessage(string message, int from)
    {
        // Do things here
        
        string[] resultat = message.Split(divider);

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
                send += divider + seleccions[i].ToString();
            }
            GameObject.FindWithTag("Chat").GetComponent<ChatController>().AddChatToChatOutput(send);
            SendToAll(send);
        }
        else if(resultat[0] == "LLEST") {
            GameObject.FindWithTag("Chat").GetComponent<ChatController>().AddChatToChatOutput(from + ": LLEST");
            ready[from - 1] = true;
            if(ready.Count >= 2) {
                bool tots = true;
                foreach(bool pReady in ready){
                    if(!pReady) {
                        tots = false;
                        break;
                    }
                }
                if(tots) SendToAll("LLEST" + divider + "0");
            }
        }else if(resultat[0] == "INPUTS"){
            GameObject.FindWithTag("Chat").GetComponent<ChatController>().AddChatToChatOutput(message);
            SendToAll("INFO" + divider + from + divider + resultat[1] + divider + resultat[2] + divider + resultat[3] + divider + resultat[4]);
        }else if(resultat[0] == "VIDA") {
            SendToAll("VIDA" + divider + from + divider + resultat[1]);
        }else if(resultat[0] == "FINISH") {
            SendToAll(message);
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
