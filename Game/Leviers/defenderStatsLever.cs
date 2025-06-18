using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class defenderStatsLever : MonoBehaviour
{
    [Tooltip("Base defender health")]
    [Range(0, 100)] [SerializeField] public int defenderHealth = 10;

    [Tooltip("Base defender armor")]
    [Range(0, 20)] [SerializeField] public int defenderArmor = 3;

    [Tooltip("Base defender melee distance for Nexus")]
    [Range(1, 5)] [SerializeField] public float defenderNexusMeleeDist = 3.5f;
}
