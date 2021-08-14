using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MetaUI : MonoBehaviour
{
    [SerializeField] private Text _money;
    [SerializeField] private Text _curScore;
    [SerializeField] private Text _maxScore;

    // Start is called before the first frame update
    void Start()
    {
        _money.text    = "Money: "    + SaveSystem.instance.Money.ToString();
        _curScore.text = "CurScore: " + SaveSystem.instance.CurScore.ToString();
        _maxScore.text = "MaxScore: " + SaveSystem.instance.MaxScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
