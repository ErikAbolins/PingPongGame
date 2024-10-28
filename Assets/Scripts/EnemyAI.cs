using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
  
    public Transform ball; // Reference to the ball
    public float speed = 10f; // Speed of the enemy paddle movement
    public float edgePosition = -3.5f; // The Y position to stay at the back of their side
    public float approachThreshold = 1.5f; // Distance from center line to start aligning with ball's X position
    public float returnForce = 10f; // The force with which the paddle hits the ball
    public float targetRange = 6.5f; // X-axis range within the player's side for aiming

    private Rigidbody2D rb;
    private Rigidbody2D ballRb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ballRb = ball.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        if (ball.position.y > -approachThreshold) // Ball is approaching or on enemy side
        {
            float predictedX = PredictBallXPosition();
            Vector2 targetPosition = new Vector2(predictedX, Mathf.Clamp(ball.position.y, edgePosition, 0));
            rb.MovePosition(Vector2.Lerp(rb.position, targetPosition, speed * Time.deltaTime));
        }
        else
        {
            rb.MovePosition(new Vector2(transform.position.x, edgePosition));
        }
    }

    float PredictBallXPosition()
    {
        if (ballRb.velocity.y > 0)
        {
            float timeToReachEnemy = Mathf.Abs(ball.position.y) / ballRb.velocity.y;
            float predictedX = ball.position.x + ballRb.velocity.x * timeToReachEnemy;
            return Mathf.Clamp(predictedX, -6.5f, 6.5f);
        }
        else
        {
            return ball.position.x;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            AimAndHitBall();
        }
    }

    void AimAndHitBall()
    {
        // Choose a random X position within the player's side to target
        float targetX = Random.Range(-targetRange, targetRange);
        
        // Calculate the vector from the ball's current position to the target position on the player's side
        Vector2 targetPosition = new Vector2(targetX, -3.5f); // Adjust -3.5f based on player side's Y range
        Vector2 aimDirection = (targetPosition - (Vector2)ball.position).normalized;
        
        // Apply force in the direction of the target position
        ballRb.velocity = aimDirection * returnForce;
    }
}