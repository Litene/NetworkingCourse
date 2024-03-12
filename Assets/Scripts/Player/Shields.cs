using System;
using Unity.Netcode;
using UnityEngine;

public class Shields : NetworkBehaviour, ITraversable {
    public NetworkVariable<int> ValueRW { get; set; } = new(0, NetworkVariableReadPermission.Owner, NetworkVariableWritePermission.Owner);
    public Action OnDepleted { get; set; }

    public override void OnNetworkSpawn() {
        if (!IsOwner) return;
        ValueRW.Value = 2;
    }

    public void InvokeDamage(ref int value) {
        Debug.Log(value);
        if (!IsOwner) return;
        
        value = ValueRW.Value > 0 ? 0 : value;
        ValueRW.Value--;
        
        if (ValueRW.Value == 0) OnDepleted?.Invoke();
    }
}