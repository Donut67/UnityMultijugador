using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientHandler : MonoBehaviour
{
    public bool test = false;
    private NetworkHelper networkHelper;
    private InputHandler[] simulations;
    private Rigidbody2D[] simulationsRbs = new Rigidbody2D[2];
    private string[] dataRecieved;
    private GameObject stones;


    const string ID_NUM_JUGADOR = "0";
    const string ID_POS_JUGADOR = "1";
    const string ID_POS_CAIXA = "2";
    const string ID_INPUTS = "3";
    const string ID_BOTO_POLSAT = "4";
    const string ID_FINAL = "5";
    const string ID_JUGADOR_MORT = "6";
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
        if (simulations == null)
        {
            simulations = FindObjectsOfType<InputHandler>();
            if (simulations.Length > 0)
            {
                simulationsRbs[0] = simulations[0].GetComponent<Rigidbody2D>();
                simulationsRbs[1] = simulations[1].GetComponent<Rigidbody2D>();
            }
        }
       /*if (!simulations[0].isLocal)
        {
            InputHandler sim = simulations[0];
            simulations[0] = simulations[1];
            simulations[1] = sim;
        }*/

        dataRecieved = message.Split('/');

        if (dataRecieved[0] == ID_NUM_JUGADOR)
        {
            Debug.LogError("Connectat");
            if (int.Parse(dataRecieved[1]) == 2)
            {
                //InputHandler[] simulationsList = FindObjectsOfType<InputHandler>();
                for (int i = 0; i < simulations.Length; i++)
                {
                    simulations[i].isLocal = !simulations[i].isLocal;

                }
            }
            if (!simulations[0].isLocal)
            {
                InputHandler sim = simulations[0];
                simulations[0] = simulations[1];
                simulations[1] = sim;
                simulationsRbs[0] = simulations[0].GetComponent<Rigidbody2D>();
                simulationsRbs[1] = simulations[1].GetComponent<Rigidbody2D>();
            }
            simulations[0].player_num = int.Parse(dataRecieved[1]);
        }
        else if (dataRecieved[0] == ID_POS_JUGADOR)
        {
            InputHandler simulation = simulations[0];
            Rigidbody2D simRb = null;
            if (dataRecieved[1] == "1")
            {
                simulation = (simulations[0].player_num == 1) ? simulations[0] : simulations[1];
                simRb = (simulations[0].player_num == 1) ? simulationsRbs[0] : simulationsRbs[1];
            }
            else if (dataRecieved[1] == "2")
            {
                simulation = (simulations[0].player_num == 2) ? simulations[0] : simulations[1];
                simRb = (simulations[0].player_num == 1) ? simulationsRbs[0] : simulationsRbs[1];
            }
            simulation.transform.position = new Vector3(float.Parse(dataRecieved[2]), float.Parse(dataRecieved[3]), float.Parse(dataRecieved[4]));
            simRb.velocity = new Vector2(float.Parse(dataRecieved[5]), float.Parse(dataRecieved[6]));

        }
        else if (dataRecieved[0] == ID_POS_CAIXA)
        {
            if (stones == null) stones = GameObject.FindWithTag("Stones");
            if (stones == null) return;
            int i = 0;
            foreach (Transform child in stones.transform)
            {
                if(i == int.Parse(dataRecieved[1]))
                {
                    child.position = new Vector3(float.Parse(dataRecieved[2]), float.Parse(dataRecieved[3]), float.Parse(dataRecieved[4]));
                }
                i++;
            }
        }
        else if (dataRecieved[0] == ID_INPUTS)
        {
            InputHandler simulation = simulations[1];
            if(dataRecieved[1] == "1")simulation.GetComponent<SimulationController>().Jump();
            simulation.GetComponent<SimulationController>().dir = int.Parse(dataRecieved[2]);
        }
        else if (dataRecieved[0] == ID_BOTO_POLSAT)
        {
            if (dataRecieved[1] == "0") FindObjectOfType<buttonSimulation>().buttonNotPressed();
            if (dataRecieved[1] == "1") FindObjectOfType<buttonSimulation>().buttonPressed();
        }
        else if (dataRecieved[0] == ID_FINAL)//cargar escena ganar
        {
            if (simulations[0].player_num == int.Parse(dataRecieved[1])) { simulations[0].gameObject.SetActive(false); }
            else simulations[1].gameObject.SetActive(false);
            FindObjectOfType<endLvlSim>().EnterDoor();
			if(dataRecieved[2] == "2")SceneManager.LoadScene("EndOfGame");

        }
        else if (dataRecieved[0] == ID_JUGADOR_MORT)//cargar escena perder
        {
            if (simulations[0].player_num == int.Parse(dataRecieved[1])) { simulations[0].GetComponent<SimulationController>().Die(); }
            else simulations[1].GetComponent<SimulationController>().Die();
            SceneManager.LoadScene("YouLose");
        }


    }

    public void SendToServer(string message)
    {
        networkHelper.SendToServer(message);
    }

}
