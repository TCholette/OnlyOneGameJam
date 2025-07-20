using UnityEngine;
using System.Collections;

public class EnemySpawnPoint : MonoBehaviour {

    [SerializeField] private GameObject enemyType;
    public Enemy enemy;
    public void Init() {
        gameObject.SetActive(true);
        GameObject enemyObj = Instantiate(enemyType, transform);
        enemyObj.transform.position = transform.position;
        StartCoroutine(Despawn());
        enemy = enemyObj.GetComponent<Enemy>();
    }

    private IEnumerator Despawn() {
        yield return new WaitForSeconds(1f);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

}
