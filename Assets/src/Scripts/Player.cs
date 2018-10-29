using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : PlayerBehavior {

	private string[] nameParts = new string[] {"crazy", "cat", "dog", "homie", "bobble", "mr", "ms", "mrs", "castle", "flip", "flop" };

	public string Name { get; private set; }

	protected override void NetworkStart() {
		base.NetworkStart();

		if(!networkObject.IsOwner) {
			transform.GetChild(0).gameObject.SetActive(false);
			GetComponent<FirstPersonController>().enabled = false;
			Destroy(GetComponent<Rigidbody>());
		}

		ChangeName();
	}

  private void ChangeName() {
    if (!networkObject.IsOwner) return;

		int first = Random.Range(0, nameParts.Length -1);
		int last = Random.Range(0, nameParts.Length -1);

		Name = nameParts[first] + " " + nameParts[last];
		networkObject.SendRpc(RPC_UPDATE_NAME, Receivers.AllBuffered, Name);
  }

  // Update is called once per frame
  void Update () {
		if (!networkObject.IsOwner) {
			// If we are not the owner then we set the position to the
			// position that is syndicated across the network for this player
			transform.position = networkObject.position;
			return;
		}

		// When our position changes the networkObject.position will detect the
		// change based on this assignment automatically, this data will then be
		// syndicated across the network on the next update pass for this networkObject
		networkObject.position = transform.position;
	}

	public override void UpdateName(RpcArgs args) {
		Name = args.GetNext<string>();
	}
}
