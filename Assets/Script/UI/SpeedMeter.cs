using System;
using TMPro;
using UnityEngine;

public class SpeedMeter : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private TextMeshProUGUI heightText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        ShowSpeed();
    }

    private void ShowSpeed()
    {
        float height = (float)Math.Round(rb.linearVelocity.magnitude * 3.6f, 1, MidpointRounding.AwayFromZero);
        heightText.text = $"{height.ToString()}km/s";
    }
}
