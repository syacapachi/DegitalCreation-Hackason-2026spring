using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class HomePresenter : MonoBehaviour
{
    [SerializeField] public HomeView homeView;
    [SerializeField] private SettingManager settingManager;
    [SerializeField] private List<Sprite> infoImages;
    private int currentTab = 0;
    void Start()
    {
        homeView.OnStartClicked += () => SceneManager.LoadScene("GameScene");
        homeView.OnSettingButtonClicked += OnSettingButtonClicked;
        homeView.OnToRankingButtonClicked += OnToRankingButtonHandler;
        homeView.OnToInfoButtonClicked += OnToInfoButtonHandler;
        homeView.OnNextButtonClicked += OnNextButtonClickedHandler;
        homeView.OnBeforeButtonClicked += OnBeforeButtonClickedHandler;
        homeView.OnCloseButtonClicked += OnCloseButtonClickedHandler;
    }

    private void OnCloseButtonClickedHandler()
    {
        homeView.infoPanel.SetActive(false);
        currentTab = 0;
    }

    private void OnToInfoButtonHandler()
    {
        homeView.infoPanel.SetActive(true);
        UpdateInfoPanelState();
    }

    private void OnBeforeButtonClickedHandler()
    {
        if (currentTab > 0)
        {
            currentTab--;
            UpdateInfoPanelState();
        }
    }

    private void OnNextButtonClickedHandler()
    {
        if (currentTab < infoImages.Count - 1)
        {
            currentTab++;
            UpdateInfoPanelState();
        }
    }

    private void UpdateInfoPanelState()
    {
        if (infoImages == null || infoImages.Count == 0) return;

        homeView.imageArea.GetComponent<Image>().sprite = infoImages[currentTab];
        
        homeView.beforeButton.SetActive(currentTab > 0);
        homeView.nextButton.SetActive(currentTab < infoImages.Count - 1);
    }

    private void OnSettingButtonClicked()
    {
        homeView.settingPanel.SetActive(true);
    }

    private void OnToRankingButtonHandler()
    {
        homeView.rankingPanel.SetActive(true);
    }
}
