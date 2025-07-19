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
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        _aI.SetPlayerAndTrack(collision.gameObject.GetComponent<Player>());
    }
}
