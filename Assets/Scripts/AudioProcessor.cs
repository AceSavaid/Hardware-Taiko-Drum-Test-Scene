using System.Collections.Generic;
using UnityEngine;

public static class AudioProcessor
{
    public static List<float> AnalyzeAudio(AudioClip clip, int bpm, string complexity)
    {
        //Could make this an enum but im tired
        float complexityMultiplier = complexity.ToLower() switch
        {
            "easy" => 0.5f,
            "medium" => 1f,
            "hard" => 1.5f,
            _ => 1f
        };

        float[] samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);

        // Beat timings
        float secondsPerBeat = 60f / bpm;
        List<float> beatTimestamps = new List<float>();

        int sampleRate = clip.frequency;
        int samplesPerBeat = Mathf.RoundToInt(sampleRate * secondsPerBeat);

        // Analyze waveform
        for (int i = 0; i < samples.Length; i += samplesPerBeat)
        {
            // Calculate average amplitude over the beat window
            float sum = 0f;
            for (int j = 0; j < samplesPerBeat && (i + j) < samples.Length; j++)
            {
                sum += Mathf.Abs(samples[i + j]);
            }

            float avgAmplitude = sum / samplesPerBeat;

            // Add beat if amplitude exceeds threshold (scaled by complexity)
            if (avgAmplitude > 0.1f / complexityMultiplier)
            {
                beatTimestamps.Add((float)i / sampleRate);
            }
        }

        return beatTimestamps;
    }
}
