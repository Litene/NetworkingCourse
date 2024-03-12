using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class Health : NetworkBehaviour {
    
    [FormerlySerializedAs("currentHealth")] public NetworkVariable<int> CurrentHealth = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private readonly List<ITraversable> _defences = new();

    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private int _shield = 2;
    [SerializeField] private int _armor = 2;

    public override void OnNetworkSpawn() {
        
        if (!IsServer) return;

        var shield = GetComponent<Shields>();
        var armor = GetComponent<Armor>();
        
        _defences.Add(armor);
        _defences.Add(shield);

        armor.OnDepleted += RemoveShield;

        if (!IsOwner) return;

        CurrentHealth.Value = _maxHealth;
    }

    private void RemoveShield() {
        Debug.Log("Shield is depleted");
    }

    public void TakeDamage(ref int damage) {
        
        damage = damage > 0 ? damage : -damage;
        Debug.Log($"Damage before defences: {damage}");
        foreach (var defence in _defences) defence.InvokeDamage(ref damage);
        Debug.Log($"Damage after defences: {damage}");
        
        CurrentHealth.Value = CurrentHealth.Value - damage > 0 ? CurrentHealth.Value - damage : 0;
    }

    public void Heal(int health) {
        var tempHealth = health > 0 ? health : -health;
        
        CurrentHealth.Value = CurrentHealth.Value + tempHealth > _maxHealth ? _maxHealth : CurrentHealth.Value + tempHealth;
    }
}

public interface ITraversable {
    public NetworkVariable<int> ValueRW { get; set; }
    public Action OnDepleted { get; set; }
    public void InvokeDamage(ref int value);
}