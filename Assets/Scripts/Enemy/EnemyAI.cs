using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] public float fireRange;
    [SerializeField] public float visionRange;
    [SerializeField] public Transform target;

    [SerializeField] private NavMeshAgent m_agent;
    [SerializeField] private Animator m_animator;
    [SerializeField] private Transform m_firePoint;


    /// <summary>
    /// Move to position
    /// </summary>
    /// <param name="position"></param>
    public void MoveTo(Vector3 position) {
        m_agent.isStopped = false;
        m_agent.SetDestination(position);
    }


    /// <summary>
    /// Stop to move
    /// </summary>
    public void Stop() {
        m_agent.isStopped = true;
    }


    /// <summary>
    /// Aim the target
    /// </summary>
    /// <param name="target"></param>
    public void Aim(Transform target) {

    }

    /// <summary>
    /// Check if the target is in the vision range, with a direct view on it
    /// </summary>
    /// <returns>true if the target is in view, else false</returns>
    public bool IsTargetVisible() {
        return RaycastTarget(transform, target, visionRange);
    }


    /// <summary>
    /// Check if the target is in the vision range, with a direct view on it
    /// </summary>
    /// <param name="target"></param>
    /// <returns>true if the target is in view, else false</returns>
    public bool IsTargetVisible(Transform target) {
        return RaycastTarget(transform, target, visionRange);
    }


    /// <summary>
    /// Check if the target is in the fire range, with a direct view on it
    /// </summary>
    /// <returns>true if the target is in range, else false</returns>
    public bool IsTargetInRange() {
        return RaycastTarget(m_firePoint, target, fireRange);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public bool IsInFiringLine(Transform target) {
        return RayCast(m_firePoint.position, m_firePoint.forward, fireRange, target);
    }


    private bool RaycastTarget(Transform src, Transform target, float range) {
        Vector3 direction = Vector3.Normalize(target.position - src.position);

        Debug.DrawLine(src.position, target.position, Color.red);
        return RayCast(src.position, direction, range, target);
    }

    private bool RayCast(Vector3 origin, Vector3 direction, float range, Transform target) {
        if (Physics.Raycast(origin, direction, out RaycastHit hit, range)) {
            return hit.collider.gameObject == target.gameObject;
        }
        return false;
    }
}
