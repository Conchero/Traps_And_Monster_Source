using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDestroy : MonoBehaviour
{
    Vector3 basePose;
    Bow bow;
    string name;

    Vector3 lastPos;

    [SerializeField] Transform raycastObject;
    // Start is called before the first frame update
    void Start()
    {
        basePose = gameObject.transform.position;
        name = gameObject.name;

        bow = GameObject.Find(name.Replace("Arrow", "")).GetComponentInChildren<GetHand>().hand.GetComponentInChildren<Bow>();
        lastPos = raycastObject.position;
    }

    void FixedUpdate()
    {
        //RaycastHit hit;

        //if (Physics.Raycast(lastPos,raycastObject.position,out hit))
        //{

        //    if (hit.collider.CompareTag("PlayerBody"))
        //    {
        //        EntityPlayer tempPlayer = hit.collider.transform.GetComponentInParent<Transform>().GetComponentInParent<Transform>().GetComponentInParent<EntityPlayer>();



        //        bool isHitable;

        //        if (tempPlayer.m_isInHisCamp)
        //        {
        //            isHitable = !GameplaySettings.Instance.m_customSettings.PlayersCannotBeDistanceHittedInCampZone;

        //        }
        //        else if (tempPlayer.m_isInEnnemyCamp)
        //        {
        //            isHitable = !GameplaySettings.Instance.m_customSettings.PlayersCannotBeDistanceHittedInEnnemiCampZone;

        //        }
        //        else
        //        {
        //            isHitable = !GameplaySettings.Instance.m_customSettings.PlayersCannotBeDistanceHittedInNeutralZone;

        //        }


        //        if (isHitable)
        //        {
        //            if (bow.ChargedBullet == false)
        //            {
        //                tempPlayer.RemoveLife(bow.Stats[0], bow.gameObject.transform.parent.gameObject.GetComponentInParent<EntityPlayer>().gameObject, null);
        //                bow.InstantiateDamageFeeback(tempPlayer.gameObject.transform.position, tempPlayer.gameObject.transform.rotation, bow.Stats[0], tempPlayer.m_armor);
        //                hit.collider.gameObject.GetComponentInParent<TpsController>().StopPlayer(0.3f);
        //            }
        //            else if (bow.ChargedBullet == true)
        //            {
        //                tempPlayer.RemoveLife(bow.Stats[0] + 1, bow.gameObject.transform.parent.gameObject.GetComponentInParent<EntityPlayer>().gameObject, null);
        //                bow.InstantiateDamageFeeback(tempPlayer.gameObject.transform.position, tempPlayer.gameObject.transform.rotation, bow.Stats[0] + 1, tempPlayer.m_armor);
        //                hit.collider.gameObject.GetComponentInParent<TpsController>().StopPlayer(0.9f);
        //            }

        //            if (bow.ComboBullet == true)
        //            {
        //                tempPlayer.RemoveLife(bow.Stats[0], bow.gameObject.transform.parent.gameObject.GetComponentInParent<EntityPlayer>().gameObject, null);
        //                bow.ComboBullet = false;
        //                bow.ComboStreak = 0;
        //                //  Debug.Log("Froze !");
        //                bow.InstantiateDamageFeeback(tempPlayer.gameObject.transform.position, tempPlayer.gameObject.transform.rotation, bow.Stats[0], tempPlayer.m_armor);
        //                hit.collider.gameObject.GetComponentInParent<TpsController>().StopPlayer(5f);
        //            }
        //        }

        //        //  collision.collider.gameObject.GetComponentInParent<Rigidbody>().AddForce(gameObject.transform.TransformVector(Vector3.forward) * 5, ForceMode.Impulse);
        //        bow.ComboStreak++;
        //        bow.ComboTimer = 0;
        //        SoundManager.Instance.ArrowHitmarkPlay(bow.gameObject);
        //        bow.transform.root.gameObject.GetComponentInChildren<UIAim>().hitProjectile = true;
        //        Destroy(gameObject);
        //    }

        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.x > basePose.x + 500)
        {
            Destroy(gameObject);
            bow.ComboStreak = 0;
            if (bow.ComboBullet == true)
            {
                bow.ComboBullet = false;
                bow.ComboStreak = 0;
            }
        }
        else if (gameObject.transform.position.x < basePose.x - 500)
        {
            Destroy(gameObject);
            bow.ComboStreak = 0;
            if (bow.ComboBullet == true)
            {
                bow.ComboBullet = false;
                bow.ComboStreak = 0;
            }
        }
        else if (gameObject.transform.position.y > basePose.y + 500)
        {
            Destroy(gameObject);
            bow.ComboStreak = 0;
            if (bow.ComboBullet == true)
            {
                bow.ComboBullet = false;
                bow.ComboStreak = 0;
            }
        }
        else if (gameObject.transform.position.y < basePose.y - 500)
        {
            Destroy(gameObject);
            bow.ComboStreak = 0;
            if (bow.ComboBullet == true)
            {
                bow.ComboBullet = false;
                bow.ComboStreak = 0;
            }
        }
        else if (gameObject.transform.position.z > basePose.z + 500)
        {
            Destroy(gameObject);
            bow.ComboStreak = 0;
            if (bow.ComboBullet == true)
            {
                bow.ComboBullet = false;
                bow.ComboStreak = 0;
            }
        }
        else if (gameObject.transform.position.z < basePose.z - 500)
        {
            Destroy(gameObject);
            bow.ComboStreak = 0;
            if (bow.ComboBullet == true)
            {
                bow.ComboBullet = false;
                bow.ComboStreak = 0;
            }
        }

        lastPos = raycastObject.position;

    }

    void TauntOnDistance(Collider _collider,Transform _posOther)
    {
        float distBEAP;
        distBEAP = Vector3.Distance(basePose, _posOther.position);

        if (distBEAP <= 9)
        {
            if (!_collider.gameObject.GetComponentInParent<Ennemi>().m_isTaunted)
            {
                _collider.gameObject.GetComponentInParent<Ennemi>().TauntEnnemi(bow.GetComponentInParent<WeaponBehaviour>().player.transform.parent.gameObject);
            }
        }
      
    }

    public void OnTriggerEnter(Collider collider)
    {

        if (collider.CompareTag("EnnemiBody"))
        {

            if (bow.ChargedBullet == false)
            {
                collider.gameObject.GetComponentInParent<Ennemi>().RemoveLife(bow.Stats[0], bow.gameObject.transform.parent.gameObject.GetComponentInParent<EntityPlayer>().gameObject);
                bow.InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, bow.Stats[0], collider.gameObject.GetComponentInParent<Ennemi>().m_armor);
                collider.gameObject.GetComponentInParent<Ennemi>().StopEnemy(0.3f);
            }
            else if (bow.ChargedBullet == true)
            {
                collider.gameObject.GetComponentInParent<Ennemi>().RemoveLife(bow.Stats[0] + 1, bow.gameObject.transform.parent.gameObject.GetComponentInParent<EntityPlayer>().gameObject);
                bow.InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, bow.Stats[0] + 1, collider.gameObject.GetComponentInParent<Ennemi>().m_armor);
                collider.gameObject.GetComponentInParent<Ennemi>().StopEnemy(0.9f);
            }

            if (bow.ComboBullet == true)
            {
                collider.gameObject.GetComponentInParent<Ennemi>().RemoveLife(bow.Stats[0], bow.gameObject.transform.parent.gameObject.GetComponentInParent<EntityPlayer>().gameObject);
                bow.ComboBullet = false;
                bow.ComboStreak = 0;
                bow.InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, bow.Stats[0], collider.gameObject.GetComponentInParent<Ennemi>().m_armor);
                collider.gameObject.GetComponentInParent<Ennemi>().StopEnemy(5f);
                //Debug.Log("Froze !");
            }
            TauntOnDistance(collider,collider.transform);
            collider.gameObject.GetComponentInParent<Rigidbody>().AddForce(gameObject.transform.TransformVector(Vector3.forward) * 5, ForceMode.Impulse);
            bow.ComboStreak++;
            bow.ComboTimer = 0;

            SoundManager.Instance.ArrowHitmarkPlay(bow.gameObject);
            bow.transform.root.gameObject.GetComponentInChildren<UIAim>().hitProjectile = true;
          //  Debug.Log(bow.ComboStreak);
            Destroy(gameObject);
        }
        else if (collider.CompareTag("EnnemiHead"))
        {

            if (bow.ChargedBullet == false)
            {
                collider.gameObject.GetComponentInParent<Ennemi>().RemoveLife(bow.Stats[0] * 2, bow.gameObject.transform.parent.gameObject.GetComponentInParent<EntityPlayer>().gameObject);
                bow.InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, bow.Stats[0] * 2, collider.gameObject.GetComponentInParent<Ennemi>().m_armor);
                collider.gameObject.GetComponentInParent<Ennemi>().StopEnemy(1f);
            }
            else if (bow.ChargedBullet == true)
            {
                collider.gameObject.GetComponentInParent<Ennemi>().RemoveLife((bow.Stats[0] + 1) * 2, bow.gameObject.transform.parent.gameObject.GetComponentInParent<EntityPlayer>().gameObject);
                bow.InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, (bow.Stats[0] + 1) * 2, collider.gameObject.GetComponentInParent<Ennemi>().m_armor);
                collider.gameObject.GetComponentInParent<Ennemi>().StopEnemy(1.5f);
            }

            if (bow.ComboBullet == true)
            {
                collider.gameObject.GetComponentInParent<Ennemi>().RemoveLife(bow.Stats[0] * 4, bow.gameObject.transform.parent.gameObject.GetComponentInParent<EntityPlayer>().gameObject);
                bow.ComboBullet = false;
                bow.ComboStreak = 0;
                bow.InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, bow.Stats[0] * 4, collider.gameObject.GetComponentInParent<Ennemi>().m_armor);
                collider.gameObject.GetComponentInParent<Ennemi>().StopEnemy(5f);

            }

            TauntOnDistance(collider, collider.transform);

            collider.gameObject.GetComponentInParent<Rigidbody>().AddForce(gameObject.transform.TransformVector(Vector3.forward) * 5, ForceMode.Impulse);
            bow.ComboStreak++;
            bow.ComboTimer = 0;
            SoundManager.Instance.ArrowHitmarkPlay(bow.gameObject);
            bow.transform.root.gameObject.GetComponentInChildren<UIAim>().hitProjectile = true;
            Destroy(gameObject);

        }
        else if (collider.CompareTag("Ground"))
        {
            bow.ComboBullet = false;
            bow.ComboStreak = 0;
            Destroy(gameObject);
        }
        else if (collider.CompareTag("Wall"))
        {
            bow.ComboBullet = false;
            bow.ComboStreak = 0;
            Destroy(gameObject);
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
                if (bow.ChargedBullet == false)
                {
                    tempPlayer.RemoveLife(bow.Stats[0], bow.gameObject.transform.parent.gameObject.GetComponentInParent<EntityPlayer>().gameObject, null);
                    bow.InstantiateDamageFeeback(tempPlayer.gameObject.transform.position, tempPlayer.gameObject.transform.rotation, bow.Stats[0], tempPlayer.m_armor);
                    collider.gameObject.GetComponentInParent<TpsController>().StopPlayer(0.3f);
                }
                else if (bow.ChargedBullet == true)
                {
                    tempPlayer.RemoveLife(bow.Stats[0] + 1, bow.gameObject.transform.parent.gameObject.GetComponentInParent<EntityPlayer>().gameObject, null);
                    bow.InstantiateDamageFeeback(tempPlayer.gameObject.transform.position, tempPlayer.gameObject.transform.rotation, bow.Stats[0] + 1, tempPlayer.m_armor);
                    collider.gameObject.GetComponentInParent<TpsController>().StopPlayer(0.9f);
                }

                if (bow.ComboBullet == true)
                {
                    tempPlayer.RemoveLife(bow.Stats[0], bow.gameObject.transform.parent.gameObject.GetComponentInParent<EntityPlayer>().gameObject, null);
                    bow.ComboBullet = false;
                    bow.ComboStreak = 0;
                    //  Debug.Log("Froze !");
                    bow.InstantiateDamageFeeback(tempPlayer.gameObject.transform.position, tempPlayer.gameObject.transform.rotation, bow.Stats[0], tempPlayer.m_armor);
                    collider.gameObject.GetComponentInParent<TpsController>().StopPlayer(5f);
                }
            }

            //  collision.collider.gameObject.GetComponentInParent<Rigidbody>().AddForce(gameObject.transform.TransformVector(Vector3.forward) * 5, ForceMode.Impulse);
            bow.ComboStreak++;
            bow.ComboTimer = 0;

            //            Debug.Log(pistol.ComboStreak);

            SoundManager.Instance.ArrowHitmarkPlay(bow.gameObject);
            bow.transform.root.gameObject.GetComponentInChildren<UIAim>().hitProjectile = true;


            Destroy(gameObject);

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
                if (bow.ChargedBullet == false)
                {
                    tempPlayer.RemoveLife(bow.Stats[0] * 2, bow.gameObject.transform.parent.gameObject.GetComponentInParent<EntityPlayer>().gameObject, null);
                    bow.InstantiateDamageFeeback(tempPlayer.gameObject.transform.position, tempPlayer.gameObject.transform.rotation, bow.Stats[0] * 2, tempPlayer.m_armor);
                    collider.gameObject.GetComponentInParent<TpsController>().StopPlayer(1f);
                }
                else if (bow.ChargedBullet == true)
                {
                    tempPlayer.RemoveLife((bow.Stats[0] + 1) * 2, bow.gameObject.transform.parent.gameObject.GetComponentInParent<EntityPlayer>().gameObject, null);
                    bow.InstantiateDamageFeeback(tempPlayer.gameObject.transform.position, tempPlayer.gameObject.transform.rotation, (bow.Stats[0] + 1) * 2, tempPlayer.m_armor);
                    collider.gameObject.GetComponentInParent<TpsController>().StopPlayer(1.5f);
                }

                if (bow.ComboBullet == true)
                {
                    tempPlayer.RemoveLife(bow.Stats[0] * 4, bow.gameObject.transform.parent.gameObject.GetComponentInParent<EntityPlayer>().gameObject, null);
                    bow.ComboBullet = false;
                    bow.ComboStreak = 0;
                    bow.InstantiateDamageFeeback(tempPlayer.gameObject.transform.position, tempPlayer.gameObject.transform.rotation, bow.Stats[0] * 4, tempPlayer.m_armor);
                    collider.gameObject.GetComponentInParent<TpsController>().StopPlayer(5f);

                    //  Debug.Log("Froze !");
                }
            }
            // collision.collider.gameObject.GetComponentInParent<Rigidbody>().AddForce(gameObject.transform.TransformVector(Vector3.forward) * 5, ForceMode.Impulse);
            bow.ComboStreak++;
            bow.ComboTimer = 0;
            SoundManager.Instance.ArrowHitmarkPlay(bow.gameObject);
            bow.transform.root.gameObject.GetComponentInChildren<UIAim>().hitProjectile = true;
            Destroy(gameObject);

            //  Debug.Log(pistol.ComboStreak);
        }
        else if (collider.CompareTag("InvocationBody"))
        {
            if (collider.gameObject.GetComponentInParent<comportementGeneralIA>().fromPlayercolor != bow.GetComponentInParent<WeaponBehaviour>().player.gameObject.transform.parent.GetComponent<EntityPlayer>().m_sColor)
            {
                if (bow.ChargedBullet == false)
                {
                    collider.gameObject.GetComponentInParent<comportementGeneralIA>().RemoveLife(bow.Stats[0], bow.gameObject.transform.parent.gameObject.GetComponentInParent<EntityPlayer>().gameObject);
                    bow.InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, bow.Stats[0], collider.gameObject.GetComponentInParent<comportementGeneralIA>().Armor);
                    collider.gameObject.GetComponentInParent<comportementGeneralIA>().StopInvoc(0.3f);
                }
                else if (bow.ChargedBullet == true)
                {
                    collider.gameObject.GetComponentInParent<comportementGeneralIA>().RemoveLife(bow.Stats[0] + 1, bow.gameObject.transform.parent.gameObject.GetComponentInParent<EntityPlayer>().gameObject);
                    bow.InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, bow.Stats[0] + 1, collider.gameObject.GetComponentInParent<comportementGeneralIA>().Armor);
                    collider.gameObject.GetComponentInParent<comportementGeneralIA>().StopInvoc(0.9f);
                }

                if (bow.ComboBullet == true)
                {
                    collider.gameObject.GetComponentInParent<comportementGeneralIA>().RemoveLife(bow.Stats[0], bow.gameObject.transform.parent.gameObject.GetComponentInParent<EntityPlayer>().gameObject);
                    bow.ComboBullet = false;
                    bow.ComboStreak = 0;
                    bow.InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, bow.Stats[0], collider.gameObject.GetComponentInParent<comportementGeneralIA>().Armor);
                    collider.gameObject.GetComponentInParent<comportementGeneralIA>().StopInvoc(5f);

                    // Debug.Log("Froze !");
                }

                collider.gameObject.GetComponentInParent<Rigidbody>().AddForce(gameObject.transform.TransformVector(Vector3.forward) * 5, ForceMode.Impulse);
                bow.ComboStreak++;
                bow.ComboTimer = 0;

                SoundManager.Instance.ArrowHitmarkPlay(bow.gameObject);
                bow.transform.root.gameObject.GetComponentInChildren<UIAim>().hitProjectile = true;
                // Debug.Log(bow.ComboStreak);
                Destroy(gameObject);
            }
        }
        else if (collider.CompareTag("InvocationHead"))
        {
            if (collider.gameObject.GetComponentInParent<comportementGeneralIA>().fromPlayercolor != bow.GetComponentInParent<WeaponBehaviour>().player.gameObject.transform.parent.GetComponent<EntityPlayer>().m_sColor)
            {
                if (bow.ChargedBullet == false)
                {
                    collider.gameObject.GetComponentInParent<comportementGeneralIA>().RemoveLife(bow.Stats[0] * 2, bow.gameObject.transform.parent.gameObject.GetComponentInParent<EntityPlayer>().gameObject);
                    bow.InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, bow.Stats[0] * 2, collider.gameObject.GetComponentInParent<comportementGeneralIA>().Armor);
                    collider.gameObject.GetComponentInParent<comportementGeneralIA>().StopInvoc(1f);
                }
                else if (bow.ChargedBullet == true)
                {
                    collider.gameObject.GetComponentInParent<comportementGeneralIA>().RemoveLife((bow.Stats[0] + 1) * 2, bow.gameObject.transform.parent.gameObject.GetComponentInParent<EntityPlayer>().gameObject);
                    bow.InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, (bow.Stats[0] + 1) * 2, collider.gameObject.GetComponentInParent<comportementGeneralIA>().Armor);
                    collider.gameObject.GetComponentInParent<comportementGeneralIA>().StopInvoc(2f);
                }

                if (bow.ComboBullet == true)
                {
                    collider.gameObject.GetComponentInParent<comportementGeneralIA>().RemoveLife(bow.Stats[0] * 4, bow.gameObject.transform.parent.gameObject.GetComponentInParent<EntityPlayer>().gameObject);
                    bow.ComboBullet = false;
                    bow.ComboStreak = 0;
                    bow.InstantiateDamageFeeback(collider.gameObject.transform.position, collider.gameObject.transform.rotation, bow.Stats[0] * 4, collider.gameObject.GetComponentInParent<comportementGeneralIA>().Armor);
                    // Debug.Log("Froze !");
                    collider.gameObject.GetComponentInParent<comportementGeneralIA>().StopInvoc(5f);
                }

                collider.gameObject.GetComponentInParent<Rigidbody>().AddForce(gameObject.transform.TransformVector(Vector3.forward) * 5, ForceMode.Impulse);
                bow.ComboStreak++;
                bow.ComboTimer = 0;
                SoundManager.Instance.ArrowHitmarkPlay(bow.gameObject);
                bow.transform.root.gameObject.GetComponentInChildren<UIAim>().hitProjectile = true;

                Debug.Log(bow.ComboStreak);
                Destroy(gameObject);
            }
        }
    }
}
