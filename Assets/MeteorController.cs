using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MeteorController : MonoBehaviour
{
    public GameObject meteorPrefab; // ������ ���������
    public GameObject bullet; // ������� ������ ����
    public GameObject player; // ������� ������ ������
    public float spawnRange = 1.5f; // �������� ��������� ����������
    public float destroyDelay = 3f; // �������� ����� ��������� ���������
    public float spawnDelay = 0.5f; // �������� ����� ��������� ����������

    private List<GameObject> meteorList = new List<GameObject>(); // ������ ���� ��������� ����������

    private void Start()
    {
        InvokeRepeating("SpawnMeteor", spawnDelay, spawnDelay);
    }

    private void SpawnMeteor()
    {
        float randomX = Random.Range(-spawnRange, spawnRange);
        Vector3 spawnPosition = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z);
        GameObject newMeteor = Instantiate(meteorPrefab, spawnPosition, Quaternion.identity);
        meteorList.Add(newMeteor); // ���������� ��������� � ������ ���� ����������

        // �������� ����������, ������� �� �������������� �� � ���
        StartCoroutine(DestroyMeteor(newMeteor, destroyDelay));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ���������� ������ ��� ������������ ������ � ���������
        if (other.gameObject == player)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        // �������� ��������� ��� ������������ � �����
        else if (other.gameObject == bullet)
        {
            Destroy(other.gameObject); // �������� ����
            foreach (GameObject meteor in meteorList)
            {
                if (meteor == null) // �������� �� null ��� ��������� ������
                {
                    continue;
                }
                if (other.bounds.Intersects(meteor.GetComponent<Collider2D>().bounds))
                {
                    Destroy(meteor);
                }
            }
        }
    }

    private IEnumerator DestroyMeteor(GameObject meteor, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!meteor.GetComponent<Collider2D>().IsTouchingLayers())
        {
            meteorList.Remove(meteor);
            Destroy(meteor);
        }
    }
}
