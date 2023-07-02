using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public void OnHit(object param)
    {
        Destroy(gameObject);
    }
}
