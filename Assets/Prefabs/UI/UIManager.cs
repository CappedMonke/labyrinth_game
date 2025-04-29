using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public TextMeshProUGUI MoveText;
    public TextMeshProUGUI RotateText;
    public TextMeshProUGUI WidthText;
    public TextMeshProUGUI HeightText;
    public TextMeshProUGUI TileSizeText;
    public TextMeshProUGUI RandomWallRemovalText;

    public Slider WidthSlider;
    public Slider HeightSlider;
    public Slider TileSizeSlider;
    public Slider RandomWallRemovalSlider;

    public Button RegenerateButton;
    private MapGenerator mapGenerator;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        WidthText.text = "Width: " + WidthSlider.value.ToString("F0");
        HeightText.text = "Height: " + HeightSlider.value.ToString("F0");
        TileSizeText.text = "Tile Size: " + TileSizeSlider.value.ToString("F2");
        RandomWallRemovalText.text = "Random Wall Removal: " + RandomWallRemovalSlider.value.ToString("F2");
    }

    void Start()
    {
        mapGenerator = FindFirstObjectByType<MapGenerator>();

        RegenerateButton.onClick.AddListener(OnRegenerateButtonClicked);
        WidthSlider.onValueChanged.AddListener(OnWidthSliderChanged);
        HeightSlider.onValueChanged.AddListener(OnHeightSliderChanged);
        TileSizeSlider.onValueChanged.AddListener(OnTileSizeSliderChanged);
        RandomWallRemovalSlider.onValueChanged.AddListener(OnRandomWallRemovalSliderChanged);
    }

    private void OnRandomWallRemovalSliderChanged(float arg0)
    {
        mapGenerator.randomWallRemovalPercentage = arg0;
        RandomWallRemovalText.text = "Random Wall Removal: " + arg0.ToString("F2");
    }

    private void OnTileSizeSliderChanged(float arg0)
    {
        mapGenerator.tileSize = arg0;
        TileSizeText.text = "Tile Size: " + arg0.ToString("F2");    
    }

    private void OnHeightSliderChanged(float arg0)
    {
        mapGenerator.height = (int)arg0;
        HeightText.text = "Height: " + arg0.ToString("F0");
    }

    private void OnWidthSliderChanged(float arg0)
    {
        mapGenerator.width = (int)arg0;
        WidthText.text = "Width: " + arg0.ToString("F0");
    }

    public void SetMoveText(string text)
    {
        MoveText.text = text;
    }

    public void SetRotateText(string text)
    {
        RotateText.text = text;
    }

    public void OnRegenerateButtonClicked()
    {
        mapGenerator.RegenerateLabyrinth();
    }
}
