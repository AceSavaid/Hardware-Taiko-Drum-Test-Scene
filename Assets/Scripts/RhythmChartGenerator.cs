using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RhythmChartGenerator : MonoBehaviour
{
    public AudioClip audioClip; // Input WAV file
    public int bpm = 120; // Beats per minute
    public string complexity = "medium"; // "easy", "medium", "hard"

    public GameObject notePrefab; 
    public Transform lane1; // Transform for the left lane
    public Transform lane2; // Transform for the right lane

    public string saveFilePath = "Assets/NotesLayout.txt"; // File to save the layout

    private float secondsPerBeat;
    private List<(float timestamp, int lane)> noteLayout; // Holds timestamp and lane data

    void Start()
    {
        // Validate inputs
        if (audioClip == null || notePrefab == null || lane1 == null || lane2 == null)
        {
            Debug.LogError("Missing required references!");
            return;
        }

        // Calculate seconds per beat
        secondsPerBeat = 60f / bpm;

        // Analyze audio for beat timings
        var beatTimestamps = AudioProcessor.AnalyzeAudio(audioClip, bpm, complexity);

        // Generate the note layout
        noteLayout = GenerateNoteLayout(beatTimestamps);

        // Save the note layout to a file
        SaveNoteLayoutToFile(noteLayout, saveFilePath);
    }

    List<(float, int)> GenerateNoteLayout(List<float> beatTimestamps)
    {
        List<(float, int)> layout = new List<(float, int)>();
        bool isLeftLane = true;

        foreach (float timestamp in beatTimestamps)
        {
            int lane = Random.Range(0, 1);// 0 for left, 1 for right
            layout.Add((timestamp, lane));
            isLeftLane = !isLeftLane; // Alternate lanes
        }

        Debug.Log("Chart generation complete!");
        return layout;
    }

    void SaveNoteLayoutToFile(List<(float, int)> layout, string filePath)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (var (timestamp, lane) in layout)
            {
                writer.WriteLine($"{timestamp},{lane}");
            }
        }
        Debug.Log($"Note layout saved to {filePath}");
    }
}