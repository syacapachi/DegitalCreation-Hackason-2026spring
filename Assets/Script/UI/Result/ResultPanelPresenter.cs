using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultPanelPresenter : MonoBehaviour
{
    [Header("Models")]
    [SerializeField] private ResultPanelModel resultPanelModel;
    
    [Header("UI Views")]
    [SerializeField] public ResultPanelView resultPanelView;

    private void OnEnable()
    {
        ShowResults();
    }

    public void ShowResults()
    {
        if (resultPanelView == null || resultPanelModel == null) return;

        resultPanelView.ClearPhotos();

        var photos = resultPanelModel.GetPhotos();
        foreach (var photo in photos)
        {
            resultPanelView.AddPhoto(photo);
        }
    }

    void Start()
    {
        // ゲーム終了時にホーム画面に戻るボタンを押した際の処理
        resultPanelView.OnToHomeButtonClick += () => SceneManager.LoadScene("Home");
    }
}
