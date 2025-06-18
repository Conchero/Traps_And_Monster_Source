using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestroy : MonoBehaviour
{
    Vector3 basePose;
    Pistol pistol;
    string name;

    // Start is called before the first frame update
    void Start()
    {
        basePose = gameObject.transform.position;
        name = gameObject.name;

        pistol = GameObject.Find(name.Replace("Bullet", "") + "/Arm/Bow").GetComponent<Pistol>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.x > basePose.x + 500)
        {
            Destroy(gameObject);
            pistol.ComboStreak = 0;
            if (pistol.ComboBullet == true)
            {
                pistol.ComboBullet = false;
                pistol.ComboStreak = 0;
            }
        }
        else if (gameObject.transform.position.x < basePose.x - 500)
        {
            Destroy(gameObject);
            pistol.ComboStreak = 0;
            if (pistol.ComboBullet == true)
            {
                pistol.ComboBullet = false;
                pistol.ComboStreak = 0;
            }
        }
        else if (gameObject.transform.position.y > basePose.y + 500)
        {
            Destroy(gameObject);
            pistol.ComboStreak = 0;
            if (pistol.ComboBullet == true)
            {
                pistol.ComboBullet = false;
                pistol.ComboStreak = 0;
            }
        }
        else if (gameObject.transform.position.y < basePose.y - 500)
        {
            Destroy(gameObject);
            pistol.ComboStreak = 0;
            if (pistol.ComboBullet == true)
            {
                pistol.ComboBullet = false;
                pistol.ComboStreak = 0;
            }
        }
        else if (gameObject.transform.position.z > basePose.z + 500)
        {
            Destroy(gameObject);
            pistol.ComboStreak = 0;
            if (pistol.ComboBullet == true)
            {
                pistol.ComboBullet = false;
                pistol.ComboStreak = 0;
            }
        }
        else if (gameObject.transform.position.z < basePose.z - 500)
        {
            Destroy(gameObject);
            pistol.ComboStreak = 0;
            if (pistol.ComboBullet == true)
            {
                pistol.ComboBullet = false;
                pistol.ComboStreak = 0;
            }
        }

    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("EnnemiBody"))
        {
            Destroy(gameObject);

            if (pistol.ChargedBullet == false)
            {
              collider.gameObject.GetComponentInParent<Ennemi>().RemoveLife(pistol.Stats[0], pistol.gameObject.transform.parent.gameObject.GetComponentInParent<EntityPlayer>().gameObject);
            }
            else if (pistol.ChargedBullet == true)
            {
                collider.gameObject.GetComponentInParent<Ennemi>().RemoveLife(pistol.Stats[0]+1, pistol.gameObject.transform.parent.gameObject.GetComponentInParent<EntityPlayer>().gameObject);
            }

            if (pistol.ComboBullet == true)
            {
                collider.gameObject.GetComponentInParent<Ennemi>().RemoveLife(pistol.Stats[0], pistol.gameObject.transform.parent.gameObject.GetComponentInParent<EntityPlayer>().gameObject);
                pistol.ComboBullet = false;
                pistol.ComboStreak = 0;
                Debug.Log("Froze !");
            }

            collider.gameObject.GetComponentInParent<Rigidbody>().AddForce(gameObject.transform.TransformVector(Vector3.forward) * 5, ForceMode.Impulse);
            pistol.ComboStreak++;
            pistol.ComboTimer = 0;

            Debug.Log(pistol.ComboStreak);
        }
        else if (collider.CompareTag("EnnemiHead"))
        {
            Destroy(gameObject);

            if (pistol.ChargedBullet == false)
            {
                collider.gameObject.GetComponentInParent<Ennemi>().RemoveLife(pistol.Stats[0] * 4, pistol.gameObject.transform.parent.gameObject.GetComponentInParent<EntityPlayer>().gameObject);
            }
            else if (pistol.ChargedBullet == true)
            {
                collider.gameObject.GetComponentInParent<Ennemi>().RemoveLife((pistol.Stats[0] + 1) * 4, pistol.gameObject.transform.parent.gameObject.GetComponentInParent<EntityPlayer>().gameObject);
            }

            if (pistol.ComboBullet == true)
            {
                collider.gameObject.GetComponentInParent<Ennemi>().RemoveLife(pistol.Stats[0] * 4, pistol.gameObject.transform.parent.gameObject.GetComponentInParent<EntityPlayer>().gameObject);
                pistol.ComboBullet = false;
                pistol.ComboStreak = 0;
                Debug.Log("Froze !");
            }

            collider.gameObject.GetComponentInParent<Rigidbody>().AddForce(gameObject.transform.TransformVector(Vector3.forward) * 5, ForceMode.Impulse);
            pistol.ComboStreak++;
            pistol.ComboTimer = 0;

            Debug.Log(pistol.ComboStreak);
        }
        else if (collider.CompareTag("Ground"))
        {
            Destroy(gameObject);
            pistol.ComboBullet = false;
            pistol.ComboStreak = 0;
        }
        else if (collider.CompareTag("Wall"))
        {
            Destroy(gameObject);
             pistol.ComboBullet = false;
            pistol.ComboStreak = 0;
        }

        else if (collider.CompareTag("PlayerBody"))
        {
            EntityPlayer tempPlayer = collider.transform.GetComponentInParent<Transform>().GetComponentInParent<Transform>().GetComponentInParent<EntityPlayer>();

            Destroy(gameObject);


            if (pistol.ChargedBullet == false)
            {
                tempPlayer.RemoveLife(pistol.Stats[0], pistol.gameObject.transform.parent.gameObject.GetComponentInParent<EntityPlayer>().gameObject, null);
            }
            else if (pistol.ChargedBullet == true)
            {
                tempPlayer.RemoveLife(pistol.Stats[0] + 1, pistol.gameObject.transform.parent.gameObject.GetComponentInParent<EntityPlayer>().gameObject, null);
            }

            if (pistol.ComboBullet == true)
            {
                tempPlayer.RemoveLife(pistol.Stats[0], pistol.gameObject.transform.parent.gameObject.GetComponentInParent<EntityPlayer>().gameObject, null);
                pistol.ComboBullet = false;
                pistol.ComboStreak = 0;
              //  Debug.Log("Froze !");
            }

          //  collision.collider.gameObject.GetComponentInParent<Rigidbody>().AddForce(gameObject.transform.TransformVector(Vector3.forward) * 5, ForceMode.Impulse);
            pistol.ComboStreak++;
            pistol.ComboTimer = 0;

//            Debug.Log(pistol.ComboStreak);

           

            
        }
        else if (collider.CompareTag("PlayerHead"))
        {
            EntityPlayer tempPlayer = collider.transform.GetComponentInParent<Transform>().GetComponentInParent<Transform>().GetComponentInParent<EntityPlayer>();

            Destroy(gameObject);

            if (pistol.ChargedBullet == false)
            {
                tempPlayer.RemoveLife(pistol.Stats[0] * 4, pistol.gameObject.transform.parent.gameObject.GetComponentInParent<EntityPlayer>().gameObject, null);
            }
            else if (pistol.ChargedBullet == true)
            {
                tempPlayer.RemoveLife((pistol.Stats[0] + 1) * 4, pistol.gameObject.transform.parent.gameObject.GetComponentInParent<EntityPlayer>().gameObject, null);
            }

            if (pistol.ComboBullet == true)
            {
                tempPlayer.RemoveLife(pistol.Stats[0] * 4, pistol.gameObject.transform.parent.gameObject.GetComponentInParent<EntityPlayer>().gameObject, null);
                pistol.ComboBullet = false;
                pistol.ComboStreak = 0;
              //  Debug.Log("Froze !");
            }

           // collision.collider.gameObject.GetComponentInParent<Rigidbody>().AddForce(gameObject.transform.TransformVector(Vector3.forward) * 5, ForceMode.Impulse);
            pistol.ComboStreak++;
            pistol.ComboTimer = 0;

          //  Debug.Log(pistol.ComboStreak);
        }
    }
}
