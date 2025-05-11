using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance { get; private set; }

    [Header("Input Info")]
    public TMP_Text moveInputKeyboardText;
    public TMP_Text rotateInputKeyboardText;
    public TMP_Text moveInputMobileText;
    public TMP_Text rotateInputMobileText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void UpdateInputTexts(Vector2 moveKeyboard, float rotateKeyboard, Vector3 moveMobile, Quaternion rotateMobile)
    {
        moveInputKeyboardText.text = $"MoveInputKeyboard: ({moveKeyboard.x:F2}, {moveKeyboard.y:F2})";
        rotateInputKeyboardText.text = $"RotateInputKeyboard: ({rotateKeyboard:F2})";
        moveInputMobileText.text = $"MoveInputMobile: ({moveMobile.x:F2}, {moveMobile.y:F2}, {moveMobile.z:F2})";
        rotateInputMobileText.text = $"RotateInputMobile: ({rotateMobile.eulerAngles.x:F2}, {rotateMobile.eulerAngles.y:F2}, {rotateMobile.eulerAngles.z:F2})";
    }
}
