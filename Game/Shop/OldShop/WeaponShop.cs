using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShop : MonoBehaviour
{
    TpsController controller;

    string playerName;
    WeaponBehaviour weapons;
    bool playerIn;
    ShopActivation shopActivation;

    bool isChoosingWeapon = true;
    int indexChoosing = 0;
    int indexPlayerWeapons = 0;
    [SerializeField] Button[] weaponsInShop;
    [SerializeField] Button[] playerWeapons;
    [SerializeField] Text money;

    bool changed = false;
    bool inputDown = false;
    int priceWeapon = 0;
    Color weaponsNotAviable = new Color(0.5f, 0.5f, 0.5f, 1);
    Color canChoose = new Color(0, 1, 0, 1);
    Color weaponSelected = new Color(0, 1, 0, 1);
    Color cantChoose = new Color(0.7f, 0.5f, 0.5f, 1);
    Color cantAfford = new Color(1f, 0f, 0f, 1);

    // Start is called before the first frame update
    void Start()
    {
        shopActivation = GetComponent<ShopActivation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (controller != null && controller.PlayerInShop == true)
        {
            //Debug.Log(this.name + ": first");
            if (shopActivation.ShopActive == true)
            {
                money.text = "Money: " + controller.gameObject.GetComponent<EntityPlayer>().Money.ToString();
                //Debug.Log(this.name + ": second");

                if (isChoosingWeapon)
                {
                    ChangeIndex();
                    ChooseWeapon();
                }
                else
                {
                    ChangeIndexPlayerWeapons();
                    playerWeapons[indexPlayerWeapons].GetComponent<Image>().color = weaponSelected;
                    ChooseWeaponToReplace();
                }

                weapons.GetBareHanded();




                for (int i = 0; i < weaponsInShop.Length; i++)
                {
                    if (weaponsInShop[i].GetComponentInChildren<Text>().text == weapons.PlayerWeapons[0])
                    {
                        weaponsInShop[i].GetComponent<Image>().color = weaponsNotAviable;
                        // Debug.Log("1" + weaponsInShop[i].GetComponentInChildren<Text>().text);
                    }
                    else if (weaponsInShop[i].GetComponentInChildren<Text>().text == weapons.PlayerWeapons[1])
                    {
                        weaponsInShop[i].GetComponent<Image>().color = weaponsNotAviable;
                        //Debug.Log("2" + weaponsInShop[i].GetComponentInChildren<Text>().text);
                    }
                    else
                    {
                        weaponsInShop[i].GetComponent<Image>().color = Color.white;
                    }
                }

                int.TryParse(weaponsInShop[indexChoosing].transform.Find(weapons.WeaponList[indexChoosing] + "Price").GetComponent<Text>().text.Replace(" coins", ""), out priceWeapon);

                for (int i = 0; i < playerWeapons.Length; i++)
                {
                    playerWeapons[i].GetComponentInChildren<Text>().text = weapons.PlayerWeapons[i];
                }

                if (weaponsInShop[indexChoosing].GetComponent<Image>().color != weaponsNotAviable)
                {
                    if (controller.gameObject.GetComponent<EntityPlayer>().Money >= priceWeapon)
                    {
                        weaponsInShop[indexChoosing].GetComponent<Image>().color = canChoose;
                    }
                    else if (controller.gameObject.GetComponent<EntityPlayer>().Money < priceWeapon)
                    {
                        weaponsInShop[indexChoosing].GetComponent<Image>().color = cantAfford;
                    }
                }
                else if (weaponsInShop[indexChoosing].GetComponent<Image>().color == weaponsNotAviable)
                {
                    weaponsInShop[indexChoosing].GetComponent<Image>().color = cantChoose;
                }



            }


        }
    }

    void ChangeIndex()
    {
        if (InputManager.Instance.GetLayoutDevice(controller.device) == "Keyboard")
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                weaponsInShop[indexChoosing].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                if (indexChoosing < 4)
                {
                    indexChoosing++;
                }
                else if (indexChoosing == 4)
                {
                    indexChoosing = 0;
                }
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {

                weaponsInShop[indexChoosing].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                if (indexChoosing > 0)
                {
                    indexChoosing--;
                }
                else if (indexChoosing == 0)
                {
                    indexChoosing = 4;
                }
            }
        }

        if (InputManager.Instance.GetLayoutDevice(controller.device) != "Keyboard")
        {
            if (InputManager.Instance.ValuePAD(controller.device, "Pad").y <= -1 && changed == false)
            {
                weaponsInShop[indexChoosing].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                if (indexChoosing < 4)
                {
                    indexChoosing++;
                    changed = true;
                }
                else if (indexChoosing == 4)
                {
                    indexChoosing = 0;
                    changed = true;
                }
            }
            if (InputManager.Instance.ValuePAD(controller.device, "Pad").y >= 1 && changed == false)
            {
                weaponsInShop[indexChoosing].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                if (indexChoosing > 0)
                {
                    indexChoosing--;
                    changed = true;
                }
                else if (indexChoosing == 0)
                {
                    indexChoosing = 4;
                    changed = true;
                }
            }

            if (InputManager.Instance.ValuePAD(controller.device, "Pad").y == 0 && changed == true)
            {
                changed = false;
            }
        }

    }

    void ChangeIndexPlayerWeapons()
    {
        if (InputManager.Instance.GetLayoutDevice(controller.device) == "Keyboard")
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                playerWeapons[indexPlayerWeapons].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                if (indexPlayerWeapons < 1)
                {
                    indexPlayerWeapons++;
                }
                else if (indexPlayerWeapons == 1)
                {
                    indexPlayerWeapons = 0;
                }
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {

                playerWeapons[indexPlayerWeapons].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                if (indexPlayerWeapons > 0)
                {
                    indexPlayerWeapons--;
                }
                else if (indexPlayerWeapons == 0)
                {
                    indexPlayerWeapons = 1;
                }
            }
        }

        if (InputManager.Instance.GetLayoutDevice(controller.device) != "Keyboard")
        {
            if (InputManager.Instance.ValuePAD(controller.device, "Pad").y >= 1 && changed == false)
            {
                playerWeapons[indexPlayerWeapons].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                if (indexPlayerWeapons < 1)
                {
                    indexPlayerWeapons++;
                    changed = true;
                }
                else if (indexPlayerWeapons == 1)
                {
                    indexPlayerWeapons = 0;
                    changed = true;
                }
            }
            if (InputManager.Instance.ValuePAD(controller.device, "Pad").y <= -1 && changed == false)
            {

                playerWeapons[indexPlayerWeapons].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                if (indexPlayerWeapons > 0)
                {
                    indexPlayerWeapons--;
                    changed = true;
                }
                else if (indexPlayerWeapons == 0)
                {
                    indexPlayerWeapons = 1;
                    changed = true;
                }
            }
            if (InputManager.Instance.ValuePAD(controller.device, "Pad").y == 0 && changed == true)
            {
                changed = false;
            }
        }
    }

    void ChooseWeapon()
    {
        if (InputManager.Instance.GetLayoutDevice(controller.device) == "Keyboard")
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (weaponsInShop[indexChoosing].GetComponent<Image>().color == canChoose)
                {
                    isChoosingWeapon = false;
                }
            }
        }

        if (InputManager.Instance.GetLayoutDevice(controller.device) != "Keyboard")
        {
            if (InputManager.Instance.isPressed(controller.device, "A", true) && inputDown == false)
            {
                if (weaponsInShop[indexChoosing].GetComponent<Image>().color == canChoose)
                {
                    isChoosingWeapon = false;
                }
                inputDown = true;
            }
            if (InputManager.Instance.isPressed(controller.device, "A", false) && inputDown == true)
            {
                inputDown = false;
            }
        }
    }

    void ChooseWeaponToReplace()
    {
        if (InputManager.Instance.GetLayoutDevice(controller.device) == "Keyboard")
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                playerWeapons[0].GetComponent<Image>().color = Color.white;
                playerWeapons[1].GetComponent<Image>().color = Color.white;
                isChoosingWeapon = true;
                inputDown = true;
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (indexPlayerWeapons == 0)
                {
                    Destroy(GameObject.Find(playerName).GetComponentInChildren<GetHand>().hand.transform.Find(weapons.PlayerWeapons[indexPlayerWeapons]).gameObject);
                    weapons.PlayerWeapons[indexPlayerWeapons] = weaponsInShop[indexChoosing].GetComponentInChildren<Text>().text;
                    weapons.IndexFirstWeapon = indexChoosing;
                    weapons.InstantiateFirstWeapon();
                }
                if (indexPlayerWeapons == 1)
                {
                    Destroy(GameObject.Find(playerName).GetComponentInChildren<GetHand>().hand.transform.Find(weapons.PlayerWeapons[indexPlayerWeapons]).gameObject);
                    weapons.PlayerWeapons[indexPlayerWeapons] = weaponsInShop[indexChoosing].GetComponentInChildren<Text>().text;
                    weapons.IndexSecondWeapon = indexChoosing;
                    weapons.InstantiateSecondWeapon();
                }
                playerWeapons[0].GetComponent<Image>().color = Color.white;
                playerWeapons[1].GetComponent<Image>().color = Color.white;

                controller.gameObject.GetComponent<EntityPlayer>().RemoveMoney(priceWeapon);

                isChoosingWeapon = true;
            }
        }

        if (InputManager.Instance.GetLayoutDevice(controller.device) != "Keyboard")
        {
            if (InputManager.Instance.isPressed(controller.device, "Y", true) && inputDown == false)
            {
                playerWeapons[0].GetComponent<Image>().color = Color.white;
                playerWeapons[1].GetComponent<Image>().color = Color.white;
                isChoosingWeapon = true;
            }
            if (InputManager.Instance.isPressed(controller.device, "A", true) && inputDown == false)
            {

                if (indexPlayerWeapons == 0)
                {
                    Destroy(GameObject.Find(playerName).GetComponentInChildren<GetHand>().hand.transform.Find(weapons.PlayerWeapons[indexPlayerWeapons]).gameObject);

                    weapons.PlayerWeapons[indexPlayerWeapons] = weaponsInShop[indexChoosing].GetComponentInChildren<Text>().text;
                    weapons.IndexFirstWeapon = indexChoosing;
                    weapons.InstantiateFirstWeapon();
                }
                if (indexPlayerWeapons == 1)
                {
                    Destroy(GameObject.Find(playerName).GetComponentInChildren<GetHand>().hand.transform.Find(weapons.PlayerWeapons[indexPlayerWeapons]).gameObject);

                    weapons.PlayerWeapons[indexPlayerWeapons] = weaponsInShop[indexChoosing].GetComponentInChildren<Text>().text;
                    weapons.IndexSecondWeapon = indexChoosing;
                    weapons.InstantiateSecondWeapon();
                }
                playerWeapons[0].GetComponent<Image>().color = Color.white;
                playerWeapons[1].GetComponent<Image>().color = Color.white;

                controller.gameObject.GetComponent<EntityPlayer>().RemoveMoney(priceWeapon);

                isChoosingWeapon = true;
                inputDown = true;
            }
            if (InputManager.Instance.isPressed(controller.device, "A", false) && inputDown == true)
            {
                inputDown = false;
            }
        }
    }





    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerName = collider.name;
            weapons = GameObject.Find(playerName).GetComponentInChildren<GetHand>().hand.GetComponent<WeaponBehaviour>();
            controller = GameObject.Find("/" + playerName).GetComponent<TpsController>();
            playerIn = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerIn = false;
        }
    }
}
