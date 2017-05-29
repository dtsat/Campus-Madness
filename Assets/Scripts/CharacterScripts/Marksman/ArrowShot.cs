using UnityEngine;
using System.Collections;

public class ArrowShot : MonoBehaviour 
{
    private Marksman marksman;

	void Start ()
    {
        marksman = GetComponentInParent<Marksman>();
	}
	
	void SpawnArrow() 
    {
        marksman.SpawnArrow();
	}

	void ResetHit()
	{
		marksman.ResetHit ();
	}

	void ResetSpecial()
	{
		marksman.ResetSpecialState ();
	}
}
