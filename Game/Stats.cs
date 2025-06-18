using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public Dictionary<string, int> nbDeath = new Dictionary<string, int>();
    public Dictionary<string, int> nbKillPlayer = new Dictionary<string, int>();
    public Dictionary<string, int> nbKillInvoc = new Dictionary<string, int>();
    public Dictionary<string, int> nbKillDemons = new Dictionary<string, int>();
    public Dictionary<string, int> nbKillNexus = new Dictionary<string, int>();
    public Dictionary<string, int> nbVaguesSurvive = new Dictionary<string, int>();
    public Dictionary<string, int> nbPoseTrap = new Dictionary<string, int>();
    public Dictionary<string, int> nbKillEnnemieByTrap = new Dictionary<string, int>();

    StateGame m_stateGame;

    private void Awake()
    {
        // Init
        // Death
        nbDeath["Red"] = 0;
        nbDeath["Blue"] = 0;
        nbDeath["Green"] = 0;
        nbDeath["Yellow"] = 0;

        // Kill player
        nbKillPlayer["Red"] = 0;
        nbKillPlayer["Blue"] = 0;
        nbKillPlayer["Green"] = 0;
        nbKillPlayer["Yellow"] = 0;

        // Kill invoc
        nbKillInvoc["Red"] = 0;
        nbKillInvoc["Blue"] = 0;
        nbKillInvoc["Green"] = 0;
        nbKillInvoc["Yellow"] = 0;

        // Kill demons
        nbKillDemons["Red"] = 0;
        nbKillDemons["Blue"] = 0;
        nbKillDemons["Green"] = 0;
        nbKillDemons["Yellow"] = 0;

        // Kill nexus
        nbKillNexus["Red"] = 0;
        nbKillNexus["Blue"] = 0;
        nbKillNexus["Green"] = 0;
        nbKillNexus["Yellow"] = 0;

        // Vagues survie
        nbVaguesSurvive["Red"] = 0;
        nbVaguesSurvive["Blue"] = 0;
        nbVaguesSurvive["Green"] = 0;
        nbVaguesSurvive["Yellow"] = 0;

        // Pose pièges
        nbPoseTrap["Red"] = 0;
        nbPoseTrap["Blue"] = 0;
        nbPoseTrap["Green"] = 0;
        nbPoseTrap["Yellow"] = 0;

        // Kill ennemie by pièges
        nbKillEnnemieByTrap["Red"] = 0;
        nbKillEnnemieByTrap["Blue"] = 0;
        nbKillEnnemieByTrap["Green"] = 0;
        nbKillEnnemieByTrap["Yellow"] = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_stateGame = FindObjectOfType<StateGame>();
    }

    // Update is called once per frame
    void Update()
    {
        MAJTextStats();
        //ConsoleOutput();
    }

    void MAJTextStats()
    {
        foreach (KeyValuePair<string, int> kvp in nbDeath)
        {
            switch (kvp.Key)
            {
                case "Red":
                    m_stateGame.m_TextStatScoreboard[0].text = kvp.Value.ToString();
                    break;
                case "Blue":
                    m_stateGame.m_TextStatScoreboard[1].text = kvp.Value.ToString();
                    break;
                case "Green":
                    m_stateGame.m_TextStatScoreboard[2].text = kvp.Value.ToString();
                    break;
                case "Yellow":
                    m_stateGame.m_TextStatScoreboard[3].text = kvp.Value.ToString();
                    break;
                default:
                    break;
            }
        }

        foreach (KeyValuePair<string, int> kvp in nbKillPlayer)
        {
            switch (kvp.Key)
            {
                case "Red":
                    m_stateGame.m_TextStatScoreboard[4].text = kvp.Value.ToString();
                    break;
                case "Blue":
                    m_stateGame.m_TextStatScoreboard[5].text = kvp.Value.ToString();
                    break;
                case "Green":
                    m_stateGame.m_TextStatScoreboard[6].text = kvp.Value.ToString();
                    break;
                case "Yellow":
                    m_stateGame.m_TextStatScoreboard[7].text = kvp.Value.ToString();
                    break;
                default:
                    break;
            }
        }

        foreach (KeyValuePair<string, int> kvp in nbKillInvoc)
        {
            switch (kvp.Key)
            {
                case "Red":
                    m_stateGame.m_TextStatScoreboard[8].text = kvp.Value.ToString();
                    break;
                case "Blue":
                    m_stateGame.m_TextStatScoreboard[9].text = kvp.Value.ToString();
                    break;
                case "Green":
                    m_stateGame.m_TextStatScoreboard[10].text = kvp.Value.ToString();
                    break;
                case "Yellow":
                    m_stateGame.m_TextStatScoreboard[11].text = kvp.Value.ToString();
                    break;
                default:
                    break;
            }
        }

        foreach (KeyValuePair<string, int> kvp in nbKillDemons)
        {
            switch (kvp.Key)
            {
                case "Red":
                    m_stateGame.m_TextStatScoreboard[12].text = kvp.Value.ToString();
                    break;
                case "Blue":
                    m_stateGame.m_TextStatScoreboard[13].text = kvp.Value.ToString();
                    break;
                case "Green":
                    m_stateGame.m_TextStatScoreboard[14].text = kvp.Value.ToString();
                    break;
                case "Yellow":
                    m_stateGame.m_TextStatScoreboard[15].text = kvp.Value.ToString();
                    break;
                default:
                    break;
            }
        }

        foreach (KeyValuePair<string, int> kvp in nbKillNexus)
        {
            switch (kvp.Key)
            {
                case "Red":
                    m_stateGame.m_TextStatScoreboard[16].text = kvp.Value.ToString();
                    break;
                case "Blue":
                    m_stateGame.m_TextStatScoreboard[17].text = kvp.Value.ToString();
                    break;
                case "Green":
                    m_stateGame.m_TextStatScoreboard[18].text = kvp.Value.ToString();
                    break;
                case "Yellow":
                    m_stateGame.m_TextStatScoreboard[19].text = kvp.Value.ToString();
                    break;
                default:
                    break;
            }
        }

        foreach (KeyValuePair<string, int> kvp in nbVaguesSurvive)
        {
            switch (kvp.Key)
            {
                case "Red":
                    m_stateGame.m_TextStatScoreboard[20].text = kvp.Value.ToString();
                    break;
                case "Blue":
                    m_stateGame.m_TextStatScoreboard[21].text = kvp.Value.ToString();
                    break;
                case "Green":
                    m_stateGame.m_TextStatScoreboard[22].text = kvp.Value.ToString();
                    break;
                case "Yellow":
                    m_stateGame.m_TextStatScoreboard[23].text = kvp.Value.ToString();
                    break;
                default:
                    break;
            }
        }

        foreach (KeyValuePair<string, int> kvp in nbPoseTrap)
        {
            switch (kvp.Key)
            {
                case "Red":
                    m_stateGame.m_TextStatScoreboard[24].text = kvp.Value.ToString();
                    break;
                case "Blue":
                    m_stateGame.m_TextStatScoreboard[25].text = kvp.Value.ToString();
                    break;
                case "Green":
                    m_stateGame.m_TextStatScoreboard[26].text = kvp.Value.ToString();
                    break;
                case "Yellow":
                    m_stateGame.m_TextStatScoreboard[27].text = kvp.Value.ToString();
                    break;
                default:
                    break;
            }
        }

        foreach (KeyValuePair<string, int> kvp in nbKillEnnemieByTrap)
        {
            switch (kvp.Key)
            {
                case "Red":
                    m_stateGame.m_TextStatScoreboard[28].text = kvp.Value.ToString();
                    break;
                case "Blue":
                    m_stateGame.m_TextStatScoreboard[29].text = kvp.Value.ToString();
                    break;
                case "Green":
                    m_stateGame.m_TextStatScoreboard[30].text = kvp.Value.ToString();
                    break;
                case "Yellow":
                    m_stateGame.m_TextStatScoreboard[31].text = kvp.Value.ToString();
                    break;
                default:
                    break;
            }
        }
    }

    void ConsoleOutput()
    {
        Debug.Log("NbDeath");
        foreach (KeyValuePair<string, int> kvp in nbDeath)
        {
            Debug.LogFormat("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
        }

        Debug.Log("nbKillPlayer");
        foreach (KeyValuePair<string, int> kvp in nbKillPlayer)
        {
            Debug.LogFormat("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
        }

        Debug.Log("nbKillInvoc");
        foreach (KeyValuePair<string, int> kvp in nbKillInvoc)
        {
            Debug.LogFormat("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
        }

        Debug.Log("nbKillDemons");
        foreach (KeyValuePair<string, int> kvp in nbKillDemons)
        {
            Debug.LogFormat("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
        }

        Debug.Log("nbKillNexus");
        foreach (KeyValuePair<string, int> kvp in nbKillNexus)
        {
            Debug.LogFormat("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
        }

        Debug.Log("nbVaguesSurvive");
        foreach (KeyValuePair<string, int> kvp in nbVaguesSurvive)
        {
            Debug.LogFormat("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
        }

        Debug.Log("nbPoseTrap");
        foreach (KeyValuePair<string, int> kvp in nbPoseTrap)
        {
            Debug.LogFormat("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
        }

        Debug.Log("nbKillEnnemieByTrap");
        foreach (KeyValuePair<string, int> kvp in nbKillEnnemieByTrap)
        {
            Debug.LogFormat("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
        }
    }
}
