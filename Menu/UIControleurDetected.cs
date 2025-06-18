using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControleurDetected : MonoBehaviour
{
    public StateMenu m_stateMenu;
    int m_nbDevice;
    int m_nbAccount;

    public GameObject[] m_indicators;
    public GameObject[] m_playerWhoJoin;

    //Stock toutes les fenettres de reglages des players
    public GameObject[] m_settingsPlayer;
    public Slider[] m_sliders;


    float m_cooldownClique = 0.0f;
    public float m_cooldownCliqueMax = 0.2f;

    // public bool[] m_settingScreenIsOpen = new bool[4];

   public void OpenSettingsPlayer(int _playerID)
    {
        if (m_settingsPlayer[_playerID].activeSelf)
        {
            m_settingsPlayer[_playerID].SetActive(false);
        }
        else
        {
            m_settingsPlayer[_playerID].SetActive(true);
        }
    }
    bool CanClick()
    {
        if (m_cooldownClique <= 0.0f)
        {
            m_cooldownClique = m_cooldownCliqueMax;
            return true;
        }
        else
        {
            return false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //Click cooldown
        m_cooldownClique = 0;

    }

    // Update is called once per frame
    void Update()
    {
       // Debug.Log("dans l'update ");

        if (m_cooldownClique > 0.0f)
        {
            m_cooldownClique -= Time.deltaTime;
        }

        if (m_stateMenu.m_eMenuState == MenuState.ModeSelection
            || m_stateMenu.m_eMenuState == MenuState.SettingsScreen
            || m_stateMenu.m_eMenuState == MenuState.SkinSelection
            || m_stateMenu.m_eMenuState == MenuState.MapSelection)
        {
           // Debug.Log("dans le bon 'update ");
            UpdateUI();
        }
    if (m_stateMenu.m_eMenuState == MenuState.Home
            || m_stateMenu.m_eMenuState == MenuState.MapSelection)
        {
            m_settingsPlayer[0].SetActive(false); 
            m_settingsPlayer[1].SetActive(false); 
            m_settingsPlayer[2].SetActive(false); 
            m_settingsPlayer[3].SetActive(false); 
        }


    }

    void UpdateUI()
    {
        //Nombres de manettes connecté
        m_nbDevice = DataManager.Instance.m_deviceConnected.Count;
        //nombre de joueurs ajouté à la liste
        m_nbAccount = m_stateMenu.m_playerJoin.Count;

        //for (int i  = 0; i < m_nbDevice; i ++)
        //{
        //    m_settingScreenIsOpen[i] = m_settingsPlayer[i].activeSelf;
        //}

        for (int i = 0; i < m_nbDevice + 1; i++)
        {
           
            //Si il y a un peripherique de co
            if (i == 1)
            {
                //Debug.Log("m_nbAccount" + m_nbAccount);
                //Si aucun joueur n'est ajouté
                if (m_nbAccount == 0)
                {
                    //On active le message d'ajout pour le premier emplacement
                    m_indicators[0].SetActive(true);
                    //On desactive le message d'ajout pour les autres emplacements
                    m_indicators[1].SetActive(false);
                    m_indicators[2].SetActive(false);
                    m_indicators[3].SetActive(false);

                    //On desactive tous les avatares de perso
                    m_playerWhoJoin[0].SetActive(false);
                    m_playerWhoJoin[1].SetActive(false);
                    m_playerWhoJoin[2].SetActive(false);
                    m_playerWhoJoin[3].SetActive(false);

                    //On desactive tous les folders de settings
                    m_settingsPlayer[0].SetActive(false);
                    m_settingsPlayer[1].SetActive(false);
                    m_settingsPlayer[2].SetActive(false);
                    m_settingsPlayer[3].SetActive(false);
                }
                //Si un joueur est ajouté
                else if (m_nbAccount == 1)
                {
                    //On desactive le message d'ajout pour les autres emplacements car il n'y as plus de manette supplementaire
                    m_indicators[0].SetActive(false);
                    m_indicators[1].SetActive(false);
                    m_indicators[2].SetActive(false);
                    m_indicators[3].SetActive(false);


                    //On active le premier avatar de perso
                    m_playerWhoJoin[0].SetActive(true);
                    m_playerWhoJoin[0].GetComponent<MenuFicheAccount>().m_UINeedUpdate = true;
                    //On desactive les autres avatars de perso
                    m_playerWhoJoin[1].SetActive(false);
                    m_playerWhoJoin[2].SetActive(false);
                    m_playerWhoJoin[3].SetActive(false);

                    //On desactive tous les autres folders de settings
                    m_settingsPlayer[1].SetActive(false);
                    m_settingsPlayer[2].SetActive(false);
                    m_settingsPlayer[3].SetActive(false);
                }
            }

            //Pas utile si on est en mode survie (un joueur)
            if (DataManager.Instance.m_gameMode == DataManager.GameMode.AFFRONTEMENT || DataManager.Instance.m_gameMode == DataManager.GameMode.EMPTY)
            {
                //Si il y a deux peripheriques de co
                if (i == 2)
                {
                    //Si aucun joueur n'est ajouté
                    if (m_nbAccount == 0)
                    {
                        if (m_stateMenu.m_eMenuState != MenuState.MapSelection)
                        {
                            //On active le message d'ajout pour le premier emplacement
                            m_indicators[0].SetActive(true);
                        }
                        else
                        {
                            //On desactive le message d'ajout pour le premier emplacement
                            m_indicators[0].SetActive(false);
                        }
                        //On desactive le message d'ajout pour les autres emplacements
                        m_indicators[1].SetActive(false);
                        m_indicators[2].SetActive(false);
                        m_indicators[3].SetActive(false);

                        //On desactive tous les avatares de perso
                        m_playerWhoJoin[0].SetActive(false);
                        m_playerWhoJoin[1].SetActive(false);
                        m_playerWhoJoin[2].SetActive(false);
                        m_playerWhoJoin[3].SetActive(false);

                        //On desactive tous les autres folders de settings
                        m_settingsPlayer[0].SetActive(false);
                        m_settingsPlayer[1].SetActive(false);
                        m_settingsPlayer[2].SetActive(false);
                        m_settingsPlayer[3].SetActive(false);
                    }
                    //Si un joueur est ajouté
                    else if (m_nbAccount == 1)
                    {
                        if (m_stateMenu.m_eMenuState != MenuState.MapSelection)
                        {
                            //Debug.Log("active");
                            //On active le message d'ajout 
                            m_indicators[1].SetActive(true);
                        }
                        else
                        {
                            // Debug.Log("Desactive");
                            //On desactive le message d'ajout 
                            m_indicators[1].SetActive(false);
                        }
                        //On desactive le message d'ajout pour les autres emplacements car il n'y as plus de manette supplementaire
                        m_indicators[0].SetActive(false);
                        m_indicators[2].SetActive(false);
                        m_indicators[3].SetActive(false);


                        //On active le premier avatar de perso
                        m_playerWhoJoin[0].SetActive(true);
                        m_playerWhoJoin[0].GetComponent<MenuFicheAccount>().m_UINeedUpdate = true;
                        //On desactive les autres avatars de perso
                        m_playerWhoJoin[1].SetActive(false);
                        m_playerWhoJoin[2].SetActive(false);
                        m_playerWhoJoin[3].SetActive(false);

                        //On desactive tous les autres folders de settings
                        m_settingsPlayer[1].SetActive(false);
                        m_settingsPlayer[2].SetActive(false);
                        m_settingsPlayer[3].SetActive(false);
                    }
                    //Si deux joueurs sont ajouté
                    else if (m_nbAccount == 2)
                    {
                        //On desactive le message d'ajout pour les autres emplacements car il n'y as plus de manette supplementaire
                        m_indicators[0].SetActive(false);
                        m_indicators[1].SetActive(false);
                        m_indicators[2].SetActive(false);
                        m_indicators[3].SetActive(false);


                        //On active le premier avatar de perso
                        m_playerWhoJoin[0].SetActive(true);
                        m_playerWhoJoin[0].GetComponent<MenuFicheAccount>().m_UINeedUpdate = true;
                        //On active le deuxieme avatar de perso
                        m_playerWhoJoin[1].SetActive(true);
                        m_playerWhoJoin[1].GetComponent<MenuFicheAccount>().m_UINeedUpdate = true;
                        //On desactive les autres avatars de perso
                        m_playerWhoJoin[2].SetActive(false);
                        m_playerWhoJoin[3].SetActive(false);

                        //On desactive tous les autres folders de settings
                        m_settingsPlayer[2].SetActive(false);
                        m_settingsPlayer[3].SetActive(false);
                    }
                }


                //Si il y a trois peripheriques de co
                if (i == 3)
                {
                    //Si aucun joueur n'est ajouté
                    if (m_nbAccount == 0)
                    {

                        if (m_stateMenu.m_eMenuState != MenuState.MapSelection)
                        {
                            //On active le message d'ajout 
                            m_indicators[0].SetActive(true);
                        }
                        else
                        {
                            //On desactive le message d'ajout 
                            m_indicators[0].SetActive(false);
                        }
                        //On desactive le message d'ajout pour les autres emplacements
                        m_indicators[1].SetActive(false);
                        m_indicators[2].SetActive(false);
                        m_indicators[3].SetActive(false);

                        //On desactive tous les avatares de perso
                        m_playerWhoJoin[0].SetActive(false);
                        m_playerWhoJoin[1].SetActive(false);
                        m_playerWhoJoin[2].SetActive(false);
                        m_playerWhoJoin[3].SetActive(false);

                        //On desactive tous les autres folders de settings
                        m_settingsPlayer[0].SetActive(false);
                        m_settingsPlayer[1].SetActive(false);
                        m_settingsPlayer[2].SetActive(false);
                        m_settingsPlayer[3].SetActive(false);
                    }
                    //Si un joueur est ajouté
                    else if (m_nbAccount == 1)
                    {

                        if (m_stateMenu.m_eMenuState != MenuState.MapSelection)
                        {
                            //On active le message d'ajout 
                            m_indicators[1].SetActive(true);
                        }
                        else
                        {
                            //On desactive le message d'ajout 
                            m_indicators[1].SetActive(false);
                        }
                        //On desactive le message d'ajout pour les autres emplacements car il n'y as plus de manette supplementaire
                        m_indicators[0].SetActive(false);
                        m_indicators[2].SetActive(false);
                        m_indicators[3].SetActive(false);


                        //On active le premier avatar de perso
                        m_playerWhoJoin[0].SetActive(true);
                        m_playerWhoJoin[0].GetComponent<MenuFicheAccount>().m_UINeedUpdate = true;
                        //On desactive les autres avatars de perso
                        m_playerWhoJoin[1].SetActive(false);
                        m_playerWhoJoin[2].SetActive(false);
                        m_playerWhoJoin[3].SetActive(false);

                        //On desactive tous les autres folders de settings
                        m_settingsPlayer[1].SetActive(false);
                        m_settingsPlayer[2].SetActive(false);
                        m_settingsPlayer[3].SetActive(false);
                    }
                    //Si deux joueurs sont ajouté
                    else if (m_nbAccount == 2)
                    {

                        if (m_stateMenu.m_eMenuState != MenuState.MapSelection)
                        {
                            //On active le message d'ajout 
                            m_indicators[2].SetActive(true);
                        }
                        else
                        {
                            //On desactive le message d'ajout 
                            m_indicators[2].SetActive(false);
                        }
                        //On desactive le message d'ajout pour les autres emplacements car il n'y as plus de manette supplementaire
                        m_indicators[0].SetActive(false);
                        m_indicators[1].SetActive(false);
                        m_indicators[3].SetActive(false);

                        //On active le premier avatar de perso
                        m_playerWhoJoin[0].SetActive(true);
                        m_playerWhoJoin[0].GetComponent<MenuFicheAccount>().m_UINeedUpdate = true;
                        //On active le deuxieme avatar de perso
                        m_playerWhoJoin[1].SetActive(true);
                        m_playerWhoJoin[1].GetComponent<MenuFicheAccount>().m_UINeedUpdate = true;
                        //On active le troisieme avatar de perso
                        m_playerWhoJoin[2].SetActive(false);
                        //On desactive les autres avatars de perso
                        m_playerWhoJoin[3].SetActive(false);


                        //On desactive tous les autres folders de settings
                        m_settingsPlayer[2].SetActive(false);
                        m_settingsPlayer[3].SetActive(false);
                    }
                    //Si trois joueurs sont ajoutés
                    else if (m_nbAccount == 3)
                    {
                        //On desactive le message d'ajout pour les autres emplacements car il n'y as plus de manette supplementaire
                        m_indicators[0].SetActive(false);
                        m_indicators[1].SetActive(false);
                        m_indicators[2].SetActive(false);
                        m_indicators[3].SetActive(false);


                        //On active le premier avatar de perso
                        m_playerWhoJoin[0].SetActive(true);
                        m_playerWhoJoin[0].GetComponent<MenuFicheAccount>().m_UINeedUpdate = true;
                        //On active le deuxieme avatar de perso
                        m_playerWhoJoin[1].SetActive(true);
                        m_playerWhoJoin[1].GetComponent<MenuFicheAccount>().m_UINeedUpdate = true;
                        //On active le troisieme avatar de perso
                        m_playerWhoJoin[2].SetActive(true);
                        m_playerWhoJoin[2].GetComponent<MenuFicheAccount>().m_UINeedUpdate = true;
                        //On desactive les autres avatars de perso
                        m_playerWhoJoin[3].SetActive(false);


                        //On desactive tous les autres folders de settings
                        m_settingsPlayer[3].SetActive(false);
                    }
                }

                //Si il y a 4 peripheriques de co
                if (i == 4)
                {
                    //Si aucun joueur n'est ajouté
                    if (m_nbAccount == 0)
                    {

                        if (m_stateMenu.m_eMenuState != MenuState.MapSelection)
                        {
                            //On active le message d'ajout 
                            m_indicators[0].SetActive(true);
                        }
                        else
                        {
                            //On desactive le message d'ajout 
                            m_indicators[0].SetActive(false);
                        }
                        //On desactive le message d'ajout pour les autres emplacements
                        m_indicators[1].SetActive(false);
                        m_indicators[2].SetActive(false);
                        m_indicators[3].SetActive(false);

                        //On desactive tous les avatares de perso
                        m_playerWhoJoin[0].SetActive(false);
                        m_playerWhoJoin[1].SetActive(false);
                        m_playerWhoJoin[2].SetActive(false);
                        m_playerWhoJoin[3].SetActive(false);

                        //On desactive tous les  folders de settings
                        m_settingsPlayer[0].SetActive(false);
                        m_settingsPlayer[1].SetActive(false);
                        m_settingsPlayer[2].SetActive(false);
                        m_settingsPlayer[3].SetActive(false);
                    }
                    //Si un joueur est ajouté
                    else if (m_nbAccount == 1)
                    {
                        if (m_stateMenu.m_eMenuState != MenuState.MapSelection)
                        {
                            //On active le message d'ajout 
                            m_indicators[1].SetActive(true);
                        }
                        else
                        {
                            //On desactive le message d'ajout 
                            m_indicators[1].SetActive(false);
                        }
                        //On desactive le message d'ajout pour les autres emplacements car il n'y as plus de manette supplementaire
                        m_indicators[0].SetActive(false);
                        m_indicators[2].SetActive(false);
                        m_indicators[3].SetActive(false);


                        //On active le premier avatar de perso
                        m_playerWhoJoin[0].SetActive(true);
                        m_playerWhoJoin[0].GetComponent<MenuFicheAccount>().m_UINeedUpdate = true;
                        //On desactive les autres avatars de perso
                        m_playerWhoJoin[1].SetActive(false);
                        m_playerWhoJoin[2].SetActive(false);
                        m_playerWhoJoin[3].SetActive(false);

                        //On desactive tous les autres folders de settings
                        m_settingsPlayer[1].SetActive(false);
                        m_settingsPlayer[2].SetActive(false);
                        m_settingsPlayer[3].SetActive(false);
                    }
                    //Si deux joueurs sont ajouté
                    else if (m_nbAccount == 2)
                    {
                        if (m_stateMenu.m_eMenuState != MenuState.MapSelection)
                        {
                            //On active le message d'ajout 
                            m_indicators[2].SetActive(true);
                        }
                        else
                        {
                            //On desactive le message d'ajout 
                            m_indicators[2].SetActive(false);
                        }
                        //On desactive le message d'ajout pour les autres emplacements car il n'y as plus de manette supplementaire
                        m_indicators[0].SetActive(false);
                        m_indicators[1].SetActive(false);
                        m_indicators[3].SetActive(false);


                        //On active le premier avatar de perso
                        m_playerWhoJoin[0].SetActive(true);
                        m_playerWhoJoin[0].GetComponent<MenuFicheAccount>().m_UINeedUpdate = true;
                        //On active le deuxieme avatar de perso
                        m_playerWhoJoin[1].SetActive(true);
                        m_playerWhoJoin[1].GetComponent<MenuFicheAccount>().m_UINeedUpdate = true;
                        //On active le troisieme avatar de perso
                        m_playerWhoJoin[2].SetActive(false);
                        //On desactive les autres avatars de perso
                        m_playerWhoJoin[3].SetActive(false);

                        //On desactive tous les autres folders de settings
                        m_settingsPlayer[2].SetActive(false);
                        m_settingsPlayer[3].SetActive(false);
                    }
                    //Si trois joueurs sont ajoutés
                    else if (m_nbAccount == 3)
                    {

                        if (m_stateMenu.m_eMenuState != MenuState.MapSelection)
                        {
                            //On active le message d'ajout 
                            m_indicators[3].SetActive(true);
                        }
                        else
                        {
                            //On desactive le message d'ajout 
                            m_indicators[3].SetActive(false);
                        }
                        //On desactive le message d'ajout pour les autres emplacements car il n'y as plus de manette supplementaire
                        m_indicators[0].SetActive(false);
                        m_indicators[1].SetActive(false);
                        m_indicators[2].SetActive(false);


                        //On active le premier avatar de perso
                        m_playerWhoJoin[0].SetActive(true);
                        m_playerWhoJoin[0].GetComponent<MenuFicheAccount>().m_UINeedUpdate = true;
                        //On active le deuxieme avatar de perso
                        m_playerWhoJoin[1].SetActive(true);
                        m_playerWhoJoin[1].GetComponent<MenuFicheAccount>().m_UINeedUpdate = true;
                        //On active le troisieme avatar de perso
                        m_playerWhoJoin[2].SetActive(true);
                        m_playerWhoJoin[2].GetComponent<MenuFicheAccount>().m_UINeedUpdate = true;
                        //On desactive les autres avatars de perso
                        m_playerWhoJoin[3].SetActive(false);


                        //On desactive tous les autres folders de settings
                        m_settingsPlayer[3].SetActive(false);
                    }
                    //Si quatre joueurs sont ajoutés
                    else if (m_nbAccount == 4)
                    {
                        //On desactive le message d'ajout pour les autres emplacements car il n'y as plus de manette supplementaire
                        m_indicators[0].SetActive(false);
                        m_indicators[1].SetActive(false);
                        m_indicators[2].SetActive(false);
                        m_indicators[3].SetActive(false);


                        //On active le premier avatar de perso
                        m_playerWhoJoin[0].SetActive(true);
                        m_playerWhoJoin[0].GetComponent<MenuFicheAccount>().m_UINeedUpdate = true;
                        //On active le deuxieme avatar de perso
                        m_playerWhoJoin[1].SetActive(true);
                        m_playerWhoJoin[1].GetComponent<MenuFicheAccount>().m_UINeedUpdate = true;
                        //On active le troisieme avatar de perso
                        m_playerWhoJoin[2].SetActive(true);
                        m_playerWhoJoin[2].GetComponent<MenuFicheAccount>().m_UINeedUpdate = true;
                        //On desactive les autres avatars de perso
                        m_playerWhoJoin[3].SetActive(true);
                        m_playerWhoJoin[3].GetComponent<MenuFicheAccount>().m_UINeedUpdate = true;
                    }
                }
            }

            if (i < m_nbAccount && (m_stateMenu.m_eMenuState == MenuState.ModeSelection || m_stateMenu.m_eMenuState == MenuState.SettingsScreen || m_stateMenu.m_eMenuState == MenuState.SkinSelection))
            {
                // Debug.Log("change sensi");
                //Debug.Log("m_nbAccount = " + m_nbAccount);
                Slider slider = m_sliders[i];
                slider.value = InputManager.Instance.GetSensitivity(m_stateMenu.m_playerJoin[i].device.GetHashCode());
                //  InputManager.Instance.GetSensitivity(m_stateMenu.m_playerJoin[i].device.GetHashCode());

                // InputManager.Instance.SetSensitivity()

                if ( m_settingsPlayer[i].activeSelf)
                {
                    //Navigation dans la barre de capacité
                    if (m_stateMenu.m_playerJoin[i].device.layout == "Keyboard")
                    {
                        if (Input.GetKeyDown(KeyCode.W))
                        {
                            slider.value--;
                            InputManager.Instance.SetSensitivity(m_stateMenu.m_playerJoin[i].device.GetHashCode(), slider.value);
                            slider.value = InputManager.Instance.GetSensitivity(m_stateMenu.m_playerJoin[i].device.GetHashCode());
                        }
                        if (Input.GetKeyDown(KeyCode.X))
                        {
                            slider.value++;
                            InputManager.Instance.SetSensitivity(m_stateMenu.m_playerJoin[i].device.GetHashCode(), slider.value);
                            slider.value = InputManager.Instance.GetSensitivity(m_stateMenu.m_playerJoin[i].device.GetHashCode());
                        }
                    }
                    if (m_stateMenu.m_playerJoin[i].device.layout != "Keyboard")
                    {
                        /*Vector2 ToucheDirectionnel = InputManager.Instance.ValuePAD(m_stateMenu.m_playerJoin[i].device.name, "Pad");

                        if (ToucheDirectionnel.x == 1 && CanClick())
                        {
                            InputManager.Instance.SetSensitivity(m_stateMenu.m_playerJoin[i].device.GetHashCode(), slider.value + 1f);
                            slider.value = InputManager.Instance.GetSensitivity(m_stateMenu.m_playerJoin[i].device.GetHashCode());

                        }
                        else if (ToucheDirectionnel.x == -1 && CanClick())
                        {
                            InputManager.Instance.SetSensitivity(m_stateMenu.m_playerJoin[i].device.GetHashCode(), slider.value - 1f);
                            slider.value = InputManager.Instance.GetSensitivity(m_stateMenu.m_playerJoin[i].device.GetHashCode());

                        }*/
						if(InputManager.Instance.isPressed(m_stateMenu.m_playerJoin[i].device.name, "LeftBumper", true) && CanClick())
                        {
                            slider.value--;
                            InputManager.Instance.SetSensitivity(m_stateMenu.m_playerJoin[i].device.GetHashCode(), slider.value);
                            slider.value = InputManager.Instance.GetSensitivity(m_stateMenu.m_playerJoin[i].device.GetHashCode());
						}
						else if(InputManager.Instance.isPressed(m_stateMenu.m_playerJoin[i].device.name, "RightBumper", true) && CanClick())
                        {
                            slider.value++;
                            InputManager.Instance.SetSensitivity(m_stateMenu.m_playerJoin[i].device.GetHashCode(), slider.value);
                            slider.value = InputManager.Instance.GetSensitivity(m_stateMenu.m_playerJoin[i].device.GetHashCode());
						}
                    }
                }
            }
        }
    }
}
