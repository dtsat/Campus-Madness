using UnityEngine;
using System.Collections;

public class KunaiShot : MonoBehaviour 
{
	private Samurai samurai;

	void Start ()
	{
		samurai = GetComponentInParent<Samurai>();
	}

	IEnumerator SpawnKunai() 
	{
		samurai.SpawnKunai();
		yield return new WaitForSeconds (0.07f);
		samurai.SpawnKunai();
		yield return new WaitForSeconds (0.07f);
		samurai.SpawnKunai();
	}

	void ResetHit()
	{
		samurai.ResetHit ();
	}

	void ResetSpecial()
	{
		samurai.ResetSpecialState();
	}
}
