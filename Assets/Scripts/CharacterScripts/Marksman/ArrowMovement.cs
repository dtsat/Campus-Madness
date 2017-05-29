using UnityEngine;
using System.Collections;

public class ArrowMovement : MonoBehaviour
{
	private float damage;

	public void SetDamage(float dmg)
	{
		damage = dmg;
	}

	public float GetDamage()
	{
		return damage;
	}
}
