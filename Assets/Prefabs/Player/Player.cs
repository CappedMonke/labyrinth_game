using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public InputActionReference moveAction;
    public InputActionReference rotateAction;

    public float moveSpeed = 5f;

    public float deadzone = 0.1f; // Define a deadzone threshold

    void OnEnable()
    {
        var magneticFieldSensor = MagneticFieldSensor.current;
        if (magneticFieldSensor != null)
        {
            InputSystem.EnableDevice(magneticFieldSensor);
        }

        var accelerometer = Accelerometer.current;
        if (accelerometer != null)
        {
            InputSystem.EnableDevice(accelerometer);
        }

        moveAction.action.Enable();
        rotateAction.action.Enable();
    }

    void OnDisable()
    {
        moveAction.action.Disable();
        rotateAction.action.Disable();
    }

    void FixedUpdate()
    {
        Vector3 moveValue = moveAction.action.ReadValue<Vector3>();
        Vector3 rotateValue = rotateAction.action.ReadValue<Vector3>();

        // Apply deadzone to moveValue
        if (Mathf.Abs(moveValue.x) < deadzone) moveValue.x = 0;
        if (Mathf.Abs(moveValue.y) < deadzone) moveValue.y = 0;

        if (moveValue != Vector3.zero)
        {
            Vector3 moveDirection = new(moveValue.x, 0, moveValue.y);
            transform.position += moveSpeed * Time.fixedDeltaTime * moveDirection;
        }

        transform.Rotate(0, -rotateValue.z, 0);

        UIManager.Instance.SetMoveText($"Move: {moveValue}");
        UIManager.Instance.SetRotateText($"Rotate: {rotateValue}");
    }
}
