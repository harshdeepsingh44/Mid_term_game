using UnityEngine;

public class Ball : MonoBehaviour
{
    public float initialSpeed = 8f;
    public float speedIncrease = 0.1f; // Speed increases slightly with each bounce
    public float maxSpeed = 15f;
    
    private Rigidbody2D rb;
    private Vector2 lastVelocity;
    private bool gameStarted = false;
    private Camera mainCamera;
    private Vector2 screenBounds;
    private float ballRadius;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        
        // Get ball radius from either CircleCollider2D or transform scale
        CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();
        if (circleCollider != null)
            ballRadius = circleCollider.radius * transform.localScale.x;
        else
            ballRadius = transform.localScale.x / 2;

        ResetBall();
    }

    private void Update()
    {
        if (!gameStarted && Input.GetKeyDown(KeyCode.Space))
        {
            LaunchBall();
        }

        if (gameStarted)
        {
            // Check screen boundaries and bounce
            Vector2 pos = transform.position;

            // Bounce off left and right edges
            if (pos.x < -screenBounds.x + ballRadius && rb.linearVelocity.x < 0)
            {
                rb.linearVelocity = new Vector2(-rb.linearVelocity.x, rb.linearVelocity.y);
            }
            else if (pos.x > screenBounds.x - ballRadius && rb.linearVelocity.x > 0)
            {
                rb.linearVelocity = new Vector2(-rb.linearVelocity.x, rb.linearVelocity.y);
            }

            // Bounce off top
            if (pos.y > screenBounds.y - ballRadius && rb.linearVelocity.y > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, -rb.linearVelocity.y);
            }
        }

        // Store last velocity for other bounce calculations
        lastVelocity = rb.linearVelocity;

        // Check if ball is below paddle (game over condition)
        if (transform.position.y < -screenBounds.y)
        {
            GameManager.Instance.LoseGame();
            ResetBall();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!gameStarted) return;

        Debug.Log($"Ball hit: {collision.gameObject.name}");

        // Handle brick collisions
        if (collision.gameObject.CompareTag("Brick"))
        {
            Brick brick = collision.gameObject.GetComponent<Brick>();
            if (brick != null)
            {
                brick.TakeHit();
            }
        }

        // Calculate reflection
        var speed = lastVelocity.magnitude;
        var direction = Vector2.Reflect(lastVelocity.normalized, collision.contacts[0].normal);
        
        // Add a small speed increase with each bounce, up to max speed
        speed = Mathf.Min(speed + speedIncrease, maxSpeed);
        
        // Apply the new velocity
        rb.linearVelocity = direction * speed;

        // Add some randomness to paddle bounces to make game more interesting
        if (collision.gameObject.CompareTag("Paddle"))
        {
            float hitPoint = (transform.position.x - collision.transform.position.x) / 
                           collision.collider.bounds.size.x;
            
            // Add slight angle change based on where the ball hits the paddle
            float angle = hitPoint * 60f; // Max 60 degree deflection
            rb.linearVelocity = Quaternion.Euler(0, 0, angle) * rb.linearVelocity;
        }
    }

    public void ResetBall()
    {
        gameStarted = false;
        rb.linearVelocity = Vector2.zero;
        transform.position = new Vector3(0, 2f, 0); // Position higher above paddle
    }

    private void LaunchBall()
    {
        gameStarted = true;
        float randomAngle = Random.Range(-30f, 30f);
        Vector2 direction = Quaternion.Euler(0, 0, randomAngle) * Vector2.up;
        rb.linearVelocity = direction * initialSpeed;
    }
}
