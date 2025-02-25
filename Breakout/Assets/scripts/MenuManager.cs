using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI subtitleText;
    public TextMeshProUGUI nameText;
    public GameObject playButton;

    void Start()
    {
        // Set the text values
        if (titleText != null) titleText.text = "MDEV 1003\nMID-TERM";
        if (subtitleText != null) subtitleText.text = "Winter 2025";
        if (nameText != null) nameText.text = "By [YOUR NAME]";
    }

    public void OnPlayButtonClick()
    {
        // Load the game scene (make sure to name your game scene "Game")
        SceneManager.LoadScene("Game");
    }
}
