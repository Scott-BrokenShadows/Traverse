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
    [SerializeField] GameObject closestSurvey;
    [SerializeField] GameObject closestTripod;
    [SerializeField] GameObject closestTribrach;
    [SerializeField] GameObject closestPrism;
    [SerializeField] GameObject closestTS;
    private GameObject[] surveyPoints;
    [SerializeField] float closestDist = Mathf.Infinity;
    [SerializeField] float interactDist = 2f;

    public TextMeshProUGUI textTripod;
    public TextMeshProUGUI textTribrach;


    // Start is called before the first frame update
    void Start()
    {
        playState = PlayerState.Character;
        surveyPoints = GameObject.FindGameObjectsWithTag("Survey");

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
                tribrachCam = closestSurvey.GetComponentInChildren<CinemachineVirtualCamera>();
                
            }
        }

        closestDist = Vector3.Distance(character.transform.position, closestSurvey.transform.position);
        closestTripod = closestSurvey.transform.Find("_Tripod").gameObject;
        closestTribrach = closestSurvey.transform.Find("_Tribrach").gameObject;
        closestPrism = closestTribrach.transform.Find("_Prism").gameObject;
        closestTS = closestTribrach.transform.Find("_TotalStation").gameObject;
    }

    void CharacterInputs()
    {
        if (characterCam.Priority < 10)
        {
            characterCam.Priority = 10;
            tribrachCam.Priority = 0;
        }

        if (closestDist < interactDist && closestTripod.active == false)
        {

            textTripod.enabled = true;
            if (Input.GetKeyDown(KeyCode.E))
            {
                closestTripod.SetActive(true);
            }
        }
        else
        {
            textTripod.enabled = false;
        }

        if (closestDist < (interactDist - 0.4f) && closestTripod.active == true)
        {
            textTribrach.enabled = true;
            if (Input.GetKeyDown(KeyCode.R))
            {
                closestTripod.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                closestTS.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                playState = PlayerState.TribrachLevel;
            }
            if (Input.GetKeyDown(KeyCode.Y))
            {
                closestPrism.SetActive(true);
            }
        }
        else
        {
            textTribrach.enabled = false;
        }


    }

    void TribrachInputs()
    {
        textTripod.enabled = false;

    }
}
