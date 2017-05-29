using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class PlayerController : MonoBehaviour
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

    [Header("Object Links")]
    public Text floatingText;
    public Text healthText;
    public Text damageText;
    public Text speedText;

    protected bool isIdle, isWalking, isAttacking, isGoingRight, isGrounded,
        isBlocking, isSpecial, isFalling, isJumping, isHit, tempInvincible,
        actionsEnabled, isDead, isInCorridor, isInElevator;
    protected AudioSource[] sounds;

    private bool camLockZ;
    private Rigidbody rigidBody;
    private Animator animator;
    private KeyBag keybag;
    private HealthBar healthBar;

	private GameController gc;

    protected virtual void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        keybag = GetComponentInChildren<KeyBag>();
        sounds = GetComponents<AudioSource>();
		gc = GameObject.Find ("GameController").GetComponent<GameController> ();
        healthBar = transform.FindChild("HealthBarPlayer").GetComponent<HealthBar>();
        isGoingRight = true;
        isGrounded = true;
        isIdle = true;
        isWalking = false;
        isAttacking = false;
        isBlocking = false;
        isSpecial = false;
        isJumping = false;
        isFalling = false;
        isHit = false;
        camLockZ = true;
        actionsEnabled = false;
        isDead = false;
        tempInvincible = false;
		isInCorridor = false;
		isInElevator = false;
        attributes[4] = attributes[5];
        floatingText.text = "";
        UpdateTexts();
        rigidBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
            isGrounded = true;
        if (other.gameObject.CompareTag("SlimeBoss") && !tempInvincible)
            getHit(1f);
        if (other.gameObject.CompareTag("MiniSlime") && !isBlocking && !tempInvincible)
            getHit(1f);

    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
            isGrounded = true;
        if (other.gameObject.CompareTag("SlimeBoss") && !tempInvincible)
            getHit(5f);
        if (other.gameObject.CompareTag("MiniSlime") && !isBlocking && !tempInvincible)
        {
            getHit(3f);
            other.gameObject.GetComponent<MiniSlime>().getKilled();
        }

        if (other.gameObject.CompareTag("MiniSlime") && isBlocking && name == "Brawler")
        {
            GetComponent<Brawler>().DecrementShieldPercent();
            other.gameObject.GetComponent<MiniSlime>().getKilled();
        }

    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }

    protected void Move()
    {
        Vector3 cameraPos = Camera.main.transform.position;

        if (actionsEnabled)
        {
            float moveHor = Input.GetAxis("Horizontal");
            float moveVer = Input.GetAxis("Vertical");
            Vector3 movement = Vector3.zero;

            if (moveHor != 0 || moveVer != 0)
            {
                if (moveHor > 0 && !isGoingRight)
                {
                    isGoingRight = true;
                    StartCoroutine(RotatePlayer());
                }
                if (moveHor < 0 && isGoingRight)
                {
                    isGoingRight = false;
                    StartCoroutine(RotatePlayer());
                }

                isWalking = true;
                isIdle = false;
            }
            else
            {
                isIdle = true;
                isWalking = false;
            }

            movement = new Vector3(moveHor, 0, moveVer);

            if (!isBlocking)
                rigidBody.AddForce(movement * 100);

            rigidBody.velocity = new Vector3
                (
                   Mathf.Clamp(rigidBody.velocity.x, -attributes[0], attributes[0]),
                   Mathf.Clamp(rigidBody.velocity.y, -attributes[0], attributes[0]),
                   Mathf.Clamp(rigidBody.velocity.z, -attributes[0], attributes[0])
                );

            if (moveHor == 0)
                rigidBody.velocity = new Vector3(0, rigidBody.velocity.y, rigidBody.velocity.z);
            if (moveVer == 0)
                rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, 0);
        }

        if (camLockZ)
            cameraPos = new Vector3(transform.position.x, (transform.position.y + 5), Camera.main.transform.position.z);
        else
            cameraPos = new Vector3(transform.position.x, (transform.position.y + 5), (transform.position.z - 28.05f));

        Camera.main.transform.position = cameraPos;
    }

    protected virtual void Attack()
    {
        if (actionsEnabled)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                isAttacking = true;
                isIdle = false;
            }
            else
                isAttacking = false;

            if (Input.GetButtonDown("Fire2"))
            {
                isBlocking = true;
                isIdle = false;
            }
            else
                isBlocking = false;

            if (Input.GetButtonDown("Fire3"))
            {
                isSpecial = true;
                isIdle = false;
            }
            else
                isSpecial = false;

            if (Input.anyKey == false)
                isIdle = true;
        }
    }

    protected void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && actionsEnabled)
        {
            rigidBody.AddForce(new Vector3(0, 1000, 0));
            isJumping = true;
        }
        else
        {
            rigidBody.AddForce(new Vector3(0, -25, 0));
            isJumping = false;
        }
    }

    protected void UpdateAnimation()
    {
        animator.SetBool("isIdle", isIdle);
        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isAttacking", isAttacking);
        animator.SetBool("isBlocking", isBlocking);
        animator.SetBool("isSpecial", isSpecial);
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isFalling", isFalling);
        animator.SetBool("isHit", isHit);
        animator.SetBool("isDead", isDead);
    }

    public virtual void SetAttributes(int index, float amount, string text, Color color, int size, bool isDamage)
    {
        if (index == 4)
        {
            if (attributes[4] + amount < attributes[5])
                attributes[4] += amount;
            else
                attributes[4] = attributes[5];
        }
        else
            attributes[index] += amount;

        FloatingText(text, color, size, isDamage);
        healthBar.UpdateHealth(attributes[4], attributes[5]);
        UpdateTexts();
    }

    public virtual void PickKey(string keyType)
    {
        keybag.KeyIncrease(keyType);
    }

    public virtual void ConsumeKey(string keyType)
    {
        keybag.KeyDecrease(keyType);
    }

    public int GetKey(string keyType)
    {
        return keybag.Getkey(keyType);
    }

    public void UnlockCameraZ(bool enter)
    {
        camLockZ = !camLockZ;

        if (!enter)
            StartCoroutine(MoveCameraToPlayer(98f));
    }

    public void getHit(float damage)
    {
        if (!isDead)
        {
            float cameraToPayer = (Camera.main.transform.position - transform.position).magnitude;
            int fontSize = (int)(400 * (1 / cameraToPayer));
            isHit = true;
            sounds[1].Play();
            SetAttributes(4, -damage, damage.ToString("0"), Color.red, fontSize, true);

            StartCoroutine(InvincibleWait());
        }
    }

    public void ResetHit()
    {
        isHit = false;
    }

    public void StopWalking()
    {
        isWalking = false;
        isIdle = true;
    }

	public bool GetActionEnabledValue()
	{
		return actionsEnabled;
	}

    public void ChangeActions()
    {
        actionsEnabled = !actionsEnabled;
    }

    void FloatingText(string text, Color color, int size, bool isDamage)
    {
        float randomPosX = Random.Range(-0.5f, 0.51f);
        Vector3 offset = new Vector3(randomPosX, 1.5f, 0);
        Vector3 spawnPosition = transform.position + offset;
        Text tempText = (Text)Instantiate(floatingText, GameObject.FindGameObjectWithTag("Canvas").transform);
        FloatingText tempFloating = tempText.GetComponent<FloatingText>();
        tempText.transform.position = Camera.main.WorldToScreenPoint(spawnPosition);
        tempFloating.ChangeColor(color);
        tempFloating.ChangeSize(size);
        tempFloating.SetType(isDamage);
        tempText.text = text;
    }

	void Die()
    {
		StartCoroutine(gc.GameOver ());
		isDead = true;
        isIdle = false;
        isHit = false;
        attributes[4] = 1;
        healthText.text = "HP: " + 0 + "/" + attributes[5];
    }

    void Update()
    {
        if (attributes[4] <= 0)
			Die();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyProjectile" && !isBlocking && !tempInvincible)
        {
            PunchMovement tempPunch = other.gameObject.GetComponent<PunchMovement>();
            getHit(tempPunch.GetDamage());
            sounds[0].Play();
        }

        if (other.tag == "OculusProjectile" && !isBlocking && !tempInvincible)
        {
            MoveProjectile tempPunch = other.gameObject.GetComponent<MoveProjectile>();
            getHit(tempPunch.GetDamage());
            sounds[0].Play();
        }

        if (other.tag == "EnemyProjectile" && isBlocking && name == "Brawler")
            GetComponent<Brawler>().DecrementShieldPercent();
    }

    void UpdateTexts()
    {
        healthText.text = "HP: " + attributes[4].ToString("0") + "/" + attributes[5].ToString("0");
        damageText.text = "DMG: " + attributes[1].ToString("0");
        speedText.text = "Speed: " + attributes[0].ToString("0");
    }

    IEnumerator RotatePlayer()
    {
        for (int i = 0; i < 18; i++)
        {
            transform.Rotate(new Vector3(0, 10, 0));
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator MoveCameraToPlayer(float dist)
    {
        float distance = Camera.main.transform.position.z - dist;

        while (Mathf.Abs(distance) > 0.25f)
        {
            Camera.main.transform.Translate(Vector3.forward * 0.1f);
            distance = Camera.main.transform.position.z - dist;
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator InvincibleWait()
    {
        tempInvincible = true;
        yield return new WaitForSeconds(0.75f);
        tempInvincible = false;
    }

	public void SetIsInCorridor(bool value)
	{
		isInCorridor = value;
	}


	public void SetIsInElevator(bool value)
	{
		 isInElevator = value;
	}
}
