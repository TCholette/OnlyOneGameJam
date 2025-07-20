using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour
{
    [SerializeField] private int index;
    [SerializeField] private GameObject entry;
    [SerializeField] private GameObject exit;
    [SerializeField] private EnemySpawnPoint[] spawns;

    private System.Collections.Generic.List<Enemy> enemies = new();
    private bool _finished = false;
    private bool _started = false;
    private Player player;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player") && !_started) {
            _started = true;
            player = collision.GetComponent<Player>();
            Init();
        }
    }
    private void Init() {
        Debug.Log("init");
        exit.SetActive(true);
        entry.SetActive(true);
        foreach (EnemySpawnPoint spawn in spawns) {
            spawn.Init();
            enemies.Add(spawn.enemy);
        }
        StartCoroutine(CheckWin());
    }
    public void Restart() {
        Debug.Log("restart");
        _finished = false;
        foreach (Enemy enemy in enemies) {
            if (enemy != null) {
                Destroy(enemy.gameObject);
            }
        }
        enemies = new();
        _started = false;
    }

    private IEnumerator CheckWin()
    {
        while (!_finished) {

            if (enemies.Count == 0) {
                _finished = true;
            } else {
                foreach (Enemy enemy in enemies) {
                    if (enemy != null) {
                        _finished = false;
                        break;
                    } else {
                        _finished = true;
                    }
                }
            }
            if (_finished) {
                Debug.Log("win");
                exit.SetActive(false);
            }
            if (player.IsDead) {
                Restart();
                break;
            }
            yield return null;
        }
        while (_finished) {
            if (player.IsDead) {
                Restart();
                break;
            }
            yield return null;
        }
    }

    private void Update() {
    }
}
