using UnityEngine;

public class CarAnimation : MonoBehaviour
{
    [SerializeField] private float speed = 5f;        
    [SerializeField] private float laneYOffset = 2f;    
    [SerializeField] private float screenBoundary = 10f; 

    private bool isTopLane = true;  
    private bool movingRight = false; 

    private void Update()
    {
        // Move the car
        float direction = movingRight ? 1 : -1;
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

        // Check if car has reached the screen boundary
        if (transform.position.x < -screenBoundary || transform.position.x > screenBoundary)
        {
            FlipCar();
            SwitchLanes();
        }
    }

    private void FlipCar()
    {
        // Flip the movement direction
        movingRight = !movingRight;

        // Flip the sprite
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
    }

    private void SwitchLanes()
    {
        isTopLane = !isTopLane;

        // Update Y position
        Vector3 newPosition = transform.position;
        newPosition.y = isTopLane ? laneYOffset : -laneYOffset;
        transform.position = newPosition;
    }
}
