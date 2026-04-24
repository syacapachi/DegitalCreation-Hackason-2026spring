using Syacapachi.Attribute;
using Syacapachi.Contracts;
using Syacapachi.Controller;
using System;
using System.Collections;
using System.Threading;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private PlayerInputReciever playerInputReciever;
    [Header("Game Settings")]
    [SerializeField] private int initialCountdown = 100;
    private bool isCountingDown = false;
    private static WaitForSeconds wait025s = new WaitForSeconds(0.25f);
    [Header("Publish Events")]
    [SerializeField] VoidEventSO OnCountdownStart;
    [SerializeField] VoidEventSO OnCountdownEnd;
    [SerializeField] VoidEventSO OnFireEventStarted;
    [SerializeField] BurstProgressEvent countDownEvent;

    private IMusicPlayer Music => audioManager;

    private void OnEnable()
    {
        if (playerInputReciever != null)
        {
            Debug.Log($"[GameManager] {playerInputReciever.name} の BoostAction を購読しました");
            playerInputReciever.BoostAction += HandleFirstBoost;
        }
        else
        {
            Debug.LogWarning("[GameManager] playerInputReciever がアサインされていないため、ブースト時にBGMが再生されません。");
        }
    }

    private void OnDisable()
    {
        if (playerInputReciever != null)
        {
            playerInputReciever.BoostAction -= HandleFirstBoost;
        }
    }

    

    private void HandleFirstBoost()
    {
        // Debug.Log("[GameManager] 最初のブーストを検知。BGMを開始します。");
        Music.PlayBackgroundMusic();

        // 最初の1回のみ実行するため、自身をイベントから削除
        if (playerInputReciever != null)
        {
            playerInputReciever.BoostAction -= HandleFirstBoost;
        }
    }

    private void Start()
    {
        GameStart();
    }
    private void GameStart()
    {
        StartCountdown(initialCountdown); // シーン読み込み時に指定時間でカウントダウンを開始
    }
    /// <summary>
    /// カウントダウンを開始
    /// </summary>
    /// <param name="duration">秒数</param>
    private void StartCountdown(int duration)
    {
        if (isCountingDown) return;
        StartCoroutine(CountdownRoutine(duration));
    }
    private IEnumerator CountdownRoutine(int duration)
    {
        isCountingDown = true;
        OnCountdownStart?.Invoke();
        int lastSecond = -1;
        for (float timer = initialCountdown; 0f < timer; timer -= 0.25f)
        {
            // 一秒単位での変化を検知
            int currentSecond = Mathf.CeilToInt(timer);
            if (currentSecond != lastSecond)
            {
                lastSecond = currentSecond;
                countDownEvent.Invoke(new Syacapachi.Camera.CameraCapture.BurstProgress(currentSecond, duration));

                // 残り時間が60秒になったら火災イベントを発火
                if (currentSecond == 60)
                {
                    try
                    {
                        OnFireEventStarted?.Invoke();
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError($"火災イベントの実行中にエラーが発生しましたが、カウントダウンは継続します: {e}");
                    }
                }
            }

            yield return wait025s;
        }

        countDownEvent.Invoke(new Syacapachi.Camera.CameraCapture.BurstProgress(0, duration));
        OnCountdownEnd?.Invoke();
    }
    [Header("Debug")]
    [SerializeField] private int debugCountdownDuration = 10;

    [OnInspectorButton]
    public void DebugStartCountdown()
    {
        StartCountdown(debugCountdownDuration);
    }
}
