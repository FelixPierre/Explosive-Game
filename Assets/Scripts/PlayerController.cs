using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    // Input values
    Vector2 moveInput;
    Vector2 lookInput;

    public float translationSpeed = 1;
    public float rotationSpeed = 1;

    public GameObject head;

    public PlayerInput playerInput;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Look();
    }

    #region Movement

    void Move() {
        Vector3 direction = new Vector3(moveInput.x, 0, moveInput.y); // moveInput is already normalized

        if (direction != Vector3.zero) {
            float scalar = Vector3.Dot(direction, transform.forward);

            Vector3 movement = transform.forward * translationSpeed * scalar * Time.deltaTime;
            rb.MovePosition(transform.position + movement);

            Vector3 forward = scalar > 0 ? transform.forward : transform.forward * -1;

            float angle = Vector3.SignedAngle(direction, forward, transform.up);
            int rotDir = angle >= 0 && angle < 180 ? -1 : 1;
            Vector3 rotation;
            if (Mathf.Abs(rotationSpeed * Time.deltaTime) > Mathf.Abs(angle)) {
                rotation = transform.up * angle * -1;
            }
            else {
                rotation = transform.up * rotationSpeed * rotDir * Time.deltaTime;
            }
            rb.MoveRotation(Quaternion.Euler(transform.rotation.eulerAngles + rotation));
        }
    }

    void Look() {
        Vector3 rotation = lookInput.x * Vector3.up;
        head.transform.Rotate(rotation * rotationSpeed / 100);
        Debug.DrawLine(head.transform.position, head.transform.position + head.transform.forward * 2, Color.red);
    }

    #endregion

    #region Input Callback

    public void Move(InputAction.CallbackContext context) {
        moveInput = context.ReadValue<Vector2>();
    }

    public void Look(InputAction.CallbackContext context) {
        lookInput = context.ReadValue<Vector2>();
    }

    public void Fire(InputAction.CallbackContext context) {
        Debug.Log("Try to fire, but this action is not implemented!");
    }

    public void ControlChanged(PlayerInput input) {
        Debug.Log(string.Format("Control changed: {0} {1}", input, input.currentControlScheme) );
    }

    #endregion
}
