using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDestroy : MonoBehaviour
{
    Vector3 basePose;
    LaserSword laserSword;
    string name;

    // Start is called before the first frame update
    void Start()
    {
        basePose = gameObject.transform.position;
        name = gameObject.name;

        laserSword = GameObject.Find(name.Replace("Laser", "")).GetComponentInChildren<GetHand>().hand.GetComponentInChildren<LaserSword>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.x > basePose.x + 200)
        {
            Destroy(gameObject);
        }
        else if (gameObject.transform.position.x < basePose.x - 200)
        {
            Destroy(gameObject);
        }
        else if (gameObject.transform.position.y > basePose.y + 200)
        {
            Destroy(gameObject);
        }
        else if (gameObject.transform.position.y < basePose.y - 200)
        {
            Destroy(gameObject);
        }
        else if (gameObject.transform.position.z > basePose.z + 200)
        {
            Destroy(gameObject);
        }
        else if (gameObject.transform.position.z < basePose.z - 200)
        {
            Destroy(gameObject);
        }

    }
    void TauntOnDistance(Collider _collider, Transform _posOther)
    {
        float distBEAP;
        distBEAP = Vector3.Distance(basePose, _posOther.position);

        if (distBEAP <= 9)
        {
            if (!_collider.gameObject.GetComponentInParent<Ennemi>().m_isTaunted)
            {
                _collider.gameObject.GetComponentInParent<Ennemi>().TauntEnnemi(laserSword.GetComponentInParent<WeaponBehaviour>().player.transform.parent.gameObject);
            }
        }

    }
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("EnnemiBody"))
        {
            Destroy(gameObject);
            collider.gameObject.GetComponentInParent<Ennemi>().RemoveLife(laserSword.Stats[1], laserSword.gameObject.transform.parent.GetComponentInParent<EntityPlayer>().gameObject);
            collider.gameObject.GetComponentInParent<Rigidbody>().AddForce(gameObject.transform.TransformVector(Vector3.forward) * 5, ForceMode.Impulse);

            laserSword.InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, laserSword.stats[1], collider.gameObject.GetComponentInParent<Ennemi>().m_armor);
            laserSword.ComboStreak++;
            laserSword.ComboTimer = 0;
            collider.gameObject.GetComponentInParent<Ennemi>().StopEnemy(0.3f);

            SoundManager.Instance.LaserHitmarkPlay(laserSword.gameObject);
            laserSword.transform.root.gameObject.GetComponentInChildren<UIAim>().hitProjectile = true;
            TauntOnDistance(collider,collider.transform);
            //Debug.Log("Ouch bullet");
        }
        else if (collider.CompareTag("EnnemiHead"))
        {
            Destroy(gameObject);
            collider.gameObject.GetComponentInParent<Ennemi>().RemoveLife(laserSword.Stats[1] * 2, laserSword.gameObject.transform.parent.GetComponentInParent<EntityPlayer>().gameObject);
            collider.gameObject.GetComponentInParent<Rigidbody>().AddForce(gameObject.transform.TransformVector(Vector3.forward) * 5, ForceMode.Impulse);
            laserSword.InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, laserSword.stats[1] * 2, collider.gameObject.GetComponentInParent<Ennemi>().m_armor);
            collider.gameObject.GetComponentInParent<Ennemi>().StopEnemy(1f);

            laserSword.ComboStreak++;
            laserSword.ComboTimer = 0;
            TauntOnDistance(collider, collider.transform);

            //Debug.Log("Ouch bullet Critical !!!");
            SoundManager.Instance.LaserHitmarkPlay(laserSword.gameObject);
            laserSword.transform.root.gameObject.GetComponentInChildren<UIAim>().hitProjectile = true;
        }
        else if (collider.CompareTag("Ground"))
        {
            Destroy(gameObject);
            laserSword.ComboTimer = 0;
            //Debug.Log("I'm destroyed (ground)");
        }
        else if (collider.CompareTag("Wall"))
        {
            Destroy(gameObject);
            laserSword.ComboTimer = 0;
            //Debug.Log("I'm destroyed (wall)");
        }
        else if (collider.CompareTag("PlayerBody"))
        {
            Destroy(gameObject);
            if (collider.gameObject.transform.parent.name != laserSword.GetComponentInParent<WeaponBehaviour>().player.transform.parent.name)
            {
                EntityPlayer tempPlayer = collider.transform.GetComponentInParent<Transform>().GetComponentInParent<Transform>().GetComponentInParent<EntityPlayer>();
                bool isHitable;

                if (tempPlayer.m_isInHisCamp)
                {
                    isHitable = !GameplaySettings.Instance.m_customSettings.PlayersCannotBeDistanceHittedInCampZone;

                }
                else if (tempPlayer.m_isInEnnemyCamp)
                {
                    isHitable = !GameplaySettings.Instance.m_customSettings.PlayersCannotBeDistanceHittedInEnnemiCampZone;

                }
                else
                {
                    isHitable = !GameplaySettings.Instance.m_customSettings.PlayersCannotBeDistanceHittedInNeutralZone;

                }


                if (isHitable)
                {
                    tempPlayer.RemoveLife(laserSword.Stats[1], laserSword.gameObject.transform.parent.GetComponentInParent<EntityPlayer>().gameObject, null);
                    //collider.transform.GetComponentInParent<Transform>().GetComponentInParent<Transform>().GetComponentInParent<Rigidbody>().AddForce(gameObject.transform.TransformVector(Vector3.forward) * 5, ForceMode.Impulse);
                    laserSword.InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, laserSword.stats[1], tempPlayer.m_armor);
                    collider.gameObject.GetComponentInParent<TpsController>().StopPlayer(0.3f);
                }

                laserSword.ComboStreak++;
                laserSword.ComboTimer = 0;
                SoundManager.Instance.LaserHitmarkPlay(laserSword.gameObject);
				laserSword.transform.root.gameObject.GetComponentInChildren<UIAim>().hitProjectile = true;
            }

        }
        else if (collider.CompareTag("PlayerHead"))
        {

            Destroy(gameObject);

            EntityPlayer tempPlayer = collider.transform.GetComponentInParent<Transform>().GetComponentInParent<Transform>().GetComponentInParent<EntityPlayer>();
            bool isHitable;

            if (tempPlayer.m_isInHisCamp)
            {
                isHitable = !GameplaySettings.Instance.m_customSettings.PlayersCannotBeDistanceHittedInCampZone;

            }
            else if (tempPlayer.m_isInEnnemyCamp)
            {
                isHitable = !GameplaySettings.Instance.m_customSettings.PlayersCannotBeDistanceHittedInEnnemiCampZone;

            }
            else
            {
                isHitable = !GameplaySettings.Instance.m_customSettings.PlayersCannotBeDistanceHittedInNeutralZone;

            }


            if (isHitable)
            {
                tempPlayer.RemoveLife(laserSword.Stats[1] * 2, laserSword.gameObject.transform.parent.GetComponentInParent<EntityPlayer>().gameObject, null);
                //collider.transform.GetComponentInParent<Transform>().GetComponentInParent<Transform>().GetComponentInParent<Rigidbody>().AddForce(gameObject.transform.TransformVector(Vector3.forward) * 5, ForceMode.Impulse);
                laserSword.InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, laserSword.stats[1] * 2, tempPlayer.m_armor);
                    collider.gameObject.GetComponentInParent<TpsController>().StopPlayer(1f);
            }

            laserSword.ComboStreak++;
            laserSword.ComboTimer = 0;
            SoundManager.Instance.LaserHitmarkPlay(laserSword.gameObject);
            laserSword.transform.root.gameObject.GetComponentInChildren<UIAim>().hitProjectile = true;
        }
        else if (collider.CompareTag("InvocationBody"))
        {
            if (collider.gameObject.GetComponentInParent<comportementGeneralIA>().fromPlayercolor != laserSword.GetComponentInParent<WeaponBehaviour>().player.gameObject.transform.parent.GetComponent<EntityPlayer>().m_sColor)
            {
                Destroy(gameObject);
                collider.gameObject.GetComponentInParent<comportementGeneralIA>().RemoveLife(laserSword.Stats[1], laserSword.gameObject.transform.parent.GetComponentInParent<EntityPlayer>().gameObject);
                collider.gameObject.GetComponentInParent<Rigidbody>().AddForce(gameObject.transform.TransformVector(Vector3.forward) * 5, ForceMode.Impulse);

                laserSword.InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, laserSword.stats[1], collider.gameObject.GetComponentInParent<comportementGeneralIA>().Armor);
                laserSword.ComboStreak++;
                laserSword.ComboTimer = 0;
                collider.gameObject.GetComponentInParent<comportementGeneralIA>().StopInvoc(0.3f);

                //Debug.Log("Ouch bullet");
                SoundManager.Instance.LaserHitmarkPlay(laserSword.gameObject);
                laserSword.transform.root.gameObject.GetComponentInChildren<UIAim>().hitProjectile = true;
            }
        }
        else if (collider.CompareTag("InvocationHead"))
        {
            if (collider.gameObject.GetComponentInParent<comportementGeneralIA>().fromPlayercolor != laserSword.GetComponentInParent<WeaponBehaviour>().player.gameObject.transform.parent.GetComponent<EntityPlayer>().m_sColor)
            {
                Destroy(gameObject);
                collider.gameObject.GetComponentInParent<comportementGeneralIA>().RemoveLife(laserSword.Stats[1] * 2, laserSword.gameObject.transform.parent.GetComponentInParent<EntityPlayer>().gameObject);
                collider.gameObject.GetComponentInParent<Rigidbody>().AddForce(gameObject.transform.TransformVector(Vector3.forward) * 5, ForceMode.Impulse);
                laserSword.InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, laserSword.stats[1] * 2, collider.gameObject.GetComponentInParent<comportementGeneralIA>().Armor);
                collider.gameObject.GetComponentInParent<comportementGeneralIA>().StopInvoc(1f);

                laserSword.ComboStreak++;
                laserSword.ComboTimer = 0;

                //Debug.Log("Ouch bullet Critical !!!");
                SoundManager.Instance.LaserHitmarkPlay(laserSword.gameObject);
                laserSword.transform.root.gameObject.GetComponentInChildren<UIAim>().hitProjectile = true;
            }
        }
    }
}
