using System;
using System.Linq;
using System.Collections.Generic;


namespace GameLogic
{
    class Snake
    {
        public Snake((int, int)[] initBody, (int,int) initDirection )
        {
            snakeBody = new List<(int, int)>(initBody);
            snakeDirection = initDirection;
        }

        public void takeStep(){
            //new head posision = snakeHead + snakeDirection
            (int,int) position = (head().Item1 + snakeDirection.Item1, head().Item2 + snakeDirection.Item2);
            
            for (int i = 1; i < snakeBody.Count; i++)
            {
                snakeBody[i - 1] = snakeBody[i];
            }
            snakeBody[snakeBody.Count - 1] = position;
        }

        public void takeStepAddLen(){
            //new head posision = snakeHead + snakeDirection
            (int,int) position = (head().Item1 + snakeDirection.Item1, head().Item2 + snakeDirection.Item2);
            snakeBody.Add(position);
        }

        public bool checkSelfCollision(){
            int index = snakeBody.IndexOf(head());
            return index < snakeBody.Count - 1;
        }

        public void wrapSnake(int width, int height){

            for (int i = 0; i < snakeBody.Count; i++)
            {
                if (snakeBody[i].Item1 >= width)
                    snakeBody[i] = (0, snakeBody[i].Item2);
                else if (snakeBody[i].Item1 < 0)
                    snakeBody[i] = (width-1, snakeBody[i].Item2);
                
                if (snakeBody[i].Item2 >= height)
                    snakeBody[i] = (snakeBody[i].Item1, 0);
                else if (snakeBody[i].Item2 < 0)
                    snakeBody[i] = (snakeBody[i].Item1, height-1);
                
            }

        }

        public bool checkAppleCollision(Apple apple){

            return head() == apple.Position;

        }

        public void setDirection((int,int) direction){
            snakeDirection = direction;
        }

        public (int,int) head(){
            return snakeBody[snakeBody.Count - 1];
        }

        public bool isBodyAt((int,int) position){
            return snakeBody.Contains(position);
        }

        List<(int,int)> snakeBody;
        public (int,int) snakeDirection;
        
    }

    class Apple
    {
        public Apple((int,int) position)
        {
            Position = position;
            
        }
        public void changePosition((int,int) position){
            Position = position;
        }

        public void newPosistion(int width, int height){
            Random rnd = new Random();
            Position = (rnd.Next(width), rnd.Next(height));

        }
        public (int, int) Position;
        
    }

    class Game
    {
        public Game(int height, int width)
        {
            Width = width;
            Height = height;

            snake = new Snake(new (int, int)[]{(0,0),(1,0),(2,0),(3,0),(4,0)}, UP);

            apple = new Apple((19, 9));

            running = true;
            
            score = 0;
            
            
        }

        public void run(){
            do
            {
                getInput();
                processInput();

                if (snake.checkAppleCollision(apple)){
                    apple.newPosistion(Width, Height);
                    snake.takeStepAddLen();
                    score++;
                }
                else
                    snake.takeStep();
                
                snake.wrapSnake(Width, Height);
                running = !snake.checkSelfCollision() && running;
                render();
                
            } while (running);
            System.Console.WriteLine("game over");
            System.Console.WriteLine("score: {0}", score);
        }

        ///<summary>
        ///gets input from the console and checks if it's a valid char
        ///</summary>
        ///<returns>
        ///stores user-input in class as 'input'
        ///</returns>
        public void getInput(){
            string tempInput = Console.ReadLine();
            
            if (tempInput.Length > 0)
                input = tempInput[0];

            
            
        }


        ///<summary>
        ///processes the input gotten from GetInput()
        ///</summary>
        public void processInput(){
            switch (input)
            {
                case 'w':
                    if(snake.snakeDirection != DOWN)
                        snake.setDirection(UP);
                    break;
                case 's':
                    if(snake.snakeDirection != UP)
                        snake.setDirection(DOWN);
                    break;
                case 'a':
                    if(snake.snakeDirection != RIGHT)
                        snake.setDirection(LEFT);
                    break;
                case 'd':
                    if(snake.snakeDirection != LEFT)
                        snake.setDirection(RIGHT);
                    break;
                case 'c':
                    running = false;
                    break;
                default:
                    break;
            }

        }

        

        public string[,] boardMatrix(){
            
            string[,] board = new string[Width, Height];
            
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if((x,y) == snake.head())
                        board[x,y] = "O";
                     else if (snake.isBodyAt((x,y)))
                        board[x,y] = "x";
                    else if ((x,y) == apple.Position)
                        board[x,y] = "a";
                    else
                        board[x,y] = " ";
                }
            }

            return board;
        }



        public void render(){
            string[,] board = boardMatrix();
            string topBottomBorder = new string('=', Width);
            topBottomBorder = "+" + topBottomBorder + "+";

            for (int i = Height - 1; i >= 0 ; i--)
            {
                if (i == Height-1)
                    System.Console.WriteLine(topBottomBorder);
                for (int j = 0; j < Width; j++)
                {
                    if(j == 0)
                        System.Console.Write("|");
                    System.Console.Write(board[j,i]);
                    if(j == Width-1)
                        System.Console.Write("|");
                }
                System.Console.WriteLine("");
                if (i == 0)
                    System.Console.WriteLine(topBottomBorder);
            }


        }
        
        public int Height { get; set; }
        public int Width { get; set; }

        public int score;

        public Snake snake;
        Apple apple;
        char input;
        public bool running;

        (int,int) UP = (0,1);
        (int,int) DOWN = (0,-1);
        (int,int) LEFT = (-1,0);
        (int,int) RIGHT = (1,0);
    }
}
