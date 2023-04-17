using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurveyToolState : MonoBehaviour
{
    public SurveyState surveyState;

    [SerializeField] GameObject groundPoint;
    [SerializeField] public GameObject tripod;
    [SerializeField] public GameObject tribrach;
    [SerializeField] public GameObject prism;
    [SerializeField] public GameObject totalStn;
    [SerializeField] public CinemachineVirtualCamera tribrachCam;
    [Header("Random 1.15 - 1.45")]
    [SerializeField] float height;
    [SerializeField] EquipMang equipment;

    // Start is called before the first frame update
    void Start()
    {
        surveyState = SurveyState.GP;
    }

    // Update is called once per frame
    void Update()
    {
        switch (surveyState)
        {
            case SurveyState.GP:
                GroundPoint();
                break;
            case SurveyState.Tripod:
                Tripod();
                break;
            case SurveyState.TotalStn:
                TotalStation();
                break;
            case SurveyState.Prism:
                Prism();
                break;

        }
    }

    void GroundPoint()
    {
        if (tripod.active == true)
        {
            equipment.tripods--;
            tripod.SetActive(false);
            
        }
    }

    void Tripod()
    {
        if (tripod.active == false)
        {
            height = Random.Range(1.15f, 1.45f);

            tripod.transform.position = new Vector3(tripod.transform.position.x, height, tripod.transform.position.z);

            float _randomY = Random.Range(-0.5f, 0.5f);
            float _randomZ = Random.Range(-0.5f, 0.5f);
            Quaternion _randomYZ = Quaternion.Euler(0f, _randomY, _randomZ);
            tribrach.transform.localRotation = _randomYZ;

            equipment.tripods++;
            tripod.SetActive(true);
                        
        }

        

        if (prism.active == true)
        {
            equipment.prisms--;
            prism.SetActive(false);
            
        }
        if (totalStn.active == true)
        {
            equipment.totalStns--;
            totalStn.SetActive(false);
            
        }

    }

    void TotalStation()
    {
        if (totalStn.active == false)
        {
            equipment.totalStns++;
            totalStn.SetActive(true);
            
        }
        
    }

    void Prism()
    {
        if (prism.active == false)
        {
            equipment.prisms++;
            prism.SetActive(true);
        }
        
        
    }
}
