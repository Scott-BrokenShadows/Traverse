using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class NotepadValues
{
    // BackSight
    public InputField BSTH;  // Target Height
    public InputField BSHZ;  // Horizontal Bearing
    public InputField BSD;   // Distance
    public InputField BSDH;  // Height Difference

    // Total Staion 
    public InputField STN;   // Where the Total Station is

    // ForeSight
    public InputField FSTH;  // Target Height
    public InputField FSHZ;  // Horizontal Bearing
    public InputField FSD;   // Distance
    public InputField FSDH;  // Height Difference

}

public class Notepad : MonoBehaviour
{
    public List<NotepadValues> nValues;
}
