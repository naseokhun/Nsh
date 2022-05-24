using System;
using System.Collections;
using System.Collections.Generic;

class Program
{

    static Random r = new Random();

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

    public static IEnumerable<int> Foo()
    {
        int x = r.Next(1, 6);

        Delay(3000);

        Console.WriteLine("옷입음");


        yield return x;
    }
 

    static void Main()
    {
        while (true)
        {
            // 코루틴 메소드 호출
            IEnumerable<int> c = Foo();
            IEnumerator<int> e = c.GetEnumerator();

            int y = r.Next(1, 6);

            Delay(5000);

            Console.WriteLine("옷입음");
            e.MoveNext();
            int ret1 = e.Current;
           
            if (ret1 == y)
            {
                Console.WriteLine("같으니까 이제 가자");
                break;
            }
            else
            {
                Console.WriteLine("다르잖아 다시 입고와");

            }

        }
       
    }
}