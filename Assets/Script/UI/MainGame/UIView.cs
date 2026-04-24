using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private GameObject batteryGuage;
    [SerializeField] public GameObject bonusPanel;
    [SerializeField] public TextMeshProUGUI bonusText;

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

    public void SetResultPanelActive(bool active)
    {
        if (resultPanel != null)
        {
            resultPanel.SetActive(active);
        }
    }

    void Start()
    {
        if (resultPanel == null)
        {
            Debug.LogWarning("UI/UIManagerのUIViewのresultPanel Inspectorを割り当ててください。");
        }
    }

    public void SetBatteryGauge(float fillAmount)
    {
        if (batteryGuage != null)
        {
            Image img = batteryGuage.GetComponent<Image>();
            if (img != null)
            {
                img.fillAmount = fillAmount;
            }
        }
    }
}
