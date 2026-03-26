namespace Syacapachi.Contracts
{
    /// <summary>シーン遷移の抽象（DIP: SceneManager 直参照の置き換え用）</summary>
    public interface ISceneNavigator
    {
        void LoadHome();
        void LoadGame();
        void LoadSceneByName(string sceneName);
    }
}
