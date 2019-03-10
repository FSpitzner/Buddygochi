using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Energy")]
public class Energy : ScriptableObject
{
    public float maxEnergy;
    public float currentEnergy;
    public float minEnergy;

    public void ChangeBy(float amount)
    {
        currentEnergy += amount;
    }
}
