using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObject : MonoBehaviour
{
    [SerializeField] Collider collider;
    

    public void SetMoveable(bool state)
    {
        collider.enabled = state;
    }
}
