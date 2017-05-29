using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class KeyBag : MonoBehaviour {

    public int goldKey;
    public int silverKey;
    // maybe more types ....

    [Header("Object Links")]
    public Text goldKeyText;
    public Text silverKeyText;

    // TODO: advanced version: 
    //     - bind key type with specific door


    void start () {
        goldKeyText.text = "0";
        silverKeyText.text = "0";
    }

    public int Getkey(string keyType)
    {
        switch (keyType)
        {
            case "GoldKey":
                return goldKey;
            case "SilverKey":
                return silverKey;
        }

        return 0;
    }

    public int KeyIncrease (string keyType)
    {
        switch (keyType) {
            case "GoldKey": 
                goldKey++;
                goldKeyText.text = goldKey.ToString();
                return goldKey;

            case "SilverKey": 
                silverKey++;
                silverKeyText.text = silverKey.ToString();
                return silverKey;
        }

        return 1;
    }

    public int KeyDecrease (string keyType)
    {
        switch (keyType) {
            case "GoldKey": 
                if (goldKey <= 0)
                    return -1;
                goldKey--;
                goldKeyText.text = goldKey.ToString();
                return goldKey;

            case "SilverKey": 
                if (silverKey <= 0)
                    return -1;
                silverKey--;
                silverKeyText.text = silverKey.ToString();
                return silverKey;
        }

        return -1;
    }
}
