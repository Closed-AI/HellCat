using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarController : MonoBehaviour
{
    [SerializeField] private Image progressBar;
    private float val;

    // Start is called before the first frame update
    void Start()
    {
        val = 0;
    }

    // Update is called once per frame
    void Update()
    {
        progressBar.fillAmount = val;
    }

    public void SetVal(float value)
    {
        val = value;
    }
}
