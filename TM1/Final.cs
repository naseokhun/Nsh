using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

delegate void CallBack(); // 콜백

class Program
{

    static Random r = new Random(); // 랜덤
    static Stopwatch watch = new Stopwatch();

    public static void Delay(int ms)
    {
        DateTime dateTimeNow = DateTime.Now;
        TimeSpan duration = new TimeSpan(0, 0, 0, 0, ms);
        DateTime dateTimeAdd = dateTimeNow.Add(duration);
        while (dateTimeAdd >= dateTimeNow)
        {
            dateTimeNow = DateTime.Now;
        }
        return;
    }

    public static IEnumerable<int> Ts()
    {
        while (true)
        {
            if (watch.ElapsedMilliseconds >= 3000)
            {

                int x = r.Next(1, 3);

                Console.WriteLine("TS : 옷 입었어");

                yield return x;
            }
        }
    }

    public static IEnumerable<int> Min()
    {
        while (true)
        {
            if (watch.ElapsedMilliseconds >= 5000)
            {
                int x = r.Next(1, 3);

                Console.WriteLine("MIN : 나도 입었어");

                yield return x;
            }
        }
    }

    public static void shirt()
    {
        while (true)
        {
            IEnumerable<int> c = Ts();
            IEnumerator<int> e = c.GetEnumerator();

            IEnumerable<int> a = Min();
            IEnumerator<int> b = a.GetEnumerator();

            e.MoveNext();

            b.MoveNext();

            int ret1 = e.Current;
            int ret2 = b.Current;

            if (ret1 == ret2)
            {
                Console.WriteLine("옷이 같네?");
                break;
            }
            else
            {
                Console.WriteLine("다르잖아 다시 입고와");
                watch.Restart();
            }
        }
    }

    static void Main()
    {
        watch.Start();

        shirt();

        Console.WriteLine("이제 아무도 못보게 가자");

    }
}