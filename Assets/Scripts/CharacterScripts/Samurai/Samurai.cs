using UnityEngine;
using UnityEngine.UI;

public class Samurai : PlayerController {

    [Header("Skills")]
    public GameObject kunai;
	public GameObject vanishSmoke;
	public GameObject specialSlash;
    public Transform kunaiSpawnPoint;
	public Transform specialSpawnPoint;

	[Header("Cooldowns")]
	public float smokeCooldown;
	public float slashCooldown;
	public Text smokeText;
	public Text slashText;

	private float smokeTimer;
	private float slashTimer;
	private GameObject specialSlashObject;

    protected override void Start()
    {
        base.Start();
    }

    void FixedUpdate()
    {
		if (smokeTimer > 0)
		{
			smokeTimer -= Time.deltaTime;
			smokeText.color = Color.red;
			smokeText.text = (int)smokeTimer + "";
		}
		if (smokeTimer <= 0)
		{
			smokeTimer = 0;
			smokeText.text = "";
		}

		if (slashTimer > 0)
		{
			slashTimer -= Time.deltaTime;
			slashText.color = Color.red;
			slashText.text = (int)slashTimer + "";
		}
		if (slashTimer <= 0)
		{
			slashTimer = 0;
			slashText.text =  "";
		}
		
        Move();
        Attack();
        Jump();
        UpdateAnimation();
    }

    public void SpawnKunai()
    {
		GameObject kunaiOject = Instantiate(kunai, kunaiSpawnPoint.position - new Vector3 (0.0f, 2.5f, 0.0f), kunaiSpawnPoint.rotation) as GameObject;
		kunaiOject.GetComponent<Rigidbody>().velocity = transform.right * 10;
		KunaiMovement tempKunai = kunaiOject.GetComponent<KunaiMovement>();
		tempKunai.SetDamage(Random.Range(attributes[1] - 1, attributes[1] + 1));
		Destroy(kunaiOject, 0.5f);
    }

	public void ResetSpecialState()
	{
		isSpecial = false;
		isIdle = true;

		GameObject[] arrayEnemies = GameObject.FindGameObjectsWithTag ("Enemy");
		for (int i = 0; i < arrayEnemies.Length; i++) 
		{
			arrayEnemies [i].GetComponent<Rigidbody> ().isKinematic = false;
			Physics.IgnoreCollision (GetComponent<Collider> (), arrayEnemies [i].GetComponent<Collider> (), false);
		}
	}

    protected override void Attack()
    {
		if (GetActionEnabledValue() && !isInCorridor && !isInElevator)
		{
			if (Input.GetButton("Fire1") && !isBlocking && !isSpecial)
			{
				isAttacking = true;
				isIdle = false;
			}
			else
				isAttacking = false;

			if (Input.GetButtonDown("Fire2") && !isAttacking && !isSpecial && smokeTimer == 0)
			{
				smokeTimer = smokeCooldown;

				isBlocking = true;
				isIdle = false;

				GameObject vanishSmokeObject = GameObject.FindWithTag ("VanishSmoke");

				vanishSmokeObject = Instantiate (vanishSmoke, transform.position + new Vector3 (0, 1, -1), transform.rotation) as GameObject;

				Physics.IgnoreCollision (transform.GetComponent<Collider> (), vanishSmokeObject.GetComponent<Collider> ());
				Destroy (vanishSmokeObject, 1);

				if (isGoingRight)
					transform.GetComponent<Rigidbody> ().AddExplosionForce (-1000, transform.position + new Vector3 (-0.1f, 0, 0), 0, 0, ForceMode.Impulse);
				else
					transform.GetComponent<Rigidbody> ().AddExplosionForce (-1000, transform.position + new Vector3 (0.1f, 0, 0), 0, 0, ForceMode.Impulse);

				isBlocking = false;
				isIdle = true;
			}

			if (Input.GetButtonDown("Fire3") && !isAttacking && !isBlocking && slashTimer == 0)
			{
				slashTimer = slashCooldown;

				isSpecial = true;
				isIdle = false;

				GameObject[] arrayEnemies = GameObject.FindGameObjectsWithTag ("Enemy");
				for (int i = 0; i < arrayEnemies.Length; i++)
				{
					arrayEnemies [i].GetComponent<Rigidbody> ().isKinematic = true;
					Physics.IgnoreCollision (GetComponent<Collider> (), arrayEnemies [i].GetComponent<Collider> ());
				}

				if (isGoingRight) {
					transform.GetComponent<Rigidbody> ().AddForce (new Vector3 (500f, 0, 0), ForceMode.Impulse);
					specialSlashObject = Instantiate (specialSlash, specialSpawnPoint.position + new Vector3 (3.5f, 0, 0), specialSpawnPoint.rotation) as GameObject;
				}
				else
				{
					transform.GetComponent<Rigidbody> ().AddForce (new Vector3 (-500f, 0, 0), ForceMode.Impulse);
					specialSlashObject = Instantiate (specialSlash, specialSpawnPoint.position + new Vector3 (-3.5f, 0, 0), specialSpawnPoint.rotation) as GameObject;
				}

				SlashSamurai tempSlash = specialSlashObject.GetComponent<SlashSamurai>();
				tempSlash.SetDamage(attributes[1]);
				Physics.IgnoreCollision (transform.GetComponent<Collider> (), specialSlashObject.GetComponent<Collider> ());
				Destroy(specialSlashObject, 0.5f);

			}
		}
    }

	public void ReduceSpecialCD()
	{
		if (slashTimer > 0)
			slashTimer -= 1;
	}
}
