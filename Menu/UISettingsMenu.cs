using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISettingsMenu : MonoBehaviour
{
    GameplaySettings m_gameplaySettings;
    int m_childCount = 0;
   public bool m_bNeedUpdate;

    GameplaySettings.Settings m_customSettings;

    void Start()
    {
        m_gameplaySettings =GameplaySettings.Instance;
        //Stock le nombre de child dans le transform
        m_childCount = transform.childCount;
        m_bNeedUpdate = true;

         m_customSettings = m_gameplaySettings.m_customSettings;
    }
    private void Update()
    {


        //mise à jour de tous les enfant en fonction des paramettres
        if (m_bNeedUpdate)
        {
            m_bNeedUpdate = false;

            
            //pour chaque enfant dans le transform

            for (int i = 0; i < m_childCount; i++)
            {
                //On set le toggle en fonction des Settings
                switch (i)
                {
                    //Weapons
                    case 0:
                        transform.GetChild(i).GetComponent<Toggle>().isOn = m_customSettings.PlayersCanUseWeapponInCampZone;
                        break;
                    case 1:
                        transform.GetChild(i).GetComponent<Toggle>().isOn = m_customSettings.PlayersCanUseWeapponInNeutralZone;
                        break;
                    case 2:
                        transform.GetChild(i).GetComponent<Toggle>().isOn = m_customSettings.PlayersCanUseWeapponInEnnemiCampZone;
                        break;
                        //Traps
                    case 3:
                        transform.GetChild(i).GetComponent<Toggle>().isOn = m_customSettings.PlayersCanUseTrapInCampZone;
                        break;
                    case 4:
                        transform.GetChild(i).GetComponent<Toggle>().isOn = m_customSettings.PlayersCanUseTrapInNeutralZone;
                        break;
                    case 5:
                        transform.GetChild(i).GetComponent<Toggle>().isOn = m_customSettings.PlayersCanUseTrapInEnnemiCampZone;
                        break;
                        //Invocation
                    case 6:
                        transform.GetChild(i).GetComponent<Toggle>().isOn = m_customSettings.PlayersCanUseInvocationInCampZone;
                        break;
                    case 7:
                        transform.GetChild(i).GetComponent<Toggle>().isOn = m_customSettings.PlayersCanUseInvocationInNeutralZone;
                        break;
                    case 8:
                        transform.GetChild(i).GetComponent<Toggle>().isOn = m_customSettings.PlayersCanUseInvocationInEnnemiCampZone;
                        break;
                        //Distance Invincibility
                    case 9:
                        transform.GetChild(i).GetComponent<Toggle>().isOn = m_customSettings.PlayersCannotBeDistanceHittedInCampZone;
                        break;
                    case 10:
                        transform.GetChild(i).GetComponent<Toggle>().isOn = m_customSettings.PlayersCannotBeDistanceHittedInNeutralZone;
                        break;
                    case 11:
                        transform.GetChild(i).GetComponent<Toggle>().isOn = m_customSettings.PlayersCannotBeDistanceHittedInEnnemiCampZone;
                        break;
                        //Invincibility
                    case 12:
                        transform.GetChild(i).GetComponent<Toggle>().isOn = m_customSettings.PlayersAreInvicibleInCampZone;
                        break;
                    case 13:
                        transform.GetChild(i).GetComponent<Toggle>().isOn = m_customSettings.PlayersAreInvicibleInNeutralZone;
                        break;
                    case 14:
                        transform.GetChild(i).GetComponent<Toggle>().isOn = m_customSettings.PlayersAreInvicibleInEnnemiCampZone;
                        break;
                    //Nexus
                    case 15:
                        transform.GetChild(i).GetComponent<Toggle>().isOn = m_customSettings.PlayersCanHitNexusCAC;
                        break;

                    //Wave
                    //traps
                    case 16:
                        transform.GetChild(i).GetComponent<Toggle>().isOn = m_customSettings.PlayersCanUseTrapInWave;
                        break;
                    case 17:
                        transform.GetChild(i).GetComponent<Toggle>().isOn = m_customSettings.PlayersCanUseTrapOutWave;
                        break;
                    //Invoc
                    case 18:
                        transform.GetChild(i).GetComponent<Toggle>().isOn = m_customSettings.PlayersCanUseInvocationInWave;
                        break;
                    case 19:
                        transform.GetChild(i).GetComponent<Toggle>().isOn = m_customSettings.PlayersCanUseInvocationOutWave;
                        break;
                        //Gemmes
                    case 20:
                        transform.GetChild(i).GetComponent<Toggle>().isOn = m_customSettings.GemmeDrop;
                        break;
                    default:
                        break;
                }

            }
        }
    }



    public void ResetCustomSettings()
    {

        m_gameplaySettings.ResetCustomSettings();
    }
    //Weappon
    public void SetPlayersCanUseWeapponInCampZone(bool _can)
    {
        m_gameplaySettings.m_customSettings.PlayersCanUseWeapponInCampZone = _can;

    }
    public void SetPlayersCanUseWeapponInNeutralZone(bool _can)
    {
        m_gameplaySettings.m_customSettings.PlayersCanUseWeapponInNeutralZone = _can;
    }
    public void SetPlayersCanUseWeapponInEnnemiCampZone(bool _can)
    {
        m_gameplaySettings.m_customSettings.PlayersCanUseWeapponInEnnemiCampZone = _can;
    }
    //Traps
    public void SetPlayersCanUseTrapInCampZone(bool _can)
    {
        m_gameplaySettings.m_customSettings.PlayersCanUseTrapInCampZone = _can;
    }
    public void SetPlayersCanUseTrapInNeutralZone(bool _can)
    {
        m_gameplaySettings.m_customSettings.PlayersCanUseTrapInNeutralZone = _can;
    }
    public void SetPlayersCanUseTrapInEnnemiCampZone(bool _can)
    {
        m_gameplaySettings.m_customSettings.PlayersCanUseTrapInEnnemiCampZone = _can;
    }
    //Invocation
    public void SetPlayersCanUseInvocationInCampZone(bool _can)
    {
        m_gameplaySettings.m_customSettings.PlayersCanUseInvocationInCampZone = _can;
    }
    public void SetPlayersCanUseInvocationInNeutralZone(bool _can)
    {
        m_gameplaySettings.m_customSettings.PlayersCanUseInvocationInNeutralZone = _can;
    }
    public void SetPlayersCanUseInvocationInEnnemiCampZone(bool _can)
    {
        m_gameplaySettings.m_customSettings.PlayersCanUseInvocationInEnnemiCampZone = _can;
    }
    //Distance Invincibility 
    public void SetPlayersCannotBeDistanceHittedInCampZone(bool _can)
    {
        m_gameplaySettings.m_customSettings.PlayersCannotBeDistanceHittedInCampZone = _can;
    }
    public void SetPlayersCannotBeDistanceHittedInNeutralZone(bool _can)
    {
        m_gameplaySettings.m_customSettings.PlayersCannotBeDistanceHittedInNeutralZone = _can;
    }
    public void SetPlayersCannotBeDistanceHittedInEnnemiCampZone(bool _can)
    {
        m_gameplaySettings.m_customSettings.PlayersCannotBeDistanceHittedInEnnemiCampZone = _can;
    }
    //Invincibility 
    public void SetPlayersAreInvicibleInCampZone(bool _can)
    {
        m_gameplaySettings.m_customSettings.PlayersAreInvicibleInCampZone = _can;
    }
    public void SetPlayersAreInvicibleInNeutralZone(bool _can)
    {
        m_gameplaySettings.m_customSettings.PlayersAreInvicibleInNeutralZone = _can;
    }
    public void SetPlayersAreInvicibleInEnnemiCampZone(bool _can)
    {
        m_gameplaySettings.m_customSettings.PlayersAreInvicibleInEnnemiCampZone = _can;
    }


    //Nexus
    //Player Can hit nexus With weapponsCAC
    public void SetPlayersCanHitNexusCAC(bool _can)
    {
        m_gameplaySettings.m_customSettings.PlayersCanHitNexusCAC = _can;
    }



    //Wave
    //Can use trap in wave/ out wave
    public void SetPlayersCanUseTrapInWave(bool _can)
    {
        m_gameplaySettings.m_customSettings.PlayersCanUseTrapInWave = _can;
    }
    public void SetPlayersCanUseTrapOutWave(bool _can)
    {
        m_gameplaySettings.m_customSettings.PlayersCanUseTrapOutWave = _can;
    }
    //Can use invocations in wave/ out wave
    public void SetPlayersCanUseInvocationInWave(bool _can)
    {
        m_gameplaySettings.m_customSettings.PlayersCanUseInvocationInWave = _can;
    }
    public void SetPlayersCanUseInvocationOutWave(bool _can)
    {
        m_gameplaySettings.m_customSettings.PlayersCanUseInvocationOutWave = _can;
    }

    //GEMME
    public void SetDropGemmes(bool _can)
    {
        m_gameplaySettings.m_customSettings.GemmeDrop = _can;
    }
}
