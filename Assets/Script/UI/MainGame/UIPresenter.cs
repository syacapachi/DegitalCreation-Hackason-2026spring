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
using static Syacapachi.Camera.CameraCapture;


public class UIPresenter : MonoBehaviour
{
    [SerializeField] private UIView uiView;
    [SerializeField] private PhotoManager photoManager;
    [SerializeField] private GameObject cameraFrame;
    [SerializeField] private GameObject blackScreen;
    [Header("Subscribe Event")]
    [SerializeField] VoidEventSO OnShutter;
    [SerializeField] PhotoDataEvent OnCaptureComplete;
    [SerializeField] VoidEventSO OnCaptureFailed;
    [SerializeField] BurstProgressEvent OnBurstProgress;
    [SerializeField] VoidEventSO OnCountdownStart;
    [SerializeField] VoidEventSO OnCountdownEnd;
    [SerializeField] BurstProgressEvent countDownEvent;

    private UIModelSetting _model;

    private IPhotoAlbum Album => photoManager;

    
    void Awake()
    {
        _model = new UIModelSetting();

       
    }

    private void Start()
    {
        UpdatePhotoCountDisplay(); // 初期表示  
    }
    private void OnEnable()
    {
        OnShutter.Register(OnShuterHandle);
        // 単発・連写問わず、撮影完了時に枚数表示を更新
        OnCaptureComplete.Register(CaptureCompleteHandle);

        OnBurstProgress.Register(BurstProgressHandle);
        OnCountdownEnd.Register(()=>SetResultPanelActive(true)); //カウントダウン終了時の処理)
        countDownEvent.Register(t => UpdateCountdownDisplay(t.current,t.total));
    }
    private void OnDisable()
    {
        OnShutter.Unregister(OnShuterHandle);
        OnCaptureComplete.Unregister(CaptureCompleteHandle);
        OnBurstProgress.Unregister(BurstProgressHandle);
        OnCountdownEnd.Unregister(() => SetResultPanelActive(true));
        countDownEvent.Unregister(t => UpdateCountdownDisplay(t.current, t.total));
    }
    private void OnShuterHandle()
    {
        ShutterAnimation().Forget();
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

    private void BurstProgressHandle(BurstProgress progress)
    {
        UpdateInfoText($"連写中: {progress.current} / {progress.total}");
    }

    private void CaptureCompleteHandle(PhotoData data)
    {
        StartCoroutine(UpdatePhotoCountNextFrame());
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
