using Syacapachi.Attribute;
using Syacapachi.Camera;
using UnityEngine;
using System.Collections.Generic;

public class ResultPanelModel : MonoBehaviour
{
    [SerializeField] private PhotoManager photoManager;
    [SerializeField] private CameraCapture cameraCapture;
    
    private int _totalScore = 0;

    private void Awake()
    {
        SubscribeEvent();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void InitInactiveModels()
    {
        var models = Object.FindObjectsByType<ResultPanelModel>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var model in models)
        {
            model.SubscribeEvent();
        }
    }

    private void SubscribeEvent()
    {
        if (cameraCapture != null)
        {
            cameraCapture.OnCaptureComplete -= AccumulateScore; // 重複登録防止
            cameraCapture.OnCaptureComplete += AccumulateScore;
        }
        else
        {
            Debug.LogWarning("[ResultPanelModel] CameraCapture is not assigned in Inspector.");
        }
    }

    private void OnDestroy()
    {
        if (cameraCapture != null)
        {
            cameraCapture.OnCaptureComplete -= AccumulateScore;
        }
    }

    /// <summary>
    /// カメラ撮影完了イベントが呼ばれるたびに、写真内のオブジェクト情報を元にスコアを加算
    /// </summary>
    private void AccumulateScore(CameraCapture.PhotoData data)
    {
        if (data == null || data.info == null) return;

        foreach (var info in data.info)
        {
            _totalScore += (int)info.GetScore();
        }
    }

    public List<Texture2D> GetPhotos()
    {
        return photoManager != null ? photoManager.GetPhotos() : new List<Texture2D>();
    }

    public string ScoreText
    {
        get { return _totalScore.ToString(); }
    }

    [Header("Debug")]
    [SerializeField] private int debugAddScoreAmount = 100;

    [OnInspectorButton]
    public void DebugAddScore()
    {
        _totalScore += debugAddScoreAmount;
        Debug.Log($"[Debug] デバッグでスコアを追加しました。現在の合計: {_totalScore}");
    }
}
