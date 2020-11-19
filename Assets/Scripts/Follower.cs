using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public Transform target;
    public AnimationCurve curve;
    public float verticalAmplitude = 1;
    public float movementSpeed = 1;
    public float distanceBeforeMoving = 1;

    public Follower opposite;
    public List<Follower> neightbors;

    private bool grounded = true;
    private float startTime;
    private float endTime;
    private Vector3 movingFrom;
    private Vector3 movingTo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (opposite.grounded) {
        if (NeighborsGrounded()) {
            if (grounded) {
                if (Vector3.Distance(target.position, transform.position) > distanceBeforeMoving) {
                    grounded = false;
                    startTime = Time.time;
                    endTime = startTime + 1 / movementSpeed;
                    movingFrom = transform.position;
                    movingTo = target.position;
                    Move();
                }
            }
            else {
                Move();
            }
        }
    }

    bool NeighborsGrounded() {
        foreach (var n in neightbors) {
            if (!n.grounded) {
                return false;
            }
        }
        return true;
    }

    void Move() {
        float currentTime = Time.time;
        if (currentTime > endTime) {
            currentTime = endTime;
            grounded = true;
        }

        float currentPos = Mathf.InverseLerp(startTime, endTime, currentTime);
        transform.position = Vector3.Lerp(movingFrom, target.position, currentPos) + Vector3.up * curve.Evaluate(currentPos) * verticalAmplitude;
        //transform.position = Vector3.Lerp(movingFrom, movingTo, currentPos) + Vector3.up * curve.Evaluate(currentPos) * verticalAmplitude;
    }
}
