using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FountainSoundStarter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.FountainSoundPlay(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
