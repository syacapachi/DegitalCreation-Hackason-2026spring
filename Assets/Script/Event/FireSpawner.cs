using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Syacapachi.Attribute;

public class FireSpawner : MonoBehaviour
{
    [SerializeField] private VoidEventSO OnFireEventStarted; // 火災イベント発生
    [SerializeField] private GameObject fireEffect;
    [SerializeField] private List<Vector3> firePosLists; // 火災が発生する場所の座標をまとめたリスト

    void Awake()
    {
        OnFireEventStarted.Register(SpawnFire);
    }

    private void SpawnFire()
    {
        if (firePosLists == null || firePosLists.Count == 0) return;
        
        int i = Random.Range(0, firePosLists.Count);
        fireEffect.transform.position = firePosLists[i];
        fireEffect.SetActive(true);
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
}
