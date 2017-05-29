using UnityEngine;
using System.Collections;

public class LightSwitch : MonoBehaviour {

    private bool turnOn;
    private Color reserve;
    private GameObject[] spotLights;

    private Vector3 onRtt = new Vector3(0,0,34f);
    private Vector3 offRtt = new Vector3(0,0,-34f);

    private float lastTime;
    private float interval = 1.5f;

	void Start () {
        spotLights = GameObject.FindGameObjectsWithTag("SwitchableLights");

        turnOn = false;

        foreach (GameObject light in spotLights)
            light.SetActive(turnOn);

        reserve = RenderSettings.ambientLight;
        RenderSettings.ambientLight = Color.black;
	}

    void OnTriggerEnter (Collider other)
    {
        if (other.tag == "Player" && Time.time - lastTime >= interval)
        {
            lastTime = Time.time;

            if (turnOn)
            {
                transform.rotation = Quaternion.Euler(offRtt);
                RenderSettings.ambientLight = Color.black;
                turnOn = false;

                foreach (GameObject light in spotLights)
                    light.SetActive(turnOn);
            }
            else
            {
                transform.rotation = Quaternion.Euler(onRtt);
                RenderSettings.ambientLight = reserve;
                turnOn = true;

                Transform itemList = GameObject.Find("Washing_Room").transform.FindChild("Items");

                for (int i = 0; i < itemList.childCount; i++)
                {
                    itemList.GetChild(i).gameObject.GetComponent<Rigidbody>().useGravity = true;
                    itemList.GetChild(i).gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    itemList.GetChild(i).gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                    Debug.Log(itemList.GetChild(i).name);
                }

                foreach (GameObject light in spotLights)
                    light.SetActive(turnOn);
            }
        }
    }
}