using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : GameLogicBehavior {

	[SerializeField]
	private Text scoreLabel;
	void Start () {
		// NetworkManager.Instance.InstantiatePlayer(position: new Vector3(0, 5, 0));
	}
	
	public override void PlayerScored(RpcArgs args) {

		string playerName = args.GetNext<string>();

		scoreLabel.text = "Last player to score was: " + playerName;
	}
}
