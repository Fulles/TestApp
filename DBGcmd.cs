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
            Port.Write("r" + Register);
        }
        if (OperationType == OperationTypes.Write)
        {
            Port.Write("w" + Register + "*" + Value);
        }
    }

    private static Operation GetArguments(string[] args)
    {
        var operation = new Operation { };
        try
        {
            OperationTypes OperationType = (OperationTypes)Enum.Parse(typeof(OperationTypes), args[2]);
            if (OperationType == OperationTypes.Read)
            {
               operation =  new Operation
                {
                    Port = args[1],
                    OperationType = (OperationTypes)Enum.Parse(typeof(OperationTypes), args[2]),  ////////////////TO DO for (Myself)ANDRUSHA)
                    Register = args[3],
                };
            }

            if (OperationType == OperationTypes.Write)
            {
               operation = new Operation
                {
                    Port = args[1],
                    OperationType = (OperationTypes)Enum.Parse(typeof(OperationTypes), args[2]),
                    Register = args[3],
                    Value = args[4]

                };
            }
        }
        catch (Exception ex)
        {
            string Error = ex.Message + "  " + DateTime.Now + "\n";
            System.IO.File.AppendAllText(@"C:\Users\Fullen\Desktop\DBGcmd\DBGcmd\log.txt", Error);
            Console.WriteLine("Enter correct type of operation (Read or Write)");
            
        }
        return operation; 
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
            string Error = ex.Message + "  " + DateTime.Now + "\n";
            System.IO.File.AppendAllText(@"C:\Users\Fullen\Desktop\DBGcmd\DBGcmd\log.txt ", Error );
            
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
