using UnityEngine;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour
{
    [SerializeField] private GameObject characterPrefab;
    [SerializeField] private Sprite[] characters;
    [SerializeField] private Vector2 gridSize = new Vector2(3, 2);
    [SerializeField] private float spacing = 2f; // Space between characters
    [SerializeField] private Vector3 gridOffset = Vector3.zero;

    private GameObject[] characterObjects;
    private int selectedCharacter = -1;
    private bool hasConfirmedSelection = false;
    public bool hasSelected { get { return selectedCharacter != -1; } }

    private CharacterAnimationController playerAnimationController;
    private Color normalColor = new Color(0.4f, 0.4f, 0.4f, 1f); // #BCBCBC
    private Color selectedColor = Color.white; // Highlight color for selected character

    void Start()
    {
        playerAnimationController = GameManager.Instance.player.GetComponent<CharacterAnimationController>();
        CreateCharacterGrid();
    }

    void CreateCharacterGrid()
    {
        characterObjects = new GameObject[characters.Length];

        // Starting position to center the grid
        float startX = -(gridSize.x - 1) * spacing / 2;
        float startY = -(gridSize.y - 1) * spacing / 2;

        for (int i = 0; i < characters.Length; i++)
        {
            // Calculate grid position
            int row = i / (int)gridSize.x;
            int col = i % (int)gridSize.x;

            // Create character object
            Vector3 position = new Vector3(
                startX + col * spacing,
                startY + row * spacing,
                0
            );

            GameObject charObject = Instantiate(characterPrefab, transform.position + position + gridOffset, Quaternion.identity, transform);
            charObject.transform.localScale = new Vector3(6, 6, 6);
            SpriteRenderer renderer = charObject.GetComponent<SpriteRenderer>();
            renderer.sprite = characters[i];
            renderer.color = normalColor;

            // Store reference and add index to object
            characterObjects[i] = charObject;
            CharacterClickHandler clickHandler = charObject.AddComponent<CharacterClickHandler>();
            clickHandler.Initialize(i, this);
        }
    }

    public void SelectCharacter(int index)
    {
        if (hasConfirmedSelection) return;

        // Deselect previous character
        if (selectedCharacter != -1)
        {
            characterObjects[selectedCharacter].GetComponent<SpriteRenderer>().color = normalColor;
            characterObjects[selectedCharacter].transform.localScale = new Vector3(6, 6, 6);
        }

        // Select new character
        selectedCharacter = index;
        characterObjects[selectedCharacter].GetComponent<SpriteRenderer>().color = selectedColor;
        characterObjects[selectedCharacter].transform.localScale = new Vector3(7.5f, 7.5f, 7.5f);

        // Update animation preview
        GameManager.Instance.participantData.characterSelect = selectedCharacter;
        playerAnimationController.SetAnimator(selectedCharacter);
    }
}

public class CharacterClickHandler : MonoBehaviour
{
    private int index;
    private CharacterSelector selector;

    public void Initialize(int characterIndex, CharacterSelector characterSelector)
    {
        index = characterIndex;
        selector = characterSelector;
    }

    void OnMouseDown()
    {
        selector.SelectCharacter(index);
    }
}
