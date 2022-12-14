using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class InputHandler : MonoBehaviour
{
    private SimulationController simulationController;

    //Input
    //Input
    [SerializeField] CinemachineVirtualCamera cam;
    [SerializeField] string inputName = "Horizontal";
    [SerializeField] string jumpInputName = "Jump";
    [SerializeField] string shootInputName = "Shoot";

    public int jumpJustPressed = 0;
    public int shootJustPressed = 0;
    public int justGotHit = 0;
    public float dir;

    private ServerHandler sh = null;
    private ClientHandler ch = null;
    string lastData = "";

    public bool isLocal = false;
    bool camAsigned = false;
    [HideInInspector] public int player_num = 1;

    private void Awake()
    {
        simulationController = GetComponent<SimulationController>();
        var g = GameObject.FindWithTag("Handler");
        sh = g.GetComponent<ServerHandler>();
        ch = g.GetComponent<ClientHandler>();
    }

    private void SendData()
    {
        string data = "";
		data = dir.ToString() + "/" + jumpJustPressed.ToString();
        if (jumpJustPressed == 1) jumpJustPressed = 0;
        if (shootJustPressed == 1) shootJustPressed = 0;
        if (justGotHit == 1) justGotHit = 0;

        //Si no ha cambiat ninguna dada no la envia
        if (data == lastData) return;
        lastData = data;
        if (sh != null)
            sh.SendToAll(data);
        else if (ch != null)
            ch.SendToServer(data);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocal || !simulationController.isAlive) return;
        if (!camAsigned)
        {
            cam.Follow = gameObject.transform;
            camAsigned = true;
        }
        dir = Input.GetAxisRaw(inputName);
        simulationController.dir = dir;
        if (Input.GetButtonDown(jumpInputName))
        {
            simulationController.Jump();
            jumpJustPressed = 1;
        }
        if (Input.GetButtonDown(shootInputName))
        {
            shootJustPressed = 1;
        }
        SendData();
    }
}
