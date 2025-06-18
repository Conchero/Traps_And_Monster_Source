using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableManager : MonoBehaviour
{
    //Stock l'objet gemme à instancier
    public GameObject m_gemmePrefab;

   public int m_nbGemmeByObject;


    /// <summary>
    ///Instantiate prefabGemmes with the attached script "Collectable" at the Vector3 Position with little random.
    ///After it set the value to the gemme
    /// </summary>
    /// <param name="_killValue"></param>
    /// <param name="_pos"></param>
    /// <returns></returns>
    public void CreateGemme(Vector3 _pos, int _killValue)
    {
        //on calcul le nombre d'objet à crée en contraignant le contenu à la valeur de m_nbGemmeByObject
        int nbObject = (int)(_killValue / m_nbGemmeByObject) + 1;

        for (int i = 0; i < nbObject; i ++)
        {
           //On ajoute de l'aleatoire à la position
            Vector3 gemmePos = _pos;
            gemmePos.x += Random.Range(-1f, 1f);
            gemmePos.z += Random.Range(-1f, 1f);
            //instantie un objet gemme
            GameObject gemme = Instantiate(m_gemmePrefab, gemmePos, Quaternion.identity);//, transform);
           //Donne la valeur de la gemme
            if (i < nbObject-1)
            {
                gemme.GetComponent<Collectable>().Value = m_nbGemmeByObject;
            }
            else
            {
                gemme.GetComponent<Collectable>().Value = _killValue% m_nbGemmeByObject;
            }
        }
    }
}
