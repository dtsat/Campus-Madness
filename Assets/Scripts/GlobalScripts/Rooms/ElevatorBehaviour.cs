using UnityEngine;
using System.Collections;

public class ElevatorBehaviour : MonoBehaviour
{
    public int elevatorIndex;

    private bool isInElevator, doorsOpened, elevatorDone;
    private GameObject elevator, door1, door2;
    private Collider protectCollider, specialBossCollider;
    private GameController gameController;
    private PlayerController playerController;
	private AudioSource doorSound;
	private GameObject[] characters;

    void Start()
    {
        if (elevatorIndex <= 1)
        {
            elevator = GameObject.Find("Hall_Elevator_1");
            door1 = elevator.transform.FindChild("ElevatorDoor1").gameObject;
            door2 = elevator.transform.FindChild("ElevatorDoor2").gameObject;
            protectCollider = elevator.transform.FindChild("ProtectCollider").GetComponent<Collider>();
        }
        else
        {
            elevator = GameObject.Find("Hall_Elevator_2");
            protectCollider = elevator.transform.FindChild("ProtectCollider").GetComponent<Collider>();
        }

        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        doorSound = GetComponent<AudioSource>();
        isInElevator = false;
        doorsOpened = false;
        protectCollider.enabled = false;
        elevatorDone = false;
    }

    void Update()
    {
		characters = GameObject.FindGameObjectsWithTag ("Player");

		for (int i = 0; i < characters.Length; i++)
			if (characters [i].activeInHierarchy)
			{
				playerController = characters [i].GetComponent<PlayerController> ();
				break;
			}
		
        if (gameController.Getlevel() == 2 && !doorsOpened && elevatorIndex <= 1)
        {
            doorsOpened = true;
            StartCoroutine(OpenDoors());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !elevatorDone)
        {
            isInElevator = true;
			playerController.SetIsInElevator (isInElevator);
            playerController.ChangeActions();
            playerController.StopWalking();
            StartCoroutine(MoveElevator());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && elevatorIndex <= 1)
        {
            gameController.NextLevel();
            isInElevator = false;
			playerController.SetIsInElevator (isInElevator);
            StartCoroutine(CloseDoors());
        }
    }

    IEnumerator MoveElevator()
    {
        yield return new WaitForSeconds(0.5f);

        float distance;

        if (elevatorIndex <= 1)
            distance = 500;
        else
            distance = 596.5f;

        if (isInElevator)
        {
            if (elevatorIndex <= 1)
                StartCoroutine(CloseDoors());

            yield return new WaitForSeconds(1);

            gameController.FadeToBlack(true);

            for (int i = 0; i < distance; i++)
            {
                elevator.transform.Translate(Vector3.up * 0.05f);

                if ((i == 400 && elevatorIndex <= 1) || (i == 500 && elevatorIndex >= 2))
                    gameController.FadeToBlack(false);

                yield return new WaitForSeconds(0.01f);
            }
        }

        elevatorDone = true;

        if (elevatorIndex <= 1)
            StartCoroutine(OpenDoors());
        else
        {
            specialBossCollider = GameObject.Find("SpecialBossCollider").gameObject.GetComponent<Collider>();
            GameObject[] obstacles = GameObject.FindGameObjectsWithTag("SlimeObstacles");
            
            foreach (GameObject obstacle in obstacles)
            {
                obstacle.GetComponent<Rigidbody>().useGravity = true;
                obstacle.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            }
            specialBossCollider.enabled = true;
			GameObject.Find ("BossSpawns").GetComponent<BossFight> ().spawnBoss ();
        }

        playerController.ChangeActions();

        if (elevatorIndex >= 2)
            playerController.UnlockCameraZ(true);
		
    }

    IEnumerator CloseDoors()
    {
		yield return new WaitForSeconds(0.5f);

        protectCollider.enabled = true;

        doorSound.Play();

        for (int i = 0; i < 100; i++)
        {
            door1.transform.Translate(Vector3.back * 0.025f);
            door2.transform.Translate(Vector3.forward * 0.025f);
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator OpenDoors()
    {
        protectCollider.enabled = false;

        doorSound.Play();

        for (int i = 0; i < 100; i++)
        {
            door1.transform.Translate(Vector3.forward * 0.025f);
            door2.transform.Translate(Vector3.back * 0.025f);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
