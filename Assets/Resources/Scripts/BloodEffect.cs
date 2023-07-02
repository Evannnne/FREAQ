using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodEffect : MonoBehaviour
{
    public GameObject bloodCube;
    public int numProjectiles;
    public float velocity;

    void Start()
    {
        for(int i = 0; i < numProjectiles; i++)
        {
            var inst = Instantiate(bloodCube);
            inst.transform.position = transform.position;
            inst.SetActive(true);
            inst.GetComponent<Rigidbody>().velocity = Random.onUnitSphere * velocity;
        }
    }
}
