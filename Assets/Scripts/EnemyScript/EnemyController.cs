using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class EnemyController : MonoBehaviour
{
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
    private bool isGoingRight, isWalking, isAttacking, isHit, isDead;
    private GameObject player;
    private Rigidbody rigidBody;
    private NavMeshAgent navMeshAgent;
    private Vector3 randomDirection, direction;
	private Animator animator;
    private GameController gameController;
    private HealthBar healthBar;

	private GameObject[] playerSides;

	protected virtual void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
		playerSides = GameObject.FindGameObjectsWithTag ("PlayerSides");
        rigidBody = GetComponent<Rigidbody>();
		animator = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        healthBar = transform.FindChild("HealthBarEnemy").GetComponent<HealthBar>();
        isGoingRight = true;
		isWalking = true;
		isAttacking = false;
		isHit = false;
		isDead = false;
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

        if (isGoingRight && direction.x < 0)
        {
            isGoingRight = false;
            StartCoroutine(RotateEnemy());
        }

        if (!isGoingRight && direction.x > 0)
        {
            isGoingRight = true;
            StartCoroutine(RotateEnemy());
        }

		if (navMeshAgent.enabled && navMeshAgent != null)
        {
            if (!InRange())
            {
                navMeshAgent.speed = attributes[0];
                isWalking = true;
            }
            else
            {
                navMeshAgent.speed = 0;
                isWalking = false;
            }

            if (player != null)
            {
                if ((transform.position - playerRight).magnitude >= (transform.position - playerLeft).magnitude)
                    navMeshAgent.SetDestination(playerLeft);
                else
                    navMeshAgent.SetDestination(playerRight);
            }
		}

        if (rigidBody.velocity.y < -2)
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, -2, rigidBody.velocity.z);
    }

    IEnumerator RotateEnemy()
    {
        for (int i = 0; i < 18; i++)
        {
            transform.Rotate(new Vector3(0, 10, 0));
            yield return new WaitForSeconds(0.01f);
        }
    }

	protected void slowMovementSpeed()
	{
		attributes[0] -= 5;
	}

	protected void originalMovementSpeed()
	{
		attributes[0] += 5;
	}

    protected virtual void Attack()
    {
        if (!isDead)
            StartCoroutine(AttackSequence());
    }

	protected IEnumerator AttackSequence()
	{
		navMeshAgent.enabled = false;
		isAttacking = true;
		yield return new WaitForSeconds (1f);
		isAttacking = false;
		navMeshAgent.enabled = true;
	}

	public void getHit(float damage, float knockback)
	{
        float cameraToPayer = (Camera.main.transform.position - transform.position).magnitude;
        int fontSize = (int)(400 * (1 / cameraToPayer));
        isHit = true;
		isAttacking = false;
		isWalking = false;
		attributes[4] -= damage;
        healthBar.UpdateHealth(attributes[4], attributes[5]);

        FloatingText(damage.ToString("0"), Color.yellow, fontSize);

		if(isGoingRight)
			rigidBody.AddForce (Vector3.left * knockback, ForceMode.Impulse);
		else
			rigidBody.AddForce (Vector3.right * knockback, ForceMode.Impulse);  
	}

    protected bool GetIsHit()
    {
        return isHit;
    }

	public void resetHit()
	{
		isHit = false;
		navMeshAgent.enabled = true;
	}

	protected void UpdateAnimation()
	{
		animator.SetBool("isWalking", isWalking);
		animator.SetBool("isAttacking", isAttacking);
		animator.SetBool("isHit", isHit);
		animator.SetBool("isDead", isDead);
	}

	protected virtual void GetKilled()
	{
		navMeshAgent.enabled = false;
		isDead = true;

        if (isGoingRight)
            rigidBody.AddForce(Vector3.left * 3f);
        else
            rigidBody.AddForce(Vector3.right * 3f);

        StartCoroutine(DestroyObject());
	}

    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
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
        }
        else if (itemGenerator >= 90)
        {
            // 10% chance to generate a key
            GameObject keyGenerated = Instantiate(keys[Random.Range(0, keys.Length)], transform.position, Quaternion.identity) as GameObject;
        }
    }

	public void destoySelf()
	{
        SpawnItems();
       // gameController.IncreaseLevel();
        Destroy (gameObject);
    }

	protected bool InRange()
	{
		if (player == null)
			return false;

		Vector3 playerPos = player.transform.position;
        float distance = (transform.position - player.transform.position).magnitude;

        if (distance <= attributes[3])
            return true;

        return false;
	}

	void Update()
	{
		if (attributes [4] <= 0)
			GetKilled ();
	}

	public void SlowedText()
	{
		float cameraToPayer = (Camera.main.transform.position - transform.position).magnitude;
		int fontSize = (int)(400 * (1 / cameraToPayer));
		FloatingText ("Slowed", Color.cyan, fontSize);
	}
}