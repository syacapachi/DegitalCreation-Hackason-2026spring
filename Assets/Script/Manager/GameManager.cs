using Syacapachi.Attribute;
using Syacapachi.Contracts;
using System;
using System.Collections;
using System.Threading;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;
    [Header("Game Settings")]
    [SerializeField] private int initialCountdown = 100;
    private bool isCountingDown = false;
    private static WaitForSeconds wait025s = new WaitForSeconds(0.25f);
    [Header("Publish Events")]
    [SerializeField] VoidEventSO OnCountdownStart;
    [SerializeField] VoidEventSO OnCountdownEnd;
    [SerializeField] BurstProgressEvent countDownEvent;

    private IMusicPlayer Music => audioManager;
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
        Music.PlayBackgroundMusic();
        int lastSecond = -1;
        for (float timer = initialCountdown; 0f < timer; timer -= 0.25f)
        {
            // 一秒単位での変化を検知
            int currentSecond = Mathf.CeilToInt(timer);
            if (currentSecond != lastSecond)
            {
                lastSecond = currentSecond;
                countDownEvent.Invoke(new Syacapachi.Camera.CameraCapture.BurstProgress(currentSecond, duration));
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
