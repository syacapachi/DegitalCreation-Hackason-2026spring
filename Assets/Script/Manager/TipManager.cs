using System;
using Syacapachi.Controller;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
public class TipManager : MonoBehaviour
{
    [SerializeField] private PlayerInputReciever reciever;
    [SerializeField] private VoidEventSO OnCountdownEnd;
    [SerializeField] private GameObject tipText;
    [SerializeField] private GameObject panel;
    [SerializeField] private Transform playerPos;

    [SerializeField] private int interval; //プレイヤー位置からRespawnTipが呼び出される基準となる時間(ミリ秒単位)
    [SerializeField] private float approximationDistance; // 座標のずれがこれ以下ならrespawnTipを表示する
    private List<Vector3> posList;
    private bool hasFlied = false;
    private bool isTipFlyClosed = false; // flyTipとrespawnTipが重複表示されることを防ぐ
    private bool isFinished = false;
    private bool isJustStarted = false; // 開始直後のrespawnTipの表示を防ぐためのフラグ


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        reciever.BoostAction += HideTipFly;
        reciever.RespawnAction += HideTipRespawn;
        OnCountdownEnd.Register(OnCountdownEndHandler);
        ShowTipFly();
        PlayerPosRecorder();
    }

    private void ShowTipFly()
    {
        tipText.GetComponent<TextMeshProUGUI>().text = $"{reciever.boostAction.GetBindingDisplayString()}を押して飛ぶ/加速できます";
        panel.SetActive(true);
        tipText.SetActive(true);
        isTipFlyClosed = false;
    }

    private void ShowTipRespawn()
    {
        if (isTipFlyClosed)
        {
            tipText.GetComponent<TextMeshProUGUI>().text = $"{reciever.respawnAction.GetBindingDisplayString()}を押して初期位置に戻ります";
            panel.SetActive(true);
            tipText.SetActive(true);
        }
        else
        {
            Debug.Log("リスポーンTipが拒否されました");
        }
    }

    private void HideTipFly()
    {
        panel.SetActive(false);
        tipText.SetActive(false);
        isTipFlyClosed = true;
        isJustStarted = true;
    }

    private void HideTipRespawn()
    {
        panel.SetActive(false);
        tipText.SetActive(false);
    }

    private void OnCountdownEndHandler()
    {
        isFinished = true;
    }

    private async void PlayerPosRecorder()
    {
        Debug.Log("PlayerPosRecorder()が実行された");
        posList = new List<Vector3>();
        int i = 0;
        
        while(isFinished == false)
        {
            Debug.Log("while文にはいった");
            
            if (playerPos == null) break; // オブジェクト破棄時のエラー防止

            posList.Add(playerPos.position);
            
            await UniTask.Delay(interval);
            
            if(i > 0)
            {
                if(Vector3.Distance(posList[i - 1], posList[i]) < approximationDistance && isJustStarted == false)
                {
                    ShowTipRespawn();
                }else if(isJustStarted == true)
                {
                    isJustStarted = false;
                }
            }
            
            i++;
        }
    }
}
