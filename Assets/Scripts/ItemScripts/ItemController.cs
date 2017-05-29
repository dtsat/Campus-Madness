using UnityEngine;
using System.Collections;

public abstract class ItemController : MonoBehaviour
{
    protected bool canOpen;

    private bool hasLanded, isPicked;
    private float rotationSpeed, lifeTime;
    private Rigidbody rigidBody;

    protected virtual void Start()
    {
        rotationSpeed = 50;
        lifeTime = 0;
        hasLanded = false;
        isPicked = false;
        canOpen = true;
        rigidBody = GetComponent<Rigidbody>();
        StartCoroutine(Safety());
    }

    protected virtual void Update()
    {
        if (lifeTime <= 4)
            lifeTime += Time.deltaTime;
        else
            hasLanded = true;
    }

    protected void Idle(Vector3 axis, bool isRotating)
    {
        if (hasLanded)
        {
            if (isRotating)
                transform.Rotate(axis * Time.deltaTime * rotationSpeed);
        }
    }

    protected virtual void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (hasLanded && !isPicked)
            {
                if (canOpen)
                {
                    isPicked = true;
                    ItemEffect(other.gameObject.GetComponent<Collider>());
                }
                else
                {
                    PlayerController player = other.gameObject.GetComponent<PlayerController>();
                    UpdatePlayerAttributes(player, 0, 0, "No Key", Color.white, 400);
                }
            }
            else
                Physics.IgnoreCollision(other.gameObject.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
        }

        if (other.gameObject.CompareTag("Items") || other.gameObject.CompareTag("Enemy"))
            Physics.IgnoreCollision(other.gameObject.GetComponent<Collider>(), gameObject.GetComponent<Collider>());

        if (other.gameObject.CompareTag("Ground") && !hasLanded)
            hasLanded = true;
    }

    protected void SpawnItem()
    {
        float offset = Random.Range(3, 7);
        Vector3 forceVector = new Vector3
            (
                Random.Range(-offset, offset),
                Random.Range(2.5f, 4.5f),
                Random.Range(-offset, offset)
            );

        rigidBody.AddForce(forceVector * 2, ForceMode.Impulse);
    }

    protected void UpdatePlayerAttributes(PlayerController playerController, int index, float amount, string text, Color color, int fontSize)
    {
        float cameraToPayer = (Camera.main.transform.position - transform.position).magnitude;
        fontSize = (int)(fontSize * (1 / cameraToPayer));
        playerController.SetAttributes(index, amount, text, color, fontSize, false);
    }

    protected void UpdatePlayerKeyBag(PlayerController playerController, string keyType)
    {
        playerController.PickKey(keyType);
    }

    protected abstract void ItemEffect(Collider other);

    IEnumerator Safety()
    {
        yield return new WaitForSeconds(3);

        hasLanded = true;
    }
}