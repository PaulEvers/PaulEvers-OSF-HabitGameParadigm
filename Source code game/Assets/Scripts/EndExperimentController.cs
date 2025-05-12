using UnityEngine;

public class EndExperimentController : MonoBehaviour
{
    private int day = 0;

    [SerializeField] private GameObject[] texts;

    void Start()
    {
        day = GameManager.Instance.day;

        texts[day -1].gameObject.SetActive(true);
    }
}
