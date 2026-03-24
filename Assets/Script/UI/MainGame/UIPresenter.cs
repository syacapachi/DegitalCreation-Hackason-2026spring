using UnityEngine;

public class UIPresenter : MonoBehaviour
{
    [SerializeField] private UIView uiView;
    private UIModel _model;

    private void Awake()
    {
        _model = new UIModel();
    }

    public void UpdateCountdown(string text)
    {
        _model.CountdownText = text;
        uiView.SetCountdownText(text);
    }

    public void UpdateInfoText(string text)
    {
        _model.InfoText = text;
        uiView.SetInfoText(text);
    }

    public void SetSettingPanelActive(bool active)
    {
        _model.IsSettingPanelActive = active;
        uiView.SetSettingPanelActive(active);
    }

    public void SetInfoPanelActive(bool active)
    {
        _model.IsInfoPanelActive = active;
        uiView.SetInfoPanelActive(active);
    }

    public void ShowSettings() => SetSettingPanelActive(true);
    public void HideSettings() => SetSettingPanelActive(false);
    public void ShowInfo() => SetInfoPanelActive(true);
    public void HideInfo() => SetInfoPanelActive(false);
}
