﻿namespace ClipHunta2.Tasks.FrameTesting.OW;

public sealed class OwElimFrameTester : TextFrameTester
{
    public static OwElimFrameTester GetInstance()
    {
        if (_instance == null)
        {
            _instance = new OwElimFrameTester();
        }

        return _instance;
    }
    protected override string[] AvoidWords()
    {
        return _avoids;
    }
    private static OwElimFrameTester? _instance = null;
    private readonly string[] _lookups = ["ELIMINATED", "NATED", "MNATED","TED"];
    private readonly string[] _avoids= ["BY", "WERE","WE"];

    protected override string[] Lookups()
    {
        return _lookups;
    }

    public OwElimFrameTester() : base("elim")
    {
    }


    public override string GetName()
    {
        return EventName;
    }
}

public sealed class OwDeathFrameTester : TextFrameTester
{
    public static OwDeathFrameTester GetInstance()
    {
        if (_instance == null)
        {
            _instance = new OwDeathFrameTester();
        }

        return _instance;
    }
    public override bool Test(string text)
    {
       
        if (text.Contains("WERE") && text.Contains("BY")) return true;
        return base.Test(text);
    }

    private static OwDeathFrameTester? _instance = null;
    private readonly string[] _lookups = ["NATED BY",  "MNATED  BY", "TED  BY" ,"OU WERE"];

    protected override string[] Lookups()
    {
        return _lookups;
    }
    

    public OwDeathFrameTester() : base("elimed")
    {
    }


    public override string GetName()
    {
        return EventName;
    }
}

public sealed class OwAssistTester : TextFrameTester
{
    public static OwAssistTester GetInstance()
    {
        if (_instance == null)
        {
            _instance = new OwAssistTester();
        }

        return _instance;
    }

    private static OwAssistTester? _instance = null;
    private readonly string[] _lookups = ["ASSIST"];

    protected override string[] Lookups()
    {
        return _lookups;
    }

    public OwAssistTester() : base("assist")
    {
    }


    public override string GetName()
    {
        return EventName;
    }
}
public sealed class OwOrbHarmonyTester : TextFrameTester
{
    public static OwOrbHarmonyTester GetInstance()
    {
        if (_instance == null)
        {
            _instance = new OwOrbHarmonyTester();
        }

        return _instance;
    }

    private static OwOrbHarmonyTester? _instance = null;
    private readonly string[] _lookups = ["OF HARMONY"];

    protected override string[] Lookups()
    {
        return _lookups;
    }

    public OwOrbHarmonyTester() : base("orbharmony")
    {
    }


    public override string GetName()
    {
        return EventName;
    }
}

public sealed class OwHealingTester : TextFrameTester
{
    public static OwHealingTester GetInstance()
    {
        if (_instance == null)
        {
            _instance = new OwHealingTester();
        }

        return _instance;
    }

    private static OwHealingTester? _instance = null;
    private readonly string[] _lookups = ["HEALING"];

    protected override string[] Lookups()
    {
        return _lookups;
    }

    public OwHealingTester() : base("healing")
    {
    }


    public override string GetName()
    {
        return EventName;
    }
}


public sealed class OwBlockingTester : TextFrameTester
{
    public static OwBlockingTester GetInstance()
    {
        if (_instance == null)
        {
            _instance = new OwBlockingTester();
        }

        return _instance;
    }

    private static OwBlockingTester? _instance = null;
    private readonly string[] _lookups = ["BLOCKING"];

    protected override string[] Lookups()
    {
        return _lookups;
    }

    public OwBlockingTester() : base("blocking")
    {
    }


    public override string GetName()
    {
        return EventName;
    }
}




public sealed class OwHeroSelectTester : TextFrameTester
{
    public static OwHeroSelectTester GetInstance()
    {
        if (_instance == null)
        {
            _instance = new OwHeroSelectTester();
        }

        return _instance;
    }

    private static OwHeroSelectTester? _instance = null;
    private readonly string[] _lookups = ["CHANGE HERO"];

    protected override string[] Lookups()
    {
        return _lookups;
    }

    public OwHeroSelectTester() : base("heroselect")
    {
    }


    public override string GetName()
    {
        return EventName;
    }
}