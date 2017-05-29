using UnityEngine;
using System.Collections;

public class SlowMovement : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Enemy")
		{
			if (other.gameObject.GetComponentInChildren<Animator> ().runtimeAnimatorController != null)
			{
				RuntimeAnimatorController animator = other.gameObject.GetComponentInChildren<Animator>().runtimeAnimatorController;
				other.gameObject.GetComponentInChildren<Animator> ().runtimeAnimatorController = null;
				other.gameObject.GetComponentInChildren<SpriteRenderer> ().color = Color.cyan;
				other.gameObject.GetComponentInChildren<Animator> ().runtimeAnimatorController = animator;
			}
			else
				other.gameObject.GetComponentInChildren<SpriteRenderer> ().color = Color.cyan;

			if (other.GetComponent<EnemyController> ().attributes [0] > 2) 
			{
				other.GetComponent<EnemyController> ().SlowedText ();
				GameObject.Find("Marksman").GetComponent<Marksman>().ReduceSpecialCD();
			}

			other.GetComponent<EnemyController> ().attributes[0] = 2;
		}

		if (other.tag == "MiniSlime") 
		{
			GetComponent<Marksman> ().ReduceSpecialCD ();
			other.GetComponent<EnemyController> ().attributes[0] = 2;
		}
	}
}
