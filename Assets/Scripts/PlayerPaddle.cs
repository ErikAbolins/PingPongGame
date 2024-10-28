using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPaddle : MonoBehaviour
{
    private Rigidbody2D rb;
    
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        // Get mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    
        // Calculate movement direction along both X and Y axes
        Vector2 moveDirection = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
    
        // Set the paddle's velocity, adjusting speed as needed
        float speed = 10f; // Increase this value to make it faster
        rb.velocity = moveDirection * speed;

        // Clamp the paddle within the game field boundaries
        rb.position = new Vector2(
            Mathf.Clamp(rb.position.x, -6.5f, 6.5f),
            Mathf.Clamp(rb.position.y, -3.5f, 3.5f)
        );

        //clamp the paddle within the player side of the table
        rb.position = new Vector2(
        Mathf.Clamp(rb.position.x, -6.5f, 6.5f), // Adjust if necessary for X-axis
        Mathf.Clamp(rb.position.y, -3.5f, -1.0f)
        );


    }

}
