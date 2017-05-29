using UnityEngine;
using System.Collections;

public class LockerWithHiddenKey : MonoBehaviour {

    public bool open;
    public bool needKey;
    public bool struck;

    Quaternion closeRtt;
    Quaternion openRtt;

    float smooth = 3f;
    int silverKeyInit;
    KeyBag keyBag;

	void Start ()
    {
        open = false;
        needKey = false;
        struck = false;

        closeRtt = transform.rotation;
        openRtt = Quaternion.Euler(0, 120f, 0);
        keyBag = GameObject.FindGameObjectWithTag("Player").transform.FindChild("KeyBag").GetComponent<KeyBag>();
	}

	void Update ()
    {
        if (open)
            transform.rotation = Quaternion.Slerp(transform.rotation, openRtt, Time.deltaTime * smooth);
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !open && !needKey)
            open = true;
    }
}
