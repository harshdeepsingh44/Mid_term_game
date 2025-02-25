using UnityEngine;

public class PaddleController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float boundaryOffset = 0.5f; // Offset from screen edges
    
    private float screenHalfWidth;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        // Calculate screen boundaries in world coordinates
        float screenRatio = (float)Screen.width / Screen.height;
        screenHalfWidth = mainCamera.orthographicSize * screenRatio;
    }

    private void Update()
    {
        if (GameManager.Instance.gameOver || GameManager.Instance.gameWon) return;

        // Get input
        float horizontalInput = Input.GetAxis("Horizontal");
        
        // Calculate new position
        Vector3 newPosition = transform.position + Vector3.right * (horizontalInput * moveSpeed * Time.deltaTime);
        
        // Clamp position to screen bounds
        float clampedX = Mathf.Clamp(newPosition.x, -screenHalfWidth + boundaryOffset, screenHalfWidth - boundaryOffset);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }
}
