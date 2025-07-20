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
    [SerializeField] private float _speed = 5f;
    public float Speed { get { return _speed; } } 
    private Rigidbody2D _body;
    public Rigidbody2D Body { get { return _body;}}
    private AbsEnemyAI _aI;
    [SerializeField] private EnemyType type;
    public float followRange;

    [SerializeField] private float shootSpeed; //degager de la

    private bool _isDead = false;
    private void Start() {
        _body = GetComponent<Rigidbody2D>();
        if (type == EnemyType.devil) {
            _aI = new BasicEnemyAI(this);
        }
        if (type == EnemyType.imp) {
            _aI = new FlyingEnemyAI(this);
        }
        _aI.Init();
    }

    private void Update() {
    }
    public void Kill() {
        if (this) {
            StartCoroutine(Die());
        }
    }
    private IEnumerator Die() {
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

    public void Shoot(Player target) {
        GameObject projectile = Instantiate(StaticManager.projectile);
        projectile.transform.position = transform.position;
        projectile.GetComponent<Rigidbody2D>().linearVelocity = (target.transform.position-transform.position).normalized * shootSpeed;
    }
}
