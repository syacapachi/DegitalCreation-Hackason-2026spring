using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomePresenter : MonoBehaviour
{
    [SerializeField] public HomeView homeView;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        homeView.OnStartClicked += () => SceneManager.LoadScene("GameScene");
        homeView.OnSettingButtonClicked += OnSettingButtonClicked; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnSettingButtonClicked()
    {
        homeView.settingPanel.SetActive(true);
    }
}
