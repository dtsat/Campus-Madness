using UnityEngine;
using System.Collections;

public class SecretDoor : MonoBehaviour
{
    private bool isOpened;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if (!isOpened)
            {
                if (player.GetKey("SilverKey") > 0)
                {
                    player.ConsumeKey("SilverKey");
                    StartCoroutine(OpenDoors());
                    isOpened = true;
                }
                else
                {
                    player.SetAttributes(0, 0, "No key", Color.white, 20, false);
                }
            }
        }

    }

    IEnumerator OpenDoors()
    {
        GameObject door1 = GameObject.Find("SecretDoor").transform.GetChild(0).gameObject;
        GameObject door2 = GameObject.Find("SecretDoor").transform.GetChild(1).gameObject;

        for (int i = 0; i < 100; i++)
        {
            door1.transform.Translate(Vector3.back * 0.025f);
            door2.transform.Translate(Vector3.forward * 0.025f);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
