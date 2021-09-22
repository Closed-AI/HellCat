using System;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    private Text[] _price;
    private Text[] _progress;
    private Text   _money;

    private void Start()
    {
        _price    = new Text[3];
        _progress = new Text[3];

        // поиск элементов UI (тексты с уровнями прокачки и стоимостями)
        // i инкременируется в диапазоне [ 1; 3 ] так как в иерархии родительского
        // объекта "UI" на позиции 0 находится задний фон "Background"

        // обработка прокачиваемых бонусов
        for (int i = 1; i <= 3; i++)
        {
            Text[] components = transform.GetChild(i).GetComponentsInChildren<Text>();
            _progress[i - 1] = components[0];
            _price[i - 1]    = components[1];
        }

        // обработка монет
        _money = transform.GetChild(4).GetComponentInChildren<Text>();

        UpdateUI();
    }

    void UpdateUI()
    {
        // работает для текстовых полей, переписать при появлении нормального UI
        for (int i = 0; i < 3; i++)
        {
            _price[i].text    = SaveSystem.Instance.Price[SaveSystem.Instance.Progress[i] - 1].ToString();
            _progress[i].text = SaveSystem.Instance.Progress[i].ToString() + "/10";
        }

        _money.text = "Money: " + SaveSystem.Instance.Money.ToString();
    }

    public void Buy(int id)
    {
        if (SaveSystem.Instance.Progress[id] < 10 &&
            SaveSystem.Instance.Money >= SaveSystem.Instance.Price[SaveSystem.Instance.Progress[id] - 1])
        {
            SaveSystem.Instance.Money -= SaveSystem.Instance.Price[SaveSystem.Instance.Progress[id] - 1];
            SaveSystem.Instance.Progress[id]++;
        }

        UpdateUI();
    }

    public void ResetProgress()
    {
        SaveSystem.Instance.ResetProgress();

        UpdateUI();
    }
}