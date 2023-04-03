using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SurveyState
{
    GP,
    Tripod,
    TotalStn,
    Prism
}
public class EquipMang : MonoBehaviour
{
    public int tripods;
    public int prisms;
    public int totalStns;

    // Start is called before the first frame update
    void Start()
    {
        tripods = 0;
        prisms = 0;
        totalStns = 0;
    }
}
