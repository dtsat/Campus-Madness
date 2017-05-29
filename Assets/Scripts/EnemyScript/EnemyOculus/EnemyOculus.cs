using UnityEngine;
using System.Collections;

public class EnemyOculus : EnemyController
{
	public GameObject firePunch;
	public Transform firePunchSpawnPoint;

    private GameObject playerObject;
    private float attackCD;
    private bool isOculusDead;
	private AudioSource[] sounds;

	protected override void Start ()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
		sounds = GetComponents<AudioSource> ();
        base.Start();
        attackCD = 2;
        isOculusDead = false;
	}

	void FixedUpdate () 
	{
        Move();

        if (InRange())
        {
			if (playerObject == null)
				return;

			RaycastHit hitInfo = new RaycastHit();

            Physics.Raycast(transform.position, playerObject.transform.position - transform.position, out hitInfo);

            if (attackCD <= 0 && !isOculusDead && (hitInfo.collider.gameObject.CompareTag("Player") || hitInfo.collider.gameObject.CompareTag("Enemy")))
            {
                Attack();
            }
        }

        attackCD -= Time.deltaTime;
	}

    protected override void GetKilled()
    {
		sounds [1].Play ();
		base.GetKilled();

        isOculusDead = true;

        transform.localScale = transform.localScale - (Vector3.one * Time.deltaTime);
    }

    protected override void Attack()
    {
		if (!GetIsHit ()) {
			if (playerObject == null)
				return;
			GameObject tempProjectileObject = (GameObject)Instantiate (firePunch, firePunchSpawnPoint.position, Quaternion.identity);
			sounds [0].Play ();
			MoveProjectile tempProjectile = tempProjectileObject.GetComponent<MoveProjectile> ();
			tempProjectile.SetDestination (playerObject.transform.position);
			tempProjectile.SetDamate (attributes [1]);
			attackCD = 2;
		}
		else
		{
			attackCD += Time.deltaTime;
			resetHit ();
		}
            
    }
}