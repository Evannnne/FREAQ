using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieBehaviour : MonoBehaviour
{
    public GameObject bloodEffectPrefab;

    private NavMeshAgent m_nma;
    private Animator m_animator;

    private void Awake()
    {
        m_nma = GetComponentInChildren<NavMeshAgent>();
        m_animator = GetComponentInChildren<Animator>();
        var rbs = GetComponentsInChildren<Rigidbody>();
        foreach (var rb in rbs)
            rb.isKinematic = true;
    }

    Vector3 recognizedPosition = Vector3.zero;
    private void Update()
    {
        if (Vector3.Distance(recognizedPosition, PlayerController.Instance.transform.position) > 5)
        {
            m_nma.SetDestination(PlayerController.Instance.transform.position);
            recognizedPosition = PlayerController.Instance.transform.position;
        }
        else transform.forward = Vector3.RotateTowards(transform.forward, (PlayerController.Instance.transform.position - transform.position).normalized, 90 * Mathf.Deg2Rad * Time.deltaTime, 9999);

        m_animator.SetBool("Moving", m_nma.velocity.magnitude >= 1);
    }

    public void OnHit(object hit)
    {
        var rbs = GetComponentsInChildren<Rigidbody>();
        foreach (var rb in rbs)
        {
            rb.isKinematic = false;
            rb.velocity = Vector3.zero;
        }

        HitData data = (HitData)hit;
        var hitRb = data.hitGameObject.GetComponent<Rigidbody>();
        if(hitRb != null)
        {
            hitRb.AddForceAtPosition(data.force, data.point);
        }

        gameObject.AddComponent<DespawnerBehaviour>();
        Destroy(GetComponentInChildren<Animator>());
        Destroy(GetComponentInChildren<NavMeshAgent>());
        Destroy(this);

        var blood = Instantiate(bloodEffectPrefab);
        blood.transform.position = data.point;
    }

}
