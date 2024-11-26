using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RhythmChartPlayer : MonoBehaviour
{
    public string loadFilePath = "Assets/NotesLayout.txt"; // File to load the layout from
    public GameObject notePrefab; // Prefab for the notes
    public Transform leftLane; // Transform for the left lane
    public Transform rightLane; // Transform for the right lane
    public AudioSource audioSource; // Audio source for playback

    private List<(float timestamp, int lane)> noteLayout;
    private float startTime;

    void Start()
    {
        // Validate inputs
        if (notePrefab == null || leftLane == null || rightLane == null || audioSource == null)
        {
            Debug.LogError("Missing required references!");
            return;
        }

        // Load the note layout
        noteLayout = LoadNoteLayoutFromFile(loadFilePath);

        if (noteLayout == null || noteLayout.Count == 0)
        {
            Debug.LogError("Failed to load note layout or layout is empty.");
            return;
        }

        // Start playing the audio
        startTime = Time.time;
        audioSource.Play();
    }

    void Update()
    {
        float currentTime = Time.time - startTime;

        // Check if it's time to spawn any notes
        for (int i = 0; i < noteLayout.Count; i++)
        {
            var (timestamp, lane) = noteLayout[i];

            if (currentTime >= timestamp)
            {
                // Determine the lane
                Transform laneTransform = lane == 0 ? leftLane : rightLane;

                Instantiate(notePrefab, laneTransform.position, Quaternion.identity);

                // Remove the note from the list to avoid re-spawning
                noteLayout.RemoveAt(i);
                i--; 
            }
        }
    }

    List<(float, int)> LoadNoteLayoutFromFile(string filePath)
    {
        List<(float, int)> layout = new List<(float, int)>();

        if (!File.Exists(filePath))
        {
            Debug.LogError($"File not found: {filePath}");
            return null;
        }

        using (StreamReader reader = new StreamReader(filePath))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var parts = line.Split(',');
                if (parts.Length == 2 && float.TryParse(parts[0], out float timestamp) && int.TryParse(parts[1], out int lane))
                {
                    layout.Add((timestamp, lane));
                }
            }
        }

        Debug.Log($"Note layout loaded from {filePath}");
        return layout;
    }
}
