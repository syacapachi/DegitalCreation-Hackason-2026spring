using UnityEngine;

public class AuidoManager : MonoBehaviour
{
    [SerializeField] AudioSource backMusic;
    [SerializeField] AudioSource shutterSound;
    public void PlayBackgroudMusic()
    {
        backMusic.Play();
    }

    public void StopBackgroundMusic()
    {
        backMusic.Stop(); 
    }

    public void PlayShutterSound()
    {
        shutterSound.Play();
    }
}
