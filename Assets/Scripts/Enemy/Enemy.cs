using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _speed = 5.0f;
    public float Speed { get { return _speed; } } 
    private Rigidbody2D _enemyBody;
    private Collider2D _collider;
    public Rigidbody2D Body { get { return _enemyBody;}}
    private AbsEnemyAI _aI;
    private void Start() {
        _enemyBody = GetComponent<Rigidbody2D>();
        _aI = new BasicEnemyAI(this);
        _collider = GetComponent<CircleCollider2D>();
    }
    public void Kill() {
        this.StartCoroutine(Die());
    }
    private IEnumerator Die() {
        //add logic that happens before death
        this.gameObject.SetActive(false);
        yield return null;
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            _aI.SetPlayerAndTrack(collision.gameObject.GetComponent<Player>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            _aI.SetPlayerAndTrack(null);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            Debug.Log("You are hit");
        }
    }
}
