using UnityEngine;
using Syacapachi.Camera;
using System;
using System.Collections;



public class UIPresenter : MonoBehaviour
{
    [SerializeField] private UIView uiView;
    [SerializeField] private CameraCapture cameraCapture;
    private UIModel _model;

    public event Action OnCountdownStart;
    public event Action OnCountdownEnd;

    private void Awake()
    {
        _model = new UIModel();
    }

    private void Start()
    {
        if (cameraCapture != null)
        {
            // === 暫定デバッグ: CameraCapture の位置を表示 ===
            string path = cameraCapture.name;
            Transform p = cameraCapture.transform.parent;
            while (p != null)
            {
                path = p.name + "/" + path;
                p = p.parent;
            }
            Debug.Log($"[DEBUG] CameraCapture is attached to: {path}");
            // ===========================================

            cameraCapture.OnBurstProgress += (current, total) => 
            {
                UpdateInfoText($"撮った枚数: {current} / {total}");
            };
        }
        else
        {
            Debug.LogWarning("[DEBUG] cameraCapture check: missing reference in UIPresenter.");
        }
    }


    public void UpdateCountdown(string text)
    {
        _model.CountdownText = text;
        uiView.SetCountdownText(text);
    }

    /// <summary>
    /// カウントダウンを開始
    /// </summary>
    /// <param name="duration">秒数</param>
    public void StartCountdown(float duration)
    {
        if (_model.IsCountingDown) return;
        StartCoroutine(CountdownRoutine(duration));
    }

    private IEnumerator CountdownRoutine(float duration)
    {
        _model.IsCountingDown = true;
        _model.RemainingTime = duration;
        OnCountdownStart?.Invoke();

        while (_model.RemainingTime > 0)
        {
            _model.RemainingTime -= Time.deltaTime;
            UpdateCountdownDisplay(_model.RemainingTime);
            yield return null;
        }

        _model.RemainingTime = 0;
        _model.IsCountingDown = false;
        UpdateCountdownDisplay(0);
        OnCountdownEnd?.Invoke();
    }

    private void UpdateCountdownDisplay(float time)
    {
        // 小数点以下を切り上げて表示
        UpdateCountdown(Mathf.CeilToInt(time).ToString());
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
