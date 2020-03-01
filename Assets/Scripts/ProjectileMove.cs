using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileMove : MonoBehaviour {

    #region Members

    private Rigidbody m_rb;

    #endregion

    #region Properties

    public float Speed { get; set; }

    public GameObject HitPrefab { get; set; }

    #endregion

    // Start is called before the first frame update
    void Start() {
        m_rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 10);
    }

    // Update is called once per frame
    void Update() {
        if (Speed != 0) {
            m_rb.MovePosition(transform.position + transform.forward * Speed * Time.deltaTime);
        }
        else {
            Debug.Log("No speed!");
        }
    }

    private void OnCollisionEnter(Collision collision) {
        Speed = 0;

        ContactPoint contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point;

        if (HitPrefab != null) {
            var hitVFX = Instantiate(HitPrefab, pos, rot);
            //var psHit = hitVFX.GetComponent<ParticleSystem>();
            //if (psHit != null) {
            //    Destroy(hitVFX, psHit.main.duration);
            //}
            //else {
            //    var psChild = hitVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
            //    Destroy(hitVFX, psChild.main.duration);
            //}
            Destroy(hitVFX, 5);
        }

        Destroy(gameObject);
    }
}
