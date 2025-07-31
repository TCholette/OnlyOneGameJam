using System.Collections;
using UnityEngine;


public enum EnemyType {
    devil = 0,
    imp = 1,
    beast = 2, 
}
public class Enemy : MonoBehaviour
{
    private Rigidbody2D _body;
    public Rigidbody2D Body { get { return _body;}}
    private AbsEnemyAI _aI;
    public EnemyType type;
    private Weapon _weaponType;
    private string MOVE_SOUND;

    [SerializeField] public GameObject hitbox;
    public Weapon Weapon {  get { return _weaponType; } }

    private bool _isDead = false;
    public void Start() {
        _body = GetComponent<Rigidbody2D>();
        if (type == EnemyType.devil) {
            _aI = new BasicEnemyAI(this);
            _weaponType = Weapon.Spear;
            MOVE_SOUND = "Walk";
        }
        if (type == EnemyType.imp) {
            _aI = new FlyingEnemyAI(this);
            _weaponType = Weapon.Spear;
            MOVE_SOUND = "Fly";
        }
        if (type == EnemyType.beast) {
            _aI = new GuardingEnemyAI(this);
            _weaponType = Weapon.Shield;
            MOVE_SOUND = "Walk";
        }
        _aI.Init();
    }

    private void Update() {
    }

    public void PlayBossMoveSound() {
        ProxyFmodPlayer.PlaySound<string>("BossMove", gameObject, new("MoveState", "Walk"));
    }

    public void PlayBasicMoveSound() {
        ProxyFmodPlayer.PlaySound<string>("CrawlerMove", gameObject, new("MoveState", "Walk"));
    }

    public void PlayImpMoveSound() {
        ProxyFmodPlayer.PlaySound<string>(MOVE_SOUND, gameObject);
    }

    public void PlayAttackSound() {
        ProxyFmodPlayer.PlaySound<int>("DemonAttack", gameObject, new("EnemyType", (int)type));
    }

    public Weapon Hit() {
        if (this) {
            if (_aI.Hit()) return _weaponType;
        }
        return Weapon.None;
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
                float x = Random.Range(-5f, 5f);
                float y = Random.Range(-5f, 5f);
                float size = Random.Range(0.1f, 0.6f);
                fleshObj.GetComponent<SpriteRenderer>().sprite = flesh;
                fleshObj.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(x, y);
                fleshObj.transform.localScale = new Vector3(size, size);
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
