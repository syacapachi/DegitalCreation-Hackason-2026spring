using System.Collections.Generic;

public class TutorialModel
{
    private List<string> _tutorialList;
    private int _tipCount = 0;

    public TutorialModel(List<string> tutorialList)
    {
        _tutorialList = tutorialList;
    }

    public string GetCurrentTip()
    {
        if (_tipCount >= 0 && _tipCount < _tutorialList.Count)
        {
            return _tutorialList[_tipCount];
        }
        return string.Empty;
    }

    public void Next()
    {
        if (_tipCount < _tutorialList.Count - 1)
        {
            _tipCount++;
        }
    }

    public void Back()
    {
        if (_tipCount > 0)
        {
            _tipCount--;
        }
    }

    public int GetTipCount() => _tipCount;
    public int GetTotalTips() => _tutorialList.Count;
    public bool IsEnd() => _tipCount >= _tutorialList.Count - 1;
    public bool IsStart() => _tipCount == 0;
}
