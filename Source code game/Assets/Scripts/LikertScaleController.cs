using UnityEngine;
using UnityEngine.UI;

public class LikertScaleController : MonoBehaviour
{
    public GameObject[] pages; // Assign the 3 page panels in the Inspector
    public Button nextButton;
    public Button previousButton;
    public Button submitButton;

    private int currentPageIndex = 0;

    void Start()
    {
        UpdatePageVisibility();
        nextButton.onClick.AddListener(NextPage);
        previousButton.onClick.AddListener(PreviousPage);
        submitButton.onClick.AddListener(Submit);
    }

    void Update()
    {
        // Enable/disable the Next button based on page validation
        nextButton.interactable = AreAllLikertItemsSelectedOnPage(currentPageIndex);
    }

    void UpdatePageVisibility()
    {
        // Activate the current page and deactivate others
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == currentPageIndex);
        }

        // Manage button visibility
        previousButton.gameObject.SetActive(currentPageIndex > 0);
        nextButton.gameObject.SetActive(currentPageIndex < pages.Length - 1);
        submitButton.gameObject.SetActive(currentPageIndex == pages.Length - 1);
    }

    bool AreAllLikertItemsSelectedOnPage(int pageIndex)
    {
        // Validate all Likert items on the current page
        //LikertItemPXI[] likertItems = pages[pageIndex].GetComponentsInChildren<LikertItemPXI>();
        //foreach (var item in likertItems)
        //{
        //    if (!item.IsItemSelected()) return false;
        //}
        return true;
    }

    void NextPage()
    {
        if (currentPageIndex < pages.Length - 1)
        {
            currentPageIndex++;
            UpdatePageVisibility();
        }
    }

    void PreviousPage()
    {
        if (currentPageIndex > 0)
        {
            currentPageIndex--;
            UpdatePageVisibility();
        }
    }

    void Submit()
    {
        Debug.Log("All pages completed. Submitting responses...");
    }
}
