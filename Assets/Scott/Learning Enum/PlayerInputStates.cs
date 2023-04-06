using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class PlayerInputStates : MonoBehaviour
{
    public PlayerState playState;

    [SerializeField] CinemachineVirtualCamera characterCam;
    [SerializeField] CinemachineVirtualCamera tribrachCam;

    [SerializeField] GameObject character;
    [SerializeField] SkinnedMeshRenderer characterSkin;
    [SerializeField] GameObject closestSurvey;
    [SerializeField] EquipMang equipment;
    private GameObject[] surveyPoints;
    [SerializeField] float closestDist = Mathf.Infinity;
    [SerializeField] float interactDist = 2f;

    public TextMeshProUGUI textTripod;
    public TextMeshProUGUI textTribrach;
    public TextMeshProUGUI textTribrachP;
    public TextMeshProUGUI textTribrachTS;
    public TextMeshProUGUI textPrism;
    public TextMeshProUGUI textTotalStn;


    // Start is called before the first frame update
    void Start()
    {
        playState = PlayerState.Character;
        surveyPoints = GameObject.FindGameObjectsWithTag("Survey");

        foreach (GameObject surveyPoint in surveyPoints)
        {
            float distanceToPlayer = Vector3.Distance(character.transform.position, surveyPoint.transform.position);

            if (distanceToPlayer < closestDist)
            {
                closestSurvey = surveyPoint;
                tribrachCam = closestSurvey.GetComponent<SurveyToolState>().tribrachCam;

            }
        }

        closestDist = Vector3.Distance(character.transform.position, closestSurvey.transform.position);

    }

    // Update is called once per frame
    void Update()
    {
        switch (playState)
        {
            case PlayerState.Character:
                CharacterInputs();
                break;
            case PlayerState.TribrachLevel:
                TribrachInputs();
                break;
        }

        

        foreach (GameObject surveyPoint in surveyPoints)
        {
            float distanceToPlayer = Vector3.Distance(character.transform.position, surveyPoint.transform.position);

            if (distanceToPlayer < closestDist)
            {
                closestSurvey = surveyPoint;
                tribrachCam = closestSurvey.GetComponent<SurveyToolState>().tribrachCam;

            }
        }

        closestDist = Vector3.Distance(character.transform.position, closestSurvey.transform.position);
        
    }

    void CharacterInputs()
    {
        if (characterSkin.enabled == false)
        {
            characterSkin.enabled = true;
        }

        

        if (closestDist < interactDist && closestSurvey.GetComponent<SurveyToolState>().surveyState == SurveyState.GP && equipment.tripods < 3)
        {

            textTripod.enabled = true;
            if (Input.GetKeyDown(KeyCode.E))
            {
                
                closestSurvey.GetComponent<SurveyToolState>().surveyState = SurveyState.Tripod;
                return;
            }
        }
        else
        {
            textTripod.enabled = false;
        }

        if (closestDist < (interactDist - 0.4f) && closestSurvey.GetComponent<SurveyToolState>().surveyState == SurveyState.Tripod)
        {
            textTribrach.enabled = true;
            if (Input.GetKeyDown(KeyCode.R))
            {
                closestSurvey.GetComponent<SurveyToolState>().surveyState = SurveyState.GP;
            }


            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("activated");
                //change camera here
                playState = PlayerState.TribrachLevel;
            }


            if (equipment.totalStns < 1)
            {
                textTribrachTS.enabled = true;
                if (Input.GetKeyDown(KeyCode.T))
                {
                    closestSurvey.GetComponent<SurveyToolState>().surveyState = SurveyState.TotalStn;
                }
            }
            else
            {
                textTribrachTS.enabled = false;
            }
            
            if (equipment.prisms < 2)
            {
                textTribrachP.enabled = true;
                if (Input.GetKeyDown(KeyCode.Y))
                {
                    closestSurvey.GetComponent<SurveyToolState>().surveyState = SurveyState.Prism;
                }
            }
            else
            {
                textTribrachP.enabled = false;
            }
        }
        else
        {
            textTribrach.enabled = false;
            textTribrachP.enabled = false;
            textTribrachTS.enabled = false;
        }

        if (closestDist < (interactDist - 0.4f) && closestSurvey.GetComponent<SurveyToolState>().surveyState == SurveyState.Prism)
        {
            textPrism.enabled = true;
            if (Input.GetKeyDown(KeyCode.R))
            {
                closestSurvey.GetComponent<SurveyToolState>().surveyState = SurveyState.Tripod;
            }
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                //rotate prism?
            }
            
        }
        else
        {
            textPrism.enabled = false;
        }

        if (closestDist < (interactDist - 0.4f) && closestSurvey.GetComponent<SurveyToolState>().surveyState == SurveyState.TotalStn)
        {
            textTotalStn.enabled = true;
            if (Input.GetKeyDown(KeyCode.R))
            {
                closestSurvey.GetComponent<SurveyToolState>().surveyState = SurveyState.Tripod;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                //rotate total stn? Change camera to looking at Total Stn so you can move it around
            }

        }
        else
        {
            textTotalStn.enabled = false;
        }


        if (characterCam.Priority < 10)
        {
            characterCam.Priority = 10;
            tribrachCam.Priority = 0;
            tribrachCam.VirtualCameraGameObject.SetActive(false);
            characterCam.VirtualCameraGameObject.SetActive(true);
        }
    }

    void TribrachInputs()
    {
        textTripod.enabled = false;
        textTotalStn.enabled = false;
        textPrism.enabled = false;
        textTribrach.enabled = false;
        textTribrachP.enabled = false;
        textTribrachTS.enabled = false;

        if (characterSkin.enabled == true)
        {
            characterSkin.enabled = false;
        }

        if (tribrachCam.Priority < 10)
        {
            characterCam.Priority = 0;
            tribrachCam.Priority = 10;
            characterCam.VirtualCameraGameObject.SetActive(false);
            tribrachCam.VirtualCameraGameObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            playState = PlayerState.Character;
        }

    }
}
