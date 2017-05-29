using UnityEngine;
using System.Collections;

public class PunchMovement : MonoBehaviour
{
    private float damage;

	void Start () 
	{
		StartCoroutine(FirePunch());
	}

	IEnumerator FirePunch()
	{
		for (int i = 0; i < 10; i++)
        {
			transform.Translate(Vector3.up * 4 * Time.deltaTime);
			yield return new WaitForSeconds (0.005f);
		}

		yield return new WaitForSeconds (0.2f);
		Destroy (gameObject);
	}

    public void SetDamage(float dmg)
    {
        damage = dmg;
    }

    public float GetDamage()
    {
        return damage;
    }
}
