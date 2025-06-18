using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject portalToTP;

    void Start()
    {
        SoundManager.Instance.PortalIdlePlay(gameObject);
    }
}
