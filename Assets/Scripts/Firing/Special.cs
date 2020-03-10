using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Special : MonoBehaviour
{
    [SerializeField] float m_castDuration = 0;

    protected float m_endTime;

    #region Properties

    public GameObject Caster { get; set; } = null;
    public float CastDuration { get => m_castDuration; }

    #endregion

    protected void Start() {
        m_endTime = Time.time + m_castDuration;
    }

    protected void Update() {
        if (Time.time > m_endTime) {
            Destroy(gameObject);
        }
    }
}
