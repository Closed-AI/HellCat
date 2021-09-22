using System;

[Serializable()]
public class GameData
{
    private int _money;
    private int _curScore;
    private int _maxScore;
    private int[] _progress;

    public GameData()
    {
        _money    = 0;
        _curScore = 0;
        _maxScore = 0;
        _progress = new int[3];

        for (int i = 0; i < 3; i++)
            _progress[i] = 1;
    }

    public int Money
    { get => _money; set => _money = value; }

    public int CurScore
    { get => _curScore; set => _curScore = value; }

    public int MaxScore
    { get => _maxScore; set => _maxScore = value; }
    
    public int[] Progress
    { get => _progress; set => _progress = value; }
}