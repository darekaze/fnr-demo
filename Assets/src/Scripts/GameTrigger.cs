using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine;

public class GameTrigger : MonoBehaviour {

	private bool started;

	void Update () {
		if (FindObjectOfType<GameBall>() != null) {
			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter(Collider c) {
		if (started) return;
		
		// Only allow the server player to start the game
		if (!NetworkManager.Instance.IsServer)
			return;
			
		Player player = c.GetComponent<Player>();
		if (player == null) return;
		started = true;

		// create the ball on the network
		GameBall ball = NetworkManager.Instance.InstantiateGameBall() as GameBall;
		ball.Reset();
		Destroy(gameObject);
	}
}
