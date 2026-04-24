using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomePresenter : MonoBehaviour
{
    [SerializeField] public HomeView homeView;
    [SerializeField] private SettingManager settingManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        homeView.OnStartClicked += () => SceneManager.LoadScene("GameScene");
        homeView.OnSettingButtonClicked += OnSettingButtonClicked;
        homeView.OnToRankingButtonClicked += OnToRankingButtonHandler;
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
