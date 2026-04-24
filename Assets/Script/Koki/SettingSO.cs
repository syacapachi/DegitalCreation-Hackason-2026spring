using UnityEngine;

[CreateAssetMenu(fileName = "SettingSO", menuName = "Scriptable Objects/SettingSO")]
public class SettingSO : ScriptableObject
{
    [SerializeField] private int bgmVolume;
    [SerializeField] private int seVolume;
    [SerializeField] private float brightness;
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private string accelerationBindingPath;
    
    public int BGMVolume {  get { return bgmVolume; } set { bgmVolume = value; } }
    public int SEVolume { get { return seVolume; } set { seVolume = value; } }
    public float Brightness {  get { return brightness; } set { brightness = value; } }
    public float MouseSensitivity { get { return mouseSensitivity; } set { mouseSensitivity = value; } }
    
    // 加速ボタンのバインディングパス
    public string AccelerationBindingPath { get { return accelerationBindingPath; } set { accelerationBindingPath = value; } }
}
