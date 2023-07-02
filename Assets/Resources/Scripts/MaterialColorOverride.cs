using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialColorOverride : MonoBehaviour
{
    public Color overrideColor;

    [SerializeField] private Renderer target;
    [SerializeField] private int materialIndex;


    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            if (materialIndex < target.materials.Length && materialIndex >= 0)
                target.materials[materialIndex].color = overrideColor;
        }   
    }
}
