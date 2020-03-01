using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FiringController : MonoBehaviour
{
    public Transform m_firePoint;
    public List<Projectile> m_projectiles;

    private bool m_isFiring = false;
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
        if (m_isFiring) {
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

    #region Input Callback

    public void Fire(InputAction.CallbackContext context) {
        if (context.started) {
            m_isFiring = true;
        }
        if (context.canceled) {
            m_isFiring = false;
        }
    }

    public void ControlChanged(PlayerInput input) {
        Debug.Log(string.Format("Control changed: {0} {1}", input, input.currentControlScheme));
    }

    #endregion
}
