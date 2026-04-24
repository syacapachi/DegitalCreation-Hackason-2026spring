using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

public class HomeView : MonoBehaviour
{
    [SerializeField] private Button gameStartButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button toRankingButton;
    [SerializeField] private Button toInfoButton;
    [SerializeField] public GameObject settingPanel;
    [SerializeField] public GameObject rankingPanel;
    [SerializeField] public GameObject infoPanel;
    [SerializeField] public GameObject nextButton;
    [SerializeField] public GameObject beforeButton;
    [SerializeField] public Button closeButton;
    [SerializeField] public GameObject imageArea;

    

    public event Action OnStartClicked;
    public event Action OnSettingButtonClicked;
    public event Action OnToRankingButtonClicked;
    public event Action OnToInfoButtonClicked;
    public event Action OnNextButtonClicked;
    public event Action OnBeforeButtonClicked;
    public event Action OnCloseButtonClicked;

    void Start()
    {
        gameStartButton.onClick.AddListener(() => OnStartClicked?.Invoke());
        settingButton.onClick.AddListener(() => OnSettingButtonClicked?.Invoke());
        toRankingButton.onClick.AddListener(() => OnToRankingButtonClicked?.Invoke());
        toInfoButton.onClick.AddListener(()=>OnToInfoButtonClicked?.Invoke());
        nextButton.GetComponent<Button>().onClick.AddListener(() => OnNextButtonClicked?.Invoke());
        beforeButton.GetComponent<Button>().onClick.AddListener(() => OnBeforeButtonClicked?.Invoke());
        closeButton.onClick.AddListener(() => OnCloseButtonClicked?.Invoke());
    }
}
