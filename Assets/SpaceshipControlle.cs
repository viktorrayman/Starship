using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    public float speed = 5.0f;
    public float padding = 0.5f;

    public GameObject bulletPrefab;
    public float bulletSpeed = 10.0f;
    public float firingRate = 0.2f;

    private float xMin, xMax;

    // Start is called before the first frame update
    void Start()
    {
        float distance = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftMost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 rightMost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));

        xMin = leftMost.x + padding;
        xMax = rightMost.x - padding;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            InvokeRepeating("Fire", 0.000001f, firingRate);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            CancelInvoke("Fire");
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }

        float newX = Mathf.Clamp(transform.position.x, xMin, xMax);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    void Fire()
    {
        Vector3 offset = new Vector3(0, 1, 0);
        GameObject bullet = Instantiate(bulletPrefab, transform.position + offset, Quaternion.identity) as GameObject;
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(0, bulletSpeed);
        Destroy(bullet, 5.0f);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<MeteorController>() != null)
        {
            FindObjectOfType<GameManager>().ResetGame();
        }
    }
}
