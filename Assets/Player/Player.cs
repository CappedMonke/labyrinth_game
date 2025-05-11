using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5.0f;
    public float backwardSpeed = 2.0f;
    public float hozizontalSpeed = 3.0f;
    public bool allowHorizontalMovement = true;

    [Header("Input Actions")]
    public InputActionReference moveActionKeyboard;
    public InputActionReference moveActionPhone;
    public InputActionReference rotateActionKeyboard;
    public InputActionReference rotateActionPhone;

    private Vector2 moveInputKeyboard;
    private Vector3 moveInputMobile;
    private Vector2 rotateInputKeyboard;
    private Quaternion rotateInputMobile;

    private void OnEnable()
    {
        moveActionKeyboard.action.performed += ctx => moveInputKeyboard = ctx.ReadValue<Vector2>();
        moveActionKeyboard.action.canceled += ctx => moveInputKeyboard = Vector2.zero;
        moveActionKeyboard.action.Enable();

        moveActionPhone.action.performed += ctx => moveInputMobile = ctx.ReadValue<Vector3>();
        moveActionPhone.action.canceled += ctx => moveInputMobile = Vector3.zero;
        moveActionPhone.action.Enable();

        rotateActionKeyboard.action.performed += ctx => rotateInputKeyboard = ctx.ReadValue<Vector2>();
        rotateActionKeyboard.action.canceled += ctx => rotateInputKeyboard = Vector2.zero;
        rotateActionKeyboard.action.Enable();

        rotateActionPhone.action.performed += ctx => rotateInputMobile = ctx.ReadValue<Quaternion>();
        rotateActionPhone.action.canceled += ctx => rotateInputMobile = Quaternion.identity;
        rotateActionPhone.action.Enable();
    }

    private void OnDisable()
    {
        moveActionKeyboard.action.Disable();
        moveActionPhone.action.Disable();
        rotateActionKeyboard.action.Disable();
        rotateActionPhone.action.Disable();
    }

    private void Update()
    {
        Vector3 moveVector = Vector3.zero;

        if (moveInputKeyboard != Vector2.zero)
        {
            moveVector = new Vector3(moveInputKeyboard.x, 0, moveInputKeyboard.y);
        }
        else if (moveInputMobile != Vector3.zero)
        {
            moveVector = new Vector3(moveInputMobile.x, 0, moveInputMobile.z);
        }

        float currentSpeed = speed;

        if (moveVector.z < 0)
        {
            currentSpeed = backwardSpeed;
        }

        if (!allowHorizontalMovement)
        {
            moveVector.x = 0;
        }
        else
        {
            moveVector.x *= (hozizontalSpeed / speed);
        }

        transform.Translate(currentSpeed * Time.deltaTime * moveVector, Space.World);

        float yaw = 0f;

        if (rotateInputKeyboard != Vector2.zero)
        {
            yaw = rotateInputKeyboard.x * 10f;
        }
        else if (rotateInputMobile != Quaternion.identity)
        {
            Vector3 euler = rotateInputMobile.eulerAngles;
            yaw = euler.y;
        }

        transform.Rotate(0, yaw * Time.deltaTime, 0);
    }
}
