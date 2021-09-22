using UnityEngine;
using UnityEngine.UI;

public class DashButton : MonoBehaviour
{

    public float cooldown;
    public float cooldownConst;
    public bool isCooldown;
    private Image dashImage;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        dashImage = GetComponent<Image>();
        player = GameObject.Find("Player");
        cooldownConst = player.GetComponent<PlayerController>().dashCooldown;
        isCooldown = true;
    }

    // Update is called once per frame
    void Update()
    {
        cooldown = player.GetComponent<PlayerController>().dashTime;
        if (isCooldown)
        {
            dashImage.fillAmount = cooldown / cooldownConst;
        }
    }
}
