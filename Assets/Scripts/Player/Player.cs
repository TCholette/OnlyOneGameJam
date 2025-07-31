using FMOD.Studio;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Player : MonoBehaviour {
    private const float BLEED_TICK_TIME = 0.05f;
    private const float MAX_LIFE = 1000;
    private const float HEALTHY_LIFE_THRESHOLD = MAX_LIFE - (2 * (MAX_LIFE/10));
    private const float MID_LIFE_THREHSOLD = MAX_LIFE / 2;
    private const float DYING_LIFE_THRESHOLD = (2 * (MAX_LIFE / 10));
    private const int MAX_CHARGES = 1;

    [SerializeField] private GameObject lifeBar;
    [SerializeField] private GameObject leftBar;
    [SerializeField] private GameObject rightBar;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject dropletTemplate;
    [SerializeField] private Transform tearContainer;
    [SerializeField] private GameObject chargeContainer;
    [SerializeField] private GameObject _weaponHitbox;
    [SerializeField] private GameObject _PantheonObject;
    [SerializeField] private GameObject _weaponWheel;
    [SerializeField] private GameObject[] _weapons;
    private EventInstance healthSounds;
    private float heartIntensity = 0f;
    private Pantheon _pantheon;

    private float _life;
    private float _lifeBarBaseScale;
    private Vector2 _leftBarPos;
    private Vector2 _rightBarPos;
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
    public AbsAttack Attack { set { attack = value; } }


    private AbsAttack attack;

    private int test = 0;

    public WeaponSelect LastWheel;


    public void HitEnemy(Enemy enemy) {
        Weapon compareWeapon = enemy.Hit();
        if (!_pantheon.IsPopulated) {
            if (_charges != 0) {
                ChangeCharges(_charges - 1);
            }
        }
        if (compareWeapon != Weapon.None) {
            if (_pantheon.IsPopulated) {
                ProxyFmodPlayer.SetParam<string>(StaticManager.gameMusic, new("Music", "Game"));
                StartCoroutine(ReturnFromPantheon());
                _pantheon.IsPopulated = false;
                ChangeCharges(0);
                StartCoroutine(enemy.Die());
            }
            else if (!SaveManager.Instance.save.weapons.Contains(compareWeapon)) {
                SaveManager.Instance.save.lastPosition = gameObject.transform.position;
                StartCoroutine(_pantheon.SendToPantheon(enemy.type));
                SaveManager.Instance.save.weapons.Add(compareWeapon);
                Debug.Log("weapon: " + (int)compareWeapon);
                _weapons[(int)compareWeapon].SetActive(true);
            }
            StartCoroutine(enemy.Die());
            ProxyFmodPlayer.PlaySound<int>("DemonDeath", gameObject, new ("EnemyType", (int)compareWeapon));
        }
    }

    private IEnumerator ReturnFromPantheon() {
        _movement.CanMove = false;
        _weaponHitbox.SetActive(false);
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        GetComponent<Animator>().SetTrigger("teleport");
        yield return new WaitForSeconds(2f);
        GetComponent<Collider2D>().enabled = true;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        gameObject.transform.position = SaveManager.Instance.save.lastPosition;
        _movement.CanMove = true;
        _movement.TempCheckpoint = null;
    }

   

    public void Hit(int damage, int bleeding, GameObject source) {
        if (!_isGuarding) {
            ProxyFmodPlayer.PlaySound<string>("PlayerHit", gameObject);
            LoseLife(damage);
            AddBleeding(bleeding);
        } else {
            attack.Special(_weaponHitbox, source);
        }
    }
    public void SwitchTest() {
        if (test == 0) {
            attack = new Slash(this);
        } else if (test == 1) {
            attack = new Dash(this, 15f, 0.1f, 1f, 0.1f);
        } else {
            attack = new Guard(this, 0.5f, 2f, 0.2f, 3f, 15f);
        }
        test++;
        test %= 3;
    }
    void Start() {
        healthSounds = (EventInstance)ProxyFmodPlayer.CreateSound<string>("Health", gameObject);
        foreach(GameObject weap in _weapons) {
            weap.SetActive(false);
        }
        _weapons[0].SetActive(true);
        _life = MAX_LIFE;
        _charges = MAX_CHARGES;
        _lifeBarBaseScale = lifeBar.transform.localScale.x;
        _movement = gameObject.GetComponent<PlayerMovement>();
        _pantheon = _PantheonObject.GetComponent<Pantheon>();
        _pantheon.Context = this;
        attack = new Slash(this);
        _leftBarPos = leftBar.transform.position;
        _rightBarPos = rightBar.transform.position;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            attack.Activate(_weaponHitbox);
            ProxyFmodPlayer.PlaySound<int>("ExecuteAbility", gameObject, new("AbilityTypes", (int)attack.Type));
        }
        if (Input.GetKey(KeyCode.Mouse1)) {
            _weaponWheel.SetActive(true);
        } else {
            _weaponWheel.SetActive(false);
        }
    }

    public void ChangeCharges(int amount) {
        _charges = amount;
        // replace code for better things
        if (_charges > 0) {
            chargeContainer.SetActive(true);
            }
        else {
            chargeContainer.SetActive(false);
        }
    }

    public void AddBleeding(int amount) {
        if (!_isDead && !_pantheon.IsPopulated) {
            _bleeding += amount;
            if (!_isBleeding) {
                _isBleeding = true;
                StartCoroutine(Bleed());
            }
        }
    }

    public void FullyHealAndRecharge() {

        //yield return new WaitForSeconds(2f);
        //_bleeding = 0;
        //_isBleeding = false;
        //_life = MAX_LIFE;
        //_charges = 0;
        //_charges = MAX_CHARGES;  no recharge after pantheon kill
        //UpdateChargeUI();
        //lifeBar.transform.localScale = new Vector3(_lifeBarBaseScale, lifeBar.transform.localScale.y);
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

    private void UpdateLifeSounds() {
        Debug.Log(_life);
        if (_life >= HEALTHY_LIFE_THRESHOLD) {
            healthSounds.stop(STOP_MODE.ALLOWFADEOUT);
            heartIntensity = 0f;
            Debug.Log("Stopping");
        }
        if (_life <= HEALTHY_LIFE_THRESHOLD) {
            PLAYBACK_STATE isPaused;
            healthSounds.getPlaybackState(out isPaused);
            if (isPaused == PLAYBACK_STATE.STOPPED) {
                healthSounds.start();
            }
            if (_life > MID_LIFE_THREHSOLD) {
                ProxyFmodPlayer.SetParam<string>(healthSounds, new("Health", "Low"));
                heartIntensity = 0f;
                Debug.Log("Starting");
            }
            else if (_life <= MID_LIFE_THREHSOLD && _life > DYING_LIFE_THRESHOLD) {
                ProxyFmodPlayer.SetParam<string>(healthSounds, new("Health", "Mid"));
                ProxyFmodPlayer.SetParam<float>(healthSounds, new("HealthIntensity", heartIntensity));
                heartIntensity += 1f/DYING_LIFE_THRESHOLD;
                Debug.Log("Mid");
            }
        }
    }

    private void UpdateLifeBar(float amount) {
        UpdateLifeSounds();
        leftBar.transform.position -= new Vector3(amount * _lifeBarBaseScale*100/(2*MAX_LIFE),0f,0f);
        rightBar.transform.position += new Vector3(amount * _lifeBarBaseScale*100/(2 * MAX_LIFE), 0f, 0f);
        lifeBar.transform.localScale = new Vector3(_life / MAX_LIFE * _lifeBarBaseScale, lifeBar.transform.localScale.y);
    }

    public void LoseLife(int amount) {
        if (!_isDead && !_pantheon.IsPopulated) {
            if (_life > amount) {
                _life -= amount;
            } else {
                _life = 0;
            }
            if (_life <= 0) {
                Die();
            }
            UpdateLifeBar(-amount);
        }
    }
    public void Heal(int life, int bleed) {
        if (!_isDead) {
            if (_life + life <= MAX_LIFE) {
                _life += life;
            } else {
                _life = MAX_LIFE;
            }
            if (_bleeding > bleed) {
                _bleeding -= bleed;
            } else {
                _bleeding = 0;
            }
        }
        UpdateLifeBar(life);
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
            healthSounds.stop(STOP_MODE.ALLOWFADEOUT);
            ProxyFmodPlayer.PlaySound<string>("PlayerDeath", gameObject);
            StartCoroutine(Respawn());
        }
    }

    private IEnumerator Respawn() {
        yield return new WaitForSeconds(0f);
        _isDead = false;
        rightBar.transform.position = _rightBarPos;
        leftBar.transform.position = _leftBarPos;
        lifeBar.transform.localScale = new Vector3(_lifeBarBaseScale, lifeBar.transform.localScale.y);
        ChangeCharges(MAX_CHARGES);
        _life = MAX_LIFE;
        _movement.Respawn();
        ProxyFmodPlayer.PlaySound<string>("PlayerRespawn", gameObject);
    }
    public void TestBleed(int amount) {
        for (int i = 0; i < amount; i++) {
            AddBleeding(1);
        }
    }
}
