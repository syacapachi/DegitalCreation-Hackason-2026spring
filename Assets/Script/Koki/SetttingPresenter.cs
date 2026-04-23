using UnityEngine;
using UnityEngine.InputSystem;

public class SetttingPresenter : MonoBehaviour
{
    [SerializeField] private SettingView settingView;
    [SerializeField] private SettingSO sso;
    [SerializeField] private FloatEventSO OnSensitivityValueChanged;
    private InputAction _boostAction;
    private const string ActionName = "Accelarate"; // 現状のスペルミスを維持
    private const string SaveKeySuffix = "_BindingPath";

    private void Awake()
    {
        // アクションの参照を取得
        _boostAction = InputSystem.actions[ActionName];

        settingView.mouseSensitivitySlider.value = sso.MouseSensitivity; 
        
        // 保存された設定をロードして適用
        LoadAndApplySettings();
    }

    void Start()
    {
        settingView.OnSaveButton += SaveSettings;
        settingView.OnAccelerationKeyButtonClick += StartRebinding;
        OnSensitivityValueChanged.Register(SensitivityValueHandler);
        
        UpdateView();
    }

    private void OnDestroy()
    {
        settingView.OnSaveButton -= SaveSettings;
        settingView.OnAccelerationKeyButtonClick -= StartRebinding;
        OnSensitivityValueChanged.Unregister(SensitivityValueHandler);
    }

    private void LoadAndApplySettings()
    {
        // PlayerPrefsからバインディング情報を読み込む
        string savedPath = PlayerPrefs.GetString(ActionName + SaveKeySuffix, string.Empty);
        
        if (!string.IsNullOrEmpty(savedPath))
        {
            _boostAction.ApplyBindingOverride(savedPath);
            sso.AccelerationBindingPath = savedPath;
        }
        else if (!string.IsNullOrEmpty(sso.AccelerationBindingPath))
        {
            _boostAction.ApplyBindingOverride(sso.AccelerationBindingPath);
        }
    }

    //保存する設定についてここに記述する
    private void SaveSettings()
    {
        // データの永続化
        if (!string.IsNullOrEmpty(sso.AccelerationBindingPath))
        {
            PlayerPrefs.SetString(ActionName + SaveKeySuffix, sso.AccelerationBindingPath);
        }
        
        // マウス感度などの保存（必要に応じて追加）
        // PlayerPrefs.SetFloat("MouseSensitivity", sso.MouseSensitivity);
        
        PlayerPrefs.Save();
        Debug.Log("Settings Saved!");
        settingView.settingPanel.SetActive(false);
    }

    private void UpdateView()
    {
        string displayName = _boostAction.GetBindingDisplayString();
        settingView.SetAccelerationKeyText(displayName);
    }

    private void StartRebinding()
    {
        settingView.ShowRebindOverlay(true);
        _boostAction.Disable();

        var rebindOperation = _boostAction.PerformInteractiveRebinding()
            .WithTargetBinding(0) // 常に最初のバインディング（Keyboard/Z等）をターゲットにする
            .WithControlsExcluding("<Mouse>/position")
            .WithControlsExcluding("<Mouse>/delta")
            .OnComplete(operation => FinishRebinding(operation))
            .OnCancel(operation => CancelRebinding(operation))
            .Start();
    }

    private void FinishRebinding(InputActionRebindingExtensions.RebindingOperation operation)
    {
        // 新しいパスを取得してSOに格納
        // WithTargetBinding(0) を指定しているため、インデックス0から取得
        string newOverridePath = _boostAction.bindings[0].overridePath;
        sso.AccelerationBindingPath = newOverridePath;

        CleanUpOperation(operation);
    }

    private void CancelRebinding(InputActionRebindingExtensions.RebindingOperation operation)
    {
        CleanUpOperation(operation);
    }

    private void CleanUpOperation(InputActionRebindingExtensions.RebindingOperation operation)
    {
        operation.Dispose();
        _boostAction.Enable();
        settingView.ShowRebindOverlay(false);
        UpdateView();
    }

    private void SensitivityValueHandler(float value)
    {
        sso.MouseSensitivity = value;
    }
}
