using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightAdjustement : MonoBehaviour
{

    private Vector3 defaultOffset;

    public float heightDelta = 1;
    public LayerMask collisionMask;

    // Start is called before the first frame update
    void Start()
    {
        defaultOffset = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        // Physics.queriesHitBackfaces = true;
        RaycastHit hit;
        Vector3 start = transform.parent.TransformPoint(defaultOffset);
        if (Physics.Raycast(start + Vector3.up * heightDelta, Vector3.down, out hit, 2 * heightDelta, collisionMask.value)) {
            transform.position = hit.point;
        }
        else {
            transform.position = start;
        }
        // Physics.queriesHitBackfaces = false;
    }

    private void OnDrawGizmos() {
        Vector3 orgn = transform.parent.TransformPoint(defaultOffset);
        Debug.DrawLine(orgn, orgn + Vector3.up * heightDelta, Color.blue);
        Debug.DrawLine(orgn, orgn + Vector3.down * heightDelta, Color.blue);
        Debug.DrawLine(orgn, orgn + Vector3.right * 0.2f, Color.red);
    }
}
