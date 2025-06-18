//script by : Alexis

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tower : MonoBehaviour
{
    public GameObject target = null;
    public Transform partToRotate;
    public Transform firePoint;

    //attributes
    float range;
    float shootCooldown;
    private float shootTimer = 0f;

    [Header("Attributes")]
    
    public int currentTier = 0;

    [Header("Need")]
    [SerializeField] GameObject bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        range = 5.5f;
        shootCooldown = 1.5f;
        InvokeRepeating("UpdateTarget", 0f, 0.1f);
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.tag == "partRotate")
            {
                partToRotate = child;
            }

        }
        for (int i = 0; i < partToRotate.childCount; i++)
        {
            Transform child = partToRotate.GetChild(i);
            if (child.tag == "firePoint")
            {
                firePoint = child;
            }

        }
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("foe");
        bool noEnemyInRange = true;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if(distanceToEnemy < range)
            {
                noEnemyInRange = false;
                if(target == null)
                {
                    target = enemy;
                }
            }
            else if(noEnemyInRange)
            {
                target = null;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer -= Time.deltaTime;
        if(target == null)
        {
            return;
        }

        Vector3 dir = target.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = lookRotation.eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
      

        if(shootTimer<=0f)
        {
            Shoot();
            target = null;
            shootTimer = shootCooldown;
        }
    }

    private void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
