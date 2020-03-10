using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : Special
{
    [SerializeField] private float m_maxRange = 1;
    //[SerializeField] private float m_expensionSpeed = 1;
    [SerializeField] private LineRenderer m_lineRenderer = null;
    [SerializeField] private float m_knockbackStrength = 0;

    float m_currentRange;
    PlayerController m_casterController;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        if (Caster != null) {
            m_casterController = Caster.GetComponent<PlayerController>();
        }
        Expend();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        Expend();
        Knockback();
    }

    void Expend() {
        Vector3 start = m_lineRenderer.GetPosition(0);
        Vector3 worldStart = m_lineRenderer.transform.TransformPoint(start);
        float scale = m_lineRenderer.transform.lossyScale.z;

        if (Physics.Raycast(worldStart, m_lineRenderer.transform.forward, out RaycastHit hit, m_maxRange * scale)) {
            Vector3 pos = m_lineRenderer.transform.InverseTransformPoint(hit.point);
            m_lineRenderer.SetPosition(1, pos);
            m_currentRange = hit.distance;
        }
        else {
            Vector3 pos = start + Vector3.forward * m_maxRange;
            m_lineRenderer.SetPosition(1, pos);
            m_currentRange = Vector3.Distance(start, pos);
        }
    }

    void Knockback() {
        if (m_casterController != null) {
            m_casterController.Slide(-m_lineRenderer.transform.forward, m_knockbackStrength);
        }
    }
}
