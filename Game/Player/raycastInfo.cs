using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class raycastInfo : MonoBehaviour
{
    [SerializeField] Camera cam;
    [Header("UI traps")]
    [SerializeField] Image repairController;
    [SerializeField] Image repairKeyboard;
    [SerializeField] Image sellingContour;
    [SerializeField] Image sellingBar;
    [SerializeField] Image sellController;
    [SerializeField] Image sellKeyboard;

    float rayLegth = 15f;
    RaycastHit[] hitsTab;
    EntityPlayer player;
    int idPlayer;

    TpsController controller;

    //possible target
    EntityPlayer targetedPlayer;
    Traps targetedTrap;
    Ennemi targetedEnnemi;
    comportementGeneralIA targetedInvoc;
    float ennemiTimer = 0f;
    float playerTimer = 0f;
    float invocTimer = 0f;
    float sellTrapTimer = 0f;
    float sellTrapTotalTime = 1.5f;
    int repairPrice = 10;

    Vector3 pos1player = new Vector3(-700, -300, 0);
    Vector3 pos2player = new Vector3(-800, -150, 0);
    Vector3 pos4Player = new Vector3(-350, -150, 0);
    //UIRaycastAvatar uiRaycastAvatar;

    // Start is called before the first frame update
    void Start()
    {
        controller = gameObject.GetComponentInParent<TpsController>();

        //uiRaycastAvatar = gameObject.GetComponentInChildren<UIRaycastAvatar>();
        player = gameObject.GetComponent<EntityPlayer>();
        idPlayer = player.m_playerId;

        PlaceSellingImages();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(cam.transform.position, cam.transform.forward * rayLegth);
        hitsTab = Physics.RaycastAll(cam.transform.position, cam.transform.forward, rayLegth);
        SortByDistance(hitsTab);

        repairKeyboard.enabled = false;
        repairController.enabled = false;

        if (targetedTrap != null)
        {
            HideTrapLife(targetedTrap);
            targetedTrap = null;
        }

        if(targetedEnnemi != null)
        {
            ennemiTimer += Time.deltaTime;
            if (ennemiTimer >= 2f)
            {
                HideEnnemiLife(targetedEnnemi);
                targetedEnnemi = null;
            }
        }

        if (targetedInvoc != null)
        {
            invocTimer += Time.deltaTime;
            if (invocTimer >= 2f)
            {
                HideInvocationLife(targetedInvoc);
                targetedInvoc = null;
            }
        }

        if (targetedPlayer != null)
        {
            playerTimer += Time.deltaTime;
            int childrens = targetedPlayer.transform.childCount;
            for (int i = 0; i < childrens; i++)
            {
                if (targetedPlayer.transform.GetChild(i).tag == "CanvasTab")
                {
                    targetedPlayer.transform.GetChild(i).GetChild(idPlayer).GetComponent<Canvas>().transform.rotation = cam.transform.rotation;
                }
            }
            if (playerTimer >= 2.5f)
            {
                HidePlayerLife(targetedPlayer);
                targetedPlayer = null;
            }
        }



        for(int i=0; i<hitsTab.Length; i++)
        {
            RaycastHit hit = hitsTab[i];
            
            if(hit.transform.tag == "Wall")
            {
                break;
            }
            if(hit.transform.tag == "Ennemi")
            {
                if (targetedEnnemi != null)
                {
                    HideEnnemiLife(targetedEnnemi);
                }
                targetedEnnemi = hit.collider.GetComponent<Ennemi>();
                DisplayEnnemiLife(targetedEnnemi);
                ennemiTimer = 0;
                break;
            }
            if (hit.transform.tag == "Invoacation")
            {
                if (targetedEnnemi != null)
                {
                    HideInvocationLife(targetedInvoc);
                }
                targetedInvoc = hit.collider.GetComponent<comportementGeneralIA>();
                DisplayInvocationLife(targetedInvoc);
                invocTimer = 0;
                break;
            }
            else if(hit.transform.tag == "Trap")
            {
                targetedTrap = hit.collider.gameObject.GetComponent<Traps>();
                DisplayTrapLife(targetedTrap);
                break;
            }
            else if (hit.transform.tag == "Player")
            {
                if (targetedPlayer != null)
                {
                    HidePlayerLife(targetedPlayer);
                }
                targetedPlayer = hit.collider.GetComponent<EntityPlayer>();
                if (targetedPlayer != null && targetedPlayer.m_playerId != gameObject.GetComponent<EntityPlayer>().m_playerId)
                {
                    DisplayPlayerLife(targetedPlayer);
                    playerTimer = 0;
                    break;
                }
            }
        }
    }

    private void DisplayPlayerLife(EntityPlayer _player)
    {
        if (_player != null && _player.m_playerId != gameObject.GetComponent<EntityPlayer>().m_playerId)
        {
            int childrens = _player.transform.childCount;
            for (int i = 0; i < childrens; i++)
            {
                if (_player.transform.GetChild(i).tag == "CanvasTab")
                {
                    _player.transform.GetChild(i).GetChild(idPlayer).GetComponent<Canvas>().enabled = true;
                    _player.transform.GetChild(i).GetChild(idPlayer).GetComponent<Canvas>().worldCamera = cam;
                    _player.transform.GetChild(i).GetChild(idPlayer).GetComponent<Canvas>().transform.rotation = cam.transform.rotation;
                }
            }
        }
    }

    private void HidePlayerLife(EntityPlayer _player)
    {
        int childrens = _player.transform.childCount;
        for (int i = 0; i < childrens; i++)
        {
            if (_player.transform.GetChild(i).tag == "CanvasTab")
            {
                _player.transform.GetChild(i).GetChild(idPlayer).GetComponent<Canvas>().enabled = false;
                _player.transform.GetChild(i).GetChild(idPlayer).GetComponent<Canvas>().transform.rotation = cam.transform.rotation;
            }
        }
    }

    private void DisplayTrapLife(Traps _trap)
    {
        if(_trap == null)
        {
            return;
        }
        
        TrapAttack trapAttack = _trap.GetComponentInChildren<TrapAttack>();
        if (_trap.currentLife != _trap.maxLife)
        {
            int childrens = _trap.transform.childCount;
            for (int i = 0; i < childrens; i++)
            {
                if (_trap.transform.GetChild(i).tag == "CanvasTab")
                {
                    _trap.transform.GetChild(i).GetChild(idPlayer).GetComponent<Canvas>().enabled = true;
                    _trap.transform.GetChild(i).GetChild(idPlayer).GetComponent<Canvas>().worldCamera = cam;
                }
            }
            if (trapAttack.player.GetComponent<EntityPlayer>().m_playerId == idPlayer)
            {
                //afficher la réparation

                if (InputManager.Instance.GetLayoutDevice(controller.device) == "Keyboard")
                {
                    if (Input.GetKeyDown(KeyCode.E) == true && player.Money>=repairPrice)
                    {
                        repairKeyboard.enabled = true;
                        _trap.Repair();
                        player.RemoveMoney(repairPrice);
                    }
                }
                if (InputManager.Instance.GetLayoutDevice(controller.device) != "Keyboard")
                {
                    repairController.enabled = true;
                    if (InputManager.Instance.isPressed(controller.device, "X", true) && player.Money >= repairPrice)
                    {
                        repairKeyboard.enabled = true;
                        _trap.Repair();
                        player.RemoveMoney(repairPrice);
                    }
                }
            }
        }

        //selling part
        if (trapAttack.player.GetComponent<EntityPlayer>().m_playerId == idPlayer)
        {
            //afficher la réparation

            if (InputManager.Instance.GetLayoutDevice(controller.device) == "Keyboard")
            {
                sellKeyboard.enabled = true;
                if (Input.GetKey(KeyCode.E) == true)
                {
                    sellTrapTimer += Time.deltaTime;
                    sellingBar.fillAmount = sellTrapTimer / sellTrapTotalTime;
                }
                else
                {       
                    sellTrapTimer = 0;
                }
            }
            if (InputManager.Instance.GetLayoutDevice(controller.device) != "Keyboard")
            {
                sellController.enabled = true;
                if (InputManager.Instance.isPressed(controller.device, "X", true))
                {
                    sellTrapTimer += Time.deltaTime;
                    sellingBar.fillAmount = sellTrapTimer / sellTrapTotalTime;
                }
                else
                {
                    sellTrapTimer = 0;
                }
            }
            if(sellTrapTimer >= 0.2f)
            {
                sellingContour.enabled = true;
                sellingBar.enabled = true;
            }
            if(sellTrapTimer >= sellTrapTotalTime)
            {
                player.AddMoney(_trap.price / 2);
                //hiding all images
                repairController.enabled = false;
                repairKeyboard.enabled = false;
                sellingContour.enabled = false;
                sellingBar.enabled = false;
                sellController.enabled = false;
                sellKeyboard.enabled = false;
                _trap.GetSold();
                SoundManager.Instance.TrapSellPlay(gameObject);
            }
        }
    }

    private void HideTrapLife(Traps _trap)
    {
        int childrens = _trap.transform.childCount;
        for (int i = 0; i < childrens; i++)
        {
            if (_trap.transform.GetChild(i).tag == "CanvasTab")
            {
                _trap.transform.GetChild(i).GetChild(idPlayer).GetComponent<Canvas>().enabled = false;
                _trap.transform.GetChild(i).GetChild(idPlayer).GetComponent<Canvas>().transform.rotation = cam.transform.rotation;
            }
        }
        repairController.enabled = false;
        repairKeyboard.enabled = false;
        sellingContour.enabled = false;
        sellingBar.enabled = false;
        sellKeyboard.enabled = false;
        sellController.enabled = false;
    }

    private void DisplayEnnemiLife(Ennemi _ennemi)
    {
        if (_ennemi != null)
        {
            int childrens = _ennemi.transform.childCount;
            for (int i = 0; i < childrens; i++)
            {
                if (_ennemi.transform.GetChild(i).tag == "CanvasTab")
                {
                    _ennemi.transform.GetChild(i).GetChild(idPlayer).GetComponent<Canvas>().enabled = true;
                    _ennemi.transform.GetChild(i).GetChild(idPlayer).GetComponent<Canvas>().worldCamera = cam;
                }
            }
        }
    }

    private void HideEnnemiLife(Ennemi _ennemi)
    {
        int childrens = _ennemi.transform.childCount;
        for (int i = 0; i < childrens; i++)
        {
            if (_ennemi.transform.GetChild(i).tag == "CanvasTab")
            {
                _ennemi.transform.GetChild(i).GetChild(idPlayer).GetComponent<Canvas>().enabled = false;
                _ennemi.transform.GetChild(i).GetChild(idPlayer).GetComponent<Canvas>().transform.rotation = cam.transform.rotation;
            }
        }
    }

    private void DisplayInvocationLife(comportementGeneralIA _invocation)
    {
        if (_invocation != null)
        {
            int childrens = _invocation.transform.childCount;
            for (int i = 0; i < childrens; i++)
            {
                if (_invocation.transform.GetChild(i).tag == "CanvasTab")
                {
                    _invocation.transform.GetChild(i).GetChild(idPlayer).GetComponent<Canvas>().enabled = true;
                    _invocation.transform.GetChild(i).GetChild(idPlayer).GetComponent<Canvas>().worldCamera = cam;
                }
            }
        }
    }

    private void HideInvocationLife(comportementGeneralIA _invocation)
    {
        int childrens = _invocation.transform.childCount;
        for (int i = 0; i < childrens; i++)
        {
            if (_invocation.transform.GetChild(i).tag == "CanvasTab")
            {
                _invocation.transform.GetChild(i).GetChild(idPlayer).GetComponent<Canvas>().enabled = false;
                _invocation.transform.GetChild(i).GetChild(idPlayer).GetComponent<Canvas>().transform.rotation = cam.transform.rotation;
            }
        }
    }


    private void SortByDistance(RaycastHit[] _tab)
    {
        int nbArray = _tab.Length;
        for (int i = 0; i < nbArray-1; i++)
        {
            for ( int j=0; j<nbArray-i-1; j++)
            {
                if(Vector3.Distance(_tab[j].point, transform.position) > Vector3.Distance(_tab[j+1].point, transform.position))
                {
                    RaycastHit temp = _tab[j];
                    _tab[j] = _tab[j + 1];
                    _tab[j + 1] = temp;
                }
            } 
        }
    }

    private void PlaceSellingImages()
    {
        int playerCount = DataManager.Instance.m_prefab.Count;
        switch(playerCount)
        {
            case 1:
                sellController.GetComponent<RectTransform>().localPosition = pos1player;
                sellKeyboard.GetComponent<RectTransform>().localPosition = pos1player;
                break;
            case 2:
                sellController.GetComponent<RectTransform>().localPosition = pos2player;
                sellKeyboard.GetComponent<RectTransform>().localPosition = pos2player;
                break;
            case 3:
            case 4:
                sellController.GetComponent<RectTransform>().localPosition = pos4Player;
                sellKeyboard.GetComponent<RectTransform>().localPosition = pos4Player;
                break;
            default:
                sellController.GetComponent<RectTransform>().localPosition = pos1player;
                sellKeyboard.GetComponent<RectTransform>().localPosition = pos1player;
                break;
        }
    }
}
