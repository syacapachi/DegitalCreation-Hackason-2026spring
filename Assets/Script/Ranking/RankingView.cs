using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankingView : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Button toRankingButton;
    [SerializeField] public GameObject rankPanel;
    [SerializeField] private GameObject rankingLine;
    [SerializeField] private TextMeshProUGUI rankText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI playerNameText;

    public event Action OnCloseButtonClicked;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        closeButton.onClick.AddListener(() => OnCloseButtonClicked?.Invoke());
    }

    // Update is called once per frame
    
}
