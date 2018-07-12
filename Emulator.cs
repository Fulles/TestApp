using System;
using System.IO.Ports;    //Nuget System.IO.Ports
using System.Collections.Generic;

class Emulator
{
    public static void Main()
    {
        
        
        SerialPort Port = GetPort();
        Console.ReadKey();
        Port.Close();
    }

    private static SerialPort GetPort()
    {
        SerialPort Port = new SerialPort("COM3");
        Port.Open();
        Port.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
        return Port;
    }
    private static void DataReceivedHandler(
                       object sender,
                       SerialDataReceivedEventArgs e)
    {
        string[] requests = { "r100000000", "r200000000", "r300000000", "r400000000",
                "r500000000", "r600000000", "r700000000", "r800000000", "r900000000"};
        string[] responses = { "ABCD1234", "ADSS3444", "QWER4321", "WRRRRERRE",
                    "ASDFGHJKL", "33444", "TYUI4567", "ZXCV9876", "MNBV0000" };
        List<string> ResponsesList = new List<string>(responses);
        List<string> RequestsList = new List<string>(requests);

        SerialPort sp = (SerialPort)sender;
        string indata = sp.ReadExisting();
        Console.WriteLine(indata);
        WriteOrRead(indata, sp, ResponsesList, RequestsList);
        

    }
    private static void WriteOrRead(string indata, SerialPort sp, List<string> ResponsesList, List<string> RequestsList)
    {
        int count = 0;
        int MaxCount = 8;
       
        Console.WriteLine(ResponsesList.Count);
        if (indata.StartsWith("w"))
        {
            WriteToRegister(indata, ResponsesList, RequestsList);
            MaxCount += 1;
            Console.WriteLine(RequestsList[MaxCount -1]);
            Console.WriteLine(ResponsesList[MaxCount -1]);
        }
        if (indata.StartsWith("r"))
        {
            Console.WriteLine(MaxCount);
            while (count < MaxCount )
            {
                if (indata.Contains(RequestsList[count]))
                {
                    sp.WriteLine(ResponsesList[count]);
                }
                count++;
            }
        }

    }
    private static string ValueForResponses(string indata)
    {
        char spliter = '*';
        int startIndex = indata.IndexOf(spliter);
        string value = indata.Remove(0, startIndex + 1);
        return value;
    }
    private static string ValueForRequests(string indata)
    {
        char spliter = '*';
        int startIndex = indata.IndexOf(spliter);
        string value = indata.Remove(startIndex);
        value = "r" + value.Remove(0, 1);
        return value;
    }
    private static void WriteToRegister(string indata, List<string> ResponsesList, List<string> RequestsList)
    {
        string ValueForResponse = ValueForResponses(indata);
        string ValueForRequest = ValueForRequests(indata);
        ResponsesList.Add(ValueForResponse);
        RequestsList.Add(ValueForRequest);
    }
}