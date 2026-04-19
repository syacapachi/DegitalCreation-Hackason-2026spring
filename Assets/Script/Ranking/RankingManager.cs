using UnityEngine;
using System.IO;

public class RankingManager : MonoBehaviour
{
    [SerializeField] private RankingData data;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    private void SaveRanking()
    {

    }

    private void LoadRanking()
    {

    }
}
