using System.Net.Sockets;
using System;
using System.Text;
using System.Net;

public class MyTcpClient
{

    static Mutex mutex = new Mutex(false);
    
    static string message = String.Empty;
    public static void Main()
    {

        LinkedList<string> list = new LinkedList<string>();

        NetworkStream stream = null;
        Console.WriteLine("/cIP:Port를 입력해주세요");
         bool state = true;
        
        while (state)
        {
            string Port = "9000";
            string server = "127.0.0.1";
            string serverInput = Console.ReadLine();

            if (serverInput == $"/c {server}:{Port}")
            {
                
                Console.Clear();
                /*foreach (string chat in list)
                {
                    Console.WriteLine(chat);
                }*/
                Int32 port = 9000;               
                TcpClient client = new TcpClient(server, port);
                stream = client.GetStream();                
                state = false;
            }
            else
            {
                Console.WriteLine("서버가 연결되지 않았습니다 주소를 올바르게 입력했는지 확인해 주세요");
                state = true;

            }
        }
        
        Thread client_listen = new Thread(() => Listen(list, stream, message));
        client_listen.Start();
        ConsoleKeyInfo inputkey;
        try
        {
            Console.WriteLine("127.0.0.1:9000에 접속시도중... ");
            Console.WriteLine("'쭈'님께 연결되었습니다. ");
            list.AddLast("127.0.0.1:9000에 접속시도중... ");
            list.AddLast("'쭈'님께 연결되었습니다. ");

            while (true)
            {
                inputkey = Console.ReadKey();
                //update(list, message, mutex);
                if (inputkey.KeyChar == 't')
                {

                    Console.Clear();
                    foreach (string chat in list)
                    {
                        Console.WriteLine(chat);
                    }
                    Console.WriteLine("[수] 님이 메시지를 입력중입니다.");
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
        Console.WriteLine("\n 상대방이 메시지 입력중 입니다.");
        //Console.Read();
    }

    public static void Listen(LinkedList<string> list, NetworkStream stream, string message)
    {
        while (true)
        {
            // 응답 UTF8 표현을 저장할 문자열입니다.
            // TcpServer 응답 바이트의 첫 번째 배치를 읽습니다.
            Byte[] data = new Byte[256];
             message = null;

            Int32 bytes = stream.Read(data, 0, data.Length);
            message = System.Text.Encoding.Default.GetString(data, 0, bytes);
            if (message != "t")
            {
                if (list.Count > 9)
                {
                    list.RemoveFirst();
                }
               
                list.AddLast("[쭈] " + message);
               
            }
          
            if (message == "t")
            {              
                Console.Clear();
                foreach (string chat in list)
                {
                    Console.WriteLine(chat);
                }
                Console.WriteLine("[쭈] 님이 메시지를 입력중입니다.");

                message = null;
     

            }
            Thread thread = new Thread(() => update(list));
            thread.Start();
        }
    }
    public static void write(LinkedList<string> list, NetworkStream stream)
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
        list.AddLast("[수] " + message);
        
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