using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    // Input values
    float m_moveInput;
    float m_rotateInput;

    [Header("Speed")]
    public float m_maxSpeed = 1;
    public float m_rotationSpeed = 1;

    Rigidbody m_rb;

    // Start is called before the first frame update
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        m_rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Rotate();
    }

    #region Cardinal Movement

    void Move() {
        Vector3 movement = transform.forward * m_moveInput * m_maxSpeed * Time.deltaTime;

        m_rb.MovePosition(transform.position + movement);
    }

    void Rotate() {
        int direction = m_moveInput >= 0 ? 1 : -1;
        Vector3 rotation = Vector3.up * direction * m_rotateInput * m_rotationSpeed * Time.deltaTime;

        m_rb.MoveRotation(transform.rotation * Quaternion.Euler(rotation));
    }

    #endregion

    #region Input Callback

    public void Move(InputAction.CallbackContext context) {
        m_moveInput = context.ReadValue<float>();
    }

    public void Rotate(InputAction.CallbackContext context) {
        m_rotateInput = context.ReadValue<float>();
    }

    public void Fire(InputAction.CallbackContext context) {
        Debug.Log("Try to fire, but this action is not implemented!");
    }

    public void ControlChanged(PlayerInput input) {
        Debug.Log(string.Format("Control changed: {0} {1}", input, input.currentControlScheme) );
    }

    #endregion
}
