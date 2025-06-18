//Script by : Alexis

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//to put in the damage zone of the trap

public class TrapAttack : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject spawnPoint;
    [SerializeField] GameObject spikes;

    List<GameObject> target = new List<GameObject>();
    List<int> indexToDelete = new List<int>();
    public float shootingTime = 2f;
    public float totalCooldown = 7f; //complete time between two shot
    public int damage = 5; //damage dealt each attack
    public float timer = 0;
    public bool readyToUse = true;
    public bool firing = false;
    public bool inCooldown = false;

    public GameObject player = null;
    string playerColor;

    public Stats scoreboardStats;
    [SerializeField] GameObject hitParticles;


    // Start is called before the first frame update
    void Start()
    {
        if (player != null)
        {
            playerColor = player.GetComponent<EntityPlayer>().m_sColor;
            scoreboardStats = (Stats)GameObject.FindObjectOfType(typeof(Stats));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            return;
        }
        if (firing || inCooldown || (readyToUse && target.Count > 0))
        {
            Firing();
        }

        if (animator != null)
        {
            if (firing)
            {
                animator.SetBool("isFiring", true);
            }
            else
            {
                animator.SetBool("isFiring", false);
            }
        }

        if(spawnPoint != null && spawnPoint.transform.childCount == 0 && firing)
        {
            Instantiate(spikes, spawnPoint.transform);
        }

        UpdateListTarget();

    }

    private void Firing()
    {
        {
            timer += Time.deltaTime;
            if (timer < shootingTime && !firing)
            {
                InvokeRepeating("DamagingEnemies", 0f, 0.4f);
                firing = true;
            }
            else if (firing && !inCooldown && timer < totalCooldown && timer >= shootingTime)
            {
                CancelInvoke();
                firing = false;
                inCooldown = true;
            }
            else if (timer > totalCooldown)
            {
                readyToUse = true;
                inCooldown = false;
                timer = 0;
            }
        }
    }

    private void DamagingEnemies()
    {
        foreach (GameObject hit in target)
        {
            if (hit == null)
            {
                indexToDelete.Add(target.IndexOf(hit));
                continue;
            }
            else if (hit.gameObject.tag == "Ennemi")
            {
                if (!hit.GetComponent<Ennemi>().RemoveLife(damage, player))
                {
                    indexToDelete.Add(target.IndexOf(hit));

                    //increase the scoreboard
                    scoreboardStats.nbKillDemons[playerColor] += 1;
                    scoreboardStats.nbKillEnnemieByTrap[playerColor] += 1;

                }
            }
            else if (hit.gameObject.tag == "Invocation")
            {
                if(!hit.GetComponent<comportementGeneralIA>().RemoveLife(damage, player))
                {
                    indexToDelete.Add(target.IndexOf(hit));
                    //increase the scoreboard
                    scoreboardStats.nbKillInvoc[playerColor] += 1;
                    scoreboardStats.nbKillEnnemieByTrap[playerColor] += 1;

                }
            }
            else if (hit.gameObject.tag == "Player")
            {

                if (!hit.GetComponent<EntityPlayer>().RemoveLife(damage, player, null))
                {
                    indexToDelete.Add(target.IndexOf(hit));
                    //increase the scoreboard
                    scoreboardStats.nbKillPlayer[playerColor] += 1;
                    scoreboardStats.nbKillEnnemieByTrap[playerColor] += 1;

                }
            }

            //particles effect
            if(hitParticles!=null)
            {
                GameObject hitEffect = Instantiate(hitParticles, hit.transform.position + Vector3.up, hitParticles.transform.rotation);
                Destroy(hitEffect, 1f);
            }
        }
        indexToDelete.Sort();
        indexToDelete.Reverse();
        foreach (int i in indexToDelete)
        {
            target.RemoveAt(i);
        }
        indexToDelete.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (player == null)
        {
            return;
        }
        if (other.gameObject.tag == "Ennemi" && (other.gameObject.GetComponent<Ennemi>().m_entityPlayer == null || other.gameObject.GetComponent<Ennemi>().m_entityPlayer.m_playerId != player.GetComponent<EntityPlayer>().m_playerId))
        {
            if (GetComponentInParent<Traps>().type == Traps.TypeTrap.WALL_TRAP)
            {
                SoundManager.Instance.WallActivationPlay(gameObject);
                SoundManager.Instance.WallEnemyHitPlay(gameObject);
            }
            else
            {
                SoundManager.Instance.LogHitPlay(gameObject);
            }

            target.Add(other.gameObject);
        }
        if (other.gameObject.tag == "Invocation" && (other.gameObject.GetComponent<comportementGeneralIA>().m_entityPlayer == null || other.gameObject.GetComponent<comportementGeneralIA>().m_entityPlayer.m_playerId != player.GetComponent<EntityPlayer>().m_playerId))
        {
            if (GetComponentInParent<Traps>().type == Traps.TypeTrap.WALL_TRAP)
            {
                SoundManager.Instance.WallActivationPlay(gameObject);
                SoundManager.Instance.WallEnemyHitPlay(gameObject);
            }
            else
            {
                SoundManager.Instance.LogHitPlay(gameObject);
            }

            target.Add(other.gameObject);
        }
        if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<EntityPlayer>().m_playerId != player.GetComponent<EntityPlayer>().m_playerId)
        {
            if (GetComponentInParent<Traps>().type == Traps.TypeTrap.WALL_TRAP)
            {
                SoundManager.Instance.WallActivationPlay(gameObject);
                SoundManager.Instance.WallEnemyHitPlay(gameObject);
            }
            else
            {
                SoundManager.Instance.LogHitPlay(gameObject);
            }
            target.Add(other.gameObject);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (player == null)
        {
            return;
        }
        target.Remove(other.gameObject);
    }

    public void SetPlayer(GameObject _player)
    {
        player = _player;
    }

    private void UpdateListTarget()
    {
        foreach (GameObject hit in target)
        {
            if (hit == null)
            {
                indexToDelete.Add(target.IndexOf(hit));
                continue;
            }
            else if (hit.gameObject.tag == "Ennemi")
            {
                if (hit.GetComponent<Ennemi>().m_isDead)
                {
                    indexToDelete.Add(target.IndexOf(hit));
                }
            }
            else if (hit.gameObject.tag == "Invocation")
            {
                if (!hit.GetComponent<comportementGeneralIA>().RemoveLife(0, player))
                {
                    indexToDelete.Add(target.IndexOf(hit));
                }
            }
            else if (hit.gameObject.tag == "Player")
            {

                if (hit.GetComponent<EntityPlayer>().m_isDead)
                {
                    indexToDelete.Add(target.IndexOf(hit));
                }
            }
        }
        indexToDelete.Sort();
        indexToDelete.Reverse();
        foreach (int i in indexToDelete)
        {
            target.RemoveAt(i);
        }
        indexToDelete.Clear();
    }
}
