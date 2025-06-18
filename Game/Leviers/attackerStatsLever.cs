using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackerStatsLever : MonoBehaviour
{
    [Tooltip("Base attacker health")]
    [Range(0, 100)] [SerializeField] public int attackerHealth = 10;

    [Tooltip("Base attacker armor")]
    [Range(0, 20)] [SerializeField] public int attackerArmor = 0;

    [Tooltip("Base attacker melee distance for Nexus")]
    [Range(1, 5)] [SerializeField] public float attackerNexusMeleeDist = 3.5f;
}
