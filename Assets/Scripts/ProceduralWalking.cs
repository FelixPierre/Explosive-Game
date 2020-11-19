using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralWalking : MonoBehaviour
{
    public float maxStepLength = 1;
    public float verticalElevation = 1;

    [SerializeField] public TargetPoint[] targetPoints;
    public LayerMask collisionMask;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var point in targetPoints) {
            Debug.Log(point.targetPointDefaultPos);
        }
    }

    // Update is called once per frame
    void Update()
    {
        MoveTargetPoint();
        UpdateTarget();
    }

    void MoveTargetPoint() {
        foreach (var point in targetPoints) {
            RaycastHit hit;
            Vector3 start = point.targetPoint.parent.TransformPoint(point.targetPointDefaultPos);
            if (Physics.Raycast(start + Vector3.up * verticalElevation, Vector3.down, out hit, 2 * verticalElevation, collisionMask.value)) {
                point.targetPoint.position = hit.point;
            }
            else {
                point.targetPoint.position = start;
            }
        }
    }

    void UpdateTarget() {
        foreach (var target in targetPoints) {
            //Debug.Log(Vector3.Distance(target.boneTarget.position, target.targetPoint.position));
            if (Vector3.Distance(target.boneTarget.position, target.targetPoint.position) > maxStepLength) {
                target.boneTarget.position = target.targetPoint.position;
            }
        }
    }

    private void OnDrawGizmos() {
        foreach (var point in targetPoints) {
            Vector3 orgn = point.targetPoint.TransformPoint(point.targetPointDefaultPos);
            Debug.DrawLine(orgn, orgn + Vector3.up * verticalElevation, Color.blue);
            Debug.DrawLine(orgn, orgn + Vector3.down * verticalElevation, Color.blue);
        }
    }
}


[System.Serializable]
public struct TargetPoint 
{
    [SerializeField] public Transform boneTarget;
    [SerializeField] public Transform targetPoint;
    [System.NonSerialized] public Vector3 targetPointDefaultPos;

    public TargetPoint(Transform boneTarget, Transform targetPoint) {
        this.boneTarget = boneTarget;
        this.targetPoint = targetPoint;
        targetPointDefaultPos = targetPoint.localPosition;
    }
}
