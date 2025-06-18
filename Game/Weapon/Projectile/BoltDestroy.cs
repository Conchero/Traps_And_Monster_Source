using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltDestroy : MonoBehaviour
{
    Vector3 basePose;
    CrossBow crossBow;
    string name;

    // Start is called before the first frame update
    void Start()
    {
        basePose = gameObject.transform.position;
        name = gameObject.name;
        //  Debug.Log(name.Replace("bolt", "") + "/Arm/CrossBow");

        crossBow = GameObject.Find(name.Replace("bolt", "")).GetComponentInChildren<GetHand>().hand.GetComponentInChildren<CrossBow>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.x > basePose.x + 100)
        {
            Destroy(gameObject);
            crossBow.ComboStreak = 0;
            if (crossBow.ComboBullet == true)
            {
                crossBow.ComboBullet = false;
                crossBow.ComboStreak = 0;
            }
        }
        else if (gameObject.transform.position.x < basePose.x - 100)
        {
            Destroy(gameObject);
            crossBow.ComboStreak = 0;
            if (crossBow.ComboBullet == true)
            {
                crossBow.ComboBullet = false;
                crossBow.ComboStreak = 0;
            }
        }
        else if (gameObject.transform.position.y > basePose.y + 100)
        {
            Destroy(gameObject);
            crossBow.ComboStreak = 0;
            if (crossBow.ComboBullet == true)
            {
                crossBow.ComboBullet = false;
                crossBow.ComboStreak = 0;
            }
        }
        else if (gameObject.transform.position.y < basePose.y - 100)
        {
            Destroy(gameObject);
            crossBow.ComboStreak = 0;
            if (crossBow.ComboBullet == true)
            {
                crossBow.ComboBullet = false;
                crossBow.ComboStreak = 0;
            }
        }
        else if (gameObject.transform.position.z > basePose.z + 100)
        {
            Destroy(gameObject);
            crossBow.ComboStreak = 0;
            if (crossBow.ComboBullet == true)
            {
                crossBow.ComboBullet = false;
                crossBow.ComboStreak = 0;
            }
        }
        else if (gameObject.transform.position.z < basePose.z - 100)
        {
            Destroy(gameObject);
            crossBow.ComboStreak = 0;
            if (crossBow.ComboBullet == true)
            {
                crossBow.ComboBullet = false;
                crossBow.ComboStreak = 0;
            }
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
                _collider.gameObject.GetComponentInParent<Ennemi>().TauntEnnemi(crossBow.GetComponentInParent<WeaponBehaviour>().player.transform.parent.gameObject);
            }
        }

    }
    public void OnTriggerEnter(Collider collider)
    {
        // Debug.Log("collider.tag" +collider.tag);
        if (collider.CompareTag("EnnemiBody"))
        {

          
            if (crossBow.ChargedBullet == false)
            {
                collider.gameObject.GetComponentInParent<Ennemi>().RemoveLife(crossBow.Stats[0], crossBow.gameObject.transform.parent.GetComponentInParent<EntityPlayer>().gameObject);
                crossBow.InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, crossBow.Stats[0], collider.gameObject.GetComponentInParent<Ennemi>().m_armor);
                collider.gameObject.GetComponentInParent<Ennemi>().StopEnemy(0.3f);
            }
            else if (crossBow.ChargedBullet == true)
            {
                collider.gameObject.GetComponentInParent<Ennemi>().RemoveLife(crossBow.Stats[1], crossBow.gameObject.transform.parent.GetComponentInParent<EntityPlayer>().gameObject);
                crossBow.InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, crossBow.Stats[1], collider.gameObject.GetComponentInParent<Ennemi>().m_armor);
                collider.gameObject.GetComponentInParent<Ennemi>().StopEnemy(2f);
            }

            if (crossBow.ComboBullet == true)
            {
                collider.gameObject.GetComponentInParent<Ennemi>().RemoveLife(crossBow.Stats[0], crossBow.gameObject.transform.parent.GetComponentInParent<EntityPlayer>().gameObject);

                crossBow.InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, crossBow.Stats[0], collider.gameObject.GetComponentInParent<Ennemi>().m_armor);
                collider.gameObject.GetComponentInParent<Ennemi>().StopEnemy(1f);
            }

            collider.gameObject.GetComponentInParent<Rigidbody>().AddForce(gameObject.transform.TransformVector(Vector3.forward) * 20, ForceMode.Impulse);
            crossBow.ComboStreak++;
            crossBow.ComboTimer = 0;
            SoundManager.Instance.ArrowHitmarkPlay(gameObject);
            crossBow.transform.root.gameObject.GetComponentInChildren<UIAim>().hitProjectile = true;
            if (crossBow.ComboBullet == false)
            {
                Destroy(gameObject);
            }
            Debug.Log(crossBow.ComboStreak);
            TauntOnDistance(collider,collider.transform);
        }
        else if (collider.CompareTag("EnnemiHead"))
        {
         

            if (crossBow.ChargedBullet == false)
            {
                collider.gameObject.GetComponentInParent<Ennemi>().RemoveLife(crossBow.Stats[0] * 2, crossBow.gameObject.transform.parent.GetComponentInParent<EntityPlayer>().gameObject);
                crossBow.InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, crossBow.Stats[0] * 2, collider.gameObject.GetComponentInParent<Ennemi>().m_armor);
                collider.gameObject.GetComponentInParent<Ennemi>().StopEnemy(1f);
            }
            else if (crossBow.ChargedBullet == true)
            {
                collider.gameObject.GetComponentInParent<Ennemi>().RemoveLife((crossBow.Stats[1]) * 2, crossBow.gameObject.transform.parent.GetComponentInParent<EntityPlayer>().gameObject);
                crossBow.InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, crossBow.Stats[1] * 2, collider.gameObject.GetComponentInParent<Ennemi>().m_armor);
                collider.gameObject.GetComponentInParent<Ennemi>().StopEnemy(3f);
            }

            if (crossBow.ComboBullet == true)
            {
                collider.gameObject.GetComponentInParent<Ennemi>().RemoveLife(crossBow.Stats[0] * 2, crossBow.gameObject.transform.parent.GetComponentInParent<EntityPlayer>().gameObject);

                crossBow.InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, crossBow.Stats[0] * 2, collider.gameObject.GetComponentInParent<Ennemi>().m_armor);
                collider.gameObject.GetComponentInParent<Ennemi>().StopEnemy(2f);
            }

            collider.gameObject.GetComponentInParent<Rigidbody>().AddForce(gameObject.transform.TransformVector(Vector3.forward) * 20, ForceMode.Impulse);
            crossBow.ComboStreak++;
            crossBow.ComboTimer = 0;
            SoundManager.Instance.ArrowHitmarkPlay(gameObject);
            crossBow.transform.root.gameObject.GetComponentInChildren<UIAim>().hitProjectile = true;

            Debug.Log(crossBow.ComboStreak);
            if (crossBow.ComboBullet == false)
            {
                Destroy(gameObject);
            }
            TauntOnDistance(collider,collider.transform);
        }
        else if (collider.CompareTag("Ground"))
        {
            Destroy(gameObject);
            crossBow.ComboBullet = false;
            crossBow.ComboStreak = 0;
        }
        else if (collider.CompareTag("Wall"))
        {
            Destroy(gameObject);
            crossBow.ComboBullet = false;
            crossBow.ComboStreak = 0;
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

            if (collider.transform.parent.name != crossBow.GetComponentInParent<WeaponBehaviour>().player.transform.parent.name)
            {
            if (isHitable)
            {
                if (crossBow.ChargedBullet == false)
                {

                    tempPlayer.RemoveLife(crossBow.Stats[0], crossBow.gameObject.transform.parent.GetComponentInParent<EntityPlayer>().gameObject, null);
                    //collider.gameObject.GetComponentInParent<Ennemi>().RemoveLife(crossBow.Stats[0] * 2, crossBow.gameObject);
                    crossBow.InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, crossBow.Stats[0], tempPlayer.m_armorMax);
                    collider.gameObject.GetComponentInParent<TpsController>().StopPlayer(0.3f);
                }
                else if (crossBow.ChargedBullet == true)
                {
                    tempPlayer.RemoveLife(crossBow.Stats[1], crossBow.gameObject.transform.parent.GetComponentInParent<EntityPlayer>().gameObject, null);
                    // collider.gameObject.GetComponentInParent<Ennemi>().RemoveLife((crossBow.Stats[1]) * 2, crossBow.gameObject);
                    crossBow.InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, crossBow.Stats[1], tempPlayer.m_armorMax);
                    collider.gameObject.GetComponentInParent<TpsController>().StopPlayer(2f);
                }

                if (crossBow.ComboBullet == true)
                {
                    tempPlayer.RemoveLife(crossBow.Stats[0], crossBow.gameObject.transform.parent.GetComponentInParent<EntityPlayer>().gameObject, null);
                    // collider.gameObject.GetComponentInParent<Ennemi>().RemoveLife(crossBow.Stats[0] * 2, crossBow.gameObject);

                    crossBow.InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, crossBow.Stats[0], tempPlayer.m_armorMax);
                    collider.gameObject.GetComponentInParent<TpsController>().StopPlayer(1f);
                }
            }

            //collider.transform.GetComponentInParent<Transform>().GetComponentInParent<Transform>().GetComponentInParent<Rigidbody>().AddForce(gameObject.transform.TransformVector(Vector3.forward) * 20, ForceMode.Impulse);
            crossBow.ComboStreak++;
            crossBow.ComboTimer = 0;
            SoundManager.Instance.ArrowHitmarkPlay(gameObject);
            crossBow.transform.root.gameObject.GetComponentInChildren<UIAim>().hitProjectile = true;
            }
            if (crossBow.ComboBullet == false)
            {
                Destroy(gameObject);
            }
            //  Debug.Log(crossBow.ComboStreak);

            // Debug.Log("playerBody");
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
                if (crossBow.ChargedBullet == false)
                {

                    tempPlayer.RemoveLife(crossBow.Stats[0] * 2, crossBow.gameObject.transform.parent.GetComponentInParent<EntityPlayer>().gameObject, null);
                    //collider.gameObject.GetComponentInParent<Ennemi>().RemoveLife(crossBow.Stats[0] * 2, crossBow.gameObject);
                    crossBow.InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, crossBow.Stats[0] * 2, tempPlayer.m_armorMax);
                    collider.gameObject.GetComponentInParent<TpsController>().StopPlayer(1f);
                }
                else if (crossBow.ChargedBullet == true)
                {
                    tempPlayer.RemoveLife(crossBow.Stats[1] * 2, crossBow.gameObject.transform.parent.GetComponentInParent<EntityPlayer>().gameObject, null);
                    // collider.gameObject.GetComponentInParent<Ennemi>().RemoveLife((crossBow.Stats[1]) * 2, crossBow.gameObject);
                    crossBow.InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, crossBow.Stats[1] * 2, tempPlayer.m_armorMax);
                    collider.gameObject.GetComponentInParent<TpsController>().StopPlayer(3f);
                }

                if (crossBow.ComboBullet == true)
                {
                    tempPlayer.RemoveLife(crossBow.Stats[0] * 2, crossBow.gameObject.transform.parent.GetComponentInParent<EntityPlayer>().gameObject, null);
                    // collider.gameObject.GetComponentInParent<Ennemi>().RemoveLife(crossBow.Stats[0] * 2, crossBow.gameObject);

                    crossBow.InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, crossBow.Stats[0] * 2, tempPlayer.m_armorMax);
                    collider.gameObject.GetComponentInParent<TpsController>().StopPlayer(2f);
                }
            }

            // collider.transform.GetComponentInParent<Transform>().GetComponentInParent<Transform>().GetComponentInParent<Rigidbody>().AddForce(gameObject.transform.TransformVector(Vector3.forward) * 20, ForceMode.Impulse);
            crossBow.ComboStreak++;
            crossBow.ComboTimer = 0;
            SoundManager.Instance.ArrowHitmarkPlay(gameObject);
            crossBow.transform.root.gameObject.GetComponentInChildren<UIAim>().hitProjectile = true;
            if (crossBow.ComboBullet == false)
            {
                Destroy(gameObject);
            }
            //  Debug.Log(crossBow.ComboStreak);
            // Debug.Log("PlayerHead");
        }
        else if (collider.CompareTag("InvocationBody"))
        {
            if (collider.gameObject.GetComponentInParent<comportementGeneralIA>().fromPlayercolor != crossBow.GetComponentInParent<WeaponBehaviour>().player.gameObject.transform.parent.GetComponent<EntityPlayer>().m_sColor)
            {

                if (crossBow.ChargedBullet == false)
                {
                    collider.gameObject.GetComponentInParent<comportementGeneralIA>().RemoveLife(crossBow.Stats[0], crossBow.gameObject.transform.parent.GetComponentInParent<EntityPlayer>().gameObject);
                    crossBow.InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, crossBow.Stats[0], collider.gameObject.GetComponentInParent<comportementGeneralIA>().Armor);
                    collider.gameObject.GetComponentInParent<comportementGeneralIA>().StopInvoc(0.3f);
                }
                else if (crossBow.ChargedBullet == true)
                {
                    collider.gameObject.GetComponentInParent<comportementGeneralIA>().RemoveLife(crossBow.Stats[1], crossBow.gameObject.transform.parent.GetComponentInParent<EntityPlayer>().gameObject);
                    crossBow.InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, crossBow.Stats[1], collider.gameObject.GetComponentInParent<comportementGeneralIA>().Armor);
                    collider.gameObject.GetComponentInParent<comportementGeneralIA>().StopInvoc(2f);
                }

                if (crossBow.ComboBullet == true)
                {
                    collider.gameObject.GetComponentInParent<comportementGeneralIA>().RemoveLife(crossBow.Stats[0], crossBow.gameObject.transform.parent.GetComponentInParent<EntityPlayer>().gameObject);

                    crossBow.InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, crossBow.Stats[0], collider.gameObject.GetComponentInParent<comportementGeneralIA>().Armor);
                    collider.gameObject.GetComponentInParent<comportementGeneralIA>().StopInvoc(1f);
                }

                collider.gameObject.GetComponentInParent<Rigidbody>().AddForce(gameObject.transform.TransformVector(Vector3.forward) * 20, ForceMode.Impulse);
                crossBow.ComboStreak++;
                crossBow.ComboTimer = 0;
                SoundManager.Instance.ArrowHitmarkPlay(gameObject);
                crossBow.transform.root.gameObject.GetComponentInChildren<UIAim>().hitProjectile = true;
                if (crossBow.ComboBullet == false)
                {
                    Destroy(gameObject);
                }
                Debug.Log(crossBow.ComboStreak);
            }
        }
        else if (collider.CompareTag("InvocationHead"))
        {

            if (collider.gameObject.GetComponentInParent<comportementGeneralIA>().fromPlayercolor != crossBow.GetComponentInParent<WeaponBehaviour>().player.gameObject.transform.parent.GetComponent<EntityPlayer>().m_sColor)
            {
                if (crossBow.ChargedBullet == false)
                {
                    collider.gameObject.GetComponentInParent<comportementGeneralIA>().RemoveLife(crossBow.Stats[0] * 2, crossBow.gameObject.transform.parent.GetComponentInParent<EntityPlayer>().gameObject);
                    crossBow.InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, crossBow.Stats[0] * 2, collider.gameObject.GetComponentInParent<comportementGeneralIA>().Armor);
                    collider.gameObject.GetComponentInParent<comportementGeneralIA>().StopInvoc(1f);
                }
                else if (crossBow.ChargedBullet == true)
                {
                    collider.gameObject.GetComponentInParent<comportementGeneralIA>().RemoveLife((crossBow.Stats[1]) * 2, crossBow.gameObject.transform.parent.GetComponentInParent<EntityPlayer>().gameObject);
                    crossBow.InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, crossBow.Stats[1] * 2, collider.gameObject.GetComponentInParent<comportementGeneralIA>().Armor);
                    collider.gameObject.GetComponentInParent<comportementGeneralIA>().StopInvoc(3f);
                }

                if (crossBow.ComboBullet == true)
                {
                    collider.gameObject.GetComponentInParent<comportementGeneralIA>().RemoveLife(crossBow.Stats[0] * 2, crossBow.gameObject.transform.parent.GetComponentInParent<EntityPlayer>().gameObject);

                    crossBow.InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, crossBow.Stats[0] * 2, collider.gameObject.GetComponentInParent<comportementGeneralIA>().Armor);
                    collider.gameObject.GetComponentInParent<comportementGeneralIA>().StopInvoc(2f);
                }

                collider.gameObject.GetComponentInParent<Rigidbody>().AddForce(gameObject.transform.TransformVector(Vector3.forward) * 20, ForceMode.Impulse);
                crossBow.ComboStreak++;
                crossBow.ComboTimer = 0;
                SoundManager.Instance.ArrowHitmarkPlay(gameObject);
                crossBow.transform.root.gameObject.GetComponentInChildren<UIAim>().hitProjectile = true;
                if (crossBow.ComboBullet == false)
                {
                    Destroy(gameObject);
                }
                Debug.Log(crossBow.ComboStreak);
            }
        }
    }
}
