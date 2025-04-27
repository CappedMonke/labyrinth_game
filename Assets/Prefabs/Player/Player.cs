using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float deadzone = 0.05f;
    private Quaternion initialGyroRotation;

    void OnEnable()
    {
        StartCoroutine(InitializeComponents());
    }

    IEnumerator InitializeComponents()
    {
        yield return new WaitForSeconds(1f);
        Input.gyro.enabled = true;
        initialGyroRotation = Input.gyro.attitude; // Store the initial gyroscope rotation
    }

    void OnDisable()
    {
        Input.gyro.enabled = false;
    }

    void FixedUpdate()
    {
        // Movement using accelerometer
        Vector3 acceleration = Input.acceleration;
        Vector2 horizontalAcceleration = (Vector2)acceleration;

        if (horizontalAcceleration.magnitude > deadzone)
        {
            Vector3 movement = moveSpeed * Time.deltaTime * new Vector3(-acceleration.y, 0, acceleration.x);
            transform.Translate(movement, Space.World);
        }

        // Rotation using gyroscope
        Quaternion gyroRotation = Input.gyro.attitude;
        Quaternion relativeRotation = Quaternion.Inverse(initialGyroRotation) * gyroRotation;
        Vector3 adjustedEulerAngles = relativeRotation.eulerAngles;
        float yaw = -adjustedEulerAngles.z;
        transform.rotation = Quaternion.Euler(0, yaw, 0);

        string formattedMove = $"Move: {acceleration.x:F2}, {acceleration.y:F2}";
        string formattedRotate = $"Rotate: Yaw {yaw:F2}";

        UIManager.Instance.SetMoveText(formattedMove);
        UIManager.Instance.SetRotateText(formattedRotate);
    }
}
