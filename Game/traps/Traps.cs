//Script by : Alexis

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Traps : MonoBehaviour
{
    public enum TypeTrap
    {
        FLOOR_TRAP,
        WALL_TRAP,
        ROOF_TRAP
    };
    //number of nodes on a side
    public float length;
    //determine where the trap can be placed
    public TypeTrap type;
    //position
    public Vector3 pos;
    //price
    public int price;
    //life
    public int maxLife;
    public int currentLife;
    //Armor
    public int m_maxArmor;
    public int m_armor;
    //Degats sur vivants  /// A UTILISER !
    public int m_degatsSurVivants;

    public bool isDestroy = false;

    [SerializeField] Image[] lifeVisual;

    [SerializeField] Shader hitShader;
    Shader[] baseShader;
    float hitTimer;
    int childs;
    bool isShaderActive;

    [SerializeField] GameObject particles;

    private void Start()
    {
        currentLife = maxLife;
        m_armor = m_maxArmor;
        //start at 1 to avoid using the shader when placed
        hitTimer = 1;
        childs = transform.childCount;
        for (int i = 0; i < childs; i++)
        {
            if (transform.GetChild(i).tag == "TrapVisual")
            {
                int secondChilds = transform.GetChild(i).childCount;
                baseShader = new Shader[secondChilds];
                for(int j=0; j< secondChilds; j++)
                {
                    baseShader[j] = transform.GetChild(i).GetChild(j).GetComponent<Renderer>().material.shader;
                }
            }
        }

        //particles
        if (particles != null)
        {
            GameObject smoke = Instantiate(particles, transform.position, particles.transform.rotation);
            Destroy(smoke, 2f);
        }

    }

    private void Update()
    {
        if(isDestroy)
        {
            Destroy(gameObject);
        }
        foreach(Image img in lifeVisual)
        {
           img.fillAmount = (float)currentLife / maxLife;
        }

        hitTimer += Time.deltaTime;
        if(hitTimer<=0.2f)
        {
            for (int i = 0; i < childs; i++)
            {
                if (transform.GetChild(i).tag == "TrapVisual")
                {
                    int secondChilds = transform.GetChild(i).childCount;
                    for (int j = 0; j < secondChilds; j++)
                    {
                        transform.GetChild(i).GetChild(j).GetComponent<Renderer>().material.shader = hitShader;
                    }
                }
            }
            
            isShaderActive = true;
        }
        else if(isShaderActive)
        {
            for (int i = 0; i < childs; i++)
            {
                if (transform.GetChild(i).tag == "TrapVisual")
                {
                    int secondChilds = transform.GetChild(i).childCount;
                    for (int j = 0; j < secondChilds; j++)
                    {
                        transform.GetChild(i).GetChild(j).GetComponent<Renderer>().material.shader = baseShader[j];
                    }
                }
            }
            isShaderActive = false;
        }

        


    }

    public bool RemoveLife(int _degats, GameObject _player)
    {
        if (_degats - m_armor > 0)
        {
            currentLife -= (_degats - m_armor);
        }
            if (currentLife <= 0)
            {
                isDestroy = true;
            }
            else
            {
                hitTimer = 0;
            }
            //Debug.Log("Vie actulelle" + currentLife);
            return !isDestroy;
       }
    



    public void Repair()
    {
        currentLife = maxLife;
    }

    public void GetSold()
    {
        GameObject smoke = Instantiate(particles, transform.position, particles.transform.rotation);
        Destroy(smoke, 2f);
        Destroy(gameObject);
    }

    

}
