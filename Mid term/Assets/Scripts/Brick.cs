using UnityEngine;

public class Brick : MonoBehaviour
{
    public bool isWhiteBrick = false;  // Set this in Inspector
    private int currentHits = 0;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetInitialColor();
        
        // Make sure we have required components
        if (GetComponent<BoxCollider2D>() == null)
        {
            BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
            collider.isTrigger = false;
        }
        
        // Set the brick tag
        gameObject.tag = "Brick";
        
        Debug.Log($"Brick initialized: {(isWhiteBrick ? "White" : "Yellow")} brick, requires {(isWhiteBrick ? 2 : 1)} hits");
    }

    private void SetInitialColor()
    {
        if (isWhiteBrick)
        {
            spriteRenderer.color = new Color(0.9f, 0.9f, 0.9f, 1f);  // White
        }
        else
        {
            spriteRenderer.color = new Color(1f, 0.92f, 0.016f, 1f);  // Yellow
        }
    }

    public void TakeHit()
    {
        currentHits++;
        int hitsNeeded = isWhiteBrick ? 2 : 1;
        Debug.Log($"Brick hit! Hits: {currentHits}/{hitsNeeded}");

        if (currentHits >= hitsNeeded)
        {
            Debug.Log("Brick destroyed!");
            GameManager.Instance.BrickDestroyed();
            Destroy(gameObject);
        }
        else if (isWhiteBrick)
        {
            // Change white brick color to indicate damage
            spriteRenderer.color = new Color(0.7f, 0.7f, 0.7f, 1f);  // Darker white
        }
    }
}
