using UnityEngine;
using System.Collections;

public class GetPlayerStruct : MonoBehaviour {

    public GameObject OpenableFacePlane;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // only get player struck once. can be cancelled if want to increase difficulty
            transform.GetComponent<CapsuleCollider>().isTrigger = false;

            OpenableFacePlane.GetComponent<LockerWithHiddenKey>().needKey = true;
            
        }
    }
}
