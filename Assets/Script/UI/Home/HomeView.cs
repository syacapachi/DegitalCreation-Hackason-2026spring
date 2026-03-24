using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class HomeView : MonoBehaviour
{
    [SerializeField] public Button gameStartButton;
    public event Action OnStartClicked;

    void Start()
    {
        gameStartButton.onClick.AddListener(() => OnStartClicked?.Invoke());
    }
}
