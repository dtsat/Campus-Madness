using UnityEngine;
using System.Collections;

public class ItemKey : ItemController {

	///--------------------------------------
    /// Key
    ///     - add key into Key Collection of the chracter
    ///--------------------------------------
    
    protected override void Start ()
    {
        base.Start();
        SpawnItem();
    }
    
    protected override void Update ()
    {
        base.Update();
        Idle(Vector3.up, true);
    }

    protected override void ItemEffect(Collider other)
    {
        string keyType = transform.tag;
        UpdatePlayerKeyBag(other.GetComponent<PlayerController>(), keyType);

        int index = 0;
        float amount = 0;
        string text = "";
        Color usedColor = Color.white;

        if (keyType == "GoldKey")
        {
            text = "Gold Key\nUnlock Boss";
            usedColor = Color.yellow;
        }
        else
        {
            text = "Silver Key\nUnlock Chests";
            usedColor = Color.magenta;
        }

        UpdatePlayerAttributes(other.GetComponent<PlayerController>(), index, amount, text, usedColor, 400);

        Destroy(gameObject);
    }
}
