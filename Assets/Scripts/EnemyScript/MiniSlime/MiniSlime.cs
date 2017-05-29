using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MiniSlime : MonoBehaviour {

	///------------------------------
	/// Attribute indexes are as follow:
	/// 0 - moveSpeed
	/// 1 - damage
	/// 2 - attackRate (attacks per second)
	/// 3 - range
	/// 4 - health
	/// 5 - maxHealth
	///------------------------------

	[Header("Attributes")]
	public float[] attributes;

	[Header("Object links")]
	public GameObject[] items;
	public GameObject[] keys;
	public Text floatingText;

	private float currentPosY;
	private bool isGoingRight, isWalking, isHit, isDead, active;
	private GameObject player;
	private Rigidbody rigidBody;
	private NavMeshAgent navMeshAgent;
	private Vector3 randomDirection, direction;
	private Animator animator;
	private GameController gameController;
	private HealthBar healthBar;

	private GameObject[] playerSides;
	public GameObject meleeHit;
	public GameObject explo;
	public Transform slimespawn;

	private BossFight bossfightScript;
	private AudioSource[] sounds;


	protected virtual void Start ()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		playerSides = GameObject.FindGameObjectsWithTag ("PlayerSides");
		sounds = GetComponents<AudioSource> ();
		rigidBody = GetComponent<Rigidbody>();
		animator = GetComponentInChildren<Animator>();
		navMeshAgent = GetComponent<NavMeshAgent>();
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		//healthBar = transform.FindChild("HealthBarEnemy").GetComponent<HealthBar>();
		isGoingRight = true;
		isWalking = true;
		isHit = false;
		isDead = false;
		transform.Translate (Vector3.up * 15f);
		bossfightScript = GameObject.Find ("BossSpawns").GetComponent<BossFight> ();
	}



	protected void Move()
	{
		
			Vector3 playerPos = Vector3.zero;
			Vector3 playerRight = Vector3.zero;
			Vector3 playerLeft = Vector3.zero;

		if (player != null) {
			playerPos = player.transform.position;
			playerRight = playerSides [0].transform.position;
			playerLeft = playerSides [1].transform.position;
		} else
			return;

			if (playerPos.y <= transform.position.y + 5)
				direction = playerPos - transform.position;
			else
				direction = randomDirection - transform.position;

			if (isGoingRight && direction.x < 0) {
				isGoingRight = false;
			}

			if (!isGoingRight && direction.x > 0) {
				isGoingRight = true;
			}

			if (navMeshAgent.enabled && navMeshAgent != null) {
				navMeshAgent.speed = attributes [0];
				isWalking = true;

				if (player != null) {
					if (Vector3.Distance (transform.position, playerRight) > Vector3.Distance (transform.position, playerLeft))
						navMeshAgent.SetDestination (playerRight);
					else
						navMeshAgent.SetDestination (playerLeft);
				}
			}

			//healthBar.transform.rotation = Quaternion.identity;

			if (rigidBody.velocity.y < -2)
				rigidBody.velocity = new Vector3 (rigidBody.velocity.x, -2, rigidBody.velocity.z);

	}


	protected void slowMovementSpeed()
	{
		attributes[0] -= 5;
	}

	protected void originalMovementSpeed()
	{
		attributes[0] += 5;
	}


	public void getHit(float damage, float knockback)
	{
		float cameraToPayer = (Camera.main.transform.position - transform.position).magnitude;
		int fontSize = (int)(400 * (1 / cameraToPayer));
		navMeshAgent.enabled = false;
		isHit = true;
		isWalking = false;
		attributes[4] -= damage;
		//healthBar.UpdateHealth(attributes[4], attributes[5]);

		FloatingText(damage.ToString("0"), Color.yellow, fontSize);

		if(isGoingRight)
			rigidBody.AddForce (Vector3.left * knockback, ForceMode.Impulse);
		else
			rigidBody.AddForce (Vector3.right * knockback, ForceMode.Impulse);	    
	}

	public void resetHit()
	{
		isHit = false;
		navMeshAgent.enabled = true;
	}

	protected void UpdateAnimation()
	{
		animator.SetBool("isWalking", isWalking);
		animator.SetBool("isHit", isHit);
		animator.SetBool("isDead", isDead);
	}

	public  void getKilled()
	{
		navMeshAgent.enabled = false;
		sounds [0].Play ();
		isDead = true;
		GameObject exp = Instantiate (explo, slimespawn.position, explo.transform.rotation) as GameObject;
		bossfightScript.killSlime ();
		Destroy (exp, 3f);
	}

	void FloatingText(string text, Color color, int size)
	{
		float randomPosX = Random.Range(-0.5f, 0.51f);
		Vector3 offset = new Vector3(randomPosX, 1.5f, 0);
		Vector3 spawnPosition = transform.position + offset;
		Text tempText = (Text)Instantiate(floatingText, GameObject.FindGameObjectWithTag("Canvas").transform);
		FloatingText tempFloating = tempText.GetComponent<FloatingText>();
		tempText.transform.position = Camera.main.WorldToScreenPoint(spawnPosition);
		tempFloating.ChangeColor(color);
		tempFloating.ChangeSize(size);
		tempFloating.SetType(true);
		tempText.text = text;
	}

	void SpawnItems()
	{
		int itemGenerator = Random.Range(0, 100);

		if (itemGenerator >= 75 && itemGenerator < 90)
		{
			// 15% chance to generate an item
			GameObject itemGenerated = Instantiate(items[Random.Range(0, items.Length)], transform.position, Quaternion.identity) as GameObject;
			Debug.Log(itemGenerated.name);
		}
		else
		{
			// 75% chance to generate nothing
			Debug.Log("Nothing generated");
		}
	}

	public void destoySelf()
	{
		SpawnItems();

		Destroy (gameObject);
	}

	void FixedUpdate()
	{
		Move ();
		UpdateAnimation ();
	}

	void Update()
	{
		if (attributes [4] <= 0 && !isDead)
			getKilled ();
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag ("Player") || other.gameObject.CompareTag ("Enemy") || other.gameObject.CompareTag ("MiniSlime") || other.gameObject.CompareTag ("SlimeBoss"))
			return;

		if (other.gameObject.tag == "Punch" && other.gameObject.GetComponent<DestroyByContact>().active)
		{
			if (!isDead) {
				Destroy (other.gameObject);
				GameObject hitObject = Instantiate (meleeHit, other.transform.position, other.transform.rotation) as GameObject;
				Destroy (hitObject, 0.5f);
				getKilled ();
			}
		}
		if (other.gameObject.tag == "Arrow" && other.gameObject.GetComponent<DestroyByContact>().active) 
		{
			if (!isDead) {
				Destroy (other.gameObject);
				GameObject hitObject = Instantiate (meleeHit, other.transform.position, other.transform.rotation) as GameObject;
				Destroy (hitObject, 0.5f);
				getKilled ();
			}
		}

		if (other.gameObject.tag == "Kunai" && other.gameObject.GetComponent<DestroyByContact>().active)
		{
			if (!isDead) {
				Destroy (other.gameObject);
				GameObject hitObject = Instantiate (meleeHit, other.transform.position, other.transform.rotation) as GameObject;
				Destroy (hitObject, 0.5f);
				getKilled ();
			}
		}
	}
		
}
