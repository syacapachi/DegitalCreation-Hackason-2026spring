using Syacapachi.Contracts;
using UnityEngine;

public class AudioManager : MonoBehaviour, IMusicPlayer, IShutterFeedback
{
    [SerializeField] AudioSource backMusic;
    [SerializeField] AudioSource shutterSound;

    public void PlayBackgroundMusic()
    {
        backMusic.Play();
    }

    /// <summary>互換用（旧スペル）。新規コードは <see cref="PlayBackgroundMusic"/> を使用。</summary>
    public void PlayBackgroudMusic() => PlayBackgroundMusic();

    public void StopBackgroundMusic()
    {
        backMusic.Stop(); 
    }

    public void PlayShutterSound()
    {
        shutterSound.Play();
    }
}
