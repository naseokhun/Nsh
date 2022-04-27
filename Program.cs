using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Threading;


class MyTcpListener
{
    static NetworkStream stream;
    static StringBuilder sb = new StringBuilder();
    static bool trigger = true;
    static Mutex mutex = new Mutex();
    static LinkedList<string> chatlist = new LinkedList<string>();
    public static void Main()
    {

        Thread sendMsgs = new Thread(Run);
        sendMsgs.Start();

        TcpListener server = null;
        try
        {
            Int32 port = 13000;
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");

            server = new TcpListener(localAddr, port);

            server.Start();

            Byte[] bytes = new Byte[256];
            String data = null;
            String cilentName = null;

            while (true)
            {
                //테스트 test
                Console.WriteLine("==============================");
                sb.AppendLine("==============================");
                Console.WriteLine("0 : 접속종료");
                sb.AppendLine("0 : 접속종료");
                Console.WriteLine("1 : 메시지 전송");
                sb.AppendLine("1 : 메시지 전송");
                Console.WriteLine("==============================");
                sb.AppendLine("==============================");

                Console.WriteLine("Waiting for a connection... ");
                chatlist.AddLast("Waiting for a connection... ");

                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Connected!");
                chatlist.AddLast("Connected!");
                data = null;

                stream = client.GetStream();

                int i;
                while (true)
                {
                    i = stream.Read(bytes, 0, bytes.Length);
                    data = null;
                    data = System.Text.Encoding.UTF8.GetString(bytes, 0, i);

                    if (data != "1" && cilentName != null)
                    {
                        if (chatlist.Count > 9)
                        {
                            chatlist.RemoveFirst();
                        }
                        chatlist.AddLast("[" + cilentName + "] " + data);
                        //data = null;
                    }

                    if (data == "수")
                    {
                        cilentName = data;
                        Console.WriteLine("[" + data + "]" + "님이 127.0.0.1에서 접속하셨습니다.");
                        chatlist.AddLast("[" + data + "]" + "님이 127.0.0.1에서 접속하셨습니다.");

                    }
                    else if (data == "1")
                    {
                        data = null;
                        Console.Clear();
                        Console.Write(sb.ToString());
                        foreach (string chat in chatlist)
                        {
                            Console.WriteLine(chat);
                        }
                        Console.WriteLine("[수] 님이 메시지를 입력중입니다.");
                        Console.SetCursorPosition(0, chatlist.Count + 7);

                    }

                    Thread thread = new Thread(() => updateMsg(chatlist));
                    thread.Start();
                }

            }
        }
        catch (SocketException e)
        {
            Console.WriteLine("SocketException: {0}", e);
        }
        finally
        {
            server.Stop();
        }

        Console.WriteLine("\nHit enter to continue...");
        Console.Read();
    }

    public static void Run()
    {
        ConsoleKeyInfo inputkey;
        while (true)
        {

            while (trigger)
            {
                if (trigger)
                {

                    inputkey = Console.ReadKey();
                    if (inputkey.KeyChar == '1')
                    {
                        Console.Clear();
                        Console.Write(sb.ToString());
                        
                        foreach (string chat in chatlist)
                        {
                            Console.WriteLine(chat);
                        }
                        //Byte[] datas = System.Text.Encoding.Default.GetBytes("1");
                        //stream.Write(datas, 0, datas.Length);
                        Console.WriteLine("[주] 님이 메시지를 입력중입니다.");
                        Console.SetCursorPosition(0, chatlist.Count + 6);

                        sendMsg();

                    }
                    else if (inputkey.KeyChar == '0')
                    {
                        Environment.Exit(0);
                    }
                    else
                    {
                        Console.Clear();
                        Console.Write(sb.ToString());
                        foreach (string chat in chatlist)
                        {
                            Console.WriteLine(chat);
                        }
                    }

                }


            }
        }
    }
    public static void sendMsg()
    {
        mutex.WaitOne();

        String name = "주";
        string message = "test";

        message = Console.ReadLine();
        Console.Clear();
        Byte[] data = System.Text.Encoding.Default.GetBytes(message);

        stream.Write(data, 0, data.Length);

        if (chatlist.Count > 9)
        {
            chatlist.RemoveFirst();
        }
        chatlist.AddLast("[" + name + "] " + message);
        Console.Write(sb.ToString());
        foreach (string chat in chatlist)
        {
            Console.WriteLine(chat);
        }
        Console.SetCursorPosition(0, chatlist.Count + 5);
        mutex.ReleaseMutex();

    }

    public static void updateMsg(LinkedList<string> chatlist)
    {
        mutex.WaitOne();
        Console.Clear();
        Console.Write(sb.ToString());
        foreach (string chat in chatlist)
        {
            Console.WriteLine(chat);
        }
        Console.SetCursorPosition(0, chatlist.Count + 5);
        mutex.ReleaseMutex();
    }
}