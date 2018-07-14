using System;
using System.IO.Ports;    //Nuget System.IO.Ports
using System.Collections.Generic;

namespace All
{
    class Register
    {

        string[] Keys = { "r100000000", "r200000000", "r300000000", "r400000000",
                "r500000000", "r600000000", "r700000000", "r800000000", "r900000000"};
        string[] Values = { "ABCD1234", "ADSS3444", "QWER4321", "WRRRRERRE",
                    "ASDFGHJKL", "33444", "TYUI4567", "ZXCV9876", "MNBV0000" };
        Dictionary<string, string> MyDictionary =
           new Dictionary<string, string>();


        public Dictionary<string, string> CreateDictionary()
        {
           
            int counter = 0;
            while (counter < Keys.Length)
            {
                MyDictionary.Add(Keys[counter], Values[counter]);
                counter++;
            }
            return MyDictionary;
        }
        public struct Dictionary
        {
            public Dictionary<string, string> dictionary;
        }

    }


    class Program
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

            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            Console.WriteLine(indata);
            WriteOrRead(indata, sp);

        }

        public static Register register = new Register { };
        public static Dictionary<string, string> Dictionary = register.CreateDictionary();

        public static  Dictionary<string, string> WriteOrRead(string indata, SerialPort sp)
        {

            Dictionary<string, string> MyDictionary = Dictionary;
            
            if (indata.StartsWith("w"))
            {
                WriteToRegister(indata, Dictionary);
            }
            if (indata.StartsWith("r"))
            {
                try
                {
                    sp.WriteLine(Dictionary[indata]);
                }
                catch
                {
                    Console.WriteLine("Enter correct key");
                    
                }
            }
            return Dictionary;
        }

 
        private static string GetValues(string indata)
        {
            char spliter = '*';
            int startIndex = indata.IndexOf(spliter);
            string value = indata.Remove(0, startIndex + 1);
            return value;
        }
        private static string GetKeys(string indata)
        {
            char spliter = '*';
            int startIndex = indata.IndexOf(spliter);
            string value = indata.Remove(startIndex);
            value = "r" + value.Remove(0, 1);
            return value;
        }
        private static void WriteToRegister(string indata, Dictionary<string, string> Dictionary)
        {
            string Value = GetValues(indata);
            string Key = GetKeys(indata);
            try
            {
                Dictionary.Add(Key, Value);
            }
            catch
            {
                Console.WriteLine("The key is already used. We cant write it to the register");
            }
        }

    }
}
