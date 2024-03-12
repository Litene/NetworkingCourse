using Unity.Netcode;
using UnityEngine;

public class HealthPack : Pickup {
    public override void OnNetworkSpawn() => OnTriggerAction += HealPlayer;

    void OnTriggerEnter2D(Collider2D other) {
        if (!IsServer) return;

        OnTriggerAction?.Invoke(other);
    }

    private void HealPlayer(Collider2D other) {
        Health health = other.GetComponentInParent<Health>();
        if (!health) return;
        health.Heal(25);

        int xPosition = Random.Range(-4, 4);
        int yPosition = Random.Range(-2, 2);

        GameObject newMine = Instantiate(_pickupPrefab, new Vector3(xPosition, yPosition, 0), Quaternion.identity);
        NetworkObject no = newMine.GetComponent<NetworkObject>();
        no.Spawn();

        NetworkObject networkObject = gameObject.GetComponent<NetworkObject>();
        networkObject.Despawn();
    }
}
