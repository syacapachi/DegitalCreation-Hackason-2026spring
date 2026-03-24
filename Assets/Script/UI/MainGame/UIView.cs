using TMPro;
using UnityEngine;

public class UIView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private TextMeshProUGUI infoText;

    public void SetCountdownText(string text)
    {
        if (countdownText != null)
        {
            countdownText.text = text;
        }
    }

    public void SetInfoText(string text)
    {
        if (infoText != null)
        {
            infoText.text = text;
        }
    }

    public void SetSettingPanelActive(bool active)
    {
        if (settingPanel != null)
        {
            settingPanel.SetActive(active);
        }
    }

    public void SetInfoPanelActive(bool active)
    {
        if (infoPanel != null)
        {
            infoPanel.SetActive(active);
        }
    }
}
