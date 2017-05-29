using UnityEngine;
using UnityEngine.UI;

public class Marksman : PlayerController {

    [Header("Skills")]
    public GameObject arrow;
	public GameObject slowTrap;
	public GameObject rainOfArrow;
	public GameObject trapSmallExplosion;
    public Transform arrowSpawnPoint;
	public Transform specialSpawnPoint;

	[Header("Cooldowns")]
	public float trapCooldown;
	public float rainCooldown;
	public Text trapText;
	public Text rainText;

	private float trapTimer;
	private float rainTimer;

	private GameObject slowTrapObject;

    protected override void Start()
    {
        base.Start();
    }

    void FixedUpdate()
    {
		if (trapTimer > 0)
		{
			trapTimer -= Time.deltaTime;
			trapText.color = Color.red;
			trapText.text = (int)trapTimer + "";
		}
		if (trapTimer <= 0)
		{
			trapTimer = 0;
			trapText.text = "";
		}

		if (rainTimer > 0)
		{
			rainTimer -= Time.deltaTime;
			rainText.color = Color.red;
			rainText.text = (int)rainTimer + "";
		}
		if (rainTimer <= 0)
		{
			rainTimer = 0;
			rainText.text = "";
		}

        Move();
        Attack();
        Jump();
        UpdateAnimation();
    }

    public void SpawnArrow()
    {
		GameObject arrowObject = Instantiate(arrow, arrowSpawnPoint.position, arrowSpawnPoint.rotation) as GameObject;
        arrowObject.GetComponent<Rigidbody>().velocity = transform.right * 20;
		ArrowMovement tempArrow = arrowObject.GetComponent<ArrowMovement>();
		tempArrow.SetDamage(Random.Range(attributes[1] - 1, attributes[1] + 1));
        Destroy(arrowObject, 2.0f);
    }

	public void ResetSpecialState()
	{
		isSpecial = false;
		isIdle = true;
	}

    protected override void Attack()
    {
		if (GetActionEnabledValue () && !isInCorridor && !isInElevator)
		{
			if (Input.GetButton("Fire1") && !isBlocking && !isSpecial)
			{
				isAttacking = true;
				isIdle = false;
			}
			else
				isAttacking = false;

			if (Input.GetButtonDown("Fire2") && !isAttacking && !isSpecial && trapTimer == 0)
			{
				trapTimer = trapCooldown;

				isBlocking = true;
				isIdle = false;

				slowTrapObject = GameObject.FindWithTag ("SlowTrap");

				if (slowTrapObject == null)
				{
					slowTrapObject = Instantiate (slowTrap, new Vector3(arrowSpawnPoint.position.x + 1f, arrowSpawnPoint.position.y, arrowSpawnPoint.position.z), Quaternion.identity ) as GameObject;
					Physics.IgnoreCollision (transform.GetComponent<Collider> (), slowTrapObject.GetComponent<Collider> ());
					Invoke ("PopTrapExplosion", 5f);

				}	

				isBlocking = false;
				isIdle = true;
			}

			if (Input.GetButtonDown("Fire3") && !isAttacking && !isBlocking && rainTimer == 0)
			{
				rainTimer = rainCooldown;

				isSpecial = true;
				isIdle = false;

				GameObject specialArrowObject = Instantiate (rainOfArrow, specialSpawnPoint.position, specialSpawnPoint.rotation) as GameObject;
				specialArrowObject.GetComponent<Rigidbody> ().velocity = -transform.up * 10;
				RainOfArrowRanged tempRainArrow = specialArrowObject.GetComponent<RainOfArrowRanged>();
				tempRainArrow.SetDamage(attributes[1]);
				Destroy(specialArrowObject, 3);
			}
		}
    }

	void PopTrapExplosion()
	{
		if (slowTrapObject != null)
		{
			Destroy (slowTrapObject);
			GameObject explosion = Instantiate (trapSmallExplosion, slowTrapObject.GetComponent<Transform> ().position, slowTrapObject.GetComponent<Transform> ().rotation) as GameObject;
			Destroy (explosion, 1);
		}
	}

	public void ReduceSpecialCD()
	{
		if (rainTimer > 0)
			rainTimer -= 1;
	}
}
