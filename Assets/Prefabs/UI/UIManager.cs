using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public TextMeshProUGUI MoveText;
    public TextMeshProUGUI RotateText;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetMoveText(string text)
    {
        MoveText.text = text;
    }

    public void SetRotateText(string text)
    {
        RotateText.text = text;
    }
}
