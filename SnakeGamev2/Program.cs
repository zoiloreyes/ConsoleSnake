
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SnakeGamev2
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.initGame();
        }

        public class Game
        {
            public Snake Snake { get; set; }
            public Fruit Fruit { get; set; }

            public int points = 0;


            public void AddPoint()
            {
                Console.SetCursorPosition(0, 0);
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write("YOU GOT A POINT BOIIII");
                points++;
                Fruit = new Fruit();
            }
            private void winMessage()
            {
                Console.SetCursorPosition(0, 0);
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write("YOU GOT A POINT BOIIII");
            }

            public void autoMove()
            {
                int sleepFor = 300;
                bool eats = false;

                while (true) {
                    
                    Thread.Sleep(sleepFor);
                    Console.Clear();
                    int d = Snake.head.direccion;
                    Console.SetCursorPosition(Console.WindowWidth - 4, Console.WindowHeight - 2);
                    Console.Write(points);
                    Fruit.show();
                    switch (d)
                    {
                        case 1:
                            eats = Snake.Move(0, -1, Fruit,1);
                            break;
                        case 2:
                            eats = Snake.Move(1, 0, Fruit,2);
                            break;
                        case 3:
                            eats = Snake.Move(0, 1, Fruit,3);
                            break;
                        case 4:
                            eats = Snake.Move(-1, 0, Fruit,4);
                            break;

                    }
                    if (eats)
                    {
                        AddPoint();
                    }
                }
            }
            public void initGame()
            {
                Snake = new Snake();
                Fruit = new Fruit();
                ConsoleKeyInfo keyInfo;
                ThreadStart starto = new ThreadStart(autoMove);
                Thread automoveThread = new Thread(starto);
                automoveThread.Start();
                while ((keyInfo = Console.ReadKey(true)).Key != ConsoleKey.Escape)
                {
                    Console.Clear();
                    Console.SetCursorPosition(Console.WindowWidth - 4, Console.WindowHeight - 2);
                    Console.Write(points);
                    Fruit.show();
                    bool eats = false;
                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.UpArrow:
                            eats = Snake.Move(0, -1, Fruit,1);
                            break;
                        case ConsoleKey.DownArrow:
                            eats = Snake.Move(0, 1, Fruit,3);
                            break;
                        case ConsoleKey.LeftArrow:
                            eats = Snake.Move(-1, 0, Fruit,4);
                            break;
                        case ConsoleKey.RightArrow:
                            eats = Snake.Move(1, 0, Fruit,2);
                            break;
                    }
                    if (eats)
                    {
                        AddPoint();
                    }
                }
            }

        }
        public class Fruit
        {
            public Coordinate Location { get; set; }
            public Fruit()
            {
                Random rnd = new Random();
                int x = rnd.Next(0, Console.WindowWidth);
                int y = rnd.Next(0, Console.WindowHeight);
                Location = new Coordinate(x, y);
            }

            public void show()
            {
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition(Location.X, Location.Y);
                Console.Write(" ");
                Console.BackgroundColor = ConsoleColor.Black;
            }
            
        }
        public class Snake
        {
            public List<Coordinate> body;
            public Coordinate head; 
            public Snake()
            {
                head = new Coordinate(0, 0);
                head.direccion = 2;
                body = new List<Coordinate>();
            }
            public bool WillEat(Fruit f)
            {
                if (this.head.X == f.Location.X && this.head.Y == f.Location.Y)
                {
                    return true;
                }
                return false;

            }
            public bool Move(int x, int y,Fruit f, int direccion  = 0)
            {
                
                Coordinate newpos = new Coordinate(head.X + x, head.Y + y);
                newpos.direccion = direccion;
                bool eats = false;
                if (CanMove(newpos))
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.SetCursorPosition(newpos.X, newpos.Y);
                    Console.Write(" ");
                    Console.BackgroundColor = ConsoleColor.Black;
                    lock (body)
                    {
                        foreach (var coord in body)
                        {
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.SetCursorPosition(coord.X, coord.Y);

                            Console.Write(" ");
                            Console.BackgroundColor = ConsoleColor.Black;
                        }
                        if (body.Any())
                        {
                            for (int i = (body.Count - 1); i >= 0; i--)
                            {
                                if (i > 0)
                                {
                                    body[i] = body[i - 1];

                                }
                                else
                                {
                                    body[i] = head;

                                }

                            }
                        }
                        head = newpos;
                        if (WillEat(f))
                        {
                            Coordinate c = head;
                            body.Insert(0, c);
                            eats = true;

                        }
                        else
                        {
                            eats = false;
                        }
                    }
                    
                    
                    


                    
                    
                    
                    
                }
                else
                {
                    Die();
                    
                }
                return eats;
            }

            public void Grow(int direction)
            {

            }

            private bool CanMove(Coordinate move)
            {
                if (move.X < 0 || move.X >= Console.WindowWidth)
                    return false;
                if (move.Y < 0 || move.Y >= Console.WindowHeight)
                    return false;
                foreach(var coord in body)
                {
                    if (move.X == coord.X && move.Y == coord.Y)
                        return false;  
                }
                return true;

            }

            public void Die()
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("You Died");
            }
        }

        public class Coordinate
        {
            public int X { get; set; }
            public int Y { get; set; }
            //1 norte, 2 este, 3 sur, 4 oeste
            public int direccion = 0;
            public Coordinate(int x, int y)
            {
                X = x;
                Y = y;
            }

            public void setCoordinate(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }
    }
}
