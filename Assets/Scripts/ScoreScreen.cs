using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScreen : MonoBehaviour
{
    [Header("Number of points per coin")]
    [SerializeField] private int MoneyFromScore;             // цена одной монеты в очках
    [Header("Price for one pattern")]
    [SerializeField] private int PatternPrice;              // цена одного паттерна в монетах

    [SerializeField] private Text ScoreText;
    [SerializeField] private Text WavesText;
    [SerializeField] private Text ScoreMoneyText;
    [SerializeField] private Text WavesMoneyText;
    [SerializeField] private Text TotalMoneyText;
    [SerializeField] private Text CoinsText;

    private int finalScore;
    private int CoinsCount;
    private int finalPatternsNumber;

    private AudioSource audioS;
    public AudioClip coinDropAudio;
    public AudioClip manyCoinsDropAudio;
    public AudioClip totalCoinsAudio;

    public GameObject gameUI;
    public GameObject restartButton;
    public GameObject menuButton;
    public GameObject skipButton;

    void Start()
    {
        finalScore = GameObject.Find("GameController").GetComponent<ScoreCounter>().score;
        CoinsCount = GameObject.Find("GameController").GetComponent<ScoreCounter>().coins;
        finalPatternsNumber = GameObject.Find("GameController").GetComponent<ScoreCounter>().PatternsNumber;
        audioS = GetComponent<AudioSource>();
        gameUI.SetActive(false);
    }


    IEnumerator Scoring()
    {
        yield return new WaitForSeconds(1);

        for (int i = 0; i <= CoinsCount; i++)                                      // прибавление монет за монеты 🤪
        {
            CoinsText.text = "Money: " + i.ToString();
            if (i != 0)
                audioS.PlayOneShot(coinDropAudio);
            if (audioS.pitch < 1.02)
                audioS.pitch += 0.00001f;
            if (CoinsCount != 0)
                yield return new WaitForSeconds(1f / CoinsCount);
        }
        audioS.pitch = 1;
        yield return new WaitForSeconds(1);

        for (int i = 0; i <= finalScore; i++)                                      // прибавление монет за очки
        {
            ScoreText.text = "Score: " + i.ToString();
            ScoreMoneyText.text = "Money: " + (i / MoneyFromScore).ToString();
            if ((i % MoneyFromScore == 0) && (i != 0))
            {
                audioS.PlayOneShot(coinDropAudio);
                if (audioS.pitch < 1.02)
                    audioS.pitch += 0.00001f;
            }    
        yield return null;
        }
        audioS.pitch = 1;
        yield return new WaitForSeconds(1);

        for (int i = 0; i < finalPatternsNumber; i++)                            // прибавление монет за паттерны
        {
            WavesText.text = "Waves: " + i.ToString();
            WavesMoneyText.text = "Money: " + (i * PatternPrice).ToString();
            if (i != 0)
                audioS.PlayOneShot(manyCoinsDropAudio);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1);
        TotalMoneyText.text = "Total money: " + ((finalScore / MoneyFromScore) + ((finalPatternsNumber - 1) * PatternPrice) + CoinsCount).ToString();
        audioS.PlayOneShot(totalCoinsAudio);
        yield return new WaitForSeconds(0.5f);
        restartButton.SetActive(true);
        menuButton.SetActive(true);
        skipButton.SetActive(false);
    }

    public void OnClickSkip()
    {
        skipButton.SetActive(false);
        StopAllCoroutines();
        CoinsText.text = "Money: " + CoinsCount.ToString();
        ScoreText.text = "Score: " + finalScore.ToString();
        ScoreMoneyText.text = "Money: " + (finalScore / MoneyFromScore).ToString();
        WavesText.text = "Waves: " + (finalPatternsNumber - 1).ToString();
        WavesMoneyText.text = "Money: " + ((finalPatternsNumber - 1) * PatternPrice).ToString();
        TotalMoneyText.text = "Total money: " + ((finalScore / MoneyFromScore) + ((finalPatternsNumber - 1) * PatternPrice) + CoinsCount).ToString();
        audioS.PlayOneShot(totalCoinsAudio);

        //---------------------------------------------------------
        SaveSystem.instance.AddMoney((finalScore / MoneyFromScore) + (finalPatternsNumber * PatternPrice));
        SaveSystem.instance.UpdateScore(finalScore);
        //---------------------------------------------------------

        restartButton.SetActive(true);
        menuButton.SetActive(true);
    }
}
