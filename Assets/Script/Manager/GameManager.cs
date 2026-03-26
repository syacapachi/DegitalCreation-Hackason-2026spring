using Syacapachi.Attribute;
using Syacapachi.Contracts;
using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] UIPresenter uIPresenter;
    [SerializeField] private AudioManager audioManager;
    [Header("Game Settings")]
    [SerializeField] private float initialCountdown = 100f;
    private bool isCountingDown = false;
    private static WaitForSeconds wait025s = new WaitForSeconds(0.25f);
    public event Action OnCountdownStart;
    public event Action OnCountdownEnd;

    private IMusicPlayer Music => audioManager;
    private void Start()
    {
        GameStart();
        OnCountdownEnd += () =>
        {
            uIPresenter.SetResultPanelActive(true); //カウントダウン終了時の処理
        };

        OnCountdownStart += () =>
        {
            if (Music != null)
                Music.PlayBackgroundMusic();
            else
                Debug.LogWarning("[UIPresenter] audioManager is missing in the Inspector!");
        };
    }
    private void GameStart()
    {
        StartCountdown(initialCountdown); // シーン読み込み時に指定時間でカウントダウンを開始
    }
    /// <summary>
    /// カウントダウンを開始
    /// </summary>
    /// <param name="duration">秒数</param>
    private void StartCountdown(float duration)
    {
        if (isCountingDown) return;
        StartCoroutine(CountdownRoutine(duration));
    }
    private IEnumerator CountdownRoutine(float duration)
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
                uIPresenter.UpdateCountdownDisplay(currentSecond, duration);
            }

            yield return wait025s;
        }

        uIPresenter.UpdateCountdownDisplay(0, duration);
        OnCountdownEnd?.Invoke();
    }
    [Header("Debug")]
    [SerializeField] private float debugCountdownDuration = 10f;

    [OnInspectorButton]
    public void DebugStartCountdown()
    {
        StartCountdown(debugCountdownDuration);
    }
}
