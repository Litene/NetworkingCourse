using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerData : NetworkBehaviour {

	public NetworkVariable<FixedString64Bytes> AccountName { get; private set; } = new NetworkVariable<FixedString64Bytes>();

	public void SetName(string playerName) {
		AccountName.Value = playerName;

	}

}