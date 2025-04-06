using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FrogController : MonoBehaviour
{

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Jump action performed!");
        }
    }

    public void LongJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Long jump action performed!");
        }
    }
}
