using UnityEngine;
using System.Collections;

public class FirePunch : MonoBehaviour
{
	private Brawler puncher;
	private AudioSource[] sounds;

	void Start()
	{
		puncher = GetComponentInParent<Brawler>();
		sounds = GetComponents<AudioSource> ();
	}

	void SpawnPunch()
	{
		puncher.SpawnPunch();
	}

	void ResetHit()
	{
		puncher.ResetHit ();
	}

	void ResetAttack()
	{
		puncher.ResetAttack ();
	}

	void punchOne()
	{
		sounds [0].Play ();
	}

	void punchTwo()
	{
		sounds [1].Play ();
	}

	void Die()
	{
		Destroy (puncher.gameObject);
	}
}