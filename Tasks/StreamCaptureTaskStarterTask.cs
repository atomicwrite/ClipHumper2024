﻿using System.ComponentModel;


namespace ClipHunta2.Tasks;

public enum StreamCaptureType
{
    Clip,
    Stream
}

public class StreamDefinition
{
    public StreamDefinition(string streamerName, StreamCaptureType streamCaptureType)
    {
        StreamCaptureType = streamCaptureType;
        StreamerName = streamerName;
    }

    public StreamCaptureType StreamCaptureType { get; init; }

    public string StreamerName { get; init; }
}

public class ThreadSafeInt
{
    public ThreadSafeInt(int item = 0)
    {
        Value = item;
    }

    public static implicit operator ThreadSafeInt(int item)
    {
        return new ThreadSafeInt(item);
    }

    public int Value { get; private set; } = 0;

    public override string ToString()
    {
        return Value.ToString();
    }

    public void Decrement()
    {
        Task.Run(() =>
        {
            Monitor.Enter(this);
            try
            {
                Value -= 1;
            }
            finally
            {
                Monitor.Exit(this);
            }
        });
    }

    public void Increment()
    {
        Task.Run(() =>
        {
            Monitor.Enter(this);
            try
            {
                Value += 1;
            }
            finally
            {
                Monitor.Exit(this);
            }
        });
    }
}

public class StreamCaptureStatus
{
    public StreamCaptureStatus(CancellationTokenSource cts)
    {
        _cts = cts;
    }

    private readonly ThreadSafeInt _finished = 0;
    private readonly ThreadSafeInt _framesCount = 0;
    private readonly ThreadSafeInt _imagesPrepped = 0;
    private readonly ThreadSafeInt _imagesScanned = 0;
    private readonly ThreadSafeInt _eventsRouted = 0;
    private readonly ThreadSafeInt _skipped = 0;
    private int _finalFrameCount = -1;
    private readonly CancellationTokenSource _cts;

    public void IncrementFinishedCount()
    {
        _finished.Increment();
    }

    public int FinalFrameCount => _finalFrameCount;


    public void SetFinalFrameCount(int number)
    {
        _finalFrameCount = number;
    }

    public void IncrementSkippedCount()
    {
        _skipped.Increment();
    }

    public void IncrementFrameCount()
    {
        _framesCount.Increment();
    }

    public void IncrementImagesPrepped()
    {
        _imagesPrepped.Increment();
    }

    public void IncrementImagesScanned()
    {
        _imagesScanned.Increment();
    }

    public void IncrementEventsRouted()
    {
        _eventsRouted.Increment();
    }

    public override string ToString()
    {
        return
            $"Duds: Skipped: {_skipped} Prepped: {_imagesPrepped} Routed: {_eventsRouted} Created: {_framesCount} Scanned {_imagesScanned} Final Count: {_finalFrameCount}";
    }

    public int FramesCount => _framesCount.Value;

    public int ImagesPrepped => _imagesPrepped.Value;

    public int ImagesScanned => _imagesScanned.Value;

    public int EventsRouted => _eventsRouted.Value;
    public int FinishedCount => _finished.Value;

 

    public void SetCanceled()
    {
        _cts.Cancel();
    }
}

public class StreamCaptureTaskStarterTask
{
    private readonly CancellationTokenSource _cts;
    private readonly string _stream;
    private readonly StreamCaptureType _captureType;


    private readonly BackgroundWorker _backgroundWorker;

    public StreamCaptureTaskStarterTask(CancellationTokenSource cts, string stream, StreamCaptureType captureType)
    {
        _cts = cts;
        _stream = stream;
        _captureType = captureType;

        _backgroundWorker = new BackgroundWorker { WorkerSupportsCancellation = true };
        _backgroundWorker.DoWork += _watch;
    }

    private void _watch(object? sender, DoWorkEventArgs e)
    {
        var captureTask = new StreamCaptureTask(_cts, new StreamDefinition(_stream, _captureType));

        var (clipId, status) = ((string?, StreamCaptureStatus))e.Argument!;
    
        if (!string.IsNullOrEmpty(clipId)) captureTask.Start(clipId, _captureType, status);
    }

    public StreamCaptureStatus Start(string? clipId, CancellationTokenSource cts)
    {
        if (_backgroundWorker.IsBusy)
        {
            throw new Exception("Already running");
        }

        if (_captureType == StreamCaptureType.Clip && string.IsNullOrEmpty(clipId))
        {
            throw new ArgumentException("Argument can't be null, need clip id");
        }

        StreamCaptureStatus status = new StreamCaptureStatus(cts);
        _backgroundWorker.RunWorkerAsync((clipId, status));
        return status;
    }

    public void Enqueue(byte[] img)
    {
    }
}