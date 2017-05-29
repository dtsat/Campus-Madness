using UnityEngine;
using System.Collections;
using System;

public class ItemPizza : ItemController
{
    ///--------------------------------------
    /// Sabre
    ///     - increases damage by 2
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

        int index = 4;
        float amount = 10;
        string text = "Pizza\nHealth +10";

        UpdatePlayerAttributes(other.gameObject.GetComponent<PlayerController>(), index, amount, text, Color.white, 400);

        Destroy(gameObject);
    }
}
