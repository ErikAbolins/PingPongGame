using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform ball; // Reference to the ball
    public float speed = 20f; // Speed of the enemy paddle movement (high to ensure accuracy)
    public float edgePosition = -3.5f; // The Y position to stay at the back of their side
    public Vector2 playAreaBounds = new Vector2(7f, 4f); // Define the play area's boundary (X, Y)

    private Rigidbody2D rb;
    private Rigidbody2D ballRb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ballRb = ball.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        TrackBall();
    }

    void TrackBall()
    {
        // Predict where the ball will reach the enemy's side (at the edge of the enemy's side)
        float predictedX = PredictBallXPosition();

        // Target position: align X with predicted ball position, keep Y at the enemy's edge
        Vector2 targetPosition = new Vector2(predictedX, edgePosition);

        // Move the paddle towards the target position smoothly
        rb.MovePosition(Vector2.Lerp(rb.position, targetPosition, speed * Time.deltaTime));
    }

    float PredictBallXPosition()
    {
        // If the ball is moving towards the enemy (positive Y velocity)
        if (ballRb.velocity.y > 0)
        {
            // Calculate time it will take for the ball to reach the enemy's Y edge
            float timeToReachEnemyEdge = Mathf.Abs((edgePosition - ball.position.y) / ballRb.velocity.y);

            // Predict the X position based on current X position and velocity
            float predictedX = ball.position.x + ballRb.velocity.x * timeToReachEnemyEdge;

            // Clamp within play area bounds to avoid moving out of bounds
            return Mathf.Clamp(predictedX, -playAreaBounds.x, playAreaBounds.x);
        }
        else
        {
            // If ball is moving away, just stay at current position
            return rb.position.x;
        }
    }
}
