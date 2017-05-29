using UnityEngine;
using System.Collections;

public class ZombieExplosion : MonoBehaviour
{
    private float damage;
    private float lifeTime;
    private Transform player;

    void Start()
    {
        lifeTime = 0;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
		if (player == null)
			return;

		transform.Translate((player.position - transform.position) * Time.deltaTime * 0.1f);

        lifeTime += Time.deltaTime;

        if (lifeTime >= 1.5f)
            Destroy(gameObject);
    }

	void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController tempPlayer = other.gameObject.GetComponent<PlayerController>();
            GetComponent<Collider>().enabled = false;
            tempPlayer.getHit(damage);
        }
    }

    public void SetDamage(float dmg)
    {
        damage = dmg;
    }
}
