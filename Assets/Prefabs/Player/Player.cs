using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float deadzone = 0.05f;

    private Quaternion initialGyroRotation;
    private float accumulatedYaw = 0f;

    void OnEnable()
    {
        StartCoroutine(InitializeComponents());
    }

    IEnumerator InitializeComponents()
    {
        yield return new WaitForSeconds(1f);
        Input.gyro.enabled = true;
        initialGyroRotation = Input.gyro.attitude;
    }

    void OnDisable()
    {
        Input.gyro.enabled = false;
    }

    void FixedUpdate()
    {
        Vector3 acceleration = Input.acceleration;
        Vector2 horizontalAcceleration = (Vector2)acceleration;

        if (horizontalAcceleration.magnitude > deadzone)
        {
            Vector3 forwardMovement = -acceleration.y * moveSpeed * Time.deltaTime * transform.right;
            Vector3 rightMovement = acceleration.x * moveSpeed * Time.deltaTime * transform.forward;
            Vector3 movement = forwardMovement + rightMovement;

            transform.Translate(movement, Space.World);
        }

        Vector3 keyboardMovement = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) keyboardMovement -= transform.right;
        if (Input.GetKey(KeyCode.S)) keyboardMovement += transform.right;
        if (Input.GetKey(KeyCode.A)) keyboardMovement -= transform.forward;
        if (Input.GetKey(KeyCode.D)) keyboardMovement += transform.forward;

        transform.Translate(moveSpeed * Time.deltaTime * keyboardMovement, Space.World);

        Quaternion gyroRotation = Input.gyro.attitude;
        Quaternion relativeRotation = Quaternion.Inverse(initialGyroRotation) * gyroRotation;
        Vector3 adjustedEulerAngles = relativeRotation.eulerAngles;
        float gyroYaw = -adjustedEulerAngles.z;

        if (Input.GetKey(KeyCode.Q)) accumulatedYaw -= 3f;
        if (Input.GetKey(KeyCode.E)) accumulatedYaw += 3f;

        float finalYaw = gyroYaw + accumulatedYaw;
        transform.rotation = Quaternion.Euler(0, finalYaw, 0);

        string formattedMove = $"Move: {acceleration.x + keyboardMovement.x:F2}, {acceleration.y + keyboardMovement.z:F2}";
        string formattedRotate = $"Rotate: Yaw {finalYaw:F2}";

        UIManager.Instance.SetMoveText(formattedMove);
        UIManager.Instance.SetRotateText(formattedRotate);
    }
}
