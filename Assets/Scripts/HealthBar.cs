using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Image hb;
    private float totalHealth = 0f;
    private float currHealth = 0f;

    // Start is called before the first frame update
    void Start()
    {
        hb = gameObject.transform.Find("Healthbar_fill").GetComponent<Image>();
        Debug.Log(hb);

    }

    public void UpdateHealth(float health)
    {
        
        if(totalHealth != 0f)
        {
            currHealth = health;
            hb.fillAmount = (100 * currHealth / totalHealth) / 100;
        }
    }

    public void SetTotalHealth(float health)
    {
        totalHealth = health;
        currHealth = health;
    }
}
