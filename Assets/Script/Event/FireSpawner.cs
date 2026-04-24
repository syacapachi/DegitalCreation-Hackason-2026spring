using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Syacapachi.Attribute;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

/// <summary>
/// 火災イベントを発生させるイベントに関連したスクリプト
/// </summary>
public class FireSpawner : MonoBehaviour
{
    
    [SerializeField] private GameObject fireEffect;
    [SerializeField] private GameObject messagePanel;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private List<Vector3> firePosLists; // 火災が発生する場所の座標をまとめたリスト
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Event")]
    [SerializeField] private VoidEventSO OnFireEventStarted; // 火災イベント発生
    [SerializeField] private VoidEventSO OnFireEventCaptured; // 火災イベントが撮影された



    void Awake()
    {
        OnFireEventStarted.Register(SpawnFire);
        OnFireEventCaptured.Register(VanishFire);
    }

    /// <summary>
    /// 火災エフェクトを表示させる
    /// </summary>
    private void SpawnFire()
    {
        if (firePosLists == null || firePosLists.Count == 0) return;
        
        int i = Random.Range(0, firePosLists.Count);
        fireEffect.transform.position = firePosLists[i];
        fireEffect.SetActive(true);
        ShowMessage();
    }

    /// <summary>
    /// イベント発生を知らせるメッセージを画面上に表示する
    /// </summary>
    private async void ShowMessage()
    {
        messageText.text = $"火災が発生したようだ！\n 写真に撮ってボーナスをゲットしよう！";
        canvasGroup.DOFade(0f, 0f);
        messagePanel.SetActive(true);
        canvasGroup.DOFade(0.9f, 0.5f);
        await UniTask.Delay(3000);
        canvasGroup.DOFade(0f, 0.5f);
        await UniTask.Delay(500);
        messagePanel.SetActive(false);
    }

    // デバッグ用
    [OnInspectorButton]
    public void DebugTriggerFireEvent()
    {
        // イベントそのものを発火させます
        // Playモード中であれば SpawnFire() が呼ばれます
        if (OnFireEventStarted != null)
        {
            OnFireEventStarted.Invoke();
        }
        else
        {
            // Eventが設定されていない場合は直接実行するなどのフォールバック
            SpawnFire();
        }
    }

    private void VanishFire()
    {
        fireEffect.SetActive(false);
    }
}
