using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : DamagerBehaviour
{
    public float startSize = 0.1f;
    public float endSize = 0.5f;
    public float time = 0.25f;

    private void Start()
    {
        transform.localScale = Vector3.one * startSize;
    }
    private void Update()
    {
        float s = transform.localScale.x;
        s += (endSize - startSize) * Time.deltaTime / time;
        s = Mathf.Clamp(s, startSize, endSize);
        transform.localScale = Vector3.one * s;
    }
    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        Destroy(gameObject);
    }
}
