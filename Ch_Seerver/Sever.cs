using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

class MyTcpListener
{
    static Mutex mutex = new Mutex(false);
    static string message = String.Empty;
    public static void Main()
    {
       
        LinkedList<string> list = new LinkedList<string>();
        NetworkStream stream = null;
        list.AddLast("Waiting Connection...");
        Console.Clear();
        foreach (string chat in list)
        {
            Console.WriteLine(chat);
        }
        
       
        
        TcpListener server = null;

        Int32 port = 9000;
        IPAddress localAddr = IPAddress.Parse("127.0.0.1");
        server = new TcpListener(localAddr, port);

        server.Start();

        TcpClient client = server.AcceptTcpClient();
        stream = client.GetStream();

        Byte[] bytes = new Byte[256];
        String data = null;
        string message = String.Empty;
        Thread server_listen = new Thread(() => Listen(list, stream, message));
        server_listen.Start();
        ConsoleKeyInfo inputkey;
        try
        {
                list.AddLast("'수' 님이 127.0.0.1에서 접속하셨습니다");
                Console.Clear();
                foreach (string chat in list)
                {
                    Console.WriteLine(chat);
                }
                
                
                while (true)
                {
                    inputkey = Console.ReadKey();
                    
                    if (inputkey.KeyChar == 't')
                    {

                        Console.Clear();
                        foreach (string chat in list)
                        {
                            Console.WriteLine(chat);
                        }
                        Console.WriteLine("[쭈] 님이 메시지를 입력중입니다.");
                        write(list, stream);
                    }

                    if (inputkey.KeyChar == 'q')
                    {
                        Environment.Exit(0);                       
                    }
                }     
        }
        catch (SocketException e)
        {
            //Console.WriteLine("SocketException: {0}", e);
        }
        
        Console.WriteLine("\nHit enter to continue...");
        //Console.Read();
    }

    public static void Listen(LinkedList<string> list,  NetworkStream stream, string message)
    {
            while (true)
            {
            
            
            Byte[] bytes = new Byte[256];
            // 응답 UTF8 표현을 저장할 문자열입니다.
            // TcpServer 응답 바이트의 첫 번째 배치를 읽습니다.

            message = null;
             Int32 byt = stream.Read(bytes, 0, bytes.Length);
            message = System.Text.Encoding.Default.GetString(bytes, 0, byt);

            if (message != "t")
            {
                if (list.Count > 9)
                {
                    list.RemoveFirst();
                }
                list.AddLast("[수] " + message);
            }
            
            if (message == "t")
            {
                
                Console.Clear();
                foreach (string chat in list)
                {
                    Console.WriteLine(chat);
                }
                Console.WriteLine("[수] 님이 메시지를 입력중입니다.");

                message = null;

                
                
            }
            Thread thread = new Thread(() => update(list));
            thread.Start();
        }
    }
    public static void write(LinkedList<string> list, NetworkStream stream)
    {
        while (true)
        {
            mutex.WaitOne();
            string message = Console.ReadLine();
            Byte[] data = System.Text.Encoding.Default.GetBytes(message);
            stream.Write(data, 0, data.Length);
            Console.Clear();
            if (list.Count > 10)
            {
                list.RemoveFirst();
            }
            list.AddLast("[쭈] " + message);

            foreach (string chat in list)
            {
                Console.WriteLine(chat);
            }
            /*
            message = Console.ReadLine();
                byte[] byteData = new byte[message.Length];
                byteData = Encoding.Default.GetBytes(message);

                stream.Write(byteData, 0, byteData.Length);
            */
            mutex.ReleaseMutex();
        }
    }
    public static void update(LinkedList<string> list)
    {
        mutex.WaitOne();
        Console.Clear();
        foreach (string chat in list)
        {
            Console.WriteLine(chat);
        }
       mutex.ReleaseMutex();
    }
}