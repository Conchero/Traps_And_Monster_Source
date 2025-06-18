using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DataManager : MonoBehaviour
{
    //Le DATA MANAGER doit stocker les manettes connecté dans une table pour les attribuer plus tard
    //Le DATA MANAGER doit stocker la liste des joueurs
    //Le DATA MANAGER doit stocker la carte actuel(si on en met plusieurs)
    //Le DATA MANAGER doit stocker le mode de jeu choisis
    //Le DATA MANAGER doit stocker les données tel que : le score(on pourrait le mettre dans le GM mais on peut en avoir l'utilité après une partie)

    public enum GameMode
    {
        EMPTY,
        AFFRONTEMENT,
        SURVIE
    }
    static DataManager instance = null;
    public static DataManager Instance { get => instance; }
    public State m_states;
    //Permet de revenir à la page de selection de perso après une partie plutot qu'au debut du programme
    public bool m_gameBeenLaunched;
    public GameMode m_gameMode;
    public int m_mapNum = 0;
    public List<Skin> m_prefab = new List<Skin>();
    public List<string> m_playerSColor = new List<string>();
    public List<GameObject> listPrefabModel = new List<GameObject>();
    public List<string> listMeshString = new List<string>();
    public List<InputDevice> m_deviceConnected = new List<InputDevice>();

    public bool m_freeMousse;

	public struct Skin
    {
        public InputDevice device;
        public GameObject go;
        public string meshName;
       // public string sColor;

        public Skin(InputDevice _device, GameObject _go, string _meshName)
        {
            this.device = _device;
            this.go = _go;
            this.meshName = _meshName;
           // this.sColor = _color;
        }
    }
	
    private void Awake()
    {
        m_states = FindObjectOfType<State>();
        if (m_states != null)
        {
            m_states.PrintState();
        }

        if (instance != null)
        {
            Destroy(gameObject);

            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        instance.m_gameBeenLaunched = false;
        m_gameMode = GameMode.EMPTY;
    }

    //public void setGameMode(GameMode _mode)
    //{
    //    m_gameMode = _mode;
    //}

}
