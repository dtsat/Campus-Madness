using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FloatingText : MonoBehaviour
{
    private bool isDamage;
    private byte fadeCountdown;
    private int defaultSize;
    private Text textObject;
    private Color defaultColor;

    void Start()
    {
        textObject = GetComponent<Text>();
        fadeCountdown = 125;
        StartCoroutine(MoveText());
    }

    IEnumerator MoveText()
    {
        float yTrans = 2;
        float xTrans = Random.Range(-0.2f, 0.21f);
        Vector3 translation;

        for (int i = 1; i < 126; i++)
        {
            if (isDamage)
                yTrans -= 0.04f;
            else
                yTrans = 1;

            translation = new Vector3(xTrans, yTrans, 0);

            transform.Translate(translation * Time.deltaTime * 75, Space.World);

            if (fadeCountdown > 100)
                textObject.color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, defaultColor.a);
            else
                textObject.color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, ((float)fadeCountdown / 100));

            textObject.fontSize = defaultSize;

            if (fadeCountdown <= 50 && defaultSize > 0)
                defaultSize -= 2;

            fadeCountdown--;

            if (textObject.fontSize <= 0)
                Destroy(gameObject);

            yield return new WaitForSeconds(0.01f);
        }
    }

    public void SetType(bool isDamage)
    {
        this.isDamage = isDamage;
    }

    public void ChangeColor(Color color)
    {
        defaultColor = color;
    }

    public void ChangeSize(int size)
    {
        defaultSize = size;
    }
}