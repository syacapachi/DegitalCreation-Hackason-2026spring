using System;
using Unity.VisualScripting;
using UnityEngine;
using System.IO;

public class SettingManager : MonoBehaviour
{
    [SerializeField] private SettingSO sso;
    [SerializeField] private SettingData settingData;
    [SerializeField] private HomeView homeView;
    [SerializeField] private SettingView settingView;

    private void Awake()
    {
        homeView.OnSettingButtonClicked += RestoreSettingFromJson;
        settingView.OnSaveButton += SaveSettingToJson;
    }
    /// <summary>
    /// SettingSOの保存情報をJsonで記録する
    /// </summary>
    private void SaveSettingToJson()
    {
        try
        {
            settingData.MouseSensitivity = sso.MouseSensitivity;
            settingData.AccelerationBindingPath = sso.AccelerationBindingPath;
            string json = JsonUtility.ToJson(settingData);
            string path = Application.dataPath + "/savedata.json";

            using (StreamWriter writer = new StreamWriter(path, false))
            {
                writer.Write(json);
                writer.Flush();
            }
            Debug.Log("Settings saved successfully.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save settings: {e.Message}");
        }
    }

    /// <summary>
    /// JsonからSettingSOに値を反映させる
    /// </summary>
    private void RestoreSettingFromJson()
    {
        try
        {
            string path = Application.dataPath + "/savedata.json";
            if (!File.Exists(path))
            {
                Debug.LogWarning("Save file not found.");
                return;
            }

            string datastr = "";
            using (StreamReader reader = new StreamReader(path))
            {
                datastr = reader.ReadToEnd();
            }

            settingData = JsonUtility.FromJson<SettingData>(datastr);

            if (sso != null && settingData != null)
            {
                sso.MouseSensitivity = settingData.MouseSensitivity;
                sso.AccelerationBindingPath = settingData.AccelerationBindingPath;
                Debug.Log("Settings restored successfully.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to restore settings: {e.Message}");
        }
    }
}

[Serializable]
public class SettingData
{
    public float MouseSensitivity;
    public string AccelerationBindingPath;
}
