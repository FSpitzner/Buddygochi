using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbController : MonoBehaviour
{
    [SerializeField] Camera camera;
    [SerializeField] LayerMask orbLayermask;
    [SerializeField] LayerMask orbTargetLayermask;
    [SerializeField] LayerMask groundLayermask;

    private Transform currentObject;
    private MoveableObject currentMoveableObject;

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            Debug.Log("Button Click");
            RaycastHit hit, hit2, hit3;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition),
                ray2 = camera.ScreenPointToRay(Input.mousePosition),
                ray3 = camera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit, orbLayermask))
            {
                Debug.DrawRay(ray.origin, ray.direction, Color.green);
                Debug.Log("Orb found! " + hit.collider.transform.name);
                currentObject = hit.collider.transform;
                currentMoveableObject = currentObject.GetComponent<MoveableObject>();
                Debug.Log("Moveable Object: " + currentMoveableObject.name);
            }
            else if(currentObject != null && Physics.Raycast(ray2, out hit2, groundLayermask))
            {
                Debug.DrawRay(ray2.origin, ray2.direction, Color.yellow);
                Debug.Log("Ground hit!");
                currentObject.transform.position = hit2.point;
            }
            else if(currentObject != null && Physics.Raycast(ray3, out hit3, orbTargetLayermask))
            {
                Debug.DrawRay(ray3.origin, ray3.direction, Color.red);
                Debug.Log("OrbTarget hit!");
                currentObject.transform.position = hit3.transform.position;
                currentMoveableObject.SetMoveable(false);
                currentObject = null;
                currentMoveableObject = null;
            }
        }else
        {
            currentObject = null;
            currentMoveableObject = null;
        }
    }
}
