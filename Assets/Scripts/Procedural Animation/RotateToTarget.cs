using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToTarget : MonoBehaviour
{
    public Transform target;
    public float delta = 0.001f;

    private Vector3 rAxis;
    private Plane rPlane;

    private void Awake() {
        if (transform.parent == null)
            rAxis = Vector3.up;
        else
            rAxis = transform.parent.up;
        rPlane = new Plane(rAxis, transform.position);
    }

    private void LateUpdate() {
        Vector3 leg = rPlane.ClosestPointOnPlane(transform.position + transform.up) - transform.position;
        Vector3 goal = rPlane.ClosestPointOnPlane(target.position) - transform.position;
        float angle = Vector3.SignedAngle(leg, goal, rAxis);

        if (angle * angle > delta * delta) 
            transform.RotateAround(transform.position, rAxis, angle);
    }

    private void OnDrawGizmos() {
        if (target != null) {
            Debug.DrawLine(transform.position, transform.position + rAxis, Color.yellow);
            Debug.DrawLine(transform.position, transform.position + transform.up, Color.green);
            Debug.DrawLine(transform.position, transform.position + (target.position - transform.position).normalized, Color.blue);
        }
    }
}
