using System;
using System.Linq;
using TwinCAT.Ads;
using TwinCAT.Ads.TypeSystem;

namespace ReadPlcComment;

internal static class Program
{
    private const string NetID = "192.168.110.254.1.1";
    private const int Port = 801;
    private const string PouName = "MAIN";
    private const int Align = -30;

    private static void Main()
    {
        using var tcClient = new TcAdsClient();
        tcClient.Connect(NetID, Port);
        var symbolLoader = SymbolLoaderFactory.Create(tcClient, SymbolLoaderSettings.Default);
        var pou = symbolLoader.Symbols.FirstOrDefault(s => s.InstancePath == PouName);

        if (pou is null) throw new NullReferenceException();

        foreach (var subSymbol in pou.SubSymbols)
        {
            Console.WriteLine($"var: {subSymbol.InstancePath,Align}comment: {subSymbol.Comment,Align}");
        }
    }
}

//  outPut:
//  var: MAIN.CHINESECOUNTER           comment: ????
//  var: MAIN.ENGLISHCOUNTER           comment: ENGLISH

//  plc side:
//  PROGRAM MAIN
//  VAR
//  EnglishCounter    : DINT;(*English*)
//  ChineseCounter    : DINT;(*中文*)
//  END_VAR