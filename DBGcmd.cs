using System;
using System.IO.Ports; //Nuget System.IO.Ports

class Program
{
    public enum OperationTypes
    {
        Read,
        Write
    }
    public struct Operation
    {
        public string Port { get; set; }
        public OperationTypes OperationType { get; set; }
        public string Register { get; set; }
        public string Value { get; set; }

    }

    public static void Main(string[] args)
    {

        var operation = GetArguments(args);
        SerialPort Port = TryGetPort(operation.Port);
        if (Port == null)
        {
            Console.WriteLine("Can't create a port");
        }
        else
        {
            WriteOrRead(Port, operation.OperationType, operation.Register, operation.Value);
        }

        Console.ReadKey();
    }


    private static void WriteOrRead(SerialPort Port, OperationTypes OperationType, string Register, string Value) 
    {
        if (OperationType == OperationTypes.Read)
        {
            Port.WriteLine("r" + Register);
        }
        if (OperationType == OperationTypes.Write)
        {
            Port.WriteLine("w" + Register + "*" + Value);
        }
    }

    private static Operation GetArguments(string[] args)
    {
        if (args.Length == 4)
        {
            return new Operation
            {
                Port = args[1],
                OperationType = (OperationTypes)Enum.Parse(typeof(OperationTypes), args[2]),
                Register = args[3],
                

            };
        }
        else
        {
            return new Operation
            {
                Port = args[1],
                OperationType = (OperationTypes)Enum.Parse(typeof(OperationTypes), args[2]),
                Register = args[3],
                Value = args[4]

            };
        }
        
    }

    private static SerialPort TryGetPort(string name)
    {
        SerialPort Port = null;
        try
        {

            Port = CreatePort(name);
        }
        catch (Exception ex)
        {
            string Error = ex.Message + "  " + DateTime.Now;
            System.IO.File.WriteAllText(@"C:\Users\Fullen\Desktop\DBGcmd\DBGcmd\log.txt", Error);
            throw;
            
        }
        return Port;
    }

   private static  SerialPort CreatePort (string name)
    {
        SerialPort Port = new SerialPort(name);
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
        Console.WriteLine("Data Received:");
        Console.Write(indata);

    }

}