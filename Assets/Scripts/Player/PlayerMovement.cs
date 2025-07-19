using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    private const float JUMP_FORCE = 10f;
    private const float MOVE_FORCE = 6f;
    private const float FRICTION = 0.01f;
    private const float GROUND_FRICTION_RATIO = 5f;

    private bool _canMove = true;
    private bool _isGrounded = false;
    private Rigidbody2D _body;
    private Vector3 _checkpointPosition;

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Ground")) {
            _isGrounded = true;
        } 
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Checkpoint")) {
            _checkpointPosition = collision.transform.position;
        }
    }

    private void Start() {
        _body = gameObject.GetComponent<Rigidbody2D>();
    }

    public void Die() {
        _canMove = false;
    }

    public void Respawn() {
        _body.bodyType = RigidbodyType2D.Static;
        transform.position = _checkpointPosition;
        _body.bodyType = RigidbodyType2D.Dynamic;
        _canMove = true;
    }
    void Update()
    {
        float newFriction = FRICTION;

        if (_isGrounded) {
            newFriction = FRICTION * GROUND_FRICTION_RATIO;
        }
        if (_canMove) {
            if (Input.GetKey(KeyCode.Space) && _isGrounded) {
                _isGrounded = false;
                _body.linearVelocityY = JUMP_FORCE;
            }

            if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A)) {
                _body.linearVelocityX = Mathf.Lerp(_body.linearVelocityX, 0, newFriction);
            } else if (Input.GetKey(KeyCode.D)) {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
                _body.linearVelocityX = MOVE_FORCE;
            } else if (Input.GetKey(KeyCode.A)) {
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
                _body.linearVelocityX = -MOVE_FORCE;
            } else {
                _body.linearVelocityX = Mathf.Lerp(_body.linearVelocityX, 0, newFriction);
            }
        }
       
    }
}
