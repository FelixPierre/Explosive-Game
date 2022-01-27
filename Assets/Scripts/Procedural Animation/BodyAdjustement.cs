using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyAdjustement : MonoBehaviour
{
    public Transform[] foot;

    private Vector3 defaultLocalPos;

    private void Awake() {
        defaultLocalPos = transform.localPosition;
    }

    private void Update() {
        AdjustHeight();
    }

    void AdjustHeight() {
        float avgHeight = 0f;
        for (int i = 0; i < foot.Length; i++) {
            avgHeight += foot[i].localPosition.y;
        }
        avgHeight /= foot.Length;

        //transform.position += avgHeight * transform.up;
        transform.localPosition = defaultLocalPos + avgHeight * Vector3.up;
    }

    void AdjustRotation() {

    }
}
