using UnityEngine;
using System.Collections;

public class CorridorBehaviour : MonoBehaviour
{
    public int corridorIndex;

    private bool isInCorridor, doorsOpened, passedThrough;
    private GameObject corridor, door1, door2, specialCollider, player;
    private Collider protectCollider;
    private GameController gameController;
    private AudioSource doorSound;
	private GameObject[] characters;

    void Start()
    {
        if (corridorIndex <= 1)
        {
            corridor = GameObject.Find("Hall_Corridor_1");
            door1 = corridor.transform.FindChild("CorridorDoor1").gameObject;
            door2 = corridor.transform.FindChild("CorridorDoor2").gameObject;
            protectCollider = corridor.transform.FindChild("ProtectCollider").GetComponent<Collider>();
        }
        else
        {
            corridor = GameObject.Find("Hall_Room_3").transform.FindChild("Colliders").gameObject;
            door1 = corridor.transform.FindChild("CorridorDoor1").gameObject;
            door2 = corridor.transform.FindChild("CorridorDoor2").gameObject;
            protectCollider = corridor.transform.FindChild("ProtectCollider").GetComponent<Collider>();
        }

        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        doorSound = GetComponent<AudioSource>();
        passedThrough = false;
        isInCorridor = false;
        doorsOpened = false;
        protectCollider.enabled = true;
    }

    void Update()
    {
		characters = GameObject.FindGameObjectsWithTag ("Player");

		for (int i = 0; i < characters.Length; i++)
			if (characters [i].activeInHierarchy)
			{
				player = characters [i];
				break;
			}

        if (corridorIndex <= 1)
        {
            if (!doorsOpened && gameController.Getlevel() == 3)
            {
                doorsOpened = true;
                StartCoroutine(OpenDoors());
            }

            if (isInCorridor)
            {
                player.transform.Translate(Vector3.forward * Time.deltaTime * 15, Space.World);
            }
        }
        else
        {
            if (!doorsOpened && gameController.Getlevel() == 4)
            {
                doorsOpened = true;
                StartCoroutine(OpenDoors());
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isInCorridor)
        {
            isInCorridor = true;
            StartCoroutine(CloseDoors());
            PlayerController playerController = other.GetComponent<PlayerController>();
			playerController.SetIsInCorridor (isInCorridor);
            playerController.UnlockCameraZ(true);

            if (corridorIndex <= 1)
            {
                playerController.ChangeActions();
                gameController.FadeToBlack(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !passedThrough && corridorIndex <= 1)
        {
            gameController.NextLevel();
            specialCollider = GameObject.Find("SpecialCollider");
            isInCorridor = false;
            passedThrough = true;
            PlayerController playerController = other.GetComponent<PlayerController>();
			playerController.SetIsInCorridor (isInCorridor);
            playerController.UnlockCameraZ(false);
            playerController.ChangeActions();
            gameController.FadeToBlack(false);
            specialCollider.GetComponent<Collider>().enabled = true;
        }
    }

    IEnumerator OpenDoors()
    {
        protectCollider.enabled = false;

        doorSound.Play();

        for (int i = 0; i < 100; i++)
        {
            door1.transform.Translate(Vector3.back * 0.025f);
            door2.transform.Translate(Vector3.forward * 0.025f);
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator CloseDoors()
    {
        protectCollider.enabled = true;

        doorSound.Play();

        for (int i = 0; i < 100; i++)
        {
            door1.transform.Translate(Vector3.forward * 0.025f);
            door2.transform.Translate(Vector3.back * 0.025f);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
