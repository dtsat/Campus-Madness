using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Brawler : PlayerController {
	
	[Header("Skills")]
	public GameObject firePunch;
	public GameObject shield;
	public GameObject tornado;
	public Transform firePunchSpawnPoint;

	[Header("Cooldowns")]
	public float shieldCooldown;
	public float tornadoCooldown;
	public Text shieldText;
	public Text tornadoText;

	private bool startedRegenerating;
	private bool startedShieldCooldown;
	private int shieldPercent;
	private float shieldTimer;
	private float tornadoTimer;

    private GameObject shieldObject = null;

    protected override void Start ()
    {
        base.Start();
		shieldPercent = 100;
		startedRegenerating = false;
		startedShieldCooldown = false;
	}
	
	void FixedUpdate ()
    {
		if (shieldTimer > 0 && startedShieldCooldown)
		{
			shieldTimer -= Time.deltaTime;
			shieldText.fontSize = 75;
			shieldText.color = Color.red;
			shieldText.text = (int)shieldTimer + "";
		}
		if (shieldTimer <= 0 && startedShieldCooldown)
		{
			shieldTimer = 0;
			startedShieldCooldown = false;
			shieldText.text = "";
		}

		if (tornadoTimer > 0)
		{
			tornadoTimer -= Time.deltaTime;
			tornadoText.color = Color.red;
			tornadoText.text = (int)tornadoTimer + "";
		}
		if (tornadoTimer <= 0)
		{
			tornadoTimer = 0;
			tornadoText.text = "";
		}
			
		if (shieldPercent < 100 && !startedRegenerating && !isBlocking)
		{
			startedRegenerating = true;
			StartCoroutine (IncrementShieldPercent ());
		}

        Move();
        Attack();
        Jump();
        UpdateAnimation();
    }

	IEnumerator Tornado()
	{
        GameObject tornadoObject = null;

        if (tornadoObject == null)
		{
			sounds [2].Play ();
			sounds [3].Play ();
			tornadoObject = Instantiate (tornado, transform.position + new Vector3 (0, 0.75f, 0), transform.rotation) as GameObject;
            TornadoMelee tempTornado = tornadoObject.GetComponent<TornadoMelee>();
            tempTornado.SetDamage(attributes[1]);
			tornadoObject.transform.parent = transform;
			yield return new WaitForSeconds (1f);
			isSpecial = false;
			Destroy (tornadoObject);
		}
	}

	public void SpawnPunch()
	{
        GameObject firePunchObject = Instantiate (firePunch, firePunchSpawnPoint.position, firePunchSpawnPoint.rotation) as GameObject;
        PunchMovement tempPunch = firePunchObject.GetComponent<PunchMovement>();
        tempPunch.SetDamage(Random.Range(attributes[1] - 2, attributes[1] + 2));
		firePunchObject.transform.parent = firePunchSpawnPoint.transform;
		Destroy (firePunchObject, 0.5f);
	}

	protected override void Attack ()
	{
		if (GetActionEnabledValue () && !isInCorridor && !isInElevator)
		{
			if (Input.GetButton ("Fire1") && !isBlocking && !isSpecial)
			{
				isAttacking = true;
				isIdle = false;
			}

			if (Input.GetButton ("Fire2") && !isAttacking && !isSpecial & shieldTimer == 0)
			{
				isBlocking = true;
				isIdle = false;

				if (shieldObject == null)
				{
					shieldObject = Instantiate (shield, transform.position, transform.rotation) as GameObject;
					shieldObject.transform.parent = transform;
				}
			}

			if (Input.GetButtonUp("Fire2"))
			{
				startedRegenerating = false;
				isBlocking = false;
				isIdle = true;
				Destroy (shieldObject);
			}

			if (Input.GetButtonDown ("Fire3") && !isAttacking && !isBlocking && tornadoTimer == 0)
			{
				tornadoTimer = tornadoCooldown;

				isSpecial = true;
				isIdle = false;

				StartCoroutine (Tornado ());
			}

			if (Input.anyKey == false)
			{
				isIdle = true;
				isBlocking = false;
				isAttacking = false;
				isSpecial = false;

				if (shieldObject != null)
					Destroy (shieldObject);
			}
		}
	}

	public void ResetAttack()
	{
		isAttacking = false;
		isIdle = true;
	}

	public void DecrementShieldPercent ()
	{
		shieldPercent -= 20;
		ShieldSizeCheck ();

		if (tornadoTimer > 0)
			tornadoTimer -= 1;
	}

	IEnumerator IncrementShieldPercent()
	{
		while (shieldPercent < 100 && !isBlocking && !isHit)
		{
			shieldPercent += 20;
			ShieldSizeCheck ();
			yield return new WaitForSeconds (2f);
		}
	}

	void ShieldSizeCheck()
	{
		switch (shieldPercent)
		{
		case 100:
			shieldText.text = "";
			break;
		case 80:
			shieldText.color = Color.blue;
			shieldText.fontSize = 45;
			shieldText.text = "80%";
			break;
		case 60:
			shieldText.color = Color.blue;
			shieldText.fontSize = 45;
			shieldText.text = "60%";
			break;
		case 40:
			shieldText.color = Color.blue;
			shieldText.fontSize = 45;
			shieldText.text = "40%";
			break;
		case 20:
			shieldText.color = Color.blue;
			shieldText.fontSize = 45;
			shieldText.text = "20%";
			break;
		case 0:
			shieldTimer = shieldCooldown;
			shieldPercent = 100;
			startedShieldCooldown = true;
			isBlocking = false;
			isIdle = true;
			Destroy (shieldObject);
			break;
		}

	}
}