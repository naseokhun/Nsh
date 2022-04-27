using System.Net.Sockets;
using System.Text;
using System.Threading;

public class MyTcpClient
{
    static StringBuilder sb = new StringBuilder();
    static Mutex mutex = new Mutex(false);
    static bool trigger = true;
    public static void Main()
    {
        LinkedList<string> chatlist = new LinkedList<string>();

        string server = "127.0.0.1";
        Int32 port = 13000;
        TcpClient client = null;
        NetworkStream stream = null;
        Console.WriteLine("==============================");
        sb.AppendLine("==============================");
        Console.WriteLine("0 : 접속종료");
        sb.AppendLine("0 : 접속종료");
        Console.WriteLine("1 : 메시지 전송");
        sb.AppendLine("1 : 메시지 전송");
        Console.WriteLine("C : 접속");
        sb.AppendLine("C : 접속");
        Console.WriteLine("==============================");
        sb.AppendLine("==============================");
        bool test = true;

        while (test)
        {
            if (Console.ReadKey().Key == ConsoleKey.C)
            {
                Console.Clear();
                Console.WriteLine(sb.ToString());
                Console.WriteLine("Ex ) 127.0.0.1 13000");
                string[] testinput = Console.ReadLine().Split(' ');
                if (testinput[0] == "127.0.0.1" && testinput[1] == "13000")
                {
                    client = new TcpClient(server, port);
                    stream = client.GetStream();
                    test = false;
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine(sb.ToString());
            }

        }
        Thread readThread = new Thread(() => readMessage(stream, chatlist));
        readThread.Start();
        ConsoleKeyInfo inputkey;
        try
        {

            Console.WriteLine("127.0.0.1:13000에 접속시도중...");
            chatlist.AddLast("127.0.0.1:13000에 접속시도중...");
            string name = "수";
            Byte[] names = System.Text.Encoding.Default.GetBytes(name);
            stream.Write(names, 0, names.Length);
            Console.WriteLine("'주'님께 연결되었습니다.");
            chatlist.AddLast("'주'님께 연결되었습니다.");


            while (true)
            {

                //테스트
                while (trigger)
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
                        // stream.Write(datas, 0, datas.Length);
                        Console.WriteLine("[수] 님이 메시지를 입력중입니다.");
                        Console.SetCursorPosition(0, chatlist.Count + 6);

                        sendMessage(stream, chatlist);


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
        catch (ArgumentNullException e)
        {
            Console.WriteLine("ArgumentNullException: {0}", e);
        }
        catch (SocketException e)
        {
            Console.WriteLine("SocketException: {0}", e);
        }

        Console.WriteLine("\n 상대방이 메시지 입력중 입니다.");
        Console.Read();
    }

    public static void readMessage(NetworkStream stream, LinkedList<string> chatlist)
    {
        while (true)
        {

            Byte[] data = new Byte[256];
            String responseData = String.Empty;
            responseData = null;
            Int32 bytes = stream.Read(data, 0, data.Length);
            responseData = System.Text.Encoding.UTF8.GetString(data, 0, bytes);
            if (responseData != "1")
            {
                if (chatlist.Count > 9)
                {
                    chatlist.RemoveFirst();
                }
                //trigger = true;
                chatlist.AddLast("[주] " + responseData);
                //responseData = null;
            }

            if (responseData == "수" || responseData == "주")
            {
                return;
            }

            if (responseData == "1")
            {

                Console.Clear();
                Console.Write(sb.ToString());
                foreach (string chat in chatlist)
                {
                    Console.WriteLine(chat);
                }
                Console.WriteLine("[주] 님이 메시지를 입력중입니다.");
                Console.SetCursorPosition(0, chatlist.Count + 7);
                responseData = null;

            }

            Thread thread = new Thread(() => updateMsg(chatlist));
            thread.Start();
        }

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

    public static void sendMessage(NetworkStream stream, LinkedList<string> chatlist)
    {
        mutex.WaitOne();
        string message = "test";
        string name = "수";
        message = Console.ReadLine();
        Byte[] data = System.Text.Encoding.Default.GetBytes(message);
        stream.Write(data, 0, data.Length);
        Console.Clear();
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
}