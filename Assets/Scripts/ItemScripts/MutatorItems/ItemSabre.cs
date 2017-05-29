using UnityEngine;
using System.Collections;
using System;

public class ItemSabre : ItemController
{
    ///--------------------------------------
    /// Sabre
    ///     - increases damage by 1
    ///--------------------------------------
    
    protected override void Start ()
    {
        base.Start();
        SpawnItem();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void ItemEffect(Collider other)
    {
        ///------------------------------
        /// Remember attribute indexes:
        /// 0 - moveSpeed
        /// 1 - damage
        /// 2 - attackRate (attacks per second)
        /// 3 - range
        /// 4 - health
        /// 5 - maxHealth
        ///------------------------------

        int index = 1;
        float amount = 1;
        string text = "Sabre\nDamage +1";

        UpdatePlayerAttributes(other.gameObject.GetComponent<PlayerController>(), index, amount, text, Color.white, 400);

        Destroy(gameObject);
    }
}
