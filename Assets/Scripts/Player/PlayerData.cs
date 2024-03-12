using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerData : NetworkBehaviour {
	[SerializeField] public NetworkVariable<FixedString64Bytes> AccountName = new ("Not Set", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
	
	
	public void SetName(string playerName) => AccountName.Value = playerName;
}