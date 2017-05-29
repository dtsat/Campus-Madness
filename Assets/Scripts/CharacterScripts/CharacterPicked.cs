using UnityEngine;
using System.Collections;

public class CharacterPicked : MonoBehaviour
{
	private int characterPicked;

	public int GetCharacterNumber()
	{
		return characterPicked;
	}

	public void SetCharacterNumber(int num)
	{
		characterPicked = num;
	}
}