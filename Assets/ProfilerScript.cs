using System.Collections.Generic;
using System.Text;
using Unity.Profiling;
using UnityEngine;

public class ProfilerScript : MonoBehaviour
{
    string statsText;
    ProfilerRecorder systemMemoryRecorder;
    ProfilerRecorder gcMemoryRecorder;
    ProfilerRecorder mainThreadTimeRecorder;
    ProfilerRecorder totalReservedMemoryRecorder;

    static double GetRecorderFrameAverage(ProfilerRecorder recorder)
    {
        var samplesCount = recorder.Capacity;
        if (samplesCount == 0)
            return 0;

        double r = 0;
        var samples = new List<ProfilerRecorderSample>(samplesCount);
        recorder.CopyTo(samples);
        for (var i = 0; i < samples.Count; ++i)
            r += samples[i].Value;
        r /= samplesCount;
        
        return r;
    }

    void OnEnable()
    {
        systemMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "System Used Memory");
        gcMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Reserved Memory");
        totalReservedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total Reserved Memory");
        mainThreadTimeRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Internal, "Main Thread", 15);
    }

    void OnDisable()
    {
        systemMemoryRecorder.Dispose();
        gcMemoryRecorder.Dispose();
        totalReservedMemoryRecorder.Dispose();
        mainThreadTimeRecorder.Dispose();
    }

    void Update()
    {
        var sb = new StringBuilder(500);
        
        sb.AppendLine($"FPS: {1 / (GetRecorderFrameAverage(mainThreadTimeRecorder) * (1e-9f)):F1}");
        sb.AppendLine($"Frame Time: {(GetRecorderFrameAverage(mainThreadTimeRecorder) * (1e-6f)):F1} ms");
        sb.AppendLine($"Total Reserved Memory: {totalReservedMemoryRecorder.LastValue / (1024 * 1024)} MB");
        sb.AppendLine($"GC Reserved Memory: {gcMemoryRecorder.LastValue / (1024 * 1024)} MB");
        sb.AppendLine($"System Used Memory: {systemMemoryRecorder.LastValue / (1024 * 1024)} MB");

        statsText = sb.ToString();
    }

    void OnGUI()
    {
        GUI.TextArea(new Rect(0, 0, 225, 80), statsText);
    }
}