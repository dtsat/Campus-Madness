using UnityEngine;
using System.Collections;

public class EnemyZombie : EnemyController
{
    public GameObject explosion;
    public ParticleSystem blood;

    private bool isZombieDead;
    private GameObject playerObject;
    private Color32[] red = new Color32[3];
    private Color defaultColor;

	private AudioSource[] sounds;

	protected override void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        defaultColor = transform.GetChild(0).GetComponent<SpriteRenderer>().color;
		sounds = GetComponents<AudioSource> ();
        red[0] = new Color32(255, 140, 140, 255);
        red[1] = new Color32(255, 70, 70, 255);
        red[2] = new Color32(255, 0, 0, 255);
        isZombieDead = false;
        base.Start();
	}

	void FixedUpdate()
	{
        if (!isZombieDead)
        {
            Move();

            if (InRange())
                Attack();
        }
	}

    protected override void Attack()
    {
        isZombieDead = true;
        StartCoroutine(Explosion());
    }

    IEnumerator Explosion()
    {
        for (int i = 0; i < red.Length; i++)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = red[i];
            yield return new WaitForSeconds(0.5f);
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = defaultColor;
            yield return new WaitForSeconds(0.1666f);
        }
		sounds [0].Play ();
		sounds [1].Play ();
        ParticleSystem tempparticles = (ParticleSystem)Instantiate(blood, transform.position, Quaternion.identity);
        Destroy(tempparticles, 3);
        GameObject tempExplosionObject = (GameObject)Instantiate(explosion, transform.position, Quaternion.identity);
        ZombieExplosion tempExplosion = tempExplosionObject.GetComponent<ZombieExplosion>();
        tempExplosion.SetDamage(attributes[1]);

        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        Destroy(transform.FindChild("HealthBarEnemy").gameObject);

        GetKilled();
    }
}