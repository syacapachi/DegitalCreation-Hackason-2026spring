using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TutorialView : MonoBehaviour
{
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private Button skipButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private TextMeshProUGUI tipText;

    public event Action OnSkipClicked;
    public event Action OnBackClicked;
    public event Action OnNextClicked;

    void Start()
    {
        skipButton?.onClick.AddListener(() => OnSkipClicked?.Invoke());
        backButton?.onClick.AddListener(() => OnBackClicked?.Invoke());
        nextButton?.onClick.AddListener(() => OnNextClicked?.Invoke());
    }

    public void SetTipText(string text)
    {
        if (tipText != null)
        {
            tipText.text = text;
        }
    }

    public void Show()
    {
        tutorialPanel.SetActive(true);
        tutorialPanel.transform.localScale = Vector3.one * 0.3f;
        tutorialPanel.transform.DOScale(1f, 0.5f);
    }

    public void Hide()
    {
        tutorialPanel.SetActive(false);
    }

    public void SetBackButtonActive(bool active)
    {
        backButton.gameObject.SetActive(active);
    }

    public void SetNextButtonActive(bool active)
    {
        nextButton.gameObject.SetActive(active);
    }
}
