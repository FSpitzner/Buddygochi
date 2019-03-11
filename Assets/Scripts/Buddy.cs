using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buddy : MonoBehaviour
{
    [SerializeField] Energy energy;
    [Tooltip("The Time between Ticks in Seconds")]
    [SerializeField] float tickInterval;
    [SerializeField] float energyDropPerTick;

    private Transform watchObject;
    private float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer >= tickInterval)
        {
            energy.ChangeBy(-energyDropPerTick);
            timer = 0f;
        }
        if (watchObject != null)
        {
            transform.LookAt(watchObject);
        }
    }

    public void Eat(EatableObject meal)
    {
        Debug.Log("Eating " + meal.objectName);
        energy.ChangeBy(meal.energyAmount);
    }

    public void RegisterWatchObject(Transform obj)
    {
        watchObject = obj;
    }

    public void ClearWatchObject()
    {
        watchObject = null;
    }
}
