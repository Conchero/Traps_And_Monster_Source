using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopActivation : MonoBehaviour
{
    TpsController controller;
    string playerName;
    bool shopActive = false;
    bool inputDown = false;

    bool playerIn = false;

    [SerializeField] GameObject Shop;

    public bool ShopActive { get => shopActive; set => shopActive = value; }
    //public bool PlayerIn { get => playerIn; set => playerIn = value; }
    public string PlayerName { get => playerName; set => playerName = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIn == true && controller != null)
        {
            if (InputManager.Instance.GetLayoutDevice(controller.device) == "Keyboard")
            {
                if (Input.GetKeyDown(KeyCode.O))
                {
                    shopActive = !shopActive;
                    controller.PlayerInShop = !controller.PlayerInShop;
                    Shop.SetActive(shopActive);
                }
            }

            if (InputManager.Instance.GetLayoutDevice(controller.device) != "Keyboard")
            {
                if (InputManager.Instance.isPressed(controller.device, "X",true) && inputDown == false)
                {
                    shopActive = !shopActive;
                    controller.PlayerInShop = !controller.PlayerInShop;

                    Shop.SetActive(shopActive);
                    inputDown = true;
                }

                if (shopActive == true)
                {

                }
                if (InputManager.Instance.isPressed(controller.device, "X", false) && inputDown == true)
                {
                    inputDown = false;
                }
            }

        }

    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerName = collider.name;
            controller = GameObject.Find("/" + playerName).GetComponent<TpsController>();
            playerIn = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerIn = false;
            controller = null;
        }
    }
}
