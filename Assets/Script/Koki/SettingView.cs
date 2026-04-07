using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingView : MonoBehaviour
{
    [SerializeField] private Button saveSettingsButton;　//保存するボタン
    [SerializeField] private Slider mouseSensitivitySlider; //マウス感度を設定するスライダー
    [SerializeField] private Button accelerationKeyButton; 
    [SerializeField] private TMPro.TextMeshProUGUI accelerationKeyText; //現在のキーを表示するテキスト
    [SerializeField] private GameObject rebindOverlay; //待機中のオーバーレイ
    [SerializeField] public GameObject settingPanel;

    public event Action OnSaveButton; //セーブボタンが押されたら発行
    public event Action<float> OnSensitivityValueChange; //マウス感度が変わったら発行
    public event Action OnAccelerationKeyButtonClick; //加速キー変更ボタンが押されたら発行
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        saveSettingsButton.onClick.AddListener(()=>OnSaveButton?.Invoke());
        if (accelerationKeyButton != null)
        {
            accelerationKeyButton.onClick.AddListener(() => OnAccelerationKeyButtonClick?.Invoke());
        }
        //mouseSensitivitySlider.onValueChanged.AddListener(() => OnSensitivityValueChange.Invoke());
    }

    public void SetAccelerationKeyText(string text)
    {
        if (accelerationKeyText != null) accelerationKeyText.text = text;
    }

    public void ShowRebindOverlay(bool show)
    {
        if (rebindOverlay != null) rebindOverlay.SetActive(show);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
