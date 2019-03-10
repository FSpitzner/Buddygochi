using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Eatable Object")]
public class EatableObject : ScriptableObject
{
    public string objectName;
    public float energyAmount;
    public Color color;
}
