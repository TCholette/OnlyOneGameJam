using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    private const float BLEED_TICK_TIME = 0.1f;
    private const float MAX_LIFE = 1000;

    [SerializeField] private GameObject lifeBar;

    private float _life;
    private float _lifeBarBaseScale;
    private int _bleeding = 0;
    private bool _isBleeding = false;
    private int _charges = 0;
    private PlayerMovement _movement;
    public int Charges { get { return _charges; } set { _charges = value; } }
    


    void Start() {
        _life = MAX_LIFE;
        _lifeBarBaseScale = lifeBar.transform.localScale.x;
        _movement = gameObject.GetComponent<PlayerMovement>();
    }

    public void AddBleeding() {
        _bleeding++;
        if (!_isBleeding) {
            _isBleeding = true;
            StartCoroutine(Bleed());
        }
    }

    private IEnumerator Bleed() {
        while (_isBleeding) {
            LoseLife(_bleeding);
            if (_life <= 0) {
                Die();
            }
            yield return new WaitForSeconds(BLEED_TICK_TIME);
        }
    }

    private void LoseLife(int amount) {
        _life -= amount;
        lifeBar.transform.localScale = new Vector3(_life / MAX_LIFE * _lifeBarBaseScale, lifeBar.transform.localScale.y);
    }
    public void Die() {
        _isBleeding = false;
        _bleeding = 0;
        _life = 0;
        _movement.Die();
        lifeBar.transform.localScale = new Vector3(0, lifeBar.transform.localScale.y);
        //Add here
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn() {
        yield return new WaitForSeconds(2f);
        lifeBar.transform.localScale = new Vector3(_lifeBarBaseScale, lifeBar.transform.localScale.y);
        _life = MAX_LIFE;
        _movement.Respawn();
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }
    public void TestBleed(int amount) {
        for (int i = 0; i < amount; i++) {
            AddBleeding();
        }
    }
}
