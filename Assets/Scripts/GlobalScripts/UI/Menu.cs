using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Menu : MonoBehaviour
{
	public Canvas quitMenu;
	public Canvas charactersMenu;
	public Canvas howToPlayMenu;
	public Button startGameButton;
	public Button howToPlayButton;
	public Button exitButton;

	public GameObject characterPicked;
	public Image[] characterImage;
	public Sprite[] sprites;

	void Start () 
	{
		quitMenu = quitMenu.GetComponent<Canvas> ();
		charactersMenu = charactersMenu.GetComponent<Canvas> ();
		howToPlayMenu = howToPlayMenu.GetComponent<Canvas> ();
		startGameButton = startGameButton.GetComponent<Button> ();
		howToPlayButton = howToPlayButton.GetComponent<Button> ();
		exitButton = exitButton.GetComponent<Button> ();
		quitMenu.enabled = false;
		charactersMenu.enabled = false;
		howToPlayMenu.enabled = false;
	}

	public void ExitPress()
	{
		quitMenu.enabled = true;
		startGameButton.enabled = false;
		howToPlayButton.enabled = false;
		exitButton.enabled = false;
	}

	public void NoPress()
	{
		quitMenu.enabled = false;
		startGameButton.enabled = true;
		howToPlayButton.enabled = true;
		exitButton.enabled = true;
	}

	public void StartGame()
	{
		charactersMenu.enabled = true;
		startGameButton.enabled = false;
		howToPlayButton.enabled = false;
		exitButton.enabled = false;
	}

	public void Brawler()
	{
		characterPicked.GetComponent<CharacterPicked> ().SetCharacterNumber (0);
		StartCoroutine(ChangeSprite (0));
		DontDestroyOnLoad (characterPicked);
		SceneManager.LoadScene (1);
	}

	public void Marksman()
	{
		characterPicked.GetComponent<CharacterPicked> ().SetCharacterNumber (1);
		StartCoroutine(ChangeSprite (1));
		DontDestroyOnLoad (characterPicked);
		SceneManager.LoadScene (1);
	}

	public void Samurai()
	{
		characterPicked.GetComponent<CharacterPicked> ().SetCharacterNumber (2);
		StartCoroutine(ChangeSprite (2));
		DontDestroyOnLoad (characterPicked);
		SceneManager.LoadScene (1);
	}

	public void BackToMenu()
	{
		charactersMenu.enabled = false;
		howToPlayMenu.enabled = false;
		startGameButton.enabled = true;
		howToPlayButton.enabled = true;
		exitButton.enabled = true;
	}

	public void HowToplay()
	{
		howToPlayMenu.enabled = true;
		startGameButton.enabled = false;
		howToPlayButton.enabled = false;
		exitButton.enabled = false;
	}

	public void ExitGame()
	{
		Application.Quit ();
	}

	IEnumerator ChangeSprite(int num)
	{
		characterImage [num].sprite = sprites [num];
		yield return new WaitForSeconds (3f);
	}
}
