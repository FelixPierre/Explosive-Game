using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(InputAction.CallbackContext context) {
        direction = context.ReadValue<Vector2>();
        Debug.Log("move: " + context);
    }

    public void Fire(InputAction.CallbackContext context) {
        Debug.Log("fire " + context);
    }
}
