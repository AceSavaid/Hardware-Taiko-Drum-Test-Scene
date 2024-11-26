using UnityEngine;

public class Note : MonoBehaviour
{
    private float timestamp;
    public float speed = 7f;

    public void SetTimestamp(float time)
    {
        timestamp = time;
    }

    void Update()
    {
        // Move the note down the screen 
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        // Destroy the note after it goes off-screen
        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }
}
