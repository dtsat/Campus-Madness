using UnityEngine;
using System.Collections;

public class ItemSilverChest : ItemController
{
    public GameObject[] items;

    private bool isClosed, isOpening, isOpen;
	private GameObject[] characters;
    private PlayerController player;
    private Animator animator;
    private ParticleSystem particles;

    ///--------------------------------------
    /// SilverChest
    ///     - spawns random items
    ///--------------------------------------

    protected override void Start()
    {
        animator = GetComponent<Animator>();
        particles = transform.FindChild("ChestEffect").GetComponent<ParticleSystem>();
        isClosed = true;
        isOpen = isOpening = false;

        base.Start();
        canOpen = false;

        UpdateAnimations();
    }

    protected override void Update()
    {
		characters = GameObject.FindGameObjectsWithTag ("Player");

		if (characters.Length == 0)
			return;

		for (int i = 0; i < characters.Length; i++)
			if (characters [i].activeInHierarchy)
			{
				player = characters [i].GetComponent<PlayerController> ();
				break;
			}
		
        if (player.GetKey("SilverKey") > 0)
            canOpen = true;
    }

    protected override void ItemEffect(Collider other)
    {
        if (!isOpen)
        {
            isOpening = true;
            isClosed = isOpen = false;

            other.GetComponent<PlayerController>().ConsumeKey("SilverKey");

            UpdateAnimations();
            StartCoroutine(SpawnItems());
        }
    }

    void UpdateAnimations()
    {
        animator.SetBool("isClosed", isClosed);
        animator.SetBool("isOpening", isOpening);
        animator.SetBool("isOpened", isOpen);
    }

    IEnumerator SpawnItems()
    {
        int numOfSpawnableItems = Random.Range(1, 6);
        float spawnChance = 100;
        float itemChance = (1 / (float)numOfSpawnableItems) * 100;
        particles.Play();

        for (int i = 0; i < numOfSpawnableItems; i++)
        {
            int randomIndex = Random.Range(0, 101);

            if (i == 2)
            {
                isOpen = true;
                isClosed = isOpening = false;

                UpdateAnimations();
            }

            if (randomIndex <= spawnChance)
            {
                Vector3 offset = Vector3.up * 2;
                GameObject tempItem = (GameObject)Instantiate(items[Random.Range(0, items.Length)], transform.position + offset, Quaternion.identity);
            }

            spawnChance -= itemChance;

            yield return new WaitForSeconds(0.05f);
        }
    }
}
