using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashButton : MonoBehaviour
{

    public float cooldown;
    public float cooldownConst;
    public bool isCooldown;
    private Image dashImage;

    // Start is called before the first frame update
    void Start()
    {
        dashImage = GetComponent<Image>();
        cooldownConst = GameObject.Find("Player").GetComponent<PlayerController>().dashCooldown;
        isCooldown = true;
    }

    // Update is called once per frame
    void Update()
    {
        cooldown = GameObject.Find("Player").GetComponent<PlayerController>().dashTime;
        if (isCooldown)
        {
            dashImage.fillAmount = cooldown / cooldownConst;
        }
    }
}
