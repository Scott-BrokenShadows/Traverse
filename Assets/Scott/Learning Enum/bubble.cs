using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bubble : MonoBehaviour
{
    [SerializeField] private GameObject tribrach;
    public float bubbleMaxRange = 0.35f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }


    public static void UpdateBubbleY(Vector3 tribrachLocalEuler, float maxPosValue)
    {
        float brachNormalized = tribrachLocalEuler / maxPosValue;
    }
    public static void UpdateBubbleZ(float maxPosValue)
    {

    }

    public static float Remap(float input, float oldLow, float oldHigh, float newLow, float newHigh)
    {
        float t = Mathf.InverseLerp(oldLow, oldHigh, input);
        return Mathf.Lerp(newLow, newHigh, t);
    }
}
