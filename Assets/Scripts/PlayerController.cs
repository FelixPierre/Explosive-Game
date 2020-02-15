using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Input values
    Vector2 moveInput;
    Vector2 lookInput;

    public float translationSpeed = 1;
    public float rotationSpeed = 1;

    public GameObject head;

    public PlayerInput playerInput;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Look();
    }

    #region Movement

    void Move() {
        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y);
        transform.Translate(movement * translationSpeed * Time.deltaTime);
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
