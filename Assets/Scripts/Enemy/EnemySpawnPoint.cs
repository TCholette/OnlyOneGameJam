using UnityEngine;
using System.Collections;

public class EnemySpawnPoint : MonoBehaviour {

    [SerializeField] private GameObject enemyType;
    [SerializeField] private GameObject[] enemyTypes;
    public Enemy enemy;
    public void Init() {
        gameObject.SetActive(true);
        GameObject enemyObj = Instantiate(enemyType, transform);
        enemyObj.transform.position = transform.position;
        StartCoroutine(Despawn());
        enemy = enemyObj.GetComponent<Enemy>();
    }

    public void Init(EnemyType type) {
        gameObject.SetActive(true);
        int i;
        switch (type) {
            case EnemyType.devil: 
                i = 0; 
                break;
            case EnemyType.imp:
                i = 1;
                break;
            case EnemyType.beast:
                i = 2;
                break;
            case EnemyType.brute:
                i = 3;
                break;
            default:
                i = 0;
                break;
        }
        GameObject enemyObj = Instantiate(enemyTypes[i], transform);
        enemyObj.transform.position = transform.position;
        StartCoroutine(Despawn());
        enemy = enemyObj.GetComponent<Enemy>();
    }

    private IEnumerator Despawn() {
        yield return new WaitForSeconds(1f);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

}
