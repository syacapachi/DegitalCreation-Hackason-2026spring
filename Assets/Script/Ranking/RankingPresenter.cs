using UnityEngine;

public class RankingPresenter : MonoBehaviour
{
    [SerializeField] private RankingView view;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        view.OnCloseButtonClicked += OnCloseButtonHandler;
    }

    private void OnCloseButtonHandler()
    {
        view.rankPanel.SetActive(false);
    }
}
