using UnityEngine;
using UnityEngine.UI;

public class SSTT : MonoBehaviour
{
    public SurveyState surveyState;
    public SurveyToolState surveyToolState;

    [SerializeField] private Transform vertBase = null;
    [SerializeField] private Transform horiBase = null;

    [SerializeField] private Transform vertRayDist;
    //[SerializeField] private Transform vertRayHgt;

    [SerializeField] private Text hDMSText = null;
    [SerializeField] private Text distText = null;
    [SerializeField] private Text hgtDiffText = null;
    [SerializeField] private Text stnHText = null;
    [SerializeField] private Text trgHText = null;

    [SerializeField] float vDegree;
    string vDMS = "00°00’00”";
    [SerializeField] float hDegree;
    string hDMS = "00°00’00”";

    [SerializeField] float distance;
    [SerializeField] float rayDistance;

    float heightDiff;
    [SerializeField] public float stnHeight; // This Station Height

    [SerializeField] public float trgHeight;

    [SerializeField] LayerMask _layerPrism;
    [SerializeField] LayerMask _layerGround;

    Prism _prism;

    void Update()
    {
        switch (surveyState)
        {
            case SurveyState.TotalStn:
                setup();
                break;
        }

        surveyState = surveyToolState.surveyState;
    }

    void setup()
    {
        GetDegree();
        //Debug.Log("get degree done");
        //GetDistance();
        //GetHeight();

        vDMS = DegreesMinutesSeconds(vDegree);
        //vDMSText.text = "V BEARING : " + vDMS;

        hDMS = DegreesMinutesSeconds(360 - hDegree);
        hDMSText.text = "BEARING : " + hDMS;

        distText.text = "DISTANCE : " + distance;
        hgtDiffText.text = "HT DIFF : " + heightDiff;
        stnHText.text = "STN HT : " + stnHeight;
        trgHText.text = "TARGET HT : " + trgHeight;
        //Debug.Log("update done");
    }

    void GetDegree()
    {
        var _vDegree = vertBase.transform.localRotation.eulerAngles;
        vDegree = (float)System.Math.Round((360 - _vDegree.z), 3); // round up
        //vDegreeText.text = $"Vertical Degrees = {vDegree.ToString()}°";

        var _hDegree = horiBase.transform.localRotation.eulerAngles;
        hDegree = (float)System.Math.Round(_hDegree.x, 3); // round up
        //hDegreeText.text = $"Horizontal Degrees = {hDegree.ToString()}°";
    }

    public void GetDistance()
    {
        Debug.Log("Send ray");
        RaycastHit hit;

        if (Physics.Raycast(vertRayDist.transform.position, vertRayDist.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, _layerPrism))
        {
            //Debug.Log("Ray hit prism");
            Debug.DrawRay(vertRayDist.transform.position, vertRayDist.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            //Debug.Log("Did Hit " + $"<{hit.transform.name}>");
            //Debug.Log("Ray hit draw");
            rayDistance = (float)System.Math.Round(hit.distance, 3);
            //Debug.Log("Ray hit distance");
            if (hit.transform.gameObject.GetComponent<Prism>() != null)
            {
                //Debug.Log("Ray hit prism what?");
                _prism = hit.transform.gameObject.GetComponent<Prism>();
            }

            distance = (float)System.Math.Round(Mathf.Cos(vDegree * Mathf.Deg2Rad) * rayDistance, 3);
            //Debug.Log("Ray hit distance calc");
            if (_prism != null) 
            {
                //Debug.Log("Ray hit prism 2");
                trgHeight = _prism.rayHeight; 
                heightDiff = (float)System.Math.Round(-((Mathf.Sin(vDegree * Mathf.Deg2Rad) * rayDistance) + stnHeight - _prism.rayHeight), 3);
                //Debug.Log("Ray distance calc");
            }
        }
        else
        {
            //Debug.Log("Ray miss");
            Debug.DrawRay(vertRayDist.transform.position, vertRayDist.transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            //Debug.Log("Did not Hit");

            rayDistance = 0;

            _prism = null;

            distance = 0;

            trgHeight = 0;
            heightDiff = 0;
        }
    }

    //void GetHeight()
    //{
    //    RaycastHit hit;

    //    if (Physics.Raycast(vertRayHgt.transform.position, vertRayHgt.transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, _layerGround))
    //    {
    //        Debug.DrawRay(vertRayHgt.transform.position, vertRayHgt.transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
    //        //Debug.Log("Did Hit " + $"<{hit.transform.name}>");

    //        rayHeight = (float)System.Math.Round(hit.distance, 3);
    //    }
    //    else
    //    {
    //        Debug.DrawRay(vertRayHgt.transform.position, vertRayHgt.transform.TransformDirection(Vector3.down) * 1000, Color.white);
    //        //Debug.Log("Did not Hit");

    //        rayHeight = 0;
    //    }
    //}

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
