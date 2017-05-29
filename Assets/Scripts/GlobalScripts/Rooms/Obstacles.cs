using UnityEngine;
using System.Collections;

public class Obstacles : MonoBehaviour
{
    ///------------------------------
    /// TYpe 1 - Non opaque objects (crates, boxes, lockers, etc)
    /// Type 2 - Opaque objects (Platforms, etc)
    ///------------------------------

    public float fadeWidth;
    public float depthOffset;
    public int type;

    private GameObject player;
    private Material material;
	private Color defaultColor;
	private GameObject[] characters;

	void Start ()
    {
        material = gameObject.GetComponent<MeshRenderer>().material;
        defaultColor = material.color;
	}
	
	void Update ()
    {
		characters = GameObject.FindGameObjectsWithTag ("Player");

		for (int i = 0; i < characters.Length; i++)
			if (characters [i].activeInHierarchy)
			{
				player = characters [i];
				break;
			}

        if (player != null)
        {
            Vector3 playerPos = player.transform.position;
            Vector3 objectPos = transform.position;

            if (playerPos.z > (objectPos.z + depthOffset) && playerPos.x >= (objectPos.x - fadeWidth) && playerPos.x <= (objectPos.x + fadeWidth) && objectPos.y >= playerPos.y - 1.33f && objectPos.y < playerPos.y + 5)
            {
                if (type == 1)
                    material.color = defaultColor + new Color(0, 0, 0, -0.75f);
                else
                    GetComponent<MeshRenderer>().enabled = false;
            }
            else {
                if (type == 1)
                    material.color = defaultColor;
                else
                    GetComponent<MeshRenderer>().enabled = true;
            }
        }
	}
}
