using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Syacapachi.Controller;
using System;
using Cysharp.Threading.Tasks;

public class BoostGuageController : MonoBehaviour
{
    [SerializeField] private Image guage;
    [SerializeField] private PlayerInputReciever reciever;

    private event Action OnGuageBeZero; 
    private void Awake()
    {
        guage.fillAmount = 1.0f;
        reciever.BoostAction += ReduceGuage;
        OnGuageBeZero += GuageBeZeroHandler;
    }

    private async void GuageBeZeroHandler()
    {
        // Debug.Log("Guage操作を開始します");
        guage.DOFillAmount(1f,(float)(reciever.waitCount/10));
        await UniTask.Delay(reciever.waitCount *100);
        guage.DOFade(1f,0f);
    }

    private void ReduceGuage()
    {
        guage.fillAmount = 0f;
        guage.DOFade(0.5f,0f);
        OnGuageBeZero?.Invoke();
    }
}
