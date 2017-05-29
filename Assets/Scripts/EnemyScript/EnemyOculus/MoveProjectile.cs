using UnityEngine;
using System.Collections;

public class MoveProjectile : MonoBehaviour
{
    private Vector3 direction;
    private float damage;
    private float lifeTime;
	
	void Update ()
    {
        transform.Translate(direction.normalized * 15 * Time.deltaTime);
        lifeTime += Time.deltaTime;

        if (lifeTime >= 5)
            Destroy(gameObject);
	}

    public void SetDestination(Vector3 destination)
    {
        direction = (destination - transform.position);
    }

    public void SetDamate(float dmg)
    {
        damage = dmg;
    }

    public float GetDamage()
    {
        return damage;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy"))
            Destroy(gameObject);
    }
}
