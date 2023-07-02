using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class UVRescaler : MonoBehaviour
{
    public Vector2 targetScale = Vector2.one;

    private void _Set()
    {
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        block.SetVector("_MainTex_ST", targetScale);
        var meshRenderer = GetComponentInChildren<MeshRenderer>();
        if(meshRenderer != null) meshRenderer.SetPropertyBlock(block);
    }

    public void OnValidate() => _Set();

    public void Start() => _Set();
}
