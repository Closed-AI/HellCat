using UnityEngine;

[System.Serializable]
public class GameData
{
    private int _money;
    private int _curScore;
    private int _maxScore;

    public GameData(SaveSystem system)
    {
        _money = system.Money;
        _curScore = system.CurScore;
        _maxScore = system.MaxScore;
    }

    public int Money
    {
        get { return _money; }
    }

    public int CurScore
    {
        get { return _curScore; }
    }

    public int MaxScore
    {
        get { return _maxScore; }
    }
}