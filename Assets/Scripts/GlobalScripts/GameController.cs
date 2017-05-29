using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController : MonoBehaviour
{
	/// <summary>
	/// Portraits:
	///    1 - Trump
	///    2 - Moon
	/// </summary>

	[Header("Attributes")]
	public int spawnPerWaves;

	[Header("Object Links")]
	public GameObject[] enemies;
	public GameObject transition;
	public Text transitionText;
	public Text nextLevelText;
	public Text cutsceneText;
	public Text returnText;
	public GameObject portrait1;
	public GameObject portrait2;
	public Transform[] spawnPoints;
	public Image[] portraits;

	public Sprite[] skillIcons;

	[Header("Skill Icons")]
	public Image icon_1;
	public Image icon_2;
	public Image icon_3;

	[Header("Pause Menu")]
	public Canvas pausedMenu;
	public Canvas areYouSureMenu;
	public Canvas areYouSureGame;
	public Button resumeButton;
	public Button backToMenuButton;
	public Button exitGameButton;

	[Header("GameOver Menu")]
	public Canvas gameOverMenu;
	public Button restartButton;

	private int level, cutsceneProgress;
	private bool waveSpawning, inCutscene, bossDefeated;
	private GameObject[] rooms = new GameObject[4];
	private Transform[] currentSpawnPoints = new Transform[5];
	private Image transitionImage;
	private char[] cutsceneString;
	private string[] roomNames = new string[4];
	private string[] progressionText = new string[3];
	private PlayerController playerController;

	private AudioSource[] audioSources;

	void Start()
	{
		Time.timeScale = 1;
		pausedMenu = pausedMenu.GetComponent<Canvas> ();   
		gameOverMenu = gameOverMenu.GetComponent<Canvas> ();   
		areYouSureMenu = areYouSureMenu.GetComponent<Canvas> ();       
		areYouSureGame = areYouSureGame.GetComponent<Canvas> ();       
		resumeButton = resumeButton.GetComponent<Button> ();       
		backToMenuButton = backToMenuButton.GetComponent<Button> ();       
		exitGameButton = exitGameButton.GetComponent<Button> ();       
		pausedMenu.enabled = false;
		gameOverMenu.enabled = false;
		areYouSureMenu.enabled = false;    
		areYouSureGame.enabled = false;
		audioSources = GetComponents<AudioSource> ();
		audioSources [0].Play ();

		if (GameObject.Find ("CharacterPicked") == null) {
			GameObject.Find ("Marksman").SetActive (false);    
			GameObject.Find ("Samurai").SetActive (false);
		} else {
			switch (GameObject.Find ("CharacterPicked").GetComponent<CharacterPicked> ().GetCharacterNumber ()) {      
			case 0:    
				GameObject.Find ("Marksman").SetActive (false);    
				GameObject.Find ("Samurai").SetActive (false);   
				playerController = GameObject.Find ("Brawler").GetComponent<Brawler> ();
				break;     
			case 1:    
				GameObject.Find ("Brawler").SetActive (false);     
				GameObject.Find ("Samurai").SetActive (false);
				playerController = GameObject.Find("Marksman").GetComponent<Marksman> ();
				break;     
			case 2:    
				GameObject.Find ("Brawler").SetActive (false);     
				GameObject.Find ("Marksman").SetActive (false);
				playerController = GameObject.Find("Samurai").GetComponent<Samurai> (); 
				break;
			}
		}
			
		transitionImage = transition.GetComponent<Image>();
		rooms[0] = GameObject.FindGameObjectWithTag("H_Room_1");
		rooms[1] = GameObject.FindGameObjectWithTag("H_Room_2");
		rooms[2] = GameObject.FindGameObjectWithTag("H_Room_3");
		rooms[3] = GameObject.FindGameObjectWithTag("H_Room_Boss");
		roomNames[0] = "Holl Building: Basement";
		roomNames[1] = "Holl Building: Main Floor";
		roomNames[2] = "Holl Building: Cafeteria";
		roomNames[3] = "Holl Building: Rooftop";
		progressionText[0] = "Continue >>";
		progressionText[1] = "Continue ^^";
		progressionText[2] = "Continue ^^";
		transitionText.text = "";
		nextLevelText.text = "";
		returnText.text = "";
		inCutscene = false;
		bossDefeated = false;
		Debug.Log (Time.timeScale);

		StartCoroutine(PlayCutscenes());
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Return) && inCutscene && level < 3)
		{
			Time.timeScale = 1;
			inCutscene = false;
			cutsceneText.text = "";
			returnText.text = "";
			StartCoroutine(FadeCutscene(true));
			StopCoroutine(PlayCutscenes());
			FadeImage(false);
		}

		if (Input.GetKeyDown(KeyCode.Return) && bossDefeated && !inCutscene)
		{
			Time.timeScale = 1;
			SceneManager.LoadScene(0);
		}

		if (Input.GetKeyDown (KeyCode.Escape))
		{
			Time.timeScale = 0;
			pausedMenu.enabled = true;
		}
	}

	public IEnumerator GameOver()
	{
		yield return new WaitForSeconds(1);
		gameOverMenu.enabled = true;
		Time.timeScale = 0;
	}

	void RenderRooms()
	{
		for (int i = 0; i < rooms.Length; i++)
		{
			if (i != (level - 1))
				rooms[i].SetActive(false);
			else
				rooms[i].SetActive(true);
		}
	}

	void RenderIcons()
	{
		if (GameObject.Find("Brawler") != null && GameObject.Find("Brawler").activeInHierarchy)
		{
			icon_1.sprite = skillIcons[0];
			icon_2.sprite = skillIcons[1];
			icon_3.sprite = skillIcons[2];
		}

		if (GameObject.Find("Marksman") != null && GameObject.Find("Marksman").activeInHierarchy)
		{
			icon_1.sprite = skillIcons[3];
			icon_2.sprite = skillIcons[4];
			icon_3.sprite = skillIcons[5];
		}

		if (GameObject.Find("Samurai") != null && GameObject.Find("Samurai").activeInHierarchy)
		{
			icon_1.sprite = skillIcons[6];
			icon_2.sprite = skillIcons[7];
			icon_3.sprite = skillIcons[8];
		}
	}

	public void NextLevel()
	{
		RenderRooms();

		switch (level)
		{
		case 1:
			for (int i = 0; i < 4; i++)
				currentSpawnPoints[i] = spawnPoints[i];
			currentSpawnPoints[4] = null;
			break;
		case 2:
			for (int i = 0; i < 5; i++)
				currentSpawnPoints[i] = spawnPoints[i + 4];
			break;
		case 3:
			for (int i = 0; i < 4; i++)
				currentSpawnPoints[i] = spawnPoints[i + 9];
			currentSpawnPoints[4] = null;
			break;
		}

		StartCoroutine(SpawnWaves());
	}

	public void IncreaseLevel()
	{
		StartCoroutine(NewLevel());
	}

	IEnumerator ChecksForEnemiesOrWait()
	{
		while (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
			yield return new WaitForSeconds (0.1f);

		IncreaseLevel ();
	}

	IEnumerator NewLevel()
	{
		yield return new WaitForSeconds(0.01f);

		if (!waveSpawning)
		{
			level++;
			StartCoroutine(NextLevelText(progressionText[level - 2]));
			Debug.Log("Increasing level to " + level);
		}
	}

	public int Getlevel()
	{
		return level;
	}

	public void FadeToBlack(bool fadeOut)
	{
		StartCoroutine(FadeImage(fadeOut));
	}

	public void PlayCutScene()
	{
		StartCoroutine(PlayCutscenes());
	}

	IEnumerator NextLevelText(string text)
	{
		for (int i = 0; i < 5; i++)
		{
			nextLevelText.text = text;
			yield return new WaitForSeconds(0.5f);
			nextLevelText.text = "";
			yield return new WaitForSeconds(0.5f);
		}
	}

	IEnumerator FadeImage(bool fadeOut)
	{
		int fadeLength = 100;

		if (fadeOut)
		{
			for (int i = 0; i < fadeLength; i++)
			{
				Color newColor = new Color(transitionImage.color.r, transitionImage.color.g, transitionImage.color.b, ((float)i / fadeLength));
				transitionImage.color = newColor;
				yield return new WaitForSeconds(0.01f);
			}

			StartCoroutine(FadeText(fadeOut));

			RenderRooms();
		}
		else
		{
			StartCoroutine(FadeText(fadeOut));

			for (int i = fadeLength; i > 0; i--)
			{
				Color newColor = new Color(transitionImage.color.r, transitionImage.color.g, transitionImage.color.b, ((float)i / fadeLength));
				transitionImage.color = newColor;
				yield return new WaitForSeconds(0.01f);
			}

			if (level > 3)
			{
				audioSources [0].Stop();
				audioSources [1].Play ();
			}
		}

		if (!bossDefeated)
			playerController.ChangeActions ();
	}

	IEnumerator FadeText(bool fadeOut)
	{
		int fadeLength = 50;

		if (level < 1)
			level = 1;

		if (level < 3)
			transitionText.text = "Level " + level + "\n\n" + roomNames[level - 1];
		else
			transitionText.text = "";

		if (fadeOut)
		{
			for (int i = 0; i < fadeLength; i++)
			{
				transitionText.GetComponent<Text>().color = new Color(1, 1, 1, ((float)i / fadeLength));
				yield return new WaitForSeconds(0.01f);
			}
		}
		else
		{
			for (int i = fadeLength; i > 0; i--)
			{
				transitionText.GetComponent<Text>().color = new Color(1, 1, 1, ((float)i / fadeLength));
				yield return new WaitForSeconds(0.01f);
			}

			transitionText.text = "";
		}
	}

	IEnumerator SpawnWaves()
	{
		yield return new WaitForSeconds(5);

		StartCoroutine(NextLevelText("FIGHT!"));

		int numOfPoints = 0;

		foreach (Transform spawnPoint in currentSpawnPoints)
		{
			if (spawnPoint != null)
				numOfPoints++;
		}

		waveSpawning = true;
		float randomSpawnInterval;
		int numOfSpawns = level * spawnPerWaves;
		float spawnRange = ((float)1 / level) * 6;

		for (int i = 0; i < numOfSpawns; i++)
		{
			randomSpawnInterval = Random.Range(spawnRange, (spawnRange * 2)); // Comment our Random.Range and replace by => 0.01f;
			Transform currentPoint = currentSpawnPoints[Random.Range(0, numOfPoints)];
			Instantiate(enemies[Random.Range(0, level)], currentPoint.position, Quaternion.identity);
			yield return new WaitForSeconds(randomSpawnInterval);
		}

		waveSpawning = false;

		StartCoroutine(ChecksForEnemiesOrWait());
	}

	IEnumerator PlayCutscenes()
	{
		inCutscene = true;

		if (level < 3)
			returnText.text = "'Return' to skip >>";
		else
			returnText.text = "'Return' to go back to the main menu";

		string currentText = "";

		if (level > 1)
			StartCoroutine(FadeCutscene(false));

		switch (level)
		{
		case 0:
			currentText = "The year is 2017. Newly elected President Ronald Drump was using Twitter while on the toilet and accidently nuked the moon. Aliens, furious that their favourite vacation spot was destroyed, have decided to invade Earth. They have chosen to strike at Concord University, where they determined the smartest people happen to be. Time to defend your school!";
			level = 1;
			break;
		default:
			currentText = "Congratulations, You defeated the Alien Slime!\n\nNext content available as DLC, pre-purchase 'Students Strike Back' on the online store for only $149.99, available on October 32nd, 2222!";
			break;
		}

		cutsceneString = new char[currentText.Length];

		for (int i = 0; i < cutsceneString.Length; i++)
			cutsceneString[i] = ' ';

		string currentString = "";

		for (int i = 0; i < cutsceneString.Length; i++)
		{
			if (inCutscene)
			{
				cutsceneString[i] = currentText[i];
				currentString = new string(cutsceneString);
				cutsceneText.text = currentString;
				yield return new WaitForSeconds(0.03f);
			}
		}

		if (inCutscene)
			Time.timeScale = 0;

		StartCoroutine(FadeCutscene(true));

		if (level < 3)
		{
			StartCoroutine(FadeText(true));

			yield return new WaitForSeconds(5);

			StartCoroutine(FadeImage(false));

			RenderIcons();
			NextLevel();
		}

		inCutscene = false;
	}

	IEnumerator FadeCutscene(bool fadeOut)
	{
		int fadeLength = 50;

		if (fadeOut)
		{
			for (int i = fadeLength; i >= 0; i--)
			{
				cutsceneText.GetComponent<Text>().color = new Color(1, 1, 1, ((float)i / fadeLength));
				returnText.GetComponent<Text>().color = new Color(1, 1, 1, ((float)i / fadeLength));
				portrait1.GetComponent<Image>().color = new Color(1, 1, 1, ((float)i / fadeLength));
				portrait2.GetComponent<Image>().color = new Color(1, 1, 1, ((float)i / fadeLength));

				yield return new WaitForSeconds(0.01f);
			}
		}
		else
		{
			for (int i = 0; i < fadeLength; i++)
			{
				cutsceneText.GetComponent<Text>().color = new Color(1, 1, 1, ((float)i / fadeLength));
				returnText.GetComponent<Text>().color = new Color(1, 1, 1, ((float)i / fadeLength));
				portrait1.GetComponent<Image>().color = new Color(1, 1, 1, ((float)i / fadeLength));
				portrait2.GetComponent<Image>().color = new Color(1, 1, 1, ((float)i / fadeLength));

				yield return new WaitForSeconds(0.01f);
			}
		}
	}

	public void SignalBossDefeated()
	{
		bossDefeated = true;
		playerController.ChangeActions ();
	}
		
	public void ResumePress()
	{
		Time.timeScale = 1;
		pausedMenu.enabled = false;
	}

	public void BackToMenuPress()
	{
		areYouSureMenu.enabled = true;
		resumeButton.enabled = false;
		backToMenuButton.enabled = false;
		exitGameButton.enabled = false;
	}

	public void ExitGamePress()
	{
		areYouSureGame.enabled = true;
		resumeButton.enabled = false;
		backToMenuButton.enabled = false;
		exitGameButton.enabled = false;
	}

	public void Restart()
	{
		SceneManager.LoadScene (1);
	}

	public void YesBackToMenuPress()
	{
		SceneManager.LoadScene (0);
		Destroy (GameObject.Find("CharacterPicked"));
	}

	public void YesExitGamePress()
	{
		Application.Quit ();
	}

	public void NoBackToMenuPress()
	{
		areYouSureMenu.enabled = false;
		resumeButton.enabled = true;
		backToMenuButton.enabled = true;
		exitGameButton.enabled = true;
	}

	public void NoExitGamePress()
	{
		areYouSureGame.enabled = false;
		resumeButton.enabled = true;
		backToMenuButton.enabled = true;
		exitGameButton.enabled = true;
	}
}