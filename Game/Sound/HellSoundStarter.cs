﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellSoundStarter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.HellSoundPlay(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
