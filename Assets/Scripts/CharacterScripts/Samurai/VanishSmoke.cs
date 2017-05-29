using UnityEngine;
using System.Collections;

public class VanishSmoke : MonoBehaviour 
{
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Enemy" || other.tag == "MiniSlime")
			GameObject.Find ("Samurai").GetComponent<Samurai> ().ReduceSpecialCD ();


	}
}
