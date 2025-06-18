using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyerStatsLever : MonoBehaviour
{
    [Tooltip("Base destroyer health")]
    [Range(0, 100)] [SerializeField] public int destroyerHealth = 10;

    [Tooltip("Base destroyer armor")]
    [Range(0, 20)] [SerializeField] public int destroyerArmor = 0;

    [Tooltip("Base destroyer melee distance for Nexus")]
    [Range(1, 5)] [SerializeField] public float destroyerNexusMeleeDist = 3.5f;
}
