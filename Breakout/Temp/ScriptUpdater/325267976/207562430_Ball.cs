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
        InitializeBall();
    }

    private void InitializeBall()
    {
        // Get required components
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component missing from ball!");
            return;
        }

        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found!");
            return;
        }

        // Calculate screen bounds accounting for camera position
        Vector3 cameraPos = mainCamera.transform.position;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Mathf.Abs(cameraPos.z)));
        
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
        
        // Ensure minimum velocity and add speed increase up to max
        speed = Mathf.Clamp(speed + speedIncrease, initialSpeed, maxSpeed);
        
        // Handle paddle collisions with special logic
        if (collision.gameObject.CompareTag("Paddle"))
        {
            // Prevent the ball from getting stuck moving horizontally
            if (Mathf.Abs(direction.y) < 0.2f)
            {
                direction.y = direction.y < 0 ? -0.2f : 0.2f;
                direction = direction.normalized;
            }

            float hitPoint = (transform.position.x - collision.transform.position.x) / 
                           collision.collider.bounds.size.x;
            
            // Add controlled angle change based on hit point
            float angle = Mathf.Clamp(hitPoint * 60f, -60f, 60f);
            direction = Quaternion.Euler(0, 0, angle) * direction;
        }

        // Apply the new velocity
        rb.linearVelocity = direction.normalized * speed;
    }

    public void ResetBall()
    {
        gameStarted = false;
        rb.linearVelocity = Vector2.zero;
        transform.position = new Vector3(0, 1f, 0); // Position slightly above paddle
    }

    private void LaunchBall()
    {
        if (!gameStarted && GameManager.Instance != null && !GameManager.Instance.gameOver)
        {
            gameStarted = true;
            float randomAngle = Random.Range(-30f, 30f);
            Vector2 direction = Quaternion.Euler(0, 0, randomAngle) * Vector2.up;
            rb.linearVelocity = direction * initialSpeed;
        }
    }
}
