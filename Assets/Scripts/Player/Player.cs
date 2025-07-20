using UnityEngine;
using System.Collections;
using TMPro;

public class Player : MonoBehaviour {
    private const float BLEED_TICK_TIME = 0.05f;
    private const float MAX_LIFE = 1000;
    private const int MAX_CHARGES = 1;

    [SerializeField] private GameObject lifeBar;
    [SerializeField] private TextMeshProUGUI bleedText;
    [SerializeField] private GameObject _weaponHitbox;

    private float _life;
    private float _lifeBarBaseScale;
    private int _bleeding = 0;
    private bool _isBleeding = false;
    private bool _isDead = false;
    private int _charges;
    private PlayerMovement _movement;
    public int Charges { get { return _charges; } set { _charges = value; } }
    public GameObject WeaponHitbox { get { return _weaponHitbox; } }
    public PlayerMovement Movement { get { return _movement; } }

    private AbsAttack attack;

    private bool test = false;

    public void SwitchTest() {
        if (test) {
            attack = new Slash(this);
        } else {
            attack = new Dash(this, 15f, 0.1f, 1f, 0.1f);
        }
    }
    void Start() {
        _life = MAX_LIFE;
        _charges = MAX_CHARGES;
        _lifeBarBaseScale = lifeBar.transform.localScale.x;
        _movement = gameObject.GetComponent<PlayerMovement>();
        attack = new Slash(this);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            attack.Activate(_weaponHitbox);
        }
    }

    public void AddBleeding(int amount) {
        if (!_isDead) {
            _bleeding += amount;
            bleedText.text = _bleeding.ToString();
            if (!_isBleeding) {
                _isBleeding = true;
                StartCoroutine(Bleed());
            }
        }
    }

    private IEnumerator Bleed() {
        while (_isBleeding) {
            LoseLife(_bleeding);
            yield return new WaitForSeconds(BLEED_TICK_TIME);
        }
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
            bleedText.text = _bleeding.ToString();
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
        _life = MAX_LIFE;
        _movement.Respawn();
    }
    public void TestBleed(int amount) {
        for (int i = 0; i < amount; i++) {
            AddBleeding(1);
        }
    }
}
