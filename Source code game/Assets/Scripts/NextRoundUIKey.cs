using System.Collections;
using UnityEngine;

public class NextRoundUIKey : MonoBehaviour
{
    [SerializeField]
    private bool isNewPhase = false;

    [SerializeField]
    private bool isDelayed = false;

    private bool canAcceptInput = true;

    [SerializeField] private GameObject delayEnable;

    private void Start()
    {
        if (isDelayed) {
            canAcceptInput = false;
            StartCoroutine(EnableInputAfterDelay(4f));
        }
    }
    void Update()
    {
        // Check if the space key was pressed
        if (Input.GetKeyDown(KeyCode.Space) && canAcceptInput)
        {
            GameManager.Instance.NextRound(isNewPhase, false, false);
        }
    }

    IEnumerator EnableInputAfterDelay(float delay)
    {
        // Wait for delay before continuing
        yield return new WaitForSeconds(delay);

        // Enable input again
        canAcceptInput = true;
        delayEnable.SetActive(true);
    }
}
