using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class HomeView : MonoBehaviour
{
    [SerializeField] private Button gameStartButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button toRankingButton;
    [SerializeField] public GameObject settingPanel;
    [SerializeField] public GameObject rankingPanel;

    public event Action OnStartClicked;
    public event Action OnSettingButtonClicked;
    public event Action OnToRankingButtonClicked;

    void Start()
    {
        gameStartButton.onClick.AddListener(() => OnStartClicked?.Invoke());
        settingButton.onClick.AddListener(() => OnSettingButtonClicked?.Invoke());
        toRankingButton.onClick.AddListener(() => OnToRankingButtonClicked?.Invoke());
    }
}
