using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
using UnityEditor;

public class PlayerInputStates : MonoBehaviour
{
    public PlayerState playState;

    [SerializeField] CinemachineVirtualCamera characterCam;
    [SerializeField] CinemachineVirtualCamera tribrachCam;

    [SerializeField] GameObject character;
    [SerializeField] SkinnedMeshRenderer characterSkin;
    [SerializeField] GameObject closestSurvey;
    [SerializeField] GameObject _tribrach;
    [SerializeField] EquipMang equipment;
    private GameObject[] surveyPoints;
    [SerializeField] float closestDist = Mathf.Infinity;
    [SerializeField] float interactDist = 2f;
    [SerializeField] float maxPosAngle = 1f;
    [SerializeField] float maxNegAngle = -1f;

    public TextMeshProUGUI textTripod;
    public TextMeshProUGUI textTribrach;
    public TextMeshProUGUI textTribrachP;
    public TextMeshProUGUI textTribrachTS;
    public TextMeshProUGUI textPrism;
    public TextMeshProUGUI textTotalStn;

    [SerializeField] private float _zEuler;
    [SerializeField] private float _yEuler;
    [SerializeField] private float _zAngle;
    [SerializeField] private float _yAngle;
    //Vector3 rInspect;

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
                _tribrach = closestSurvey.GetComponent<SurveyToolState>().tribrach;

            }
        }

        closestDist = Vector3.Distance(character.transform.position, closestSurvey.transform.position);

        //GetRotationInspector();
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

    //void GetRotationInspector()
    //{
    //    rInspect = TransformUtils.GetInspectorRotation(_tribrach.transform);
    //}

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

        _zEuler = _tribrach.transform.localEulerAngles.z;
        _yEuler = _tribrach.transform.localEulerAngles.y;

        if (_tribrach.transform.localEulerAngles.z >= 180)
        {
            _zAngle = (_tribrach.transform.localEulerAngles.z - 360);
            
        }
        else
        {
            _zAngle = _tribrach.transform.localEulerAngles.z;
        }

        if (_tribrach.transform.localEulerAngles.y >= 180)
        {
            _yAngle = (_tribrach.transform.localEulerAngles.y - 360);

        }
        else
        {
            _yAngle = _tribrach.transform.localEulerAngles.y;
        }


        if (_zAngle <= maxPosAngle && Input.GetKey(KeyCode.W))
        {
            Debug.Log("W key");
            _tribrach.transform.localEulerAngles += new Vector3(0f, 0f, 0.003f) * Time.deltaTime;
            //Call to buble to update position

            //_tribrach.transform.localEulerAngles = new Vector3(_tribrach.transform.localEulerAngles.x, _tribrach.transform.localEulerAngles.y, Mathf.Clamp(_tribrach.transform.localEulerAngles.z, maxNegAngle, maxPosAngle));
        }

        if (_zAngle >= maxNegAngle && Input.GetKey(KeyCode.E))
        {
            Debug.Log("E key");
            _tribrach.transform.localEulerAngles += new Vector3(0f, 0f, -0.003f) * Time.deltaTime;
            //_tribrach.transform.localEulerAngles = new Vector3(_tribrach.transform.localEulerAngles.x, _tribrach.transform.localEulerAngles.y, Mathf.Clamp(_tribrach.transform.localEulerAngles.z, maxNegAngle, maxPosAngle));
        }

        if (_zAngle <= maxPosAngle && _yAngle <= maxPosAngle && Input.GetKey(KeyCode.A))
        {
            Debug.Log("A key");
            _tribrach.transform.localEulerAngles += new Vector3(0f, 0.001f, 0.002f) * Time.deltaTime;
            //_tribrach.transform.localEulerAngles = new Vector3(_tribrach.transform.localEulerAngles.x, Mathf.Clamp(_tribrach.transform.localEulerAngles.y, maxNegAngle, maxPosAngle), Mathf.Clamp(_tribrach.transform.localEulerAngles.z, maxNegAngle, maxPosAngle));
        }

        if (_zAngle >= maxNegAngle && _yAngle >= maxNegAngle && Input.GetKey(KeyCode.Z))
        {
            Debug.Log("Z key");
            _tribrach.transform.localEulerAngles += new Vector3(0f, -0.001f, -0.002f) * Time.deltaTime;
            //_tribrach.transform.localEulerAngles = new Vector3(_tribrach.transform.localEulerAngles.x, Mathf.Clamp(_tribrach.transform.localEulerAngles.y, maxNegAngle, maxPosAngle), Mathf.Clamp(_tribrach.transform.localEulerAngles.z, maxNegAngle, maxPosAngle));
        }

        if (_zAngle <= maxPosAngle && _yAngle >= maxNegAngle && Input.GetKey(KeyCode.C))
        {
            Debug.Log("C key");
            _tribrach.transform.localEulerAngles += new Vector3(0f, -0.001f, 0.002f) * Time.deltaTime;
            //_tribrach.transform.localEulerAngles = new Vector3(_tribrach.transform.localEulerAngles.x, Mathf.Clamp(_tribrach.transform.localEulerAngles.y, maxNegAngle, maxPosAngle), Mathf.Clamp(_tribrach.transform.localEulerAngles.z, maxNegAngle, maxPosAngle));
        }

        if (_zAngle >= maxNegAngle && _yAngle <= maxPosAngle && Input.GetKey(KeyCode.F))
        {
            Debug.Log("F key");
            _tribrach.transform.localEulerAngles += new Vector3(0f, 0.001f, -0.002f) * Time.deltaTime;
            //_tribrach.transform.localEulerAngles = new Vector3(_tribrach.transform.localEulerAngles.x, Mathf.Clamp(_tribrach.transform.localEulerAngles.y, maxNegAngle, maxPosAngle), Mathf.Clamp(_tribrach.transform.localEulerAngles.z, maxNegAngle, maxPosAngle));
        }
    }
}
