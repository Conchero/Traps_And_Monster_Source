using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlay : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.transform.parent.transform.parent.GetComponentInParent<EntityPlayer>() != null)
        {
            if (gameObject.transform.parent.transform.parent.GetComponentInParent<EntityPlayer>().m_playerId != 0)
            {
                Destroy(GetComponent<MusicPlay>());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SoundManager.Instance.GameMusicPlay)
        {
            SoundManager.Instance.InGameMusicPlay(gameObject);
        }
        else if (SoundManager.Instance.GameMusicPlay == false)
        {
            SoundManager.Instance.InGameMusicStop(gameObject);
        }

        if (SoundManager.Instance.MusicDefeatPlay)
        {
            SoundManager.Instance.DefeatMusicPlay(gameObject);
        }
        else if (SoundManager.Instance.MusicDefeatPlay == false)
        {
            SoundManager.Instance.DefeatMusicStop(gameObject);
        }

        if (SoundManager.Instance.MusicVictoryPlay)
        {
            SoundManager.Instance.VictoryMusicPlay(gameObject);
        }
        else if (SoundManager.Instance.MusicVictoryPlay == false)
        {
            SoundManager.Instance.VictoryMusicStop(gameObject);
        }
    }
}
