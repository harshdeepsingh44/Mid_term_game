using UnityEngine;

public class ScreenBounds : MonoBehaviour
{
    void Start()
    {
        Camera mainCamera = Camera.main;
        Vector2 screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));

        // Create walls
        CreateWall("LeftWall", new Vector2(-screenBounds.x - 0.5f, 0), new Vector2(1, screenBounds.y * 2));
        CreateWall("RightWall", new Vector2(screenBounds.x + 0.5f, 0), new Vector2(1, screenBounds.y * 2));
        CreateWall("TopWall", new Vector2(0, screenBounds.y + 0.5f), new Vector2(screenBounds.x * 2, 1));
    }

    void CreateWall(string name, Vector2 position, Vector2 size)
    {
        GameObject wall = new GameObject(name);
        wall.transform.parent = transform;
        wall.transform.position = position;

        // Add BoxCollider2D
        BoxCollider2D collider = wall.AddComponent<BoxCollider2D>();
        collider.size = size;
    }
}
