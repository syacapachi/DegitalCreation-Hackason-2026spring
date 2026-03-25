using UnityEngine;

public class SetttingPresenter : MonoBehaviour
{
    [SerializeField] private SettingView settingView;

    void Start()
    {
        settingView.OnSaveButton += SaveSettings;
    }

    //保存する設定についてここに記述する
    private void SaveSettings()
    {

    }
}
