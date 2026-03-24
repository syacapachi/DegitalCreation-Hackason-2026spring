public class UIModel
{
    private string _countdownText;
    private string _infoText;
    private bool _isSettingPanelActive;
    private bool _isInfoPanelActive;

    public string CountdownText
    {
        get => _countdownText;
        set => _countdownText = value;
    }

    public string InfoText
    {
        get => _infoText;
        set => _infoText = value;
    }

    public bool IsSettingPanelActive
    {
        get => _isSettingPanelActive;
        set => _isSettingPanelActive = value;
    }

    public bool IsInfoPanelActive
    {
        get => _isInfoPanelActive;
        set => _isInfoPanelActive = value;
    }
}
