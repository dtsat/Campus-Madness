using UnityEngine;
using System.Collections;

public class RainOfArrowRanged : MonoBehaviour {

	public GameObject meleeHit;
	public float destroyTime;

	private float damage;

	void Start () {
		Destroy (gameObject, destroyTime);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Enemy") 
		{
			other.gameObject.GetComponent<EnemyController> ().getHit ((Random.Range(damage - 1.5f, damage + 1.5f)), 1f);

			GameObject hitObject = Instantiate (meleeHit, other.transform.position, other.transform.rotation) as GameObject;
			Destroy (hitObject, 0.5f);
		}
		else if (other.gameObject.tag == "MiniSlime") 
		{
			other.gameObject.GetComponent<MiniSlime> ().getKilled ();
			GameObject hitObject = Instantiate (meleeHit, other.transform.position, other.transform.rotation) as GameObject;
			Destroy (hitObject, 0.5f);
		}
	}

	public void SetDamage(float dmg)
	{
		damage = dmg * 3;
	}
}
