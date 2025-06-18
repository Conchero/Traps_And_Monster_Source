using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollecteur : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectable"))
        {
            if (other.GetComponent<Collectable>().AlreadyCollected == false)
            {
                //Debug.Log("Add m:oney");
                //On donne l'argent contenu dans la gemme au player
                GetComponent<EntityPlayer>().AddMoney(other.GetComponent<Collectable>().Value);
                other.GetComponent<Collectable>().Destroy();
            }
        }
    }
}
