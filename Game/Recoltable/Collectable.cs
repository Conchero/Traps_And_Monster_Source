using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public enum Type
    {
        GEMME,
        HEAL,
        BOOST_ARMOR,
    }

    //Reference du prefab
    public GameObject m_prefabMeshCollectable;
    public Type m_type;


    //permet de savoir si l'objet a deja été collecté
    bool m_alreadyCollected;
    public bool AlreadyCollected { get => m_alreadyCollected; set => m_alreadyCollected = value; }


    //gemme value
    private int m_value;
    public int Value { get => m_value; set => this.m_value = value; }

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(m_prefabMeshCollectable, transform);
        AlreadyCollected = false;
    }

    void Update()
    {

    }


    /// <summary>
    /// Destroy the game object visuel and this game object
    /// </summary>
    public void Destroy()
    {
        AlreadyCollected = true;
        Destroy(gameObject);
    }
}
