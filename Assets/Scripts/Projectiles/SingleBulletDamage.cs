using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleBulletDamage : MonoBehaviour {
    [SerializeField] int damage = 5;

    void OnTriggerEnter2D(Collider2D other) {
        Health health = other.transform.GetComponentInParent<Health>();
        if (health == null) return;
        var tempDamage = damage;
        health.TakeDamage(ref tempDamage);
    }
}