using UnityEngine;
using System.Collections;

public class SlimeBoss : MonoBehaviour {


	private Rigidbody rb;
	private GameObject player;
	private Animator animator;
	public NavMeshAgent navMeshAgent;
	public bool isIdle, isMoving, isSpray, isJump;
	public GameObject slimeSpray;
	public Transform spraySpawn;
	public float speed;
	private int phase;

	private AudioSource[] sounds;

	private BossFight bossfightScript;

	void Start () 
	{
		rb = GetComponentInChildren<Rigidbody> ();
		player = GameObject.FindGameObjectWithTag("Player");
		sounds = GetComponents<AudioSource> ();
		animator = GetComponentInChildren<Animator>();
		navMeshAgent = GetComponent<NavMeshAgent>();
		isIdle = true;
		isMoving = false;
		isSpray = false;
		speed = 1.5f;
		phase = 4;
		bossfightScript = GameObject.Find ("BossSpawns").GetComponent<BossFight> ();
		transform.Translate (Vector3.up * 15f);
	}

	public void resetState()
	{
		isIdle = true;
		isSpray = false;
		isMoving = false;
		isJump = false;
	}

	public IEnumerator spray()
	{
		navMeshAgent.speed = 0f;
		isSpray = true;
		isJump = false;
		yield return new WaitForSeconds (2f);
		GameObject spray = Instantiate (slimeSpray, spraySpawn.position, slimeSpray.transform.rotation) as GameObject;
		sounds [0].Play ();
		sounds [1].Play ();
		Destroy (spray.gameObject, 5f);
		StartCoroutine (shrink ());
		StartCoroutine (bossfightScript.SpawnSlimes ());
		phase--;
		if (phase <= 1) 
		{
			for (int i = 0; i < 10; i++) 
			{
				transform.localScale = new Vector3 (transform.localScale.x - 0.02f, transform.localScale.y - 0.02f, transform.localScale.z - 0.02f);
				yield return new WaitForSeconds (0.15f);
			}
			Destroy (gameObject, 5f);
			bossfightScript.endFight ();
		}
	}

	IEnumerator shrink()
	{
		for (int i = 0; i < 10; i++) 
		{
			transform.localScale = new Vector3 (transform.localScale.x - 0.02f, transform.localScale.y - 0.02f, transform.localScale.z - 0.02f);
			yield return new WaitForSeconds (0.15f);
		}
		speed *= 2f;
	}

	void move()
	{
		isMoving = true;
		isIdle = false;
		if (player == null)
			return;
		navMeshAgent.destination = player.transform.position;
	}


	public IEnumerator jump()
	{
		yield return new WaitForSeconds (8f);
		isJump = true;
	}

	void FixedUpdate () 
	{
		move ();
	}

	void updateAnimation()
	{
		animator.SetBool("isIdle", isIdle);
		animator.SetBool("isMoving", isMoving);
		animator.SetBool("isSpray", isSpray);
		animator.SetBool("isJump", isJump);
	}

	void Update()
	{
		updateAnimation ();
	}
}
