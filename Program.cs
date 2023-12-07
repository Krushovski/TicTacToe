using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTT
{
    class Node
    {
        public List<Node> Children = new List<Node>();
        public int[,] Board = new int[3, 3];
        public int? Value;
    }
    internal class Program
    {
        public const int PLAYER1 = 1;
        public const int PLAYER2 = 2;
        public static Int32 Winner(Int32[,] board)
        {
            var col1 = board[0, 0] & board[1, 0] & board[2, 0];
            var col2 = board[0, 1] & board[1, 1] & board[2, 1];
            var col3 = board[0, 2] & board[1, 2] & board[2, 2];
            var line1 = board[0, 0] & board[0, 1] & board[0, 2];
            var line2 = board[1, 0] & board[1, 1] & board[1, 2];
            var line3 = board[2, 0] & board[2, 1] & board[2, 2];
            var diagonal1 = board[0, 0] & board[1, 1] & board[2, 2];
            var diagonal2 = board[0, 2] & board[1, 1] & board[2, 0];

            return
            col1 == 1 ||
            col2 == 1 ||
            col3 == 1 ||
            line1 == 1 ||
            line2 == 1 ||
            line3 == 1 ||
            diagonal1 == 1 ||
            diagonal2 == 1 ? 1 :
            col1 == 2 ||
            col2 == 2 ||
            col3 == 2 ||
            line1 == 2 ||
            line2 == 2 ||
            line3 == 2 ||
            diagonal1 == 2 ||
            diagonal2 == 2 ? 2 :
            0;
        }

        public static void Grow(
            Node node,
            int player,
            int maximizer)
        {
            var winner = Winner(node.Board);
            if (winner != 0)
            {
                node.Value =
                    winner ==
                    maximizer ?
                    1 :
                    -1;
                return;
            }
            for (int i = 0; i < node.Board.GetLength(0); i++)
            {
                for (int j = 0; j < node.Board.GetLength(1); j++)
                {
                    if (node.Board[i, j] == 0)
                    {
                        var newChild = new Node();

                        Array.Copy(node.Board, newChild.Board, node.Board.Length);

                        newChild.Board[i, j] = player;

                        node.Children.Add(newChild);

                        Grow(newChild,
                            player ==
                            PLAYER1 ?
                            PLAYER2 :
                            PLAYER1,
                            maximizer);
                        if (node.Value == null ||
                          (player == maximizer && node.Value < newChild.Value) ||
                          (player != maximizer && node.Value > newChild.Value))
                        {
                            node.Value = newChild.Value;
                        }
                    }
                }
            }
            if (node.Value == null)
            {
                node.Value = 0;
            }
        }
        static void Main(string[] args)
        {
            Node root = new Node();
            Grow(root,
                PLAYER1,
                PLAYER1);
            Node currentNode = root;
            int currentPlayer = PLAYER1;
            while (currentNode != null)
            {
                Console.Clear();
                PrintBoard(currentNode.Board);
                if (Winner(currentNode.Board) == 1)
                {
                    Console.WriteLine();
                    Console.WriteLine("Winner is Player 1");
                    Console.WriteLine("Press 'q' to escape..");
                    if (Console.ReadKey().KeyChar == 'q')                    
                        return;
                    
                }
                if (Winner(currentNode.Board) == 2)
                {
                    Console.WriteLine();
                    Console.WriteLine("Winner is Player 2");
                    Console.WriteLine("Press 'q' to escape..");
                    if (Console.ReadKey().KeyChar == 'q')                     
                        return;
                }
                if (currentPlayer == PLAYER1)
                {
                    int move = Console.ReadKey().KeyChar - '1';
                    Node moveNode = null;
                    foreach (var child in currentNode.Children)
                    {
                        for (int i = 0; i < currentNode.Board.GetLength(0) && moveNode == null; i++)
                        {
                            for (int j = 0; j < currentNode.Board.GetLength(1) && moveNode == null; j++)
                            {
                                if (currentNode.Board[i, j] != child.Board[i, j] && move == (i * 3 + j))
                                {
                                    moveNode = child;
                                }
                            }
                        }
                        if (moveNode != null)
                        {
                            currentNode = moveNode;
                            break;
                        }
                    }
                }
                else
                {
                    Node minNode = null;
                    foreach (var child in currentNode.Children)
                    {
                        if (minNode == null || minNode.Value > child.Value)
                        {
                            minNode = child;
                        }
                    }
                    currentNode = minNode;
                }

                currentPlayer =
                    currentPlayer ==
                    PLAYER1 ?
                    PLAYER2 :
                    PLAYER1;                                                                    
            }
            Console.WriteLine();
            Console.WriteLine("Tie!");
            Console.WriteLine("Press 'q' to escape..");
            if (Console.ReadKey().KeyChar == 'q')
                return;
        }
        public static void PrintBoard(int[,] board)
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    Console.Write(
                        (board[i, j] == PLAYER1 ? "X" :
                        board[i, j] == PLAYER2 ? "O" :
                        (i * 3 + j + 1).ToString()) + " ");
                }
                Console.WriteLine();
            }

        }
    }
}
