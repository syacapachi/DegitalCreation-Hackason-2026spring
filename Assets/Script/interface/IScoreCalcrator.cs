using Syacapachi.Utils;

namespace Syacapachi.Contracts
{
    public interface IScoreCalcrator
    {
        public float CalcrateScore(PhotoAnalyzer.PhotoObjectInfo info);
    }
}