using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Syacapachi.Attribute;
using Syacapachi.Camera;
using Syacapachi.Contracts;
using Syacapachi.Manager;
using UnityEngine;
using UnityEngine.UI;


public class UIPresenter : MonoBehaviour
{
    [SerializeField] private UIView uiView;
    [SerializeField] private CameraCapture cameraCapture;
    [SerializeField] private PhotoManager photoManager;
    [SerializeField] private GameObject cameraFrame;
    [SerializeField] private GameObject blackScreen;

    private UIModelSetting _model;

    private ICaptureService Capture => cameraCapture;
    private IPhotoAlbum Album => photoManager;

    
    void Awake()
    {
        _model = new UIModelSetting();

        if (Capture != null)
        {
            Capture.OnShutter += () => ShutterAnimation().Forget();
        }
    }

    private void Start()
    {
        UpdatePhotoCountDisplay(); // 初期表示

        if (Capture != null)
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

            // 単発・連写問わず、撮影完了時に枚数表示を更新
            Capture.OnCaptureComplete += (data) => 
            {
                StartCoroutine(UpdatePhotoCountNextFrame());
            };

            Capture.OnBurstProgress += (current, total) => 
            {
                UpdateInfoText($"連写中: {current} / {total}");
            };
        }
        else
        {
            Debug.LogWarning("[DEBUG] cameraCapture check: missing reference in UIPresenter.");
        }
    }

    private IEnumerator UpdatePhotoCountNextFrame()
    {
        // PhotoManager 側に写真が追加（AddPhoto）されるのを待つため1フレーム遅延
        yield return new WaitForEndOfFrame();
        UpdatePhotoCountDisplay();
    }

    private void UpdatePhotoCountDisplay()
    {
        if (Album == null) return;
        
        int currentCount = Album.GetPhotos().Count;
        int maxCount = Album.MaxPhotos;
        int remaining = maxCount - currentCount;
        
        // 撮影枚数と残り枚数の2つの情報を表示
        UpdateInfoText($"残り枚数: {remaining}");
    }

    public void UpdateCountdown(string text)
    {
        _model.CountdownText = text;
        uiView.SetCountdownText(text);
    }

    private async UniTask ShutterAnimation()
    {
        ManagerLocator.Instance.AudioManager.PlayShutterSound();
        await FadeIn();
        blackScreen.GetComponent<Image>().DOFade(0f, 0.2f);
        cameraFrame.transform.DOScale(1f, 0.2f);
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
    }

    private async UniTask FadeIn()
    {
        blackScreen.GetComponent<Image>().DOFade(1f, 0.2f);
        cameraFrame.transform.DOScale(0.8f,0.2f);
        await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
    }

    

    

    public void UpdateCountdownDisplay(int currentSecond, float totalDuration)
    {
        // 小数点以下を切り上げた一秒単位の数値を表示
        UpdateCountdown(currentSecond.ToString());

        // バッテリーゲージを一秒単位で減らす (残り時間 / 制限時間)
        float fillAmount = (float)currentSecond / totalDuration;
        uiView.SetBatteryGauge(fillAmount);
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

    public void SetResultPanelActive(bool active)
    {
        _model.IsResultPanelActive = active;
        uiView.SetResultPanelActive(active);
    }

    public void ShowSettings() => SetSettingPanelActive(true);
    public void HideSettings() => SetSettingPanelActive(false);
    public void ShowInfo() => SetInfoPanelActive(true);
    public void HideInfo() => SetInfoPanelActive(false);
    public void ShowResult() => SetResultPanelActive(true);
    
    
}
