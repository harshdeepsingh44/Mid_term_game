using UnityEngine;

public class WallSetup : MonoBehaviour
{
    public float wallThickness = 1f;

    void Start()
    {
        Camera mainCamera = Camera.main;
        float height = mainCamera.orthographicSize * 2;
        float width = height * mainCamera.aspect;

        // Create walls
        CreateWall("TopWall", new Vector3(0, mainCamera.orthographicSize, 0), new Vector3(width, wallThickness, 1));
        CreateWall("LeftWall", new Vector3(-width/2, 0, 0), new Vector3(wallThickness, height, 1));
        CreateWall("RightWall", new Vector3(width/2, 0, 0), new Vector3(wallThickness, height, 1));
    }

    void CreateWall(string name, Vector3 position, Vector3 scale)
    {
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.name = name;
        wall.transform.parent = transform;
        wall.transform.position = position;
        wall.transform.localScale = scale;
        
        // Remove the mesh renderer to make it invisible
        Destroy(wall.GetComponent<MeshRenderer>());
        
        // Add Rigidbody2D and configure it
        Rigidbody2D rb = wall.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
        
        // Replace the BoxCollider with a BoxCollider2D
        Destroy(wall.GetComponent<BoxCollider>());
        wall.AddComponent<BoxCollider2D>();
    }
}
