using System.Collections;
using UnityEngine;


public enum EnemyType {
    devil,
    imp,
    beast,
    brute
}
public class Enemy : MonoBehaviour
{
    private Rigidbody2D _body;
    public Rigidbody2D Body { get { return _body;}}
    private AbsEnemyAI _aI;
    public EnemyType type;

    private bool _isDead = false;
    public void Start() {
        _body = GetComponent<Rigidbody2D>();
        if (type == EnemyType.devil) {
            _aI = new BasicEnemyAI(this);
        }
        if (type == EnemyType.imp) {
            _aI = new FlyingEnemyAI(this);
        }
        if (type == EnemyType.beast) {
            _aI = new GuardingEnemyAI(this);
        }
        _aI.Init();
    }

    private void Update() {
    }
    public void Hit() {
        if (this) {
            _aI.Hit();
        }
    }
    public IEnumerator Die() {
        if (!_isDead) {
            //FLESH
            int amount = Random.Range(5, 10);
            for (int i = 0; i < amount; i++) {
                int index = Random.Range(0, StaticManager.flesh.Length);
                Sprite flesh = StaticManager.flesh[index];
                GameObject fleshObj = Instantiate(StaticManager.fleshTemplate);
                fleshObj.transform.position = transform.position;
                float x = Random.Range(-50, 50) / 10f;
                float y = Random.Range(-50, 50) / 10f;
                fleshObj.GetComponent<SpriteRenderer>().sprite = flesh;
                fleshObj.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(x, y);
            }
            Destroy(gameObject);
            yield return null;
        }
    }

    public void Shoot(Player target, float shootSpeed) {
        GameObject projectile = Instantiate(StaticManager.projectile);
        projectile.transform.position = transform.position;
        projectile.GetComponent<Rigidbody2D>().linearVelocity = (target.transform.position-transform.position).normalized * shootSpeed;
    }
}
