using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCullingDistanceMenu : MonoBehaviour
{

    public float m_foliageCullingDistance;
    // Start is called before the first frame update
    void Start()
    {
        Camera cam = GetComponent<Camera>();
        //cam.cullingMask =
        //        (1 << LayerMask.NameToLayer("Default"))
        //        | (1 << LayerMask.NameToLayer("TransparentFX"))
        //        | (1 << LayerMask.NameToLayer("Ignore Raycast"))
        //        | (1 << LayerMask.NameToLayer("Water"))
        //        | (1 << LayerMask.NameToLayer("UI"))
        //        //| (1 << LayerMask.NameToLayer("P" + (i + 1)))
        //        | (1 << LayerMask.NameToLayer("Cadavre"))
        //        | (1 << LayerMask.NameToLayer("Decor"))
        //        | (1 << LayerMask.NameToLayer("GrassRed"))
        //        | (1 << LayerMask.NameToLayer("GrassBlue"))
        //        | (1 << LayerMask.NameToLayer("GrassGreen"))
        //        | (1 << LayerMask.NameToLayer("GrassYellow"))
        //        | (1 << LayerMask.NameToLayer("Foliage"))
        //        | (1 << LayerMask.NameToLayer("Trap"))
        //        | (1 << LayerMask.NameToLayer("Demons"))
        //        | (1 << LayerMask.NameToLayer("Invoc"));

        //Culling distance
        float[] distances = new float[32];
        distances[LayerMask.NameToLayer("GrassRed")] = m_foliageCullingDistance;
        distances[LayerMask.NameToLayer("GrassBlue")] = m_foliageCullingDistance;
        distances[LayerMask.NameToLayer("GrassGreen")] = m_foliageCullingDistance;
        distances[LayerMask.NameToLayer("GrassYellow")] = m_foliageCullingDistance;
        cam.layerCullDistances = distances;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
