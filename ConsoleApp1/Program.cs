using System;
using System.Threading;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Create new Thread...\n");

            // Tạo ra một Thread con, để chạy song song với Thread chính (main thread).
            Thread newThread = new Thread(WriteB);

            Console.WriteLine("Start newThread...\n");

            // Kích hoạt chạy newThread.
            newThread.Start();

            Console.WriteLine("Call Write('-') in main Thread...\n");

            

            // Trong Thread chính ghi ra các ký tự '-'
            while (true)
            {
                Console.Write('-');

                // Ngủ (sleep) 70 mili giây.
                Thread.Sleep(7000);
            }
            

            Console.WriteLine("Main Thread finished!\n");
            Console.Read();
        }

        public static void WriteB()
        {
            // Vòng lặp 100 lần ghi ra ký tự 'B'
            for (int i = 0; i < 100; i++)
            {
                Console.Write('B');
            }

        }
    }
}
