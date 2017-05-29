using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour
{
    private Color defaultColor;

    void Start()
    {
        defaultColor = transform.GetChild(0).GetComponent<SpriteRenderer>().color;
    }

    public void UpdateHealth(float health, float fullHealth)
    {
        float ratio = (health / fullHealth) * 100;

        for (int i = 19; i >= 0; i--)
        {
            if (ratio >= (i * 5))
                transform.GetChild(i).GetComponent<SpriteRenderer>().color = defaultColor;
            else
                transform.GetChild(i).GetComponent<SpriteRenderer>().color = Color.gray;
        }
    }
}
