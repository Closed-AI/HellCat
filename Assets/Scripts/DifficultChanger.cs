using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class RULE_ENUM
{
    static public int BONUS  = 0;
    static public int AIM    = 1;
    static public int GLOBAL = 2;
    static public int COINS  = 3;
    static public int TRAP   = 4;
}

public class DifficultChanger : MonoBehaviour
{
    private GameController _controller;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<GameController>();
    }

    public void changeDifficult()
    {

    }
}
