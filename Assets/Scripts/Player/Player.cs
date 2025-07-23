using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Player : MonoBehaviour {
    private const float BLEED_TICK_TIME = 0.05f;
    private const float MAX_LIFE = 1000;
    private const int MAX_CHARGES = 1;

    [SerializeField] private GameObject lifeBar;
    [SerializeField] private GameObject dropletTemplate;
    [SerializeField] private Transform tearContainer;
    [SerializeField] private GameObject chargeContainer;
    [SerializeField] private GameObject _weaponHitbox;
    [SerializeField] private GameObject _PantheonObject;
    private Pantheon _pantheon;

    private float _life;
    private float _lifeBarBaseScale;
    private int _bleeding = 0;
    private bool _isBleeding = false;
    private bool _isDead = false;
    private int _charges;
    private bool _isAttackCooldown;
    private bool _isGuarding;
    private PlayerMovement _movement;
    public bool IsAttackCooldown { get { return _isAttackCooldown; } set { _isAttackCooldown = value; } }
    public bool IsGuarding { /*get { return _isAttackCooldown; }*/ set { _isGuarding = value; } }
    public bool IsDead { get { return _isDead; } }
    public int Charges { get { return _charges; } set { _charges = value; } }
    public GameObject WeaponHitbox { get { return _weaponHitbox; } }
    public PlayerMovement Movement { get { return _movement; } }

    private AbsAttack attack;

    private int test = 0;


    public void HitEnemy(Enemy enemy) {
        Weapon compareWeapon = enemy.Hit();
        if (_pantheon.IsPopulated && compareWeapon != Weapon.None) {
            _pantheon.IsPopulated = false;
            _movement.CanMove = false;
            gameObject.transform.position = SaveManager.Instance.save.lastPosition;
            _movement.CanMove = true;
            _movement.TempCheckpoint = null;
            FullyHealAndRecharge();
            return;
        }
        if (compareWeapon != Weapon.None) { 
            if (!SaveManager.Instance.save.weapons.Contains(compareWeapon)) {
                SaveManager.Instance.save.lastPosition = gameObject.transform.position;
                FullyHealAndRecharge();
                StartCoroutine(_pantheon.SendToPantheon(enemy.type));
                SaveManager.Instance.save.weapons.Add(compareWeapon);
            }
            StartCoroutine(enemy.Die());
            return;
        }
        _charges--;
        UpdateChargeUI();
        foreach (Weapon wep in SaveManager.Instance.save.weapons) {
            Debug.Log(wep);
        }
    }

   

    public void Hit(int damage, int bleeding, GameObject source) {
        if (!_isGuarding) {
            LoseLife(damage);
            AddBleeding(bleeding);
        } else {
            attack.Special(_weaponHitbox, source);
        }
    }
    public void SwitchTest() {
        test++;
        if (test == 0) {
            attack = new Slash(this);
        } else if (test == 1) {
            attack = new Dash(this, 15f, 0.1f, 1f, 0.1f);
        } else {
            attack = new Guard(this, 0.5f, 2f, 0.2f, 3f, 15f);
        }
        test %= 3;
    }
    void Start() {
        _life = MAX_LIFE;
        _charges = MAX_CHARGES;
        _lifeBarBaseScale = lifeBar.transform.localScale.x;
        _movement = gameObject.GetComponent<PlayerMovement>();
        _pantheon = _PantheonObject.GetComponent<Pantheon>();
        _pantheon.Context = this;
        attack = new Slash(this);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            attack.Activate(_weaponHitbox);
        }
    }

    public void UpdateChargeUI() {
        // replace code for better things
        if (_charges > 0) {
            chargeContainer.SetActive(true);
            }
        else {
            chargeContainer.SetActive(false);
        }
    }

    public void AddBleeding(int amount) {
        if (!_isDead) {
            _bleeding += amount;
            if (!_isBleeding) {
                _isBleeding = true;
                StartCoroutine(Bleed());
            }
        }
    }

    public void FullyHealAndRecharge() {
        _bleeding = 0;
        _isBleeding = false;
        _life = MAX_LIFE;
        _charges = MAX_CHARGES;
        UpdateChargeUI();
        lifeBar.transform.localScale = new Vector3(_lifeBarBaseScale, lifeBar.transform.localScale.y);
    }

    private IEnumerator Bleed() {
        int i = 0;
        while (_isBleeding) {
            LoseLife(_bleeding);
            if (i > 10) {
                i = 0;
                StartCoroutine(CreateDroplets());
            }
            i += _bleeding;
            yield return new WaitForSeconds(BLEED_TICK_TIME);
        }
    }

    private IEnumerator CreateDroplets() {
        int bleed = _bleeding;
        GameObject droplet = Instantiate(dropletTemplate, tearContainer);
        float rand = Random.Range(lifeBar.transform.position.x - lifeBar.transform.localScale.x * 100f / 2f, lifeBar.transform.position.x + lifeBar.transform.localScale.x*100f/2f);;
        droplet.transform.position = new Vector2(rand ,lifeBar.transform.position.y);
        yield return new WaitForSeconds(BLEED_TICK_TIME / (bleed*1f));

    }

    public void LoseLife(int amount) {
        if (!_isDead) {
            if (_life > amount) {
                _life -= amount;
                lifeBar.transform.localScale = new Vector3(_life / MAX_LIFE * _lifeBarBaseScale, lifeBar.transform.localScale.y);
            } else {
                _life = 0;
            }

            if (_life <= 0) {
                Die();
            }
        }
    }
    public void Die() {
        if (!_isDead) {
            _isDead = true;
            _isBleeding = false;
            _bleeding = 0;
            _life = 0;
            _movement.Die();
            lifeBar.transform.localScale = new Vector3(0, lifeBar.transform.localScale.y);
            //Add here
            StartCoroutine(Respawn());
        }
    }

    private IEnumerator Respawn() {
        yield return new WaitForSeconds(0f);
        _isDead = false;
        lifeBar.transform.localScale = new Vector3(_lifeBarBaseScale, lifeBar.transform.localScale.y);
        _charges = MAX_CHARGES;
        UpdateChargeUI();
        _life = MAX_LIFE;
        _movement.Respawn();
    }
    public void TestBleed(int amount) {
        for (int i = 0; i < amount; i++) {
            AddBleeding(1);
        }
    }
}
