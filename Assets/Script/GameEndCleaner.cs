using System;
using UnityEngine;

/// <summary>
/// ゲーム終了時に不要なオブジェクトを非表示にして予期せぬ動作を防ぐクラス
/// </summary>
public class GameEndCleaner : MonoBehaviour
{
    [SerializeField] private GameObject photoCameraRoot;
    [SerializeField] private GameObject worldParent;

    [Header("Event")]
    [SerializeField] VoidEventSO OnCountDownEnd;

    private void OnEnable()
    {
        OnCountDownEnd?.Register(DestroyObject);
    }

    private void OnDisable()
    {
        OnCountDownEnd?.Unregister(DestroyObject);
    }

    /// <summary>
    /// 不要なオブジェクトを非表示にする
    /// </summary>
    private void DestroyObject()
    {
        photoCameraRoot.SetActive(false);
        worldParent.SetActive(false);
    }
}
