using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public int totalBricks = 0;
    public int remainingBricks;
    public bool gameOver = false;
    public bool gameWon = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        try
        {
            ResetGame();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to initialize game: {e.Message}");
            // Attempt to recover by reloading the scene
            RestartGame();
        }
    }

    private void Update()
    {
        // Only allow restart if game is over or won
        if ((gameOver || gameWon) && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }

        // Add escape key to return to menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnToMenu();
        }
    }

    private void ResetGame()
    {
        // Reset game state
        gameOver = false;
        gameWon = false;

        // Count total bricks at start
        GameObject[] bricks = GameObject.FindGameObjectsWithTag("Brick");
        if (bricks == null || bricks.Length == 0)
        {
            Debug.LogWarning("No bricks found in the scene!");
        }
        totalBricks = bricks.Length;
        remainingBricks = totalBricks;

        // Find and reset ball
        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        if (ball != null)
        {
            Ball ballScript = ball.GetComponent<Ball>();
            if (ballScript != null)
            {
                ballScript.ResetBall();
            }
        }

        // Reset paddle position
        GameObject paddle = GameObject.FindGameObjectWithTag("Paddle");
        if (paddle != null)
        {
            paddle.transform.position = new Vector3(0, paddle.transform.position.y, 0);
        }

        Debug.Log("Game Reset - Press SPACE to launch the ball!");
    }

    public void BrickDestroyed()
    {
        remainingBricks--;
        
        if (remainingBricks <= 0)
        {
            WinGame();
        }
    }

    public void WinGame()
    {
        gameWon = true;
        Debug.Log("Congratulations! You've won the game!");
        // Optional: Restart game after delay
        Invoke("RestartGame", 3f);
    }

    public void LoseGame()
    {
        gameOver = true;
        Debug.Log("Game Over!");
        // Optional: Restart game after delay
        Invoke("RestartGame", 3f);
    }

    public void RestartGame()
    {
        try
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to restart game: {e.Message}");
            // Fallback to loading the first scene
            SceneManager.LoadScene(0);
        }
    }

    private void ReturnToMenu()
    {
        try
        {
            SceneManager.LoadScene(0); // Assuming menu is always scene 0
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to return to menu: {e.Message}");
        }
    }
}
