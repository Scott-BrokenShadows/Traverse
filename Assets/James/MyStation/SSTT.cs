using UnityEngine;
using UnityEngine.UI;

public class SSTT : MonoBehaviour
{
    [SerializeField] private Transform vertBase = null;
    [SerializeField] private Transform horiBase = null;

    [SerializeField] private Transform vertRayDist;

    [SerializeField] private Text vDegreeText = null;
    [SerializeField] private Text hDegreeText = null;

    [ReadOnly] public float vDegree;
    [ReadOnly] public float hDegree;

    [ReadOnly] public float vDistance;

    [SerializeField] LayerMask _layerMask;

    [SerializeField] Vector2 minMaxVertBase;

    [SerializeField] private float rotationSpeed = 5.0f;

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

        elevate = Input.GetAxis("Vertical") * rotationSpeed * Time.deltaTime;
        vertBase.transform.Rotate(elevate, 0, 0);
        newElevation = currentElevation + elevate;
        SetCurrentElevation(newElevation);

        GetDegree();
        GetDistance();
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
        vDegree = (float)System.Math.Round(_vDegree.x, 1); // round up
        vDegreeText.text = vDegree.ToString();

        var _hDegree = horiBase.transform.localRotation.eulerAngles;
        hDegree = (float)System.Math.Round(_hDegree.y, 1); // round up
        hDegreeText.text = hDegree.ToString();
    }

    void GetDistance()
    {
        RaycastHit hit;

        if (Physics.Raycast(vertRayDist.transform.position, vertRayDist.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, _layerMask))
        {
            Debug.DrawRay(vertRayDist.transform.position, vertRayDist.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Did Hit " + $"<{hit.transform.name}>");

            vDistance = (float)System.Math.Round(hit.distance, 1);
        }
        else
        {
            Debug.DrawRay(vertRayDist.transform.position, vertRayDist.transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");

            vDistance = 0;
        }
    }
}
