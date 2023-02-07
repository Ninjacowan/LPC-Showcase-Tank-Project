using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class lightDisable : MonoBehaviour
{
    public Light light;
    public MeshRenderer meshRenderer;
    [PreviewField] public Material glass;
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.name);

        if (collision.gameObject.name == "bullet" || collision.gameObject.name == "bullet(Clone)" || collision.gameObject.name == "Tank Hull")
        {
            light.enabled = false;
            //Debug.Log(meshRenderer.materials[1]);
            Material[] mats = meshRenderer.materials;
            mats[1] = glass;
            meshRenderer.materials = mats;

        }

    }
}
