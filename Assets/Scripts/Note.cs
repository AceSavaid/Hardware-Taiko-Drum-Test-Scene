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
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        // Destroy the note after it goes past hit zone
        if (transform.position.x < -20f)
        {
            Destroy(gameObject);
        }
    }
}
