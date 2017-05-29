using UnityEngine;
using System.Collections;

public class BossFight : MonoBehaviour {

	public GameObject boss;
	private GameObject slimeBoss;
	private SlimeBoss bossScript;
	private GameController gc;
	public Transform bossSpawn;
	public GameObject miniSlimes;
	public Transform[] miniSpawnLocations;

	public int numSlimes;

	private bool bossAlive, gameOver;
	private AudioSource[] audioSources;

	void Start ()
	{
		audioSources = GetComponents<AudioSource> ();
		gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		numSlimes = 100;
		bossAlive = true;
		gameOver = false;
	}

	public void spawnBoss()
	{
		slimeBoss = Instantiate (boss, bossSpawn.position, boss.transform.rotation) as GameObject;
		StartCoroutine (slimeBoss.GetComponent<SlimeBoss> ().jump ());
	}

	public IEnumerator SpawnSlimes()
	{
		numSlimes = 10;
		for(int i = 0; i < miniSpawnLocations.Length; i++)
		{
			Instantiate(miniSlimes, miniSpawnLocations[i].position, miniSlimes.transform.rotation);
			yield return new WaitForSeconds (0.5f);
		}
	}

	public void killSlime()
	{
		if (numSlimes > 0)
			numSlimes--;
	}

	public void endFight()
	{
		bossAlive = false;
	}


	void Update ()
	{
		if (numSlimes <= 1 && bossAlive)
		{
			numSlimes = 100;
			StartCoroutine(slimeBoss.GetComponent<SlimeBoss>().jump());
		}
		else if (GameObject.FindGameObjectsWithTag("MiniSlime").Length <= 0 && !gameOver && !bossAlive)
		{
			audioSources [1].Play ();
			gameOver = true;
			gc.SignalBossDefeated ();
			StartCoroutine (GameOver ());
		}          
	}

	IEnumerator GameOver()
	{
		gc.FadeToBlack(true);
		yield return new WaitForSeconds (2);
		gc.PlayCutScene();
	}
}