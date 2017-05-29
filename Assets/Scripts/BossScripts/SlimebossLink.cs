using UnityEngine;
using System.Collections;

public class SlimebossLink : MonoBehaviour {

	private SlimeBoss sb;
	private AudioSource[] sounds;

	void Start () 
	{
		sb = GetComponentInParent<SlimeBoss> ();
		sounds = GetComponents<AudioSource> ();
	}

	void jumpup()
	{
		sounds [0].Play ();
	}

	void land()
	{
		sounds [1].Play ();
		sounds [2].Play ();
	}
	
	void reset()
	{
		sb.resetState ();
	}

	void spray()
	{
		StartCoroutine (sb.spray());
	}

	void reposition()
	{
		sb.navMeshAgent.speed = 20f;
	}

	void resetSpeed()
	{
		sb.navMeshAgent.speed = sb.speed;
	}

}
