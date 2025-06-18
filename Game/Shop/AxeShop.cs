using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Profiling;
using UnityEngine.UI;

public class AxeShop : MonoBehaviour
{

    public string shopBelonging;
    GameObject axe;
    bool playerInShop;
    GameObject statsCanvas;
    WeaponBehaviour weapons;
    GameObject player;
    TpsController controller;
    int price;


    //Modif ben
    StateGame m_stateGame = null;
    GameObject m_playerRed = null;
    GameObject m_playerBlue = null;
    GameObject m_playerGreen = null;
    GameObject m_playerYellow = null;

    public GameObject m_objectToChangeLayer;
    CustomSampler codeBen;
    CustomSampler codeShowHide;
    CustomSampler codeUi;
    CustomSampler codeBuy;

    bool click = false;

    // Start is called before the first frame update
    void Start()
    {

        //shopBelonging = gameObject.transform.parent.name.Replace(" Shop", "");
        axe = gameObject.transform.Find("Axe").gameObject;
        statsCanvas = gameObject.transform.Find("Stats").gameObject;
        int.TryParse(statsCanvas.transform.Find("Price").gameObject.GetComponent<TextMeshProUGUI>().text.Replace("coins", ""), out price);



        codeBen = CustomSampler.Create("Code Ben");
        codeShowHide = CustomSampler.Create("Code ShowHide");
        codeUi = CustomSampler.Create("Code ShowUi");
        codeBuy = CustomSampler.Create("Code Buy");
    }

    // Update is called once per frame
    void Update()
    {

        //Modif ben
        if (m_stateGame == null)
        {
            m_stateGame = FindObjectOfType<StateGame>();
            if (m_stateGame != null)
            {
                // Debug.Log("State Game existe");
                for (int i = 0; i < 4; i++)
                {
                    // Debug.Log(" m_stateGame.camps[i].m_isUse = " + m_stateGame.camps[i].m_isUsed);
                    if (m_stateGame.camps[i].m_isUsed)
                    {
                        if (m_stateGame.camps[i].m_sColor == shopBelonging)
                        {

                            switch (m_stateGame.camps[i].m_sColor)
                            {
                                case "Red":
                                    m_playerRed = m_stateGame.camps[i].m_linkedPlayer.gameObject;
                                    break;
                                case "Blue":
                                    m_playerBlue = m_stateGame.camps[i].m_linkedPlayer.gameObject;
                                    break;
                                case "Green":
                                    m_playerGreen = m_stateGame.camps[i].m_linkedPlayer.gameObject;
                                    break;
                                case "Yellow":
                                    m_playerYellow = m_stateGame.camps[i].m_linkedPlayer.gameObject;
                                    break;
                                default:
                                    break;

                            }
                            //Debug.Log(" m_objectToChangeLayer.layer before" + m_objectToChangeLayer.layer);
                            // m_stateGame.camps[i].m_needToUpdateUiPlayer = true;
                            m_objectToChangeLayer.layer = LayerMask.NameToLayer("P" + (m_stateGame.camps[i].m_linkedPlayer.m_playerId + 1).ToString());
                            // Debug.Log(" m_objectToChangeLayer.layer after" + m_objectToChangeLayer.layer);
                        }
                    }

                }

            }
        }



        ShowAndHideWeapons();



        ShowUI();



        BuyWeapons();

        if (player != null)
        {
            if (player.GetComponent<EntityPlayer>().m_isDead == true)
            {
                playerInShop = false;
            }
        }

    }

    void ShowAndHideWeapons()
    {
        if (shopBelonging == "Red")
        {
            if (m_playerRed)
            {
                if (m_playerRed.GetComponentInChildren<GetHand>().hand.transform.Find("Axe") != null)
                {
                    axe.GetComponentInChildren<MeshRenderer>().enabled = false;
                }
                else
                {
                    axe.GetComponentInChildren<MeshRenderer>().enabled = true;
                }
            }
        }
        else if (shopBelonging == "Blue")
        {
            if (m_playerBlue)
            {
                if (m_playerBlue.GetComponentInChildren<GetHand>().hand.transform.Find("Axe") != null)
                {
                    axe.GetComponentInChildren<MeshRenderer>().enabled = false;
                }
                else
                {
                    axe.GetComponentInChildren<MeshRenderer>().enabled = true;
                }
            }
        }
        else if (shopBelonging == "Green")
        {
            if (m_playerGreen)
            {
                if (m_playerGreen.GetComponentInChildren<GetHand>().hand.transform.Find("Axe") != null)
                {
                    axe.GetComponentInChildren<MeshRenderer>().enabled = false;
                }
                else
                {
                    axe.GetComponentInChildren<MeshRenderer>().enabled = true;
                }
            }
        }
        else if (shopBelonging == "Yellow")
        {
            if (m_playerYellow)
            {
                if (m_playerYellow.GetComponentInChildren<GetHand>().hand.transform.Find("Axe") != null)
                {
                    axe.GetComponentInChildren<MeshRenderer>().enabled = false;
                }
                else
                {
                    axe.GetComponentInChildren<MeshRenderer>().enabled = true;
                }
            }
        }
    }

