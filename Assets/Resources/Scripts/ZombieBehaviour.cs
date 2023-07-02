using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieBehaviour : MonoBehaviour
{

    private void Awake()
    {
        var rbs = GetComponentsInChildren<Rigidbody>();
        foreach (var rb in rbs)
            rb.isKinematic = true;
    }
    public void OnHit(object foo)
    {
        var rbs = GetComponentsInChildren<Rigidbody>();
        foreach (var rb in rbs)
        {
            rb.isKinematic = false;
            rb.velocity = Vector3.zero;
        }
        Destroy(GetComponentInChildren<Animator>());
        Destroy(GetComponentInChildren<NavMeshAgent>());
    }

}
