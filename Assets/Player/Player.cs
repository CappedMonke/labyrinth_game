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
    private float rotateInputKeyboard;
    private Quaternion rotateInputMobile;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            InputSystem.EnableDevice(Accelerometer.current);
            InputSystem.EnableDevice(UnityEngine.InputSystem.Gyroscope.current);
        }

        moveActionKeyboard.action.performed += ctx => moveInputKeyboard = ctx.ReadValue<Vector2>();
        moveActionKeyboard.action.canceled += ctx => moveInputKeyboard = Vector2.zero;
        moveActionKeyboard.action.Enable();

        rotateActionKeyboard.action.performed += ctx => rotateInputKeyboard = ctx.ReadValue<float>();
        rotateActionKeyboard.action.canceled += ctx => rotateInputKeyboard = 0f;
        rotateActionKeyboard.action.Enable();

        moveActionPhone.action.performed += ctx => moveInputMobile = ctx.ReadValue<Vector3>();
        moveActionPhone.action.canceled += ctx => moveInputMobile = Vector3.zero;
        moveActionPhone.action.Enable();

        rotateActionPhone.action.performed += ctx => rotateInputMobile = ctx.ReadValue<Quaternion>();
        rotateActionPhone.action.canceled += ctx => rotateInputMobile = Quaternion.identity;
        rotateActionPhone.action.Enable();
    }

    private void OnDisable()
    {
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            InputSystem.DisableDevice(Accelerometer.current);
            InputSystem.DisableDevice(UnityEngine.InputSystem.Gyroscope.current);
        }

        moveActionKeyboard.action.Disable();
        rotateActionKeyboard.action.Disable();
        moveActionPhone.action.Disable();
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
            moveVector = new Vector3(moveInputMobile.x, 0, moveInputMobile.y);
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

        Vector3 adjustedMoveVector = rb.rotation * moveVector;

        Vector3 newPosition = rb.position + currentSpeed * Time.deltaTime * adjustedMoveVector;
        rb.MovePosition(newPosition);

        float yaw = 0f;

        if (rotateInputKeyboard != 0f)
        {
            yaw = rotateInputKeyboard * 250f;
        }
        else if (rotateInputMobile != Quaternion.identity)
        {
            Vector3 euler = rotateInputMobile.eulerAngles;
            yaw = euler.y;
        }

        rb.MoveRotation(rb.rotation * Quaternion.Euler(0, yaw * Time.deltaTime, 0));

        UiManager.Instance.UpdateInputTexts(moveInputKeyboard, rotateInputKeyboard, moveInputMobile, rotateInputMobile);
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void OnCollisionStay(Collision collision)
    {
        rb.linearVelocity = Vector3.zero;
    }
}
