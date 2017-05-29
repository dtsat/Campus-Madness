using UnityEngine;
using System.Collections;

public class PoisonItem : ItemController
{
    ///--------------------------------------
    /// Poison
    ///     - decreases damage by 2
    ///--------------------------------------

    protected override void Start()
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
        float amount = -2;
        string text = "Poison\nDamage -2";

        UpdatePlayerAttributes(other.gameObject.GetComponent<PlayerController>(), index, amount, text, Color.red, 400);

        Destroy(gameObject);
    }
}
