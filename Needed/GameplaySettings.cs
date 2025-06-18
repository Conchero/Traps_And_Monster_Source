using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplaySettings : MonoBehaviour
{
    static GameplaySettings instance = null;
    public static GameplaySettings Instance { get => instance; }

    Settings m_defaultSettings;
    public Settings m_customSettings;

    public struct Settings
    {

        //AREA
        //Use Weappons per area
        public bool PlayersCanUseWeapponInCampZone;
        public bool PlayersCanUseWeapponInNeutralZone;
        public bool PlayersCanUseWeapponInEnnemiCampZone;
        //Use traps per area
        public bool PlayersCanUseTrapInCampZone;
        public bool PlayersCanUseTrapInNeutralZone;
        public bool PlayersCanUseTrapInEnnemiCampZone;
        //Use Invocation per area
        public bool PlayersCanUseInvocationInCampZone;
        public bool PlayersCanUseInvocationInNeutralZone;
        public bool PlayersCanUseInvocationInEnnemiCampZone;
        //Distance Invincibility per area
        public bool PlayersCannotBeDistanceHittedInCampZone;
        public bool PlayersCannotBeDistanceHittedInNeutralZone;
        public bool PlayersCannotBeDistanceHittedInEnnemiCampZone;
        //Invincibility per area
        public bool PlayersAreInvicibleInCampZone;
        public bool PlayersAreInvicibleInNeutralZone;
        public bool PlayersAreInvicibleInEnnemiCampZone;





        //Nexus
        //Player Can hit nexus With weapponsCAC
        public bool PlayersCanHitNexusCAC;
        //activer/Desactiver le fait que le joueur puisse tapper le nexus avec des armes CAC
        //Player Can hit nexus With weapponsDistance



        //Wave
        //Can use trap in wave/ out wave
        public bool PlayersCanUseTrapInWave;
        public bool PlayersCanUseTrapOutWave;
        //activer Desactiver la possiblité de poser des pieges dans 2 cas : entre les manches, pendant les manches
        //Can use invocations in wave/ out wave
        public bool PlayersCanUseInvocationInWave;
        public bool PlayersCanUseInvocationOutWave;
        //activer Desactiver la possiblité d'invoquer des unités dans 2 cas : entre les manches, pendant les manches


        //Gemme
        public bool GemmeDrop;


        //ABANDON
        //Activ Multicible => Pas le temps
        //activer/Desactiver le multiCible(Si on change de nexus cible cela change ou non les cibles de toutes nos Ia)

        //interWave timer ==> abandonné à cause du systeme de toggle
        //temps entre les vagues du chaos


        //Stats  ==> abandonné à cause du systeme de toggle
        //Stats des nexus +gain en cas de kill
        //Stats des players +gain en cas de kill
        //Stats et cout de chaques pieges
        //Stats et cout de chaques invocation +gain en cas de kill
        //Stats de chaques démons +gain en cas de kill

    }



    public void ResetCustomSettings()
    {

        m_customSettings = m_defaultSettings;

        FindObjectOfType<UISettingsMenu>().m_bNeedUpdate = true;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);

            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        InitSettings();
    }

    //initialisation réalisé seulement à l'awake
    void InitSettings()
    {
        //AREA RESTRICTIONS
        //Weappon
        m_defaultSettings.PlayersCanUseWeapponInCampZone = true;
        m_defaultSettings.PlayersCanUseWeapponInNeutralZone = true;
        m_defaultSettings.PlayersCanUseWeapponInEnnemiCampZone = false;
        //Traps
        m_defaultSettings.PlayersCanUseTrapInCampZone = true;
        m_defaultSettings.PlayersCanUseTrapInNeutralZone = false;
        m_defaultSettings.PlayersCanUseTrapInEnnemiCampZone = false;
        //Invocation
        m_defaultSettings.PlayersCanUseInvocationInCampZone = true;
        m_defaultSettings.PlayersCanUseInvocationInNeutralZone = true;
        m_defaultSettings.PlayersCanUseInvocationInEnnemiCampZone = false;
        //Distance Invincibility 
        m_defaultSettings.PlayersCannotBeDistanceHittedInCampZone = false;
        m_defaultSettings.PlayersCannotBeDistanceHittedInNeutralZone = false;
        m_defaultSettings.PlayersCannotBeDistanceHittedInEnnemiCampZone = false;
        //Invincibility 
        m_defaultSettings.PlayersAreInvicibleInCampZone = false;
        m_defaultSettings.PlayersAreInvicibleInNeutralZone = false;
        m_defaultSettings.PlayersAreInvicibleInEnnemiCampZone = false;
        //


        //Nexus
        //Player Can hit nexus With weapponsCAC
        m_defaultSettings.PlayersCanHitNexusCAC = false;
        //activer/Desactiver le fait que le joueur puisse tapper le nexus avec des weapponsDistance



        //Wave
        //Can use trap in wave/ out wave
        m_defaultSettings.PlayersCanUseTrapInWave = false;
        m_defaultSettings.PlayersCanUseTrapOutWave = true;
        //Can use invocations in wave/ out wave
        m_defaultSettings.PlayersCanUseInvocationInWave = true;
        m_defaultSettings.PlayersCanUseInvocationOutWave = true;

        //Gemme
        m_defaultSettings.GemmeDrop = true;


        //assigne les valeurs par defaut aux réglages personnalisé du joueur
        m_customSettings = m_defaultSettings;
    }
    //initialisation réalisé seulement à l'awake
    void InitDebugSettings()
    {
        //AREA RESTRICTIONS
        //Weappon
        m_defaultSettings.PlayersCanUseWeapponInCampZone = true;
        m_defaultSettings.PlayersCanUseWeapponInNeutralZone = true;
        m_defaultSettings.PlayersCanUseWeapponInEnnemiCampZone = true;
        //Traps
        m_defaultSettings.PlayersCanUseTrapInCampZone = true;
        m_defaultSettings.PlayersCanUseTrapInNeutralZone = true;
        m_defaultSettings.PlayersCanUseTrapInEnnemiCampZone = false;
        //Invocation
        m_defaultSettings.PlayersCanUseInvocationInCampZone = true;
        m_defaultSettings.PlayersCanUseInvocationInNeutralZone = true;
        m_defaultSettings.PlayersCanUseInvocationInEnnemiCampZone = false;
        //Distance Invincibility 
        m_defaultSettings.PlayersCannotBeDistanceHittedInCampZone = false;
        m_defaultSettings.PlayersCannotBeDistanceHittedInNeutralZone = false;
        m_defaultSettings.PlayersCannotBeDistanceHittedInEnnemiCampZone = false;
        //Invincibility 
        m_defaultSettings.PlayersAreInvicibleInCampZone = false;
        m_defaultSettings.PlayersAreInvicibleInNeutralZone = false;
        m_defaultSettings.PlayersAreInvicibleInEnnemiCampZone = false;
        //


        //Nexus
        //Player Can hit nexus With weapponsCAC
        m_defaultSettings.PlayersCanHitNexusCAC = true;
        //activer/Desactiver le fait que le joueur puisse tapper le nexus avec des weapponsDistance



        //Wave
        //Can use trap in wave/ out wave
        m_defaultSettings.PlayersCanUseTrapInWave = true;
        m_defaultSettings.PlayersCanUseTrapOutWave = true;
        //Can use invocations in wave/ out wave
        m_defaultSettings.PlayersCanUseInvocationInWave = true;
        m_defaultSettings.PlayersCanUseInvocationOutWave = true;


        //Gemme
        m_defaultSettings.GemmeDrop = true;



        //assigne les valeurs par defaut aux réglages personnalisé du joueur
        m_customSettings = m_defaultSettings;
    }

  
}
