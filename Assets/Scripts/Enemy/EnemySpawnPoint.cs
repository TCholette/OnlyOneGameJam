using UnityEngine;
using System.Collections;

public class EnemySpawnPoint : MonoBehaviour {

    [SerializeField] private EnemyType enemyType;
    public Enemy enemy;
    public void Init() {
        Init(enemyType);
    }

    public void Init(EnemyType type) {
        gameObject.SetActive(true);
        GameObject enemyObj = Instantiate(StaticManager.enemyTemplates[type], transform);
        enemyObj.transform.position = transform.position;
        StartCoroutine(Despawn());
        enemy = enemyObj.GetComponent<Enemy>();
    }

    private IEnumerator Despawn() {
        yield return new WaitForSeconds(1f);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

}
