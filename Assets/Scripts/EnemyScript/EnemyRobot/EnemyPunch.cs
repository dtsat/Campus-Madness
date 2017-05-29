using UnityEngine;
using System.Collections;

public class EnemyPunch : MonoBehaviour {

	private EnemyRobot puncher;
	private AudioSource[] audioSource;

	void Start()
	{
		puncher = GetComponentInParent<EnemyRobot>();
		audioSource = GetComponents<AudioSource> ();
	}

	void spawnPunch()
	{
		puncher.spawnPunch();
	}

	void resetHit()
	{
		puncher.resetHit();
	}

	void killSelf()
	{
		puncher.destoySelf ();
	}

	void killNoise()
	{
		audioSource [0].Play ();
	}

	void getHit()
	{
		audioSource [2].Play ();
	}

	void attackNoise()
	{
		audioSource [1].Play ();
	}


}
