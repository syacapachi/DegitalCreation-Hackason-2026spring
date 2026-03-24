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

    private float _remainingTime;
    public float RemainingTime
    {
        get => _remainingTime;
        set => _remainingTime = value;
    }

    private bool _isCountingDown;
    public bool IsCountingDown
    {
        get => _isCountingDown;
        set => _isCountingDown = value;
    }
}
