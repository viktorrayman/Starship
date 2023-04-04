using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MeteorController : MonoBehaviour
{
    public GameObject meteorPrefab; // префаб метеорита
    public GameObject bullet; // игровой объект пули
    public GameObject player; // игровой объект игрока
    public float spawnRange = 1.5f; // диапазон появления метеоритов
    public float destroyDelay = 3f; // задержка перед удалением метеорита
    public float spawnDelay = 0.5f; // задержка между созданием метеоритов

    private List<GameObject> meteorList = new List<GameObject>(); // список всех созданных метеоритов

    private void Start()
    {
        InvokeRepeating("SpawnMeteor", spawnDelay, spawnDelay);
    }

    private void SpawnMeteor()
    {
        float randomX = Random.Range(-spawnRange, spawnRange);
        Vector3 spawnPosition = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z);
        GameObject newMeteor = Instantiate(meteorPrefab, spawnPosition, Quaternion.identity);
        meteorList.Add(newMeteor); // добавление метеорита в список всех метеоритов

        // удаление метеоритов, которые не соприкоснулись ни с чем
        StartCoroutine(DestroyMeteor(newMeteor, destroyDelay));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // перезапуск уровня при столкновении игрока и метеорита
        if (other.gameObject == player)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        // удаление метеорита при столкновении с пулей
        else if (other.gameObject == bullet)
        {
            Destroy(other.gameObject); // удаление пули
            foreach (GameObject meteor in meteorList)
            {
                if (meteor == null) // проверка на null для избежания ошибок
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
