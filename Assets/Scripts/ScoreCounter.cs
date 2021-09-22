using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Text ScoreText;
    [SerializeField] private Text PatternsCount;
    [SerializeField] private GameObject progressBarObject;
    private ProgressBarController _progressBar;
    public int score = 0;
    public int coins = 0;
    public int PatternNumber = 1;
    private float ProgressbarValue = 0;
    private float scoreSpeed;
    private float MaxProgressBarScore;

    void Start()
    {
        scoreSpeed = GameObject.Find("GameController").GetComponent<GameController>().scoreSpeed;
        MaxProgressBarScore = GameObject.Find("GameController").GetComponent<GameController>().patternScoreAdd;
        _progressBar = progressBarObject.GetComponent<ProgressBarController>();
    }

    // Update is called once per frame
    void Update()
    {
        ScoreText.text = "Score: " + score.ToString() + "\nMoney: " + coins.ToString();                   //Отображаем очки
        PatternsCount.text = PatternNumber.ToString();                                                    //Отображаем количество пройденых паттернов
        _progressBar.SetVal(ProgressbarValue / MaxProgressBarScore);
        if (ProgressbarValue > MaxProgressBarScore)
        {
            ProgressbarValue = 0;
            MaxProgressBarScore = GameObject.Find("GameController").GetComponent<GameController>().patternScoreAdd;
        }
    }

    IEnumerator Scorer()
    {
        while (true)
        {
            yield return new WaitForSeconds(scoreSpeed);
            score++;
            ProgressbarValue++;
        }
    }

    public void StartScorer()
    {
        StartCoroutine(Scorer());
    }

    public void StopScorer()
    {
        StopAllCoroutines();
    }
}