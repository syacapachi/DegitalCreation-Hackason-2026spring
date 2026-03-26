namespace Syacapachi.Contracts
{
    /// <summary>BGM の再生制御（ISP: Presenter はこの能力だけに依存する）</summary>
    public interface IMusicPlayer
    {
        void PlayBackgroundMusic();
        void StopBackgroundMusic();
    }
}
