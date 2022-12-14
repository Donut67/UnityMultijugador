using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeerController : MonoBehaviour
{
	[SerializeField] PeerController otherPeer;
	[SerializeField] PlayerController player;
	[SerializeField] SimulationController simulation;
	private string[] dataRecieved;

	private void LateUpdate() {
		sendData();
	}
	private void sendData() {
		string data = "";
		data = player.myRigidbody.velocity.x.ToString() + "/"  + player.jumpJustPressed.ToString() + "/" + player.shootJustPressed.ToString() + "/" + player.Alive.ToString() + "/" + player.justGotHit.ToString();
		if (player.jumpJustPressed == 1) player.jumpJustPressed = 0;
		if (player.shootJustPressed == 1) player.shootJustPressed = 0;
		if (player.justGotHit == 1) player.justGotHit = 0;
		otherPeer.recieveData(data);
	}

	public void recieveData(string data) {
		dataRecieved = data.Split('/');
		simulation.dir = float.Parse(dataRecieved[0]);
		if(dataRecieved[1] == "1") {
			simulation.Jump();
		}
		if(dataRecieved[2] == "1"){
			simulation.Shoot();
		}
		if(dataRecieved[3] == "0") {
			simulation.Die();
		}
		if (dataRecieved[4] == "1") {
			simulation.FlashWhite();
		}
	}
}
