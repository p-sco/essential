using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CooldownTimer : MonoBehaviour
{
    private bool onCooldown;
    private float cooldownTime;
    private float initialTime;
    private float initialWidth;
    private TextMeshProUGUI cooldownText;
    private GameObject player;
    public GameObject coveringPanel;
    // Start is called before the first frame update
    void Start()
    {
        onCooldown = false;
        RectTransform rt = GameObject.Find("Image").GetComponent<RectTransform>();
        initialWidth = rt.rect.width;
        rt.sizeDelta = new Vector2(0, rt.rect.height);
        cooldownText = GetComponent<TextMeshProUGUI>();
        cooldownText.SetText("");
        player = GameObject.Find("/Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(onCooldown == true){
            int cooldownDisplay = (int)cooldownTime;
            RectTransform rt = GameObject.Find("Image").GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2((cooldownTime / initialTime * initialWidth), rt.rect.height);
            cooldownText.SetText(cooldownDisplay.ToString());
            cooldownTime -= Time.deltaTime;

            if (cooldownTime <= 0)
            {
                onCooldown = false;
                cooldownText.SetText("");
            }
        }
    }

    public void SetCooldown(float cooldown){
        onCooldown = true;
        cooldownTime = cooldown;
        initialTime = cooldown;
    }
}
