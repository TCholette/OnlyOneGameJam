using System.Collections;
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

    private float _life;
    private float _lifeBarBaseScale;
    private int _bleeding = 0;
    private bool _isBleeding = false;
    private bool _isDead = false;
    private int _charges;
    private PlayerMovement _movement;
    public bool IsDead { get { return _isDead; } }
    public int Charges { get { return _charges; } set { _charges = value; } }
    public GameObject WeaponHitbox { get { return _weaponHitbox; } }
    public PlayerMovement Movement { get { return _movement; } }

    private AbsAttack attack;

    private bool test = false;


    public void removeCharge() {
        _charges--;
        UpdateChargeUI();
    }

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
        float rand = Random.Range(lifeBar.transform.position.x - 300, lifeBar.transform.position.x + 300);
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
