using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPositionning : MonoBehaviour
{

    public UIAim m_UIAim;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = m_UIAim.m_target;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = m_UIAim.m_target;
    }
}
