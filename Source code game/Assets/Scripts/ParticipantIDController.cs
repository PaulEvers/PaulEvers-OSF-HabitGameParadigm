using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ParticipantIDController : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField participantInputField;
    [SerializeField]
    private Button nextButton;

    private string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
    private void Start()
    {
        // Check for ParticipantEmail in PlayerPrefs
        string savedEmail = PlayerPrefs.GetString("ParticipantEmail", "");
        if (!string.IsNullOrEmpty(savedEmail))
        {
            participantInputField.text = savedEmail;
        }

        // Listen for changes in input field and button
        participantInputField.onValueChanged.AddListener(OnInputFieldChanged);
        nextButton.onClick.AddListener(OnNextButtonClicked);

        OnInputFieldChanged(savedEmail);
    }

    private void OnInputFieldChanged(string inputText)
    {
        // Check if there is text and is an email
        nextButton.interactable = !string.IsNullOrEmpty(inputText) && Regex.IsMatch(inputText, emailPattern);
    }

    private void OnNextButtonClicked()
    {
        string email = participantInputField.text;
        // Save participant email to PlayerPrefs
        PlayerPrefs.SetString("ParticipantEmail", email);

        SceneManager.LoadScene("Scene_MazeGeneration_Engaging");
    }

    private void OnDestroy()
    {
        // Clean up listeners
        participantInputField.onValueChanged.RemoveListener(OnInputFieldChanged);
        nextButton.onClick.RemoveListener(OnNextButtonClicked);
    }
}
