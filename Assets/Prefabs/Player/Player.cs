using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public InputActionReference moveAction;
    public InputActionReference rotateAction;

    public float moveSpeed = 5f;
    public float rotateSpeed = 100f;

    public float deadzone = 0.1f; // Define a deadzone threshold

    void OnEnable()
    {
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

        // Move the player
        if (moveValue != Vector3.zero)
        {
            Vector3 moveDirection = new(moveValue.x, 0, moveValue.y);
            transform.position += moveSpeed * Time.fixedDeltaTime * moveDirection;
        }

        Debug.Log($"Move Value: {moveValue}");
        Debug.Log($"Rotate Value: {rotateValue}");

        // Rotate the player
        // if (rotateValue.x == 1)
        // {
        //     transform.Rotate(Vector3.up, rotateSpeed * Time.fixedDeltaTime);
        //     Debug.Log("angle: " + rotateSpeed * Time.fixedDeltaTime);
        // }
        // else if (rotateValue.x == -1)
        // {
        //     transform.Rotate(Vector3.up, -rotateSpeed * Time.fixedDeltaTime);
        //     Debug.Log("angle: " + rotateSpeed * Time.fixedDeltaTime);
        // }
    }
}
