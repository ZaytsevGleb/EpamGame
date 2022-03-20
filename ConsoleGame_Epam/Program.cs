using System;
using static System.Console;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace ConsoleGame_Epam
{
    class Program
    {
        const string firstPlayer = "X";
        const  string secondPlayer = "0"; 
        public static int height = 30;
        public static int width = 20;
        public static int heightGround = width + 2;
        public static int widthGround = height + 2;
        public static int count = 20;
        public static string groundMark = "■";
        public static int step1 = 0;
        public static int step2 = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("Правила игры:\n1.Кто займет большее количество ячеек к концу ходов, тот и победил.\n2.Если к концу игры у двух игроков будет одинаковое количество ячеек, то будет ничья.\n3.В игре используются только клавиша ввода и клавиши чисел.\n4.Если все ячейки поля заняты, то произойдет досрочное окончание игры и будет выявлен победитель стандартным способом.\n5.Минимальный размер поля  20 на 30. Максимальный 60 на 60.\nХорошей игры, мы начинаем.\nНажмите Enter.");
            while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
            Console.WriteLine("Введите длину поля: ");
            width = isValue(width,20);
            Console.WriteLine("Введите высоту поля: ");
            height = isValue(height,30);
            Console.WriteLine("Введите количество ходов для каждого из игроков: ");
            count = isValue(count);
            if ((height * width / (count * 2)) > 15)
            {
                while(height * width / (count * 2) > 15)
                {
                    Console.WriteLine($"Заданное количество ходов слишком маленькое для поля с длиной  {height} и выcотой  {width}.\nВведите новое число:");
                    count = isValue(count);
                }
            }
            heightGround = width + 2;
            widthGround = height + 2;
            string[,] playground = new string[widthGround, heightGround];
            CreatePlayground(playground, widthGround, heightGround);
            Play(playground, count);
            WhoWin(playground);
        }
        public static void CreatePlayground(string[,] arr, int width,int height)
        {
            Console.Clear();
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (i == 0 || i == width - 1)
                    {
                        if (j != 0 && j != height - 1)
                        {
                            arr[i, j] = j.ToString();
                        }
                    }
                    else if (j == 0 || j == height - 1)

                    {
                        if (i != 0 && i != width)
                        {
                            arr[i, j] = i.ToString();
                        }
                    }
                    else
                    {
                        arr[i, j] = groundMark;
                    }
                    Console.Write((String.Format("{0,3}", arr[i, j])));
                }
                Thread.Sleep(10);
                Console.WriteLine();
            }
        }
        public static void Play(string[,] arr, int count)
        {
            int move = 1;
            int moveCount = 0;
            do
            {
                if (move == 1 && moveCount<count*2)
                {
                    
                    step1++;
                    Console.WriteLine($"Игрок 1 ваш {step1} ход :");
                    Player_move(arr,move);
                    move = 2;
                    moveCount++;
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Draw(arr);
                    while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
                }
                else if (move == 2 && moveCount < count*2)
                {
                    step2++;
                    Console.WriteLine($"Игрок 2 ваш {step2} ход :");
                    Player_move(arr,move);
                    move = 1;
                    moveCount++;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Draw(arr);
                    while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
                }
                bool field = End(arr);
                if(field == false)
                {
                    moveCount = count * 2;
                }
            } while (moveCount < count * 2);
        }
        public static void Player_move(string[,] arr, int move)
        {
            int x = 0;
            int y = 0;
            Console.WriteLine("Бросаем кубики");
            while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
            Random rnd = new Random();
            int randomResult1 = rnd.Next(1, 7);
            int randomResult2 = rnd.Next(1, 7);
            bool res =  CanContain(arr, randomResult1, randomResult2);
            if(res == false)
            {
                Console.WriteLine($"Упс, кажется поле с шириной - {randomResult1} и высотой - {randomResult2} нельзя разместить.");
                Console.WriteLine("Ладно-ладно, дам одну попытку перебросить кубики, но только одну!");
                while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
                randomResult1 = rnd.Next(1, 7);
                randomResult2 = rnd.Next(1, 7);
                res = CanContain(arr, randomResult1, randomResult2);
                if(res == false)
                {
                    Console.WriteLine("\n" + randomResult1 + "\n" + randomResult2);
                    Console.WriteLine("Эх, видимо не судбьа\nПропускаем ход");
                    while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
                    Console.Clear();
                    return;
                }

            }
            for (int i = 0; i <= 6; i++)
            {
                Console.Write(".");
                Thread.Sleep(100);
            }
            Console.WriteLine("\n" + randomResult1 + "\n" + randomResult2);
            Console.WriteLine("Введите координаты заполнения");
            x = isValue(x);
            y = isValue(y);
            bool field = IsFree(arr, x, y, randomResult1, randomResult2);

            while (field == false)
            {
                Console.WriteLine("Данные координаты выходят за границы поля либо заходят на уже занятую территорию. Введите новые координаты");
                x = isValue(x);
                y = isValue(y);
                field = IsFree(arr, x, y, randomResult1, randomResult2);
            }
            Console.Clear();

                for (int i = y; i < y + randomResult2; i++)
                {
                    for (int j = x; j < x + randomResult1; j++)
                    {
                        if (move == 1)
                        {
                            arr[i, j] = firstPlayer;
                        }
                        else
                        {
                            arr[i, j] = secondPlayer;
                        }

                    }
                }
        }
        public static void Draw(string[,] arr)
        {
           
            for (int i = 0; i < widthGround; i++)
            {
                for (int j = 0; j < heightGround; j++)
                {
                    Console.Write((String.Format("{0,3}", arr[i, j])));
                }
                Thread.Sleep(10);
                Console.WriteLine();
            }
        }
        public static bool IsFree(string[,] arr, int x, int y, int rnd1,int rnd2)
        {
            if (y + rnd2 > widthGround - 1 || x + rnd1 > heightGround - 1 || x == 0 || y == 0 )
            {
                return false;
            }
            else
            {

                for (int i = y; i < y + rnd2; i++)
                {
                    for (int j = x; j < x + rnd1; j++)
                    {
                        if (arr[i, j] == firstPlayer || arr[i, j] == secondPlayer)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        public static void WhoWin(string[,] arr)
        {
            int first = 0;
            int second = 0;
            for (int i = 0; i < widthGround; i++)
            {
                for (int j = 0; j < heightGround; j++)
                {
                    if(arr[i,j] == firstPlayer)
                    {
                        first++;
                    }else if(arr[i,j] == secondPlayer)
                    {
                        second++; 
                    }
                }
            }
            if (first > second)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\nПервый игрок победил!!");
            }
            else if(second > first)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\nВторой игрок победил");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Ничья");
            }
        }
        public static int isValue(int value)
        {
            while (!int.TryParse(Console.ReadLine(), out value))
            {
                Console.WriteLine("Введенная строка не является числом, повторите ввод");
            }
            return value;
        }
        public static int isValue(int value, int min)
        {
            while (!int.TryParse(Console.ReadLine(), out value))
            {
                Console.WriteLine("Введенная строка не является числом, повторите ввод");
            }
            while (value > 60 || value < min)
            {
                Console.WriteLine($"Введенная строка не должна быть меньше минимального значения {min} и больше максимального значения 60"); ;
                while (!int.TryParse(Console.ReadLine(), out value))
                {
                }
            }
            return value;
        }
        public static bool CanContain(string[,] arr,int rnd1, int rnd2 )
        {
            bool flag = true;
            for (int i = 1; i < widthGround-1 && i+rnd2-1 < widthGround-1; i++)
            {
                for (int j = 1; j < heightGround-1 && j + rnd1-1 < heightGround-1; j++)
                {
                    if(arr[i,j] == groundMark && j+rnd1-1 <heightGround-1 && i+rnd2-1 <widthGround-1)
                    {
                        for (int a = i; a < i+rnd2; a++)
                        {
                            for (int b = j; b < j+rnd1; b++)
                            {
                                if (arr[a, b] != groundMark)
                                {
                                    flag = false;
                                    break;
                                }
                            }
                            if(flag == false)
                            {
                                break;
                            }
                        }
                        if(flag == true)
                        {
                            return flag;
                        }
                        else
                        {
                            j = j + rnd1-1;
                            flag = true;
                        }
                    }
                }
            }
            return false;
        }
        public static bool End(string[,]arr)
        {
            for (int i = 1; i < widthGround-1; i++)
            {
                for (int j = 1; j < heightGround-1; j++)
                {
                    if (arr[i,j] == groundMark)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

    }
}
