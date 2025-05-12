using TMPro;
using UnityEngine;

public class DebugMenu : MonoBehaviour
{
    [SerializeField]
    TMP_Text debugText;

    public bool isActive = true;

    void Update()
    {
        // Check if the 'B' key was pressed
        if (Input.GetKeyDown(KeyCode.B))
        {
            foreach (Transform child in transform) 
            {
                child.gameObject.SetActive(!isActive); 
            }

            isActive = !isActive;
        }

        if (!isActive) { return; }

        debugText.text = "Current gamestate: " + GameManager.Instance.GetCurrentGameState() + "\n"
            + "Current round: " + GameManager.Instance.round + "\n"
            + "Current totalRounds: " + GameManager.Instance.totalRound + "\n"
            + "Current seed: " + GameManager.Instance.currentSeed + "\n"
            + "\n"
            + "Spawn percentage: " + GameManager.Instance.coinController.spawnPercentage + "%\n"
            + "Spawn distance: " + GameManager.Instance.coinController.spawnDistance + "\n"
            + "\n"
            + "Total distance: " + GameManager.Instance.coinController.totalDistance + "\n"
            + "Distance to finish: " + GameManager.Instance.finishAi.path.remainingDistance + "\n";
    }
}
