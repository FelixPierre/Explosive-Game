using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderProceduralAnimation : MonoBehaviour {

    #region Members

    public Transform[] legTargets;
    public bool alsoMoveOppositeLeg = false;
    public float legRange = 1f;
    public float stepHeight = 0.1f;
    public int smoothness = 10;
    public float legVerticalRange = 1f;

    private int nbLeg;
    private Vector3[] defaultLegPositions;
    private Vector3[] lastLegPositions;
    private bool[] legMoving;

    private Vector3 velocity;
    private Vector3 lastBodyPosition;

    #endregion

    #region Init

    void Start() {
        lastBodyPosition = transform.position;

        nbLeg = legTargets.Length;
        defaultLegPositions = new Vector3[nbLeg];
        lastLegPositions = new Vector3[nbLeg];
        legMoving = new bool[nbLeg];
        for (int i = 0; i < nbLeg; i++) {
            defaultLegPositions[i] = legTargets[i].localPosition;
            lastLegPositions[i] = legTargets[i].position;
            legMoving[i] = false;
        }
    }

    #endregion

    #region Update

    void FixedUpdate() {
        UpdateVelocity();

        if (NoLegMoving()) {
            // Check which leg need to move the most
            Vector3[] desiredPositions = new Vector3[nbLeg];
            float maxDistance = legRange;
            int legToMove = -1;

            for (int i = 0; i < nbLeg; i++) {
                desiredPositions[i] = transform.TransformPoint(defaultLegPositions[i]);
                float distance = Vector3.ProjectOnPlane(desiredPositions[i] - lastLegPositions[i], transform.up).magnitude;
                if (distance > maxDistance) {
                    maxDistance = distance;
                    legToMove = i;
                }
            }

            // Move Leg
            if (legToMove != -1) {
                legMoving[legToMove] = true;
                Vector3 bodyTravel = velocity * smoothness * Time.fixedDeltaTime * 0.75f; // Distance travelled by the body during the step (scaled down)
                Vector3 velocityOffset = Mathf.Clamp(velocity.magnitude, 0f, legRange) * velocity.normalized;
                Vector3 targetPoint = desiredPositions[legToMove] + bodyTravel + velocityOffset;
                targetPoint = MatchToSurfaceFromAbove(targetPoint, legVerticalRange, transform.up)[0];
                StartCoroutine(PerformStep(legToMove, targetPoint));

                // Also move opposite leg if it need to
                if (alsoMoveOppositeLeg) {
                    int oppositeLeg = (legToMove & 1) == 0 ? legToMove + 1 : legToMove - 1;
                    float distance = Vector3.ProjectOnPlane(desiredPositions[oppositeLeg] - lastLegPositions[oppositeLeg], transform.up).magnitude;
                    if (distance > legRange) {
                        legMoving[oppositeLeg] = true;
                        targetPoint = desiredPositions[oppositeLeg] + bodyTravel + velocityOffset;
                        targetPoint = MatchToSurfaceFromAbove(targetPoint, legVerticalRange, transform.up)[0];
                        StartCoroutine(PerformStep(oppositeLeg, targetPoint));
                    }
                }
            }
        }

        // Position of legs not moving doesn't change
        for (int i = 0; i < nbLeg; i++) {
            if (!legMoving[i]) {
                legTargets[i].position = lastLegPositions[i];
            }
        }
    }

    void UpdateVelocity() {
        velocity = (transform.position - lastBodyPosition) / Time.fixedDeltaTime;
        lastBodyPosition = transform.position;
    }

    IEnumerator PerformStep(int legIndex, Vector3 targetPoint) {
        Vector3 startPos = lastLegPositions[legIndex];
        for (int i = 1; i <= smoothness; ++i) {
            legTargets[legIndex].position = Vector3.Lerp(startPos, targetPoint, i / (float)(smoothness + 1f));
            legTargets[legIndex].position += transform.up * Mathf.Sin(i / (float)(smoothness + 1f) * Mathf.PI) * stepHeight; // Vertical offset
            yield return new WaitForFixedUpdate();
        }
        legTargets[legIndex].position = targetPoint;
        lastLegPositions[legIndex] = legTargets[legIndex].position;
        legMoving[legIndex] = false;
    }

    #endregion

    #region Utils

    private bool NoLegMoving() {
        for (int i = 0; i < nbLeg; i++) {
            if (legMoving[i]) { return false; }
        }
        return true;
    }

    static Vector3[] MatchToSurfaceFromAbove(Vector3 point, float halfRange, Vector3 up) {
        Vector3[] res = new Vector3[2];
        RaycastHit hit;
        Ray ray = new Ray(point + halfRange * up, -up);

        if (Physics.Raycast(ray, out hit, 2f * halfRange)) {
            res[0] = hit.point;
            res[1] = hit.normal;
        }
        else {
            res[0] = point;
        }
        return res;
    }

    #endregion

    #region Debug

    private void OnDrawGizmosSelected() {
        for (int i = 0; i < nbLeg; i++) {
            Gizmos.color = legMoving[i] ? Color.yellow : Color.red;
            Gizmos.DrawWireSphere(legTargets[i].position, 0.05f);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.TransformPoint(defaultLegPositions[i]), legRange);
        }
    }

    #endregion
}
