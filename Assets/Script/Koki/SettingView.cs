using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingView : MonoBehaviour
{
    [SerializeField] private Button saveSettingsButton;

    public event Action OnSaveButton; //セーブボタンが押されたら発行

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        saveSettingsButton.onClick.AddListener(()=>OnSaveButton?.Invoke());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
