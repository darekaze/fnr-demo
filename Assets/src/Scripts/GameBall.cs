using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using UnityEngine;

public class GameBall : GameBallBehavior {

	private Rigidbody rigidbodyRef;
	private GameLogic gameLogic;

	private void Awake() {
		rigidbodyRef = GetComponent<Rigidbody>();
		gameLogic = FindObjectOfType<GameLogic>();
	}
	
	void Update () {
		if (!networkObject.IsOwner) {
			transform.position = networkObject.position;
			return;
		}
		networkObject.position = transform.position;
	}

	public void Reset() {
		transform.position = Vector3.up * 10;
		rigidbodyRef.velocity = Vector3.zero;

		// Create a random force
		Vector3 force = new Vector3(0, 0, 0);
		force.x = Random.Range(300, 500);
		force.z = Random.Range(300, 500);

		// Randomly invert
		if (Random.value < 0.5f)
			force.x *= -1;

		if (Random.value < 0.5f)
			force.z *= -1;

		rigidbodyRef.AddForce(force);
	}

	private void OnCollisionEnter(Collision c) {

		if (!networkObject.IsServer)
			return;

		// Only move if a player touched the ball
		if (c.gameObject.GetComponent<Player>() == null)
			return;

		// Call an RPC on the Game Logic to print the player's name as the last
		// player to touch the ball
		gameLogic.networkObject.SendRpc(
			GameLogicBehavior.RPC_PLAYER_SCORED,
			Receivers.All,
			c.gameObject.GetComponent<Player>().Name
		);

		Reset();
	}
}
