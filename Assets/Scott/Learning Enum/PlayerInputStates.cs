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
    [SerializeField] CinemachineVirtualCamera tribrachPosCam;
    [SerializeField] CinemachineVirtualCamera totalFaceCam;
    [SerializeField] CinemachineVirtualCamera totalAimCam;

    [SerializeField] GameObject character;
    [SerializeField] SkinnedMeshRenderer characterSkin;
    [SerializeField] GameObject closestSurvey;
    [SerializeField] GameObject _tribrach;
    [SerializeField] GameObject _totalStn;
    [SerializeField] EquipMang equipment;
    private GameObject[] surveyPoints;
    [SerializeField] float closestDist = Mathf.Infinity;
    [SerializeField] float interactDist = 2f;
    [SerializeField] float maxPosAngle = 1f;
    [SerializeField] float maxNegAngle = -1f;
    [SerializeField] float levelSpeed = 2f;
    [SerializeField] float currentBearing;
    [SerializeField] float currentElevation;

    public TextMeshProUGUI textTripod;
    public TextMeshProUGUI textTribrach;
    public TextMeshProUGUI textTribrachP;
    public TextMeshProUGUI textTribrachTS;
    public TextMeshProUGUI textPrism;
    public TextMeshProUGUI textTotalStn;
    public Canvas tribrachCanvas;
    public Canvas tribrachPosCanvas;
    //public Canvas totalFaceCanvas;
    public Canvas totalAimCanvas;

    [SerializeField] private float _zEuler;
    [SerializeField] private float _yEuler;
    [SerializeField] private float _zAngle;
    [SerializeField] private float _yAngle;

    [SerializeField] private Transform vertBase = null;
    [SerializeField] private Transform horiBase = null;
    [SerializeField] Vector2 minMaxVertBase;
    [SerializeField] private float rotationSpeed = 5.0f;

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
                tribrachCanvas = closestSurvey.GetComponent<SurveyToolState>().tribrachCanvas;
                tribrachPosCanvas = closestSurvey.GetComponent<SurveyToolState>().tribrachPosCanvas;
                totalAimCanvas = closestSurvey.GetComponent<SurveyToolState>().totalAimCanvas;
                _tribrach = closestSurvey.GetComponent<SurveyToolState>().tribrach;
                _totalStn = closestSurvey.GetComponent<SurveyToolState>().totalStn;
                tribrachCam = closestSurvey.GetComponent<SurveyToolState>().tribrachCam;
                tribrachPosCam = closestSurvey.GetComponent<SurveyToolState>().tribrachPosCam;
                totalAimCam = closestSurvey.GetComponent<SurveyToolState>().TSAimCam;
                totalFaceCam = closestSurvey.GetComponent<SurveyToolState>().TSFaceCam;
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
            case PlayerState.TribrachPosition:
                TribrachPosInputs();
                break;
            case PlayerState.TotalStation:
                TotalStnFace();
                break;
            case PlayerState.TotalStationPosition:
                TotalStnAim();
                break;
        }

        

        foreach (GameObject surveyPoint in surveyPoints)
        {
            float distanceToPlayer = Vector3.Distance(character.transform.position, surveyPoint.transform.position);

            if (distanceToPlayer < closestDist)
            {
                closestSurvey = surveyPoint;
                horiBase = closestSurvey.GetComponent<SurveyToolState>().totalStn.transform;
                vertBase = closestSurvey.GetComponent<SurveyToolState>().totalStn.gameObject.transform.GetChild(1).transform;
                _tribrach = closestSurvey.GetComponent<SurveyToolState>().tribrach;
                _totalStn = closestSurvey.GetComponent<SurveyToolState>().totalStn;
                tribrachCam = closestSurvey.GetComponent<SurveyToolState>().tribrachCam;
                tribrachPosCam = closestSurvey.GetComponent<SurveyToolState>().tribrachPosCam;
                totalAimCam = closestSurvey.GetComponent<SurveyToolState>().TSAimCam;
                totalFaceCam = closestSurvey.GetComponent<SurveyToolState>().TSFaceCam;

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

        tribrachCanvas.enabled = false;
        tribrachPosCanvas.enabled = false;
        totalAimCanvas.enabled = false;

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
                playState = PlayerState.TotalStation;
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
            tribrachPosCam.Priority = 0;
            totalFaceCam.Priority = 0;
            totalAimCam.Priority = 0;
            tribrachPosCam.VirtualCameraGameObject.SetActive(false);
            tribrachCam.VirtualCameraGameObject.SetActive(false);
            characterCam.VirtualCameraGameObject.SetActive(true);
            totalAimCam.VirtualCameraGameObject.SetActive(false);
            totalFaceCam.VirtualCameraGameObject.SetActive(false);
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

        tribrachCanvas.enabled = true;
        tribrachPosCanvas.enabled = false;
        totalAimCanvas.enabled = false;

        if (characterSkin.enabled == true)
        {
            characterSkin.enabled = false;
        }

        if (tribrachCam.Priority < 10)
        {
            characterCam.Priority = 0;
            tribrachCam.Priority = 10;
            tribrachPosCam.Priority = 0;
            totalFaceCam.Priority = 0;
            totalAimCam.Priority = 0;
            tribrachPosCam.VirtualCameraGameObject.SetActive(false);
            tribrachCam.VirtualCameraGameObject.SetActive(true);
            characterCam.VirtualCameraGameObject.SetActive(false);
            totalAimCam.VirtualCameraGameObject.SetActive(false);
            totalFaceCam.VirtualCameraGameObject.SetActive(false);
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
            _tribrach.transform.localEulerAngles += new Vector3(0f, 0f, 0.03f) * Time.deltaTime * levelSpeed;
        }

        if (_zAngle >= maxNegAngle && Input.GetKey(KeyCode.E))
        {
            _tribrach.transform.localEulerAngles += new Vector3(0f, 0f, -0.03f) * Time.deltaTime * levelSpeed;
        }

        if (_zAngle <= maxPosAngle && _yAngle <= maxPosAngle && Input.GetKey(KeyCode.A))
        {
            _tribrach.transform.localEulerAngles += new Vector3(0f, 0.01f, 0.02f) * Time.deltaTime * levelSpeed;
        }

        if (_zAngle >= maxNegAngle && _yAngle >= maxNegAngle && Input.GetKey(KeyCode.Z))
        {
            _tribrach.transform.localEulerAngles += new Vector3(0f, -0.01f, -0.02f) * Time.deltaTime * levelSpeed;
        }

        if (_zAngle <= maxPosAngle && _yAngle >= maxNegAngle && Input.GetKey(KeyCode.C))
        {
            _tribrach.transform.localEulerAngles += new Vector3(0f, -0.01f, 0.02f) * Time.deltaTime * levelSpeed;
        }

        if (_zAngle >= maxNegAngle && _yAngle <= maxPosAngle && Input.GetKey(KeyCode.F))
        {
            _tribrach.transform.localEulerAngles += new Vector3(0f, 0.01f, -0.02f) * Time.deltaTime * levelSpeed;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            playState = PlayerState.TribrachPosition;
        }
    }

    void TribrachPosInputs()
    {
        textTripod.enabled = false;
        textTotalStn.enabled = false;
        textPrism.enabled = false;
        textTribrach.enabled = false;
        textTribrachP.enabled = false;
        textTribrachTS.enabled = false;

        tribrachCanvas.enabled = false;
        tribrachPosCanvas.enabled = true;
        totalAimCanvas.enabled = false;

        if (tribrachPosCam.Priority < 10)
        {
            characterCam.Priority = 0;
            tribrachCam.Priority = 0;
            tribrachPosCam.Priority = 10;
            totalFaceCam.Priority = 0;
            totalAimCam.Priority = 0;
            tribrachPosCam.VirtualCameraGameObject.SetActive(true);
            tribrachCam.VirtualCameraGameObject.SetActive(false);
            characterCam.VirtualCameraGameObject.SetActive(false);
            totalAimCam.VirtualCameraGameObject.SetActive(false);
            totalFaceCam.VirtualCameraGameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            playState = PlayerState.TribrachLevel;
        }

        

        
        Vector3 movement = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            movement += _tribrach.transform.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movement -= _tribrach.transform.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            movement += _tribrach.transform.right;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movement -= _tribrach.transform.right;
        }

        // Apply the movement vector to the object's local position
        _tribrach.transform.localPosition += movement * (levelSpeed * 0.005f) * Time.deltaTime;
    }

    void TotalStnFace()
    {

        if (characterSkin.enabled == true)
        {
            characterSkin.enabled = false;
        }

        textTripod.enabled = false;
        textTotalStn.enabled = false;
        textPrism.enabled = false;
        textTribrach.enabled = false;
        textTribrachP.enabled = false;
        textTribrachTS.enabled = false;

        tribrachCanvas.enabled = false;
        tribrachPosCanvas.enabled = false;
        totalAimCanvas.enabled = false;

        if (totalFaceCam.Priority < 10)
        {
            characterCam.Priority = 0;
            tribrachCam.Priority = 0;
            tribrachPosCam.Priority = 0;
            totalFaceCam.Priority = 10;
            totalAimCam.Priority = 0;
            tribrachPosCam.VirtualCameraGameObject.SetActive(false);
            tribrachCam.VirtualCameraGameObject.SetActive(false);
            characterCam.VirtualCameraGameObject.SetActive(false);
            totalAimCam.VirtualCameraGameObject.SetActive(false);
            totalFaceCam.VirtualCameraGameObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            playState = PlayerState.Character;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            playState = PlayerState.TotalStationPosition;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            closestSurvey.GetComponent<SSTT>().GetDistance();
        }
    }

    void TotalStnAim()
    {
        if (characterSkin.enabled == true)
        {
            characterSkin.enabled = false;
        }

        textTripod.enabled = false;
        textTotalStn.enabled = false;
        textPrism.enabled = false;
        textTribrach.enabled = false;
        textTribrachP.enabled = false;
        textTribrachTS.enabled = false;

        tribrachCanvas.enabled = false;
        tribrachPosCanvas.enabled = false;
        totalAimCanvas.enabled = true;

        if (totalAimCam.Priority < 10)
        {
            characterCam.Priority = 0;
            tribrachCam.Priority = 0;
            tribrachPosCam.Priority = 0;
            totalFaceCam.Priority = 0;
            totalAimCam.Priority = 10;
            tribrachPosCam.VirtualCameraGameObject.SetActive(false);
            tribrachCam.VirtualCameraGameObject.SetActive(false);
            characterCam.VirtualCameraGameObject.SetActive(false);
            totalAimCam.VirtualCameraGameObject.SetActive(true);
            totalFaceCam.VirtualCameraGameObject.SetActive(false);
        }

        float traverse = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        //horiBase.transform.Rotate(0, traverse, 0);
        float newBearing = currentBearing + traverse;
        SetCurrentBearing(newBearing);

        float elevate = -Input.GetAxis("Vertical") * rotationSpeed * Time.deltaTime;
        //vertBase.transform.Rotate(elevate, 0, 0);
        float newElevation = currentElevation + elevate;
        SetCurrentElevation(newElevation);

        if (Input.GetKeyDown(KeyCode.R))
        {
            playState = PlayerState.TotalStation;
        }
    }

    void SetCurrentBearing(float rot)
    {
        //currentBearing = Mathf.Clamp(rot, 0, 360);
        currentBearing = rot;
        // Rotate Horizontal
        //horiBase.transform.rotation = Quaternion.Euler(0, rot, 0);
        horiBase.transform.localRotation = Quaternion.Euler(rot, 0, 0);
    }

    void SetCurrentElevation(float rot)
    {
        // Rotate Vertical
        currentElevation = Mathf.Clamp(rot, minMaxVertBase.x, minMaxVertBase.y);
        vertBase.transform.localRotation = Quaternion.Euler(rot, -90, 0);
    }

}
