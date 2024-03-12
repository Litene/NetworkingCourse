using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HostManager : ScriptableObject {

	public string joinCode = "";
	private const int MaxConnections = 20;
	Allocation allocation;


	public async Task<bool> InitSeverAsync() {
		await Task.Delay(0);
		return true;
	}


	public async Task StartHostAsync() {
		allocation = await Relay.Instance.CreateAllocationAsync(MaxConnections);

		if (allocation != null) joinCode = await Relay.Instance.GetJoinCodeAsync(allocation.AllocationId);
		Debug.Log("JoinCode: " + joinCode);

		UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
		if (transport == null) return;

		RelayServerData relayServerData = new RelayServerData(allocation, "udp"); // dtls
		transport.SetRelayServerData(relayServerData);
		NetworkManager.Singleton.StartHost();
		NetworkManager.Singleton.SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
	}

}