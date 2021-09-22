using UnityEngine;
using UnityEngine.UI;

public class MetaUI : MonoBehaviour
{
    [SerializeField] private Text _money;
    [SerializeField] private Text _curScore;
    [SerializeField] private Text _maxScore;

    // Start is called before the first frame update
    private void Start()
    {
        _money.text    = "Money: "    + SaveSystem.Instance.Money.ToString();
        _curScore.text = "CurScore: " + SaveSystem.Instance.CurScore.ToString();
        _maxScore.text = "MaxScore: " + SaveSystem.Instance.MaxScore.ToString();
    }
}
