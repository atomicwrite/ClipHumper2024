﻿using ClipHunta2.TaskManagers;
using ClipHunta2.Tasks.LongTask;
using Serilog;
using Tesseract;

namespace ClipHunta2.Tasks;

public class TesseractTask : LongTaskWithReturn<Pix, string>, IDisposable
{
    public TesseractTask(CancellationTokenSource ctr, string tesseractDataPath,
        string tesseractLanguage = "eng",
        EngineMode mode = EngineMode.Default) : base(ctr)
    {
        _engine = new TesseractEngine(tesseractDataPath, tesseractLanguage, mode);
        _engine.SetVariable("debug_file", "/dev/null");
        _engine.SetVariable("load_system_dawg", false);
       _engine.SetVariable("load_freq_dawg", false);
        _engine.SetVariable("tessedit_char_whitelist", "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890>()\t ");
        _count = new ThreadSafeInt(0);
    }

    public override void StartTask()
    {
    }

    public override int Count()
    {
        return _count.Value;
    }

    protected object GetTop()
    {
        return TesseractLongTaskManager.GetInstance().GetTopTasker();
    }

    private readonly TesseractEngine _engine;
    private readonly ThreadSafeInt _count;


 

    public void Dispose()
    {
        _engine.Dispose();
    }

    public async Task<string?> GetText(Pix pix)
    {
        _count.Increment();
        Monitor.Enter(_engine);
        try
        {
            using var page = _engine.Process(pix);
            return page.GetText();
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex, "Error in Get Text");
            Console.WriteLine(ex);
        }
        finally
        {
            Monitor.Exit(_engine);
            _count.Decrement();
        }

        return null;
    }
}