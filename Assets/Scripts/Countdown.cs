using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    // Start is called before the first frame update
    public void finish()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
