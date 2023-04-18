using UnityEngine;
using UnityEngine.UI;

public class SSTT : MonoBehaviour
{
    [SerializeField] private Transform vertBase = null;
    [SerializeField] private Transform horiBase = null;

    [SerializeField] private Transform vertRayDist;
    [SerializeField] private Transform vertRayHgt;

    //[SerializeField] private Text vDegreeText = null;
    //[SerializeField] private Text vDMSText = null;
    //[SerializeField] private Text hDegreeText = null;
    [SerializeField] private Text hDMSText = null;
    [SerializeField] private Text distText = null;
    [SerializeField] private Text hgtDiffText = null;
    [SerializeField] private Text stnHText = null;
    [SerializeField] private Text trgHText = null;

    float vDegree;
    string vDMS = "00°00’00”";
    float hDegree;
    string hDMS = "00°00’00”";

    float distance;
    float rayDistance;

    float heightDiff;
    float rayHeight; // This Station Height

    float trgHeight;

    [SerializeField] LayerMask _layerPrism;
    [SerializeField] LayerMask _layerGround;

    [SerializeField] Vector2 minMaxVertBase;

    [SerializeField] private float rotationSpeed = 5.0f;

    Prism _prism;

    private float traverse;
    private float currentBearing;
    private float newBearing;

    private float elevate;
    private float currentElevation;
    private float newElevation;

    void Update()
    {
        traverse = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        horiBase.transform.Rotate(0, traverse, 0);
        newBearing = currentBearing + traverse;
        SetCurrentBearing(newBearing);

        elevate = -Input.GetAxis("Vertical") * rotationSpeed * Time.deltaTime;
        vertBase.transform.Rotate(elevate, 0, 0);
        newElevation = currentElevation + elevate;
        SetCurrentElevation(newElevation);

        GetDegree();
        GetDistance();
        GetHeight();

        vDMS = DegreesMinutesSeconds(vDegree);
        //vDMSText.text = "V BEARING : " + vDMS;

        hDMS = DegreesMinutesSeconds(hDegree);
        hDMSText.text = "BEARING : " + hDMS;

        distText.text = "DISTANCE : " + distance;
        hgtDiffText.text = "HT DIFF : " + heightDiff;
        stnHText.text = "STN HT : " + rayHeight;
        trgHText.text = "TARGET HT : " + trgHeight;
    }

    void SetCurrentBearing(float rot)
    {
        //currentBearing = Mathf.Clamp(rot, 0, 360);
        currentBearing = rot;
        horiBase.transform.rotation = Quaternion.Euler(0, rot, 0);
    }

    void SetCurrentElevation(float rot)
    {
        currentElevation = Mathf.Clamp(rot, minMaxVertBase.x, minMaxVertBase.y);
        vertBase.transform.rotation = Quaternion.Euler(rot, currentBearing, 0);
    }

    void GetDegree()
    {
        var _vDegree = vertBase.transform.localRotation.eulerAngles;
        vDegree = (float)System.Math.Round(_vDegree.x, 3); // round up
        //vDegreeText.text = $"Vertical Degrees = {vDegree.ToString()}°";

        var _hDegree = horiBase.transform.localRotation.eulerAngles;
        hDegree = (float)System.Math.Round(_hDegree.y, 3); // round up
        //hDegreeText.text = $"Horizontal Degrees = {hDegree.ToString()}°";
    }

    void GetDistance()
    {
        RaycastHit hit;

        if (Physics.Raycast(vertRayDist.transform.position, vertRayDist.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, _layerPrism))
        {
            Debug.DrawRay(vertRayDist.transform.position, vertRayDist.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            //Debug.Log("Did Hit " + $"<{hit.transform.name}>");

            rayDistance = (float)System.Math.Round(hit.distance, 3);

            if (hit.transform.gameObject.GetComponent<Prism>() != null)
            {
                _prism = hit.transform.gameObject.GetComponent<Prism>();
            }

            distance = (float)System.Math.Round(Mathf.Cos(vDegree * Mathf.Deg2Rad) * rayDistance, 3);

            if (_prism != null) 
            { 
                trgHeight = _prism.rayHeight; 
                heightDiff = (float)System.Math.Round(-((Mathf.Sin(vDegree * Mathf.Deg2Rad) * rayDistance) + rayHeight - _prism.rayHeight), 3);
            }
        }
        else
        {
            Debug.DrawRay(vertRayDist.transform.position, vertRayDist.transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            //Debug.Log("Did not Hit");

            rayDistance = 0;

            _prism = null;

            distance = 0;

            trgHeight = 0;
            heightDiff = 0;
        }
    }

    void GetHeight()
    {
        RaycastHit hit;

        if (Physics.Raycast(vertRayHgt.transform.position, vertRayHgt.transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, _layerGround))
        {
            Debug.DrawRay(vertRayHgt.transform.position, vertRayHgt.transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
            //Debug.Log("Did Hit " + $"<{hit.transform.name}>");

            rayHeight = (float)System.Math.Round(hit.distance, 3);
        }
        else
        {
            Debug.DrawRay(vertRayHgt.transform.position, vertRayHgt.transform.TransformDirection(Vector3.down) * 1000, Color.white);
            //Debug.Log("Did not Hit");

            rayHeight = 0;
        }
    }

    string DegreesMinutesSeconds(float dDegree)
    {
        // https://manoa.hawaii.edu/exploringourfluidearth/physical/world-ocean/locating-points-globe/compare-contrast-connect-converting-decimal-degrees

        float _dDegrees, _dMinutes, _hSecond = 0;
        float _dM, _dS = 0;

        // Degrees
        _dDegrees = Mathf.FloorToInt(dDegree);
        // Minutes
        // Calculate Minutes with no Decimal "Minutes = (Degrees with Decimal - Degrees) x 60" then round it into an int
        _dM = (float)System.Math.Round(dDegree - _dDegrees, 3) * 60; 
        _dMinutes = Mathf.FloorToInt(_dM);
        // Seconds
        // Calculate Seconds with no Decimal "Seconds = (Minutes with Decimal - Minutes) x 60" then round it into an int
        _dS = (float)System.Math.Round(_dM - _dMinutes, 3) * 60;
        _hSecond = Mathf.FloorToInt(_dS);

        // Degrees° Minutes’ Seconds”
        return $"{_dDegrees.ToString("00")}°{_dMinutes.ToString("00")}’{_hSecond.ToString("00")}”";
    }
}
