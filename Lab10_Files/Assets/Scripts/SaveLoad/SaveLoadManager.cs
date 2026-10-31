using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

/*
 * Saves the game when the save action (S) is pressed and loads it when the load action (L) is pressed, and shows a short message on screen
*/

public class SaveLoadManager : MonoBehaviour
{
    [SerializeField] private InputActionReference saveAction;
    [SerializeField] private InputActionReference loadAction;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private string positionsFileName = "Positions.json";
    [SerializeField] private string scoreFileName = "Score.bin";
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private float statusDuration = 1.5f;

    private float statusHideTime;

    // Enables the save and load actions
    private void OnEnable()
    {
        if (saveAction != null)
        {
            saveAction.action.Enable();
        }

        if (loadAction != null)
        {
            loadAction.action.Enable();
        }
    }

    // Disables the save and load actions
    private void OnDisable()
    {
        if (saveAction != null)
        {
            saveAction.action.Disable();
        }

        if (loadAction != null)
        {
            loadAction.action.Disable();
        }
    }

    // Hides the status message until there is something to show
    private void Start()
    {
        if (statusText != null)
        {
            statusText.enabled = false;
        }
    }

    // Checks for the save and load keys and hides the status message once its time is up
    private void Update()
    {
        if (saveAction != null && saveAction.action.WasPressedThisFrame())
        {
            SaveGame();
        }
        else if (loadAction != null && loadAction.action.WasPressedThisFrame())
        {
            LoadGame();
        }

        if (statusText != null && statusText.enabled && Time.time >= statusHideTime)
        {
            statusText.enabled = false;
        }
    }

    // Saves every saveable object's location to JSON and the score to a binary file
    private void SaveGame()
    {
        SavingService.SaveGame(positionsFileName);

        if (scoreManager != null)
        {
            SavingService.SaveScore(scoreFileName, scoreManager.Score);
        }

        ShowStatus("Game Saved");
    }

    // Loads the locations from JSON and the score from the binary file
    private void LoadGame()
    {
        bool loadedPositions = SavingService.LoadGame(positionsFileName);
        bool loadedScore = false;

        if (scoreManager != null && SavingService.TryLoadScore(scoreFileName, out int score))
        {
            scoreManager.SetScore(score);
            loadedScore = true;
        }

        ShowStatus(loadedPositions || loadedScore ? "Game Loaded" : "No Save Found");
    }

    // Shows a message on screen for a short time
    private void ShowStatus(string message)
    {
        if (statusText == null)
        {
            return;
        }

        statusText.text = message;
        statusText.enabled = true;
        statusHideTime = Time.time + statusDuration;
    }
}