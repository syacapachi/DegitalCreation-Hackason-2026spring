using System.Collections.Generic;
using UnityEngine;

public class TutorialPresenter : MonoBehaviour
{
    [SerializeField] private List<string> tutorialList;
    [SerializeField] private TutorialView tutorialView;

    private TutorialModel _model;

    void Start()
    {
        _model = new TutorialModel(tutorialList);
        
        tutorialView.OnSkipClicked += CloseWindow;
        tutorialView.OnNextClicked += MoveNext;
        tutorialView.OnBackClicked += MoveBack;

        UpdateView();
        tutorialView.Show();
    }

    private void MoveNext()
    {
        _model.Next();
        UpdateView();
    }

    private void MoveBack()
    {
        _model.Back();
        UpdateView();
    }

    private void UpdateView()
    {
        tutorialView.SetTipText(_model.GetCurrentTip());
        tutorialView.SetBackButtonActive(!_model.IsStart());
        tutorialView.SetNextButtonActive(!_model.IsEnd());
    }

    public void CloseWindow()
    {
        tutorialView.Hide();
    }
}
