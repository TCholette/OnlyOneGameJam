using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float jumpForce = 0.02f;
    public float moveForce = 0.02f;
    public float friction = 0.02f;
    public float groundFrictionRatio = 1f;

    public bool isGrounded = false;

    void Start() {
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Ground")) {
            isGrounded = true;
        }
    }

    void Update()
    {
        float newFriction = friction;
        if (isGrounded) {
            newFriction = friction * groundFrictionRatio;
        }

        if (Input.GetKey(KeyCode.Space) && isGrounded) {
            isGrounded = false;
            gameObject.GetComponent<Rigidbody2D>().linearVelocityY = jumpForce;
        }
        if (Input.GetKey(KeyCode.A)) {
            if (!Input.GetKey(KeyCode.D)) {
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
                gameObject.GetComponent<Rigidbody2D>().linearVelocityX = -moveForce;
            } else if (gameObject.GetComponent<Rigidbody2D>().linearVelocityX > 0) {
                gameObject.GetComponent<Rigidbody2D>().linearVelocityX -= newFriction;
            } else if (gameObject.GetComponent<Rigidbody2D>().linearVelocityX < 0) {
                gameObject.GetComponent<Rigidbody2D>().linearVelocityX += newFriction;
            }
        }
        else if (Input.GetKey(KeyCode.D)) {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
            gameObject.GetComponent<Rigidbody2D>().linearVelocityX = moveForce;
        } else if (gameObject.GetComponent<Rigidbody2D>().linearVelocityX > 0) {
            gameObject.GetComponent<Rigidbody2D>().linearVelocityX -= newFriction;
        } else if (gameObject.GetComponent<Rigidbody2D>().linearVelocityX < 0) {
            gameObject.GetComponent<Rigidbody2D>().linearVelocityX += newFriction;
        }
    }
}
