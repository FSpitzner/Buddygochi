using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buddy : MonoBehaviour
{
    [SerializeField] Energy energy;
    

    public void Eat(EatableObject meal)
    {
        Debug.Log("Eating " + meal.objectName);
        energy.ChangeBy(meal.energyAmount);
    }
}
