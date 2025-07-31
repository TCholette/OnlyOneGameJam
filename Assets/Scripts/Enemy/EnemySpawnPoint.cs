using UnityEngine;
using System.Collections;

public class EnemySpawnPoint : MonoBehaviour {

    [SerializeField] private EnemyType enemyType;
    public Enemy enemy;
    public void Init(bool isMandatory) {
        Init(enemyType, isMandatory);
    }

    public void Init(EnemyType type, bool isMandatory) {
        gameObject.SetActive(true);
        GameObject enemyObj = Instantiate(StaticManager.enemyTemplates[type], transform);
        enemyObj.transform.position = transform.position;
        StartCoroutine(Despawn());
        enemy = enemyObj.GetComponent<Enemy>();
        if (isMandatory) {
            enemyObj.GetComponent<SpriteRenderer>().color = new Color(1f, 0.5f, 0.5f, 1f);
        }
    }

    private IEnumerator Despawn() {
        yield return new WaitForSeconds(1f);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

}
