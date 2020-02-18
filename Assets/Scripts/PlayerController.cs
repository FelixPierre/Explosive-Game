using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    // Input values
    Vector2 m_moveInput;
    Vector2 m_lookInput;

    public float m_maxSpeed = 1;
    public float m_accelaration = 1;
    public float m_rotationSpeed = 1;
    private float m_currentSpeed = 0;

    public GameObject m_head;

    Rigidbody m_rb;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        m_rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Look();
    }

    #region Movement

    void Move() {
        Vector3 direction = new Vector3(m_moveInput.x, 0, m_moveInput.y); // moveInput is already normalized

        if (direction == Vector3.zero) {
            m_currentSpeed = 0; // Player stopped
        }
        else {
            float scalar = Vector3.Dot(direction.normalized, transform.forward);
            
            Vector3 forward = scalar > 0 ? transform.forward : transform.forward * -1;

            // Increase speed if not rotating
            float maxPossibleSpeed = m_maxSpeed * direction.magnitude;
            if (1 - Mathf.Abs(scalar) < 10e-3 && m_currentSpeed < maxPossibleSpeed) {
                m_currentSpeed += m_accelaration * Time.deltaTime;
            }
            m_currentSpeed = Mathf.Min(m_currentSpeed, maxPossibleSpeed); // speed can't be higher than max

            Vector3 movement = forward * m_currentSpeed * Time.deltaTime;

            // Rotate
            float angle = Vector3.SignedAngle(direction, forward, transform.up);
            int rotDir = angle >= 0 && angle < 180 ? -1 : 1;
            Vector3 rotation;
            if (Mathf.Abs(m_rotationSpeed * Time.deltaTime) > Mathf.Abs(angle)) {
                rotation = transform.up * angle * -1;
            }
            else {
                rotation = transform.up * m_rotationSpeed * rotDir * Time.deltaTime;
            }

            // Apply
            m_rb.MovePosition(transform.position + movement);
            m_rb.MoveRotation(transform.rotation * Quaternion.Euler(rotation));
        }
    }

    void Look() {
        Vector3 rotation = m_lookInput.x * Vector3.up;
        m_head.transform.Rotate(rotation * m_rotationSpeed / 100);
        Debug.DrawLine(m_head.transform.position, m_head.transform.position + m_head.transform.forward * 2, Color.red);
    }

    #endregion

    #region Input Callback

    public void Move(InputAction.CallbackContext context) {
        m_moveInput = context.ReadValue<Vector2>();
    }

    public void Look(InputAction.CallbackContext context) {
        m_lookInput = context.ReadValue<Vector2>();
    }

    public void Fire(InputAction.CallbackContext context) {
        Debug.Log("Try to fire, but this action is not implemented!");
    }

    public void ControlChanged(PlayerInput input) {
        Debug.Log(string.Format("Control changed: {0} {1}", input, input.currentControlScheme) );
    }

    #endregion
}
