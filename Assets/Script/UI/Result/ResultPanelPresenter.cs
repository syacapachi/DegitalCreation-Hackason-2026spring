using UnityEngine;

public class ResultPanelPresenter : MonoBehaviour
{
    [Header("Models")]
    [SerializeField] private ResultPanelModel resultPanelModel;
    
    [Header("UI Views")]
    [SerializeField] public ResultPanelView resultPanelView;

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
}