    void ShowUI()
    {
        if (playerInShop == true)
        {
            if (axe.GetComponentInChildren<MeshRenderer>().enabled == true)
            {
                statsCanvas.SetActive(true);
            }
        }
        else if (playerInShop == false)
        {
            statsCanvas.SetActive(false);
        }
    }

    void BuyWeapons()
    {

        if (playerInShop == true)
        {
            if (axe.GetComponentInChildren<MeshRenderer>().enabled == true)
            {
                if (player.GetComponent<EntityPlayer>().Money >= price)
                {
                    if (InputManager.Instance.GetLayoutDevice(controller.device) == "Keyboard")
                    {
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            weapons.GetBareHanded();
                            SoundManager.Instance.BuyWeaponPlay(gameObject);
                            if (weapons.HaveFirstWeapon == true)
                            {
                                Destroy(player.GetComponentInChildren<GetHand>().hand.transform.Find(weapons.PlayerWeapons[0]).gameObject);
                                weapons.PlayerWeapons[0] = axe.name;
                                weapons.IndexFirstWeapon = 1;
                                weapons.InstantiateFirstWeapon();
                                weapons.SelectWeapon1();
                            }
                            else if (weapons.HaveFirstWeapon == false)
                            {
                                Destroy(player.GetComponentInChildren<GetHand>().hand.transform.Find(weapons.PlayerWeapons[1]).gameObject);
                                weapons.PlayerWeapons[1] = axe.name;
                                weapons.IndexSecondWeapon = 1;
                                weapons.InstantiateSecondWeapon();
                                weapons.SelectWeapon2();
                            }
                            player.GetComponent<EntityPlayer>().RemoveMoney(price);
                        }
                    }


                    if (InputManager.Instance.GetLayoutDevice(controller.device) != "Keyboard")
                    {
                        if (InputManager.Instance.isPressed(controller.device, "X", true))
                        {
                            weapons.GetBareHanded();
                            SoundManager.Instance.BuyWeaponPlay(gameObject);
                            if (weapons.HaveFirstWeapon == true)
                            {
                                Destroy(player.GetComponentInChildren<GetHand>().hand.transform.Find(weapons.PlayerWeapons[0]).gameObject);
                                weapons.PlayerWeapons[0] = axe.name;
                                weapons.IndexFirstWeapon = 1;
                                weapons.InstantiateFirstWeapon();
                                weapons.SelectWeapon1();
                            }
                            else if (weapons.HaveFirstWeapon == false)
                            {
                                Destroy(player.GetComponentInChildren<GetHand>().hand.transform.Find(weapons.PlayerWeapons[1]).gameObject);
                                weapons.PlayerWeapons[1] = axe.name;
                                weapons.IndexSecondWeapon = 1;
                                weapons.InstantiateSecondWeapon();
                                weapons.SelectWeapon2();
                            }
                            player.GetComponent<EntityPlayer>().RemoveMoney(price);
                         
                        }
                     
                    }

                }
                else
                {
                    if (InputManager.Instance.GetLayoutDevice(controller.device) == "Keyboard")
                    {
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            player.GetComponent<EntityPlayer>().DrawNotEnoughMoney();
                            SoundManager.Instance.CantBuyPlay(gameObject);
                        }
                    }

                    if (InputManager.Instance.GetLayoutDevice(controller.device) != "Keyboard")
                    {
                        if (InputManager.Instance.isPressed(controller.device, "X", true) && click == false)
                        {
                            player.GetComponent<EntityPlayer>().DrawNotEnoughMoney();
                            SoundManager.Instance.CantBuyPlay(gameObject);
                            click = true;
                        }
                        else if (InputManager.Instance.isPressed(controller.device, "X", false))
                        {
                            click = false;

                        }

                    }

                }



            }
        }
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            if (shopBelonging == "Red")
            {
                if (collider.GetComponent<EntityPlayer>().m_sColor == "Red")
                {
                    playerInShop = true;
                    weapons = GameObject.Find(collider.name).GetComponentInChildren<GetHand>().hand.GetComponent<WeaponBehaviour>();
                    player = GameObject.Find(collider.name).gameObject;
                    controller = GameObject.Find(collider.name).GetComponent<TpsController>();

                    if (InputManager.Instance.GetLayoutDevice(controller.device) == "Keyboard")
                    {
                        statsCanvas.transform.Find("InteractionButton").GetComponent<Text>().text = "Press E \nTo Buy";
                    }

                    if (InputManager.Instance.GetLayoutDevice(controller.device) != "Keyboard")
                    {
                        statsCanvas.transform.Find("InteractionButton").GetComponent<Text>().text = "Press X \nTo Buy";
                    }
                }
            }
            else if (shopBelonging == "Blue")
            {
                if (collider.GetComponent<EntityPlayer>().m_sColor == "Blue")
                {
                    playerInShop = true;
                    weapons = GameObject.Find(collider.name).GetComponentInChildren<GetHand>().hand.GetComponent<WeaponBehaviour>();
                    player = GameObject.Find(collider.name).gameObject;
                    controller = GameObject.Find(collider.name).GetComponent<TpsController>();

                    if (InputManager.Instance.GetLayoutDevice(controller.device) == "Keyboard")
                    {
                        statsCanvas.transform.Find("InteractionButton").GetComponent<Text>().text = "Press E \nTo Buy";
                    }

                    if (InputManager.Instance.GetLayoutDevice(controller.device) != "Keyboard")
                    {
                        statsCanvas.transform.Find("InteractionButton").GetComponent<Text>().text = "Press X \nTo Buy";
                    }
                }
            }
            else if (shopBelonging == "Green")
            {
                if (collider.GetComponent<EntityPlayer>().m_sColor == "Green")
                {
                    playerInShop = true;
                    weapons = GameObject.Find(collider.name).GetComponentInChildren<GetHand>().hand.GetComponent<WeaponBehaviour>();
                    player = GameObject.Find(collider.name).gameObject;
                    controller = GameObject.Find(collider.name).GetComponent<TpsController>();

                    if (InputManager.Instance.GetLayoutDevice(controller.device) == "Keyboard")
                    {
                        statsCanvas.transform.Find("InteractionButton").GetComponent<Text>().text = "Press E \nTo Buy";
                    }

                    if (InputManager.Instance.GetLayoutDevice(controller.device) != "Keyboard")
                    {
                        statsCanvas.transform.Find("InteractionButton").GetComponent<Text>().text = "Press X \nTo Buy";
                    }
                }
            }
            else if (shopBelonging == "Yellow")
            {
                if (collider.GetComponent<EntityPlayer>().m_sColor == "Yellow")
                {
                    playerInShop = true;
                    weapons = GameObject.Find(collider.name).GetComponentInChildren<GetHand>().hand.GetComponent<WeaponBehaviour>();
                    player = GameObject.Find(collider.name).gameObject;
                    controller = GameObject.Find(collider.name).GetComponent<TpsController>();

                    if (InputManager.Instance.GetLayoutDevice(controller.device) == "Keyboard")
                    {
                        statsCanvas.transform.Find("InteractionButton").GetComponent<Text>().text = "Press E \nTo Buy";
                    }

                    if (InputManager.Instance.GetLayoutDevice(controller.device) != "Keyboard")
                    {
                        statsCanvas.transform.Find("InteractionButton").GetComponent<Text>().text = "Press X \nTo Buy";
                    }
                }
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {

        if (collider.CompareTag("Player"))
        {
            if (shopBelonging == "Red")
            {
                if (collider.GetComponent<EntityPlayer>().m_sColor == "Red")
                {
                    playerInShop = false;

                }
            }
            else if (shopBelonging == "Blue")
            {
                if (collider.GetComponent<EntityPlayer>().m_sColor == "Blue")
                {
                    playerInShop = false;

                }
            }
            else if (shopBelonging == "Green")
            {
                if (collider.GetComponent<EntityPlayer>().m_sColor == "Green")
                {
                    playerInShop = false;

                }
            }
            else if (shopBelonging == "Yellow")
            {
                if (collider.GetComponent<EntityPlayer>().m_sColor == "Yellow")
                {
                    playerInShop = false;

                }
            }
        }
    }
}
