using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendLeg : MonoBehaviour
{
    [SerializeField] Transform groundPoint;

    [SerializeField] Transform tripodLeg1;
    [SerializeField] Transform tripodLeg2;
    [SerializeField] Transform tripodLeg3;

    Transform LegExtend1;
    Transform LegExtend2;
    Transform LegExtend3;

    [SerializeField] Vector2 minMaxRotation = new Vector2(0f, 90f);
    [SerializeField] Vector2 minMaxExtend = new Vector2 (0.02f, 0.17f);
    [Range(0, 1)] public float rotateLeg;
    [Range (0, 1)] public float extendLeg;

    [SerializeField] LayerMask _layerMask;
    [SerializeField] float distance;

    // Start is called before the first frame update
    void Awake()
    {
        LegExtend1 = tripodLeg1.gameObject.transform.GetChild(0);
        LegExtend2 = tripodLeg2.gameObject.transform.GetChild(0);
        LegExtend3 = tripodLeg3.gameObject.transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        float _LegExtend = Mathf.Lerp(minMaxExtend.x, minMaxExtend.y, extendLeg);
        float _TripodLeg = Mathf.Lerp(minMaxRotation.x, minMaxRotation.y, rotateLeg);

        LegExtend1.transform.localPosition = new Vector3(_LegExtend, LegExtend1.transform.localPosition.y, LegExtend1.transform.localPosition.z);
        LegExtend2.transform.localPosition = new Vector3(_LegExtend, LegExtend2.transform.localPosition.y, LegExtend2.transform.localPosition.z);
        LegExtend3.transform.localPosition = new Vector3(_LegExtend, LegExtend3.transform.localPosition.y, LegExtend3.transform.localPosition.z);

        tripodLeg1.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, _TripodLeg));
        tripodLeg2.transform.localRotation = Quaternion.Euler(new Vector3(123.077f, 0, _TripodLeg));
        tripodLeg3.transform.localRotation = Quaternion.Euler(new Vector3(-120, 0, _TripodLeg));

        Distance();
    }

    private void FixedUpdate()
    {
        rotateLeg = Mathf.Lerp(1, 0, Point(Remap(distance, 0.3f, 4f, 0, 1)));
    }

    void Distance()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.up), out hit, Mathf.Infinity, _layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(-Vector3.up) * hit.distance, Color.yellow);
            distance = (float)System.Math.Round(hit.distance, 1);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(-Vector3.up) * 1000, Color.white);
            distance = 0;
        }

    }

    public float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    float Point(float coord)
    {
        float A = 0;
        float B = 0.2f;
        float C = 0.25f;
        float D = 1f;

        float M = Mathf.Lerp(A, B, coord);
        float N = Mathf.Lerp(B, C, coord);
        float O = Mathf.Lerp(C, D, coord);

        float X = Mathf.Lerp(M, N, coord);
        float Y = Mathf.Lerp(N, O, coord);

        return Mathf.Lerp(X, Y, coord);
    }

    private void OnDrawGizmos()
    {
        
    }
}
