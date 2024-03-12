using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Pickup : NetworkBehaviour {
    [FormerlySerializedAs("pickupPrefab")] [SerializeField] protected GameObject _pickupPrefab;

    protected Action<Collider2D> OnTriggerAction;
}