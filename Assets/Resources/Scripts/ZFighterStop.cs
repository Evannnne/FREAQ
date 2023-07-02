using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZFighterStop : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale *= Random.Range(0.995f, 1.005f);   
    }
}
