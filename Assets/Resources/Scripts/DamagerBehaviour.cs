using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagerBehaviour : MonoBehaviour
{
    public float damage;
    public float force;
    public virtual void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.SendMessageUpwards("OnHit", new HitData() { 
            damage = damage, 
            force = (collision.contacts[0].point - transform.position).normalized * force, 
            point = collision.contacts[0].point,
            hitGameObject = collision.gameObject
        }, SendMessageOptions.DontRequireReceiver);
    }
}
