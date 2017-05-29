using UnityEngine;
using System.Collections;

public class MiniSlimeLink : MonoBehaviour {

	private MiniSlime slime;

	void Start()
	{
		slime = GetComponentInParent<MiniSlime> ();
	}

	void resetHit()
	{
		slime.resetHit();
	}

	void killSelf()
	{
		slime.destoySelf ();
	}
}
