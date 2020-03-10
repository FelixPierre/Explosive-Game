using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    #region Members

    [SerializeField] private float m_speed = 1;
    [SerializeField] private float m_fireRate = 1;

    public GameObject m_muzzlePrefab;
    public GameObject m_projectilePrefab;
    public GameObject m_hitPrefab;

    #endregion

    #region Properties

    public float FireRate {
        get => m_fireRate;
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // Muzzle
        if (m_muzzlePrefab != null) {
            var muzzleVFX = Instantiate(m_muzzlePrefab, transform.position, Quaternion.identity);
            muzzleVFX.transform.parent = transform;
            muzzleVFX.transform.forward = gameObject.transform.forward;
            var psMuzzle = muzzleVFX.GetComponent<ParticleSystem>();
            if (psMuzzle != null) {
                Destroy(muzzleVFX, psMuzzle.main.duration);
            }
            //else {
            //    var psChild = muzzleVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
            //    Destroy(muzzleVFX, psChild.main.duration);
            //}
        }

        // Projectile
        if (m_projectilePrefab != null) {
            var projVFX = Instantiate(m_projectilePrefab, transform.position, Quaternion.identity);
            projVFX.transform.forward = gameObject.transform.forward;
            //var psProj = projVFX.GetComponent<ParticleSystem>();
            //if (psProj != null) {
            //    Destroy(projVFX, psProj.main.duration);
            //}
            //else {
            //    var psChild = projVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
            //    Destroy(projVFX, psChild.main.duration);
            //}
            var move = projVFX.GetComponent<ProjectileMove>();
            if (move != null) {
                move.Speed = m_speed;
                move.HitPrefab = m_hitPrefab;
            }
        }

        Destroy(gameObject, 10);
    }
}
