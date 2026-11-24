using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    public InputAction escPressed;
    public GameObject pauseMenu;
    public TextMeshProUGUI speedInfo;
    public TextMeshProUGUI geerInfo;
    public CarController carController;

    private bool isPaused = false;

    private void PauseGame()
    {
        pauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true; // Make the cursor visible
        Time.timeScale = 0f; // Pause the game
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
        Cursor.visible = false; // Hide the cursor
        Time.timeScale = 1f; // Resume the game
        isPaused = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        escPressed.Enable();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor at the start
        Cursor.visible = false; // Hide the cursor at the start
    }

    // Update is called once per frame
    void Update()
    {
        if (escPressed.triggered)
        {
            // Handle the escape key press
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        int speedKmh = Mathf.RoundToInt(carController.GetSpeed() * 3.6f);
        string geer = carController.IsReverse() ? "R" : "D";
        speedInfo.text = speedKmh.ToString();
        geerInfo.text = geer;

    }
}
