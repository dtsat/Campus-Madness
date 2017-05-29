using UnityEngine;
using System.Collections;

public class EnemyRobot : EnemyController
{
	public GameObject firePunch;
	public Transform firePunchSpawnPoint;

	protected override void Start ()
    {
        base.Start();
	}
	
	public void spawnPunch()
	{
        GameObject firePunchObject = Instantiate (firePunch, firePunchSpawnPoint.position, firePunchSpawnPoint.rotation) as GameObject;
        PunchMovement tempPunch = firePunchObject.GetComponent<PunchMovement>();
		firePunchObject.transform.parent = firePunchSpawnPoint.transform;
        tempPunch.SetDamage(Random.Range(attributes[1] - 1, attributes[1] + 1));
		Destroy (firePunchObject, 0.5f);
	}

	void FixedUpdate () 
	{
        Move();

		if (InRange ())
			StartCoroutine(AttackSequence ());

		UpdateAnimation ();
	}
}