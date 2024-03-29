using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;


// this should be refactored... its the same behavior.. 
public class StandardMine : Pickup {
    public override void OnNetworkSpawn() => OnTriggerAction += DamagePlayer;
    [SerializeField] private int _damage = 25;
    void OnTriggerEnter2D(Collider2D other) {
        if (!IsServer) return;

        OnTriggerAction?.Invoke(other);
    }

    private void DamagePlayer(Collider2D other) {
        Health health = other.GetComponentInParent<Health>();
        if (!health) return;
        
        var tempDamage = _damage;
        health.TakeDamage(ref tempDamage);

        int xPosition = Random.Range(-4, 4);
        int yPosition = Random.Range(-2, 2);

        GameObject newMine = Instantiate(_pickupPrefab, new Vector3(xPosition, yPosition, 0), Quaternion.identity);
        NetworkObject no = newMine.GetComponent<NetworkObject>();
        no.Spawn();

        NetworkObject networkObject = gameObject.GetComponent<NetworkObject>();
        networkObject.Despawn();
    }
}