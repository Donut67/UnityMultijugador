using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ClientHandler : MonoBehaviour
{
    public bool test = false;
    private NetworkHelper networkHelper;
    public int jugador = 1;
    public int[] seleccions = {-1, -1, -1, -1};
    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public bool StartClient(int localPort, string remoteIP, int remotePort)
    {
        networkHelper = FindObjectOfType<NetworkHelper>();
        networkHelper.onConnect.AddListener(ConnectedToServer);
        networkHelper.onDisconnect.AddListener(DisconnectedFromServer);
        networkHelper.onMessageReceived.AddListener(ReceiveMessage);
        return networkHelper.ConnectToServer(localPort, remoteIP, remotePort);
    }

    private void ConnectedToServer()
    {
        if (!test) SceneManager.LoadScene("Client");
    }

    private void DisconnectedFromServer()
    {
    }

    private void ReceiveMessage(string message)
    {
        // Do things here
        string[] resultat = message.Split('|');

        if(resultat[0] == "JUGADOR") {
            jugador = Int32.Parse(resultat[1]);
            GameObject.FindWithTag("Jugador").GetComponent<TextMeshProUGUI>().text = "Jugador: P" + resultat[1];
        }
        else if(resultat[0] == "SELECT") {
            CharacterSelection cs = GameObject.FindWithTag("Selector").GetComponent<CharacterSelection>();
            for(int i = 0; i < 4; i++) {
                int qui = Int32.Parse(resultat[1 + i]);
                seleccions[i] = qui;
                if(qui == -1) cs.DisSelectBox(i);
                else cs.SelectBox(qui, i);
            }
        }
        else if(resultat[0] == "LLEST"){
            GameObject.FindWithTag("Jugador").GetComponent<TextMeshProUGUI>().text = message;
            SceneManager.LoadScene("JocPrincipal", LoadSceneMode.Single);
        }
        else if(resultat[0] == "INFO") {
            ControladorJugadors cj = GameObject.FindWithTag("ControladorJugadors").GetComponent<ControladorJugadors>();
            cj.sendInfoToPlayer(Int32.Parse(resultat[1]) - 1, float.Parse(resultat[2]), resultat[3] == "true", resultat[4] == "true");
        }
        else if(resultat[0] == "FINISH"){
            int equipGuanyador = Int32.Parse(resultat[1]);
            // Que fer quan s'acaba la partida
        }
        
        // Example: Print message on chat
    }

    public void SendToServer(string message)
    {
        networkHelper.SendToServer(message);
    }

}
