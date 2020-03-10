using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FiringController : MonoBehaviour
{
    [Header("Normal shot")]
    public Transform m_firePoint;
    public List<Projectile> m_projectiles;

    [Header("Special shot")]
    public Transform m_spFirePoint;
    public List<Special> m_specials;

    private bool m_isFiring = false;
    private bool m_isFiringSpecial = false;
    private float m_specialEndTime = 0;
    private Projectile m_currentProjectile;
    private float m_nextTimeToFire = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_currentProjectile = m_projectiles[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isFiringSpecial) {
            ShootSpecial();
        }
        else if (m_isFiring && Time.time > m_specialEndTime) {
            Shoot();
        }
    }

    void Shoot() {
        if (Time.time >= m_nextTimeToFire) {
            m_nextTimeToFire = Time.time + 1 / m_currentProjectile.FireRate;
            SpawnProjectile(m_currentProjectile);
        }
    }

    void SpawnProjectile(Projectile projectile) {
        var p = Instantiate(projectile, m_firePoint);
        p.transform.position = m_firePoint.position;
        Destroy(p.gameObject, 3);
    }

    void ShootSpecial() {
        Special spe = m_specials[0];
        m_specialEndTime = Time.time + spe.CastDuration;
        m_isFiringSpecial = false;

        SpawnSpecial(m_specials[0]);
    }

    void SpawnSpecial(Special special) {
        var spe = Instantiate(special, m_firePoint);
        spe.transform.position = m_firePoint.position;

        spe.Caster = gameObject;
    }

    #region Input Callback

    public void Fire(InputAction.CallbackContext context) {
        if (context.started) {
            m_isFiring = true;
        }
        else if (context.canceled) {
            m_isFiring = false;
        }
    }

    public void Special(InputAction.CallbackContext context) {
        if (context.started) {
            m_isFiringSpecial = true;
        }
    }

    public void ControlChanged(PlayerInput input) {
        Debug.Log(string.Format("Control changed: {0} {1}", input, input.currentControlScheme));
    }

    #endregion
}
