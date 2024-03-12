using System;
using Unity.Netcode;

public class Armor : NetworkBehaviour, ITraversable {
    public NetworkVariable<int> ValueRW { get; set; } =
        new(0, NetworkVariableReadPermission.Owner, NetworkVariableWritePermission.Owner);

    public Action OnDepleted { get; set; }

    public override void OnNetworkSpawn() {
        if (!IsOwner) return;

        ValueRW.Value = 3;
    }

    public void InvokeDamage(ref int value) {
        if (!IsOwner) return;
        value = value - ValueRW.Value;
    }
}