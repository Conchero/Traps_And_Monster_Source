using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killerStatsLever : MonoBehaviour
{
    [Tooltip("Base killer health")]
    [Range(0, 100)] [SerializeField] public int killerHealth = 10;

    [Tooltip("Base killer armor")]
    [Range(0, 20)] [SerializeField] public int killerArmor = 0;

    [Tooltip("Base killer melee distance for Nexus")]
    [Range(1, 5)] [SerializeField] public float killerNexusMeleeDist = 3.5f;
}
