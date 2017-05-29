using UnityEngine;
using System.Collections;

public class KunaiMovement : MonoBehaviour
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
