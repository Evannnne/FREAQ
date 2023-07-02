using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnerBehaviour : MonoBehaviour
{
    public float delay = 10;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Despawn", delay);   
    }
    void Despawn() { Destroy(gameObject); }
}
