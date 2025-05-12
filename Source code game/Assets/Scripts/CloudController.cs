using UnityEngine;

public class CloudScroller : MonoBehaviour
{
    public float scrollSpeed = 2.0f; // Speed at which the clouds scroll
    public float resetPositionX = -10.0f; // X position to reset the cloud
    public float startPositionX = 10f; // X position to start the cloud

    void Update()
    {
        // Move the cloud left
        transform.Translate(Vector2.left * scrollSpeed * Time.deltaTime);

        // If the cloud is past the reset position, move it back to the start position
        if (transform.position.x < resetPositionX)
        {
            ResetPosition();
        }
    }

    void ResetPosition()
    {
        Vector3 newPosition = transform.position;
        newPosition.x = startPositionX;
        transform.position = newPosition;
    }
}
