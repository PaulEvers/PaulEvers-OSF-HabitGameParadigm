using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;

public class LikertScale : MonoBehaviour
{
    [System.Serializable]
    public class LikertItem
    {
        public string question;
        public ToggleGroup toggleGroup;
    }

    [System.Serializable]
    public class LikertPage
    {
        public List<LikertItem> items;
    }

    public List<LikertPage> pages;
    private int currentPageIndex = 0;

    [SerializeField]
    private Button continueButton;
    [SerializeField]
    private Button backButton;

    private bool hasAlreadySubmitted = false;
    public bool isPXISurveyActive = false;

    private void Start()
    {
        continueButton.onClick.AddListener(OnNextButtonClicked);
        backButton.onClick.AddListener(OnBackButtonClicked);

        foreach (var page in pages)
        {
            foreach (var question in page.items)
            {
                foreach (var toggle in question.toggleGroup.GetComponentsInChildren<Toggle>())
                {
                    toggle.onValueChanged.AddListener(delegate { CheckToggleGroups(); });
                }
            }
        }

        UpdatePageState();
    }

    private void UpdatePageState()
    {
        // Show/hide questions based on current page
        for (int i = 0; i < pages.Count; i++)
        {
            bool isCurrentPage = i == currentPageIndex;
            foreach (var question in pages[i].items)
            {
                question.toggleGroup.transform.parent.gameObject.SetActive(isCurrentPage);
                question.toggleGroup.gameObject.SetActive(isCurrentPage);
            }
        }

        backButton.gameObject.SetActive(currentPageIndex > 0);
        continueButton.GetComponentInChildren<TMP_Text>().text =
           (currentPageIndex == pages.Count - 1) ? "Submit" : "Continue";

        CheckToggleGroups();
    }

    private void CheckToggleGroups()
    {
        foreach (var question in pages[currentPageIndex].items)
        {
            if (!IsToggleGroupFilled(question.toggleGroup))
            {
                continueButton.interactable = false;
                return;
            }
        }
        continueButton.interactable = true;
    }

    private bool IsToggleGroupFilled(ToggleGroup group)
    {
        foreach (var toggle in group.GetComponentsInChildren<Toggle>())
        {
            if (toggle.isOn)
            {
                return true;
            }
        }
        return false;
    }

    private void OnNextButtonClicked()
    {
        if (currentPageIndex == pages.Count - 1)
        {
            if (hasAlreadySubmitted) { return; }
            hasAlreadySubmitted = true;

            if (isPXISurveyActive) { SubmitPXISurvey(); return; }
            SubmitHabitSurvey();
        }
        else
        {
            currentPageIndex++;
            UpdatePageState();
        }
    }

    private void OnBackButtonClicked()
    {
        if (currentPageIndex > 0)
        {
            currentPageIndex--;
            UpdatePageState();
        }
    }

    private void SubmitHabitSurvey()
    {
        HabitSurveyData surveyData = new HabitSurveyData();
        surveyData.participantEmail = PlayerPrefs.GetString("ParticipantEmail");
        surveyData.day = PlayerPrefs.GetInt("Day");

        int questionIndex = 0;
        for (int pageIndex = 0; pageIndex < pages.Count; pageIndex++)
        {
            foreach (var item in pages[pageIndex].items)
            {
                int value = 0;
                foreach (var toggle in item.toggleGroup.GetComponentsInChildren<Toggle>())
                {
                    if (toggle.isOn)
                    {
                        value = int.Parse(toggle.gameObject.name);
                        break;
                    }
                }

                switch (questionIndex)
                {
                    case 0: surveyData.srbai1 = value; break;
                    case 1: surveyData.srbai2 = value; break;
                    case 2: surveyData.srbai3 = value; break;
                    case 3: surveyData.srbai4 = value; break;
                }
                questionIndex++;
            }
        }

        StartCoroutine(SendSurveyData(surveyData));
    }

    private IEnumerator SendSurveyData(HabitSurveyData data)
    {
        string jsonData = JsonUtility.ToJson(data);

        ////////////// TODO: Change to your api endpoint //////////////
        string url = "/api/addHabitSurvey";
        //////////////////////////////////////////////////////////////
    
        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Survey data sent successfully");
                SceneManager.LoadScene("SurveyPXI");
            }
            else
            {
                Debug.LogError("Error sending survey data: " + request.error);
                hasAlreadySubmitted = false;
            }
        }
    }

    private void SubmitPXISurvey()
    {
        if (!isPXISurveyActive) return;

        PXISurveyData surveyData = new PXISurveyData();
        surveyData.participantEmail = PlayerPrefs.GetString("ParticipantEmail");
        surveyData.day = PlayerPrefs.GetInt("Day");

        int questionIndex = 0;
        for (int pageIndex = 0; pageIndex < pages.Count; pageIndex++)
        {
            foreach (var item in pages[pageIndex].items)
            {
                int value = 0;
                foreach (var toggle in item.toggleGroup.GetComponentsInChildren<Toggle>())
                {
                    if (toggle.isOn)
                    {
                        value = int.Parse(toggle.gameObject.name);
                        break;
                    }
                }

                switch (questionIndex)
                {
                    case 0: surveyData.aa = value; break;
                    case 1: surveyData.ch = value; break;
                    case 2: surveyData.ec = value; break;
                    case 3: surveyData.gr = value; break;
                    case 4: surveyData.pf = value; break;
                    case 5: surveyData.aut = value; break;
                    case 6: surveyData.cur = value; break;
                    case 7: surveyData.imm = value; break;
                    case 8: surveyData.mas = value; break;
                    case 9: surveyData.mea = value; break;
                    case 10: surveyData.enj = value; break;
                }
                questionIndex++;
            }
        }

        StartCoroutine(SendPXISurveyData(surveyData));
    }

    private IEnumerator SendPXISurveyData(PXISurveyData data)
    {
        string jsonData = JsonUtility.ToJson(data);
        ////////////// TODO: Change to your api endpoint //////////////
        string url = "/api/addPXISurvey";
        //////////////////////////////////////////////////////////////

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("PXI survey data sent successfully");
                SceneManager.LoadScene("End");
            }
            else
            {
                Debug.LogError("Error sending PXI survey data: " + request.error);
            }
        }
    }

    private void OnPXISurveyButtonClicked()
    {
        if (isPXISurveyActive)
        {
            SubmitPXISurvey();
        }
    }

    private void OnDestroy()
    {
        continueButton.onClick.RemoveListener(OnNextButtonClicked);
        backButton.onClick.RemoveListener(OnBackButtonClicked);
    }
}
