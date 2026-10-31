using UnityEngine;
using TMPro;

/*
 * Observer: subscribes to Target.OnTargetHit, adds the points to the score and updates the score text
*/

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private string scoreFormat = "Score: {0}";

    public int Score { get; private set; }

    // Starts listening for target hits
    private void OnEnable()
    {
        Target.OnTargetHit += HandleTargetHit;
    }

    // Stops listening for target hits so no stale subscription is left behind
    private void OnDisable()
    {
        Target.OnTargetHit -= HandleTargetHit;
    }

    // Shows the starting score
    private void Start()
    {
        UpdateScoreText();
    }

    // Called by the subject whenever a target is hit
    private void HandleTargetHit(int points)
    {
        Score += points;
        UpdateScoreText();
    }

    // Writes the current score into the UI text
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = string.Format(scoreFormat, Score);
        }
    }
}