using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Transform ball; // Reference to the ball
    public Transform playerPaddle; // Reference to the player's paddle
    public Transform enemyPaddle; // Reference to the enemy's paddle
    public Text playerScoreText; // UI Text to display the player's score
    public Text enemyScoreText; // UI Text to display the enemy's score
    public Vector2 playAreaBounds = new Vector2(7f, 4f); // Define the play area's boundary (X, Y)

    private Vector2 ballStartPosition;
    private Vector2 playerPaddleStartPosition;
    private Vector2 enemyPaddleStartPosition;
    private Rigidbody2D ballRb;

    private int playerScore = 0;
    private int enemyScore = 0;

    void Start()
    {
        // Store initial positions
        ballStartPosition = ball.position;
        playerPaddleStartPosition = playerPaddle.position;
        enemyPaddleStartPosition = enemyPaddle.position;
        ballRb = ball.GetComponent<Rigidbody2D>();

        // Initialize the score display
        UpdateScoreUI();
    }

    void Update()
    {
        CheckOutOfBounds();
    }

    void CheckOutOfBounds()
    {
        // If the ball's position is outside the defined play area bounds
        if (Mathf.Abs(ball.position.y) > playAreaBounds.y)
        {
            if (ball.position.y > playAreaBounds.y)
            {
                // Player missed, increase enemy score
                enemyScore++;
            }
            else
            {
                // Enemy missed, increase player score
                playerScore++;
            }

            // Update the score display
            UpdateScoreUI();

            // Reset the game state after a point is scored
            ResetGame();
        }
    }

    void UpdateScoreUI()
    {
        // Update the score display in the UI
        playerScoreText.text = playerScore.ToString();
        enemyScoreText.text = enemyScore.ToString();
    }

    void ResetGame()
    {
        // Stop ball movement and reset position
        ballRb.velocity = Vector2.zero;
        ball.position = ballStartPosition;

        // Reset paddles to starting positions
        playerPaddle.position = playerPaddleStartPosition;
        enemyPaddle.position = enemyPaddleStartPosition;

        // Optionally, add a delay before serving the ball
        Invoke("ServeBall", 1f); // Wait 1 second before serving
    }

    void ServeBall()
    {
        // Give the ball an initial random direction for a new rally
        float serveDirectionX = Random.Range(0, 2) == 0 ? -1 : 1; // Random left or right
        float serveDirectionY = Random.Range(0, 2) == 0 ? -1 : 1; // Random up or down
        ballRb.velocity = new Vector2(serveDirectionX, serveDirectionY).normalized * 5f; // Adjust speed as needed
    }
}

