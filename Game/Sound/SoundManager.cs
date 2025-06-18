using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    static SoundManager instance = null;
    public static SoundManager Instance { get => instance; }

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

    }

    public bool GameMusicPlay;
    public bool MusicDefeatPlay;
    public bool MusicVictoryPlay;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //intro musics
    bool IntroAfterLifeIsPlaying = false;
    public void IntroAfterLifePlay(GameObject _source)
    {
        if (IntroAfterLifeIsPlaying == false)
        {
            AkSoundEngine.PostEvent("IntroAfterLifePlay", _source);
            IntroAfterLifeIsPlaying = true;
        }
    }

    public void IntroAfterLifeStop(GameObject _source)
    {
        AkSoundEngine.PostEvent("IntroAfterLifeStop", _source);
        IntroAfterLifeIsPlaying = false;
    }

    bool IntroCreaIsPlaying = false;
    public void IntroCreaPlay(GameObject _source)
    {
        if (IntroCreaIsPlaying == false)
        {
            AkSoundEngine.PostEvent("IntroCreaPlay", _source);
            IntroCreaIsPlaying = true;
        }
    }

    public void IntroCreaStop(GameObject _source)
    {
        AkSoundEngine.PostEvent("IntroCreaStop", _source);
        IntroCreaIsPlaying = false;
    }

    bool TrailerIsPlaying = false;
    public void TrailerMusicPlay(GameObject _source)
    {
        if (TrailerIsPlaying == false)
        {
            AkSoundEngine.PostEvent("TrailerMusicPlay", _source);
            TrailerIsPlaying = true;
        }
    }

    public void TrailerMusicStop(GameObject _source)
    {
        AkSoundEngine.PostEvent("TrailerMusicStop", _source);
        TrailerIsPlaying = false;
    }

    //Intro Menu musics
    bool MusicMenuIsPlaying = false;
    public void MenuMusicPlay(GameObject _source)
    {
        if (MusicMenuIsPlaying == false)
        {
            AkSoundEngine.PostEvent("MenuMusicPlay", _source);
            MusicMenuIsPlaying = true;
        }
    }

    public void MenuMusicStop(GameObject _source)
    {
        AkSoundEngine.PostEvent("MenuMusicStop", _source);
        MusicMenuIsPlaying = false;
    }

    //Music In Game
    bool InGameMusicIsPlaying = false;
    public void InGameMusicPlay(GameObject _source)
    {
        if (InGameMusicIsPlaying == false)
        {
           AkSoundEngine.PostEvent("InGameMusicPlay", _source);
            InGameMusicIsPlaying = true;
        }
    }

    public void InGameMusicStop(GameObject _source)
    {
        AkSoundEngine.PostEvent("InGameMusicStop", _source);
        InGameMusicIsPlaying = false;
    }


    // Victory Music

    bool VictoryMusicIsPlaying = false;
    public void VictoryMusicPlay(GameObject _source)
    {
        if (VictoryMusicIsPlaying == false)
        {
            AkSoundEngine.PostEvent("VictoryMusicPlay", _source);
            VictoryMusicIsPlaying = true;
        }
    }

    public void VictoryMusicStop(GameObject _source)
    {
        AkSoundEngine.PostEvent("VictoryMusicStop", _source);
        VictoryMusicIsPlaying = false;
    }

    // Defeat Music

    bool DefeatMusicIsPlaying = false;
    public void DefeatMusicPlay(GameObject _source)
    {
        if (DefeatMusicIsPlaying == false)
        {
            AkSoundEngine.PostEvent("DefeatMusicPlay", _source);
            DefeatMusicIsPlaying = true;
        }
    }

    public void DefeatMusicStop(GameObject _source)
    {
        AkSoundEngine.PostEvent("DefeatMusicStop", _source);
        DefeatMusicIsPlaying = false;
    }

    //Players Weapons Sound

    public void ComboActivatedPlay(GameObject _source)
    {
        AkSoundEngine.PostEvent("ComboActivated", _source);
    }

    public void WeaponAttackPlay(GameObject _source)
    {
        AkSoundEngine.PostEvent("WeaponAttack", _source);
    }

    public void WeaponEnnemiHitPlay(GameObject _source)
    {
        AkSoundEngine.PostEvent("WeaponEnnemiHit", _source);
    }

    public void WeaponLordHitPlay(GameObject _source)
    {
        AkSoundEngine.PostEvent("WeaponLordHit", _source);
    }

    public void WeaponWallHitPlay(GameObject _source)
    {
        AkSoundEngine.PostEvent("WeaponWallHit", _source);
    }

    public void BowShootPlay(GameObject _source)
    {
        AkSoundEngine.PostEvent("BowShoot", _source);
    }

    public void BuyWeaponPlay(GameObject _source)
    {
        AkSoundEngine.PostEvent("BuyWeapon", _source);
    }

    public void ArrowHitmarkPlay(GameObject _source)
    {
        AkSoundEngine.PostEvent("ArrowHitmarkPlay", _source);
    }

    public void CrossBowShootPlay(GameObject _source)
    {
        AkSoundEngine.PostEvent("CrossBowShoot", _source);
    }

    public void LaserShootPlay(GameObject _source)
    {
        AkSoundEngine.PostEvent("LaserShoot", _source);
    }

    public void LaserHitmarkPlay(GameObject _source)
    {
        AkSoundEngine.PostEvent("LaserHitmark", _source);
    }

    public void LaserIgnitionPlay(GameObject _source)
    {
        AkSoundEngine.PostEvent("LaserIgnition", _source);
    }

    public void LaserDesactivationPlay(GameObject _source)
    {
        AkSoundEngine.PostEvent("LaserDesactivation", _source);
    }

    public void LaserHumPlay(GameObject _source)
    {
        AkSoundEngine.PostEvent("LaserHumPlay", _source);
    }

    public void LaserHumStop(GameObject _source)
    {
        AkSoundEngine.PostEvent("LaserHumStop", _source);
    }


    //Players run Sound
    public void RunPlayerPlay(GameObject _source)
    {
     
            AkSoundEngine.PostEvent("RunPlayerPlay", _source);
    }

    public void RunPlayerStop(GameObject _source)
    {
        AkSoundEngine.PostEvent("RunPlayerStop", _source);
    }

    //Players init Sound
    public void InitPlayerPlay(GameObject _source)
    {

        AkSoundEngine.PostEvent("InitPlayer", _source);
    }


    //Players invocation Sound
    public void PlayerInvocatePlay(GameObject _source)
    {
      AkSoundEngine.PostEvent("PlayerInvocate", _source);
    }

    public void CoinPickingUpPlay(GameObject _source)
    {
        AkSoundEngine.PostEvent("CoinPickingUp", _source);
    }


    //Players Hurt Sound
    public void PlayerHurtPlay(GameObject _source)
    {
        AkSoundEngine.PostEvent("PlayerHurt", _source);
    }

    public void PlayerDiePlay(GameObject _source)
    {
        AkSoundEngine.PostEvent("PlayerDie", _source);
    }

    public void CantBuyPlay(GameObject _source)
    {
        AkSoundEngine.PostEvent("CantBuy", _source);
    }

    //Players invocation Sound
    public void TrapSpawnPlay(GameObject _source)
    {
        AkSoundEngine.PostEvent("TrapSpawn", _source);
    }

    public void TrapSellPlay(GameObject _source)
    {
        AkSoundEngine.PostEvent("TrapSell", _source);
    }

    public void WallActivationPlay(GameObject _source)
    {
        AkSoundEngine.PostEvent("WallActivation", _source);
    }

    public void WallEnemyHitPlay(GameObject _source)
    {
        AkSoundEngine.PostEvent("WallEnemyHit", _source);
    }

    public void LogHitPlay(GameObject _source)
    {
        AkSoundEngine.PostEvent("LogHit", _source);
    }




    //Ennemi run Sound
    public void RunEnnemiPlay(GameObject _source)
    {

        AkSoundEngine.PostEvent("RunEnnemiPlay", _source);
    }

    public void RunEnnemiStop(GameObject _source)
    {
        AkSoundEngine.PostEvent("RunEnnemiStop", _source);
    }

    public void GolemIdlePlay(GameObject _source)
    {

        AkSoundEngine.PostEvent("GolemIdlePlay", _source);
    }

    public void GolemIdleStop(GameObject _source)
    {
        AkSoundEngine.PostEvent("GolemIdleStop", _source);
    }

    //Ennemi fire Sound
    public void EnnemiFireballPlay(GameObject _source)
    {

        AkSoundEngine.PostEvent("EnemyFireballPlay", _source);
    }

    public void EnnemiFirePlay(GameObject _source)
    {

        AkSoundEngine.PostEvent("EnnemiFirePlay", _source);
    }

    public void EnnemiFireStop(GameObject _source)
    {
        AkSoundEngine.PostEvent("EnnemiFireStop", _source);
    }

    public void GolemFirePlay(GameObject _source)
    {

        AkSoundEngine.PostEvent("GolemFirePlay", _source);
    }

    public void GolemFireStop(GameObject _source)
    {
        AkSoundEngine.PostEvent("GolemFireStop", _source);
    }

    //Ennemi attack Sound
    public void EnnemiAttack(GameObject _source)
    {

        AkSoundEngine.PostEvent("EnnemiAttack", _source);
    }

    public void GolemAttack(GameObject _source)
    {

        AkSoundEngine.PostEvent("GolemAttack", _source);
    }

    //Ennemi hurt Sound
    public void EnemyHurtPlay(GameObject _source)
    {

        AkSoundEngine.PostEvent("EnemyHurt", _source);
    }

    public void GolemHurtPlay(GameObject _source)
    {

        AkSoundEngine.PostEvent("GolemHurt", _source);
    }

    public void GolemDeathPlay(GameObject _source)
    {

        AkSoundEngine.PostEvent("GolemDeath", _source);
    }

    //Ennemi run Sound
    public void InvocationRunPlay(GameObject _source)
    {

        AkSoundEngine.PostEvent("InvocationRunPlay", _source);
    }

    public void InvocationRunStop(GameObject _source)
    {
        AkSoundEngine.PostEvent("InvocationRunStop", _source);
    }

    public void InvocationHurtPlay(GameObject _source)
    {

        AkSoundEngine.PostEvent("InvocationHurt", _source);
    }

    public void InvocationHitPlay(GameObject _source)
    {

        AkSoundEngine.PostEvent("InvocationHit", _source);
    }

    //Menu Sound
    public void MenuSelect(GameObject _source)
    {
        AkSoundEngine.PostEvent("MenuSelect", _source);
    }
    public void MenuConfirm(GameObject _source)
    {
        AkSoundEngine.PostEvent("MenuConfirm", _source);
    }

    //Nexus idle Sound
    public void NexusIdlePlay(GameObject _source)
    {
        AkSoundEngine.PostEvent("NexusIdlePlay", _source);
//        Debug.Log("play");
    }

    public void NexusIdleStop(GameObject _source)
    {
        AkSoundEngine.PostEvent("NexusIdleStop", _source);
      //  Debug.Log("stop");
    }

    public void NexusHitPlay(GameObject _source)
    {
        AkSoundEngine.PostEvent("NexusHit", _source);
    }
    public void NexusDeathPlay(GameObject _source)
    {
        AkSoundEngine.PostEvent("NexusDeath", _source);
    }
    //Portal idle Sound
    public void PortalIdlePlay(GameObject _source)
    {

        AkSoundEngine.PostEvent("PortalIdlePlay", _source);
    }

    public void PortalIdleStop(GameObject _source)
    {
        AkSoundEngine.PostEvent("PortalIdleStop", _source);
    }


    //Portal Travel Sound
    public void PortalTravelPlay(GameObject _source)
    {

        AkSoundEngine.PostEvent("PortalTravel", _source);
    }

    //Capacity Change Sound
    public void CapacityChangePlay(GameObject _source)
    {

        AkSoundEngine.PostEvent("CapacityChange", _source);
    }

    public void BodyDropPlay(GameObject _source)
    {
        AkSoundEngine.PostEvent("BodyDrop", _source);
    }

    public void WaveAnnouncerPlay(GameObject _source)
    {
        AkSoundEngine.PostEvent("WaveAnnouncerPlay", _source);
    }

    public void WaveEndPlay(GameObject _source)
    {
        AkSoundEngine.PostEvent("WaveEnd", _source);
    }

    // HellSound
    public void HellSoundPlay(GameObject _source)
    {
     
            AkSoundEngine.PostEvent("HellSoundPlay", _source);
        
    }

    public void HellSoundStop(GameObject _source)
    {
        AkSoundEngine.PostEvent("HellSoundStop", _source);
    }


    // FountainSound
    public void FountainSoundPlay(GameObject _source)
    {
     
            AkSoundEngine.PostEvent("FountainPlay", _source);
    }

    public void FountainSoundStop(GameObject _source)
    {
        AkSoundEngine.PostEvent("FountainStop", _source);
    }

    public void StopAllSound()
    {

        AkSoundEngine.StopAll();
        InGameMusicIsPlaying = false;
        IntroAfterLifeIsPlaying = false;
        IntroCreaIsPlaying = false;
        MusicMenuIsPlaying = false;
        GameMusicPlay = false;
        MusicDefeatPlay = false;
        MusicVictoryPlay = false;
        TrailerIsPlaying = false;
    }

}
