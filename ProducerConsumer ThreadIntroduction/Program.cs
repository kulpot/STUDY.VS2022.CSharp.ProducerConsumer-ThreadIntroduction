using System;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;

//ref link:https://www.youtube.com/watch?v=t850VUYAlpQ&list=PLRwVmtr-pp06KcX24ycbC-KkmAISAFKV5&index=12
// 

class MainClass
{
    static byte[] values = new byte[500000000];
    static long[] portionResults;
    static int portionSize;
    static int numThreads;

    static void GenerateInts()
    {
        var rand = new Random(987);
        for (int i = 0; i < values.Length; i++)
            values[i] = (byte)rand.Next(10);
    }
    static void SumYourPortion(object portionNumber)
    {
        long sum = 0;
        int portionNumberAsInt = (int)portionNumber;
        int baseIndex = portionNumberAsInt * portionSize;
        for (int i = baseIndex; i < baseIndex + portionSize; i++)
        {
            sum += values[i];
        }
        portionResults[portionNumberAsInt] = sum;
    }
    static void Main()
    {
        portionResults = new long[Environment.ProcessorCount];
        portionSize = values.Length / Environment.ProcessorCount;
        GenerateInts();
        Console.WriteLine("Summing...");
        Stopwatch watch = new Stopwatch();
        watch.Start();
        long total1 = 0;
        for (int i = 0; i < values.Length; i++)
            total1 += values[i];
        watch.Stop();
        Console.WriteLine("Total value is: " + total1);
        Console.WriteLine("Time to sum: " + watch.Elapsed);
        Console.WriteLine();

        //---------Divide and Conquer----
        watch.Reset();
        watch.Start();
        Thread[] threads = new Thread[Environment.ProcessorCount];
        for (int i = 0; i < Environment.ProcessorCount; i++)
        {
            threads[i] = new Thread(SumYourPortion);
            threads[i].Start(i);
        }
        for (int i = 0; i < Environment.ProcessorCount; i++)
            threads[i].Join();
        long total2 = 0;
        for (int i = 0; i < Environment.ProcessorCount; i++)
            total2 += portionResults[i];
        watch.Stop();
        Console.WriteLine("Total value is: " + total2);
        Console.WriteLine("Time to sum: " + watch.Elapsed);
    }
}