//par Max Bonnaud
using System.Collections;
using System.Collections.Generic;
//using UnityEditor;
using UnityEngine;

public class WaveManagerLevers : MonoBehaviour
{

    [Tooltip("base wave points, added to diffuculty per wave")]
    [Range(0, 50)][SerializeField] public int startingEnnemyPoints = 5;

    [Tooltip("points per wave, added to base points")]
    [Range(0, 50)][SerializeField] public int additionnalPointPerWave = 3;

    [Tooltip("break time in seconds between to waves")]
    [Range(0.0f, 300.0f)] [SerializeField] public float timeBetweenWaves = 60.0f;

}

