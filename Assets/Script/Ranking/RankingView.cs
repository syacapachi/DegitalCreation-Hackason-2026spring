using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankingView : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Button toRankingButton;
    [SerializeField] public GameObject rankPanel;
    [SerializeField] public GameObject rankingLine; // Original UI object to clone
    [SerializeField] public TextMeshProUGUI rankText;
    [SerializeField] public TextMeshProUGUI scoreText;
    [SerializeField] public TextMeshProUGUI playerNameText;
    [SerializeField] public Transform rankArea;

    public event Action OnCloseButtonClicked;

    void Start()
    {
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(() => OnCloseButtonClicked?.Invoke());
        }
    }

    // 既存の生成済みのランキングUIをクリアする（Viewの役割）
    public void ClearRankingArea()
    {
        foreach (Transform child in rankArea)
        {
            // 雛形のrankingLine自身は消さずに残す
            if (child.gameObject != rankingLine)
            {
                Destroy(child.gameObject);
            }
        }
        // コピー元となる雛形自体は非表示にしておく
        if (rankingLine != null) rankingLine.SetActive(false);
    }

    // UIを生成してテキストを挿入する（Viewの役割。元のプレハブやオブジェクトの仕組みを崩さないよう踏襲）
    public void AddRankingLine(string rank, string name, string score)
    {
        if (rankText != null) rankText.text = rank;
        if (playerNameText != null) playerNameText.text = name;
        if (scoreText != null) scoreText.text = score;
        
        // 親を明示的に指定し、worldPositionStays を false にすることで、
        // UIが遠くの座標に設定されたりスケールが潰れるのを防ぎます
        GameObject clone = Instantiate(rankingLine, rankArea, false);
        clone.transform.SetParent(rankArea, false); // 念のため
        clone.transform.localScale = Vector3.one;   // UIの表示崩れ防止
        clone.SetActive(true);
    }
}
