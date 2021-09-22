using UnityEngine;

public class BonusObject : MonoBehaviour
{
    [SerializeField] private int _bonusType;
    [SerializeField] private int _duration;
    
    public BonusObject(int duration, int bonusType)
    {
        _duration  = duration;
        _bonusType = bonusType;
    }

    private void Start()
    {
        _duration = SaveSystem.Instance.Progress[_bonusType] + 5;
        Destroy(gameObject, _duration);
    }

    public int Duration
    { get => _duration; }

    public int BonusType
    { get => _bonusType; }
}
