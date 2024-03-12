using System;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using static FGAuthentication;

public class ClientManager : ScriptableObject {

	JoinAllocation allocation;

	public async Task<bool> InitAsync() {
		await UnityServices.InitializeAsync();
		AuthState currentState = await FGAuthentication.FGAuthen(5);
		bool isAuth = (currentState == AuthState.Authorized);
		return isAuth;
	}

	public async Task StartClientAsync(String joinCode) {
		allocation = await Relay.Instance.JoinAllocationAsync(joinCode);
		UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
		if (transport is null) return;

		RelayServerData relayServerData = new RelayServerData(allocation, "udp");
		transport.SetRelayServerData(relayServerData);
		NetworkManager.Singleton.StartClient();

	}


}