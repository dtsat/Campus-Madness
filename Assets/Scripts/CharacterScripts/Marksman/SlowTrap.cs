using UnityEngine;
using System.Collections;

public class SlowTrap : MonoBehaviour
{
    public GameObject snowPatch;
	private GameObject slowTrapObject;
	private AudioSource audioSource;
	private bool triggerSound;

	void Start()
	{
		slowTrapObject = GameObject.FindWithTag ("SlowTrap");
		audioSource = GetComponent<AudioSource> ();
		triggerSound = false;
	}

    void OnTriggerEnter(Collider other)
    {
		if (other.tag == "Enemy" && slowTrapObject != null)
		{
			if (!triggerSound)
			{
				triggerSound = true;
				audioSource.Play ();
			}
			GetComponentInChildren<SpriteRenderer> ().enabled = false;
			Destroy (gameObject, 2);
			GameObject snow = Instantiate(snowPatch, transform.position - new Vector3 (0.0f, 0.14f, 0.0f), transform.rotation) as GameObject;
			Destroy (snow, 5);
		}
    }

	void OnTriggerStay(Collider other)
	{
		if (other.tag == "Enemy" && slowTrapObject != null)
		{
			if (!triggerSound)
			{
				triggerSound = true;
				audioSource.Play ();
			}
			GetComponentInChildren<SpriteRenderer> ().enabled = false;
			Destroy (gameObject, 2);
			GameObject snow = Instantiate(snowPatch, transform.position - new Vector3 (0.0f, 0.14f, 0.0f), transform.rotation) as GameObject;
			Destroy (snow, 5);
		}
	}
}