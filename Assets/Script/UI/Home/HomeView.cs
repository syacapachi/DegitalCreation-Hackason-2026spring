using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class HomeView : MonoBehaviour
{
    [SerializeField] public Button gameStartButton;
    [SerializeField] public Button settingButton;
    [SerializeField] public GameObject settingPanel;
    public event Action OnStartClicked;
    public event Action OnSettingButtonClicked;


    void Start()
    {
        gameStartButton.onClick.AddListener(() => OnStartClicked?.Invoke());
        settingButton.onClick.AddListener(() => OnSettingButtonClicked?.Invoke());
    }
}
