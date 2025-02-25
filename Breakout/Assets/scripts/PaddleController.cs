using UnityEngine;

public class PaddleController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float boundaryOffset = 0.5f; // Offset from screen edges
    
    private float screenHalfWidth;
    private Camera mainCamera;
    private string horizontalAxis = "Horizontal";
    private bool isInitialized = false;

    private void Start()
    {
        InitializePaddle();
    }

    private void InitializePaddle()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found! Please ensure there is a camera tagged as 'MainCamera' in the scene.");
            return;
        }

        // Calculate screen boundaries in world coordinates
        float screenRatio = (float)Screen.width / Screen.height;
        screenHalfWidth = mainCamera.orthographicSize * screenRatio;
        isInitialized = true;
    }

    private void Update()
    {
        if (!isInitialized)
        {
            InitializePaddle();
            return;
        }

        MovePaddle();
    }

    private void MovePaddle()
    {
        // Get input
        float horizontalInput = Input.GetAxis(horizontalAxis);
        
        // Calculate new position
        Vector3 newPosition = transform.position + Vector3.right * (horizontalInput * moveSpeed * Time.deltaTime);
        
        // Clamp position to screen bounds
        float clampedX = Mathf.Clamp(newPosition.x, -screenHalfWidth + boundaryOffset, screenHalfWidth - boundaryOffset);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }
}
