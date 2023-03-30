using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour {

    [SerializeField] private int damage = 25;

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Enemy") {
            Health health = collision.GetComponent<Health>();
            health.Damage(damage);

        }
    }

}
