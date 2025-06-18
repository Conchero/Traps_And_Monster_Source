using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodiumTransform : MonoBehaviour
{
    public SkinSelectionModelManager m_skinSelectionModelManager;
    //Couleur actuel
    public string sColor = "";
    //Objet associé
    public GameObject m_associatePrefab;
    //objet instantié
    private GameObject m_currentPrefab;

    //Triggers
    public bool needToInstantiate;
    public bool needToUpdateColor;
    public bool needToDestroy;

    // Particule
    public List<ParticleSystem> particles = new List<ParticleSystem>();
    float currentLifeTime = 0;
	
    public List<Gradient> gradients = new List<Gradient>();

    private void Start()
    {
        needToInstantiate = false;
        needToUpdateColor = false;
        needToDestroy = false;
		
        ParticleSystem.MainModule mainTrail = particles[1].main;
        mainTrail.startLifetime = 0;
        mainTrail = particles[2].main;
        mainTrail.startLifetime = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if(needToInstantiate)
        {
            needToInstantiate = !needToInstantiate;
            m_currentPrefab = Instantiate(m_associatePrefab);
            m_currentPrefab.transform.position = transform.position;
            m_currentPrefab.transform.rotation = transform.rotation;
            needToUpdateColor = true;

            currentLifeTime = 1.46f;
        }

        ParticleSystem.MainModule mainTrail = particles[1].main;
        mainTrail.startLifetime = Mathf.Lerp(mainTrail.startLifetime.constant, currentLifeTime, Time.deltaTime * 5);
        mainTrail = particles[2].main;
        mainTrail.startLifetime = Mathf.Lerp(mainTrail.startLifetime.constant, currentLifeTime, Time.deltaTime * 5);

        if (needToUpdateColor)
        {
            needToUpdateColor = !needToUpdateColor;
            //Material
            //recherche du premier material present dans le SMR
            SkinnedMeshRenderer tempSkin = m_currentPrefab.GetComponentInChildren<SkinnedMeshRenderer>();
            //Assignation du material
            switch (sColor)
            {
                case "Red":
                    tempSkin.material = m_skinSelectionModelManager.m_playerMaterials[0];
					
                    particles[0].Play();
                    ParticleSystem.MainModule main = particles[0].main;
                    main.startColor = Color.red;

                    main = particles[1].main;
                    main.startColor = gradients[0];
                    main = particles[2].main;
                    main.startColor = gradients[0];
                    break;
                case "Blue":
                    tempSkin.material = m_skinSelectionModelManager.m_playerMaterials[1];
					
                    particles[0].Play();
                    main = particles[0].main;
                    main.startColor = Color.blue;

                    main = particles[1].main;
                    main.startColor = gradients[1];
                    main = particles[2].main;
                    main.startColor = gradients[1];
                    break;
                case "Green":
                    tempSkin.material = m_skinSelectionModelManager.m_playerMaterials[2];
					
                    particles[0].Play();
                    main = particles[0].main;
                    main.startColor = Color.green;

                    main = particles[1].main;
                    main.startColor = gradients[2];
                    main = particles[2].main;
                    main.startColor = gradients[2];
                    break;
                case "Yellow":
                    tempSkin.material = m_skinSelectionModelManager.m_playerMaterials[3];
					
                    particles[0].Play();
                    main = particles[0].main;
                    main.startColor = Color.yellow;

                    main = particles[1].main;
                    main.startColor = gradients[3];
                    main = particles[2].main;
                    main.startColor = gradients[3];
                    break;
                default:
                    break;
            }
        }
        if (needToDestroy)
        {
            needToDestroy = !needToDestroy;
            Destroy(m_currentPrefab);
            sColor = "";
			
            currentLifeTime = 0;
        }


    }
}
