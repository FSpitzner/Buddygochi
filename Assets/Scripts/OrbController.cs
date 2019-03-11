using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbController : MonoBehaviour
{
    [SerializeField] Camera camera;
    [SerializeField] LayerMask orbLayermask;
    [SerializeField] LayerMask orbTargetLayermask;
    [SerializeField] LayerMask groundLayermask;
    [SerializeField] GameEvent startLookingEvent;
    [SerializeField] GameEvent stopLookingEvent;

    private Transform currentObject;
    private MoveableObject currentMoveableObject;

    private void FixedUpdate()
    {
        Vector3 inputPos;
        bool buttonPressed;
#if UNITY_EDITOR
        //Debug.Log("Editor");
        inputPos = Input.mousePosition;
        buttonPressed = Input.GetMouseButton(0);
#else
        //Debug.Log("Android");
        if (Input.touchCount > 0){
            inputPos = Input.GetTouch(0).position;
            buttonPressed = Input.GetTouch(0).phase == TouchPhase.Ended ? false : true;
        }
        else{
            inputPos = new Vector3(0,0,0);
            buttonPressed = false;
        }
#endif
        if (buttonPressed)
        {
            Debug.Log("Button Click");
            RaycastHit hit, hit2, hit3;
            Ray ray = camera.ScreenPointToRay(inputPos),
                ray2 = camera.ScreenPointToRay(inputPos),
                ray3 = camera.ScreenPointToRay(inputPos);
            if(Physics.Raycast(ray, out hit, 100f, orbLayermask))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.green);
                Debug.Log("Orb found! " + hit.collider.transform.name);
                currentObject = hit.collider.transform;
                currentMoveableObject = currentObject.GetComponent<MoveableObject>();
                Debug.Log("Moveable Object: " + currentMoveableObject.name);
            }
            else if(currentObject != null && Physics.Raycast(ray3, out hit3, 100f, orbTargetLayermask))
            {
                Debug.DrawLine(ray3.origin, hit3.point, Color.red);
                Debug.Log("OrbTarget hit!");
                currentObject.transform.position = hit3.transform.position;
                currentMoveableObject.SetMoveable(false);
                currentObject = null;
                currentMoveableObject = null;
            }
            else if (currentObject != null && Physics.Raycast(ray2, out hit2, 100f, groundLayermask))
            {
                Debug.DrawLine(ray2.origin, hit2.point, Color.yellow);
                Debug.Log("Ground hit!");
                currentObject.transform.position = hit2.point;
            }
        }
        else
        {
            currentObject = null;
            currentMoveableObject = null;
        }
        if(currentObject != null)
        {
            RaiseCurrentlySelected(currentObject);
        }
        else
        {
            RaiseNothingSelected();
        }
    }

    public void RaiseCurrentlySelected(Transform currentObject)
    {
        startLookingEvent.Raise(this, currentObject);
    }

    public void RaiseNothingSelected()
    {
        stopLookingEvent.Raise(this);
    }
}