using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HugeLaserDestroy : MonoBehaviour
{
    Vector3 basePose;
    LaserSword laserSword;

    string name;

    // Start is called before the first frame update
    void Start()
    {
        name = gameObject.name;

        laserSword = GameObject.Find(name.Replace("HugeLaser", "")).GetComponentInChildren<GetHand>().hand.GetComponentInChildren<LaserSword>();
        //Debug.Log(basePose);
        SoundManager.Instance.LaserHumPlay(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

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

    public void OnTriggerStay(Collider collider)
    {
        if (collider.CompareTag("EnnemiBody"))
        {
            collider.gameObject.GetComponentInParent<Ennemi>().RemoveLife(laserSword.Stats[3], laserSword.gameObject.transform.parent.GetComponentInParent<EntityPlayer>().gameObject);
            //Debug.Log("Ouch bullet");
            laserSword.InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, laserSword.stats[3], collider.gameObject.GetComponentInParent<Ennemi>().m_armor);
                collider.gameObject.GetComponentInParent<Ennemi>().StopEnemy(0.1f);
            TauntOnDistance(collider,collider.transform);
        }
        else if (collider.CompareTag("EnnemiHead"))
        {
            collider.gameObject.GetComponentInParent<Ennemi>().RemoveLife((laserSword.Stats[3] * 2), laserSword.gameObject.transform.parent.GetComponentInParent<EntityPlayer>().gameObject);
            //Debug.Log("Ouch bullet Critical !!!");
            laserSword.InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, laserSword.stats[3] * 2, collider.gameObject.GetComponentInParent<Ennemi>().m_armor);
                collider.gameObject.GetComponentInParent<Ennemi>().StopEnemy(0.3f);
            TauntOnDistance(collider,collider.transform);
        }
        else if (collider.CompareTag("PlayerBody"))
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
                if (collider.transform.parent.name != laserSword.GetComponentInParent<WeaponBehaviour>().player.transform.name)
                {
                    tempPlayer.RemoveLife(laserSword.Stats[3], laserSword.gameObject.transform.parent.GetComponentInParent<EntityPlayer>().gameObject, null);
                    laserSword.InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, laserSword.stats[3], tempPlayer.m_armor);
                    collider.gameObject.GetComponentInParent<TpsController>().StopPlayer(0.1f);
                }
        
            }
        }
        else if (collider.CompareTag("PlayerHead"))
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
                if (collider.transform.parent.name != laserSword.GetComponentInParent<WeaponBehaviour>().player.transform.name)
                {
                    tempPlayer.RemoveLife(laserSword.Stats[3] * 2, laserSword.gameObject.transform.parent.GetComponentInParent<EntityPlayer>().gameObject, null);
                    laserSword.InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, laserSword.stats[3] * 2, tempPlayer.m_armor);
                    collider.gameObject.GetComponentInParent<TpsController>().StopPlayer(0.3f);
                }
            }
        }
        else if (collider.CompareTag("InvocationBody"))
        {
            if (collider.gameObject.GetComponentInParent<comportementGeneralIA>().fromPlayercolor != laserSword.GetComponentInParent<WeaponBehaviour>().player.gameObject.transform.parent.GetComponent<EntityPlayer>().m_sColor)
            {
                collider.gameObject.GetComponentInParent<comportementGeneralIA>().RemoveLife(laserSword.Stats[3], laserSword.gameObject.transform.parent.GetComponentInParent<EntityPlayer>().gameObject);
                //Debug.Log("Ouch bullet");
                laserSword.InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, laserSword.stats[3], collider.gameObject.GetComponentInParent<comportementGeneralIA>().Armor);

                collider.gameObject.GetComponentInParent<comportementGeneralIA>().StopInvoc(0.1f);
            }
        }
        else if (collider.CompareTag("InvocationHead"))
        {
            if (collider.gameObject.GetComponentInParent<comportementGeneralIA>().fromPlayercolor != laserSword.GetComponentInParent<WeaponBehaviour>().player.gameObject.transform.parent.GetComponent<EntityPlayer>().m_sColor)
            {
                collider.gameObject.GetComponentInParent<comportementGeneralIA>().RemoveLife((laserSword.Stats[3] * 2), laserSword.gameObject.transform.parent.GetComponentInParent<EntityPlayer>().gameObject);
                //Debug.Log("Ouch bullet Critical !!!");
                laserSword.InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, laserSword.stats[3] * 2, collider.gameObject.GetComponentInParent<comportementGeneralIA>().Armor);
                collider.gameObject.GetComponentInParent<comportementGeneralIA>().StopInvoc(0.3f);
            }
        }

    }
}
