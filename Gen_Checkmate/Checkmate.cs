using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Gen_Checkmate
{
    public static class Checkmate
    {
        private const string Bking = "bK";
        private const string BOther = "bO";
        private const string Wking = "wK";
        private const string WRook = "wR";
        private const string WKnight = "wN";
        private const string Empty = "..";                              

        public static bool IsCheckmate(string[] lines)
        {
            var board = BuildBoard(lines);
            if (null == board) throw new ArgumentException("Invalid board situation input", "lines");

            Console.WriteLine();
            Console.WriteLine("Left Top corner is [x = 0, y = 0], x ++> right, y ++> down");
            var black = BlackBoard(board);
            Console.WriteLine("Black");
            TestShowBoard(black);

            var white = WhiteBoard(board, black);
            Console.WriteLine("White");
            TestShowBoard(white);
            
            return TestCheckmate(black, white);
        }

        /// <summary>
        /// pre: only black king can move!
        /// </summary>
        /// <param name="black"></param>
        /// <param name="white"></param>
        /// <returns></returns>
        private static bool TestCheckmate(int[,] black, int[,] white)
        {
            Console.WriteLine();
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    if ((black[x, y] == 1 || black[x, y] == 4) && (white[x, y] == 0))
                    {
                        Console.WriteLine("Black_King can safely move to square [x = {0}, y = {1}]", x, y);
                        return false ;
                    }
                }
            }
            Console.WriteLine();
            return true;
        }

        private static void TestShowBoard(int[,] board)
        {
            Console.WriteLine();
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++) 
                    Console.Write("\t" + board[x, y]);
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        /// <summary>
        /// build black king move board:  
        ///  0 = no move,
        ///  1 = can move,
        ///  2 = black other, cannot move,
        /// 
        ///  3 = white piece,
        ///  4 = white piece & can move,
        /// </summary>
        /// <param name="situation"></param>
        /// <returns></returns>
        static int[,] BlackBoard(List<List<Piece>> situation)
        {
            var board = InitBoard();
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    if (null == situation[y][x]) continue;

                    switch (situation[y][x].ChessType)
                    {
                        case PieceType.BlackOther:
                            board[x, y] = 2;
                            break;
                        case PieceType.BlackKing:
                            AssignKingMove(board, situation[y][x]);
                            break;
                        default://other white piece
                            board[x, y] = board[x, y] == 1 ? 4 : 3;
                            break;
                    }
                }
            }

            return board;
        }
        
        /// <summary>
        /// 8 cases
        /// </summary>
        /// <param name="board"></param>
        /// <param name="king"></param>
        private static void AssignKingMove(int[,] board, Piece king)
        {
            var m = king.X - 1;
            var n = king.Y;
            if (IsValid(m, n) && board[m, n] != 2) board[m, n] = board[m, n] == 3 ? 4 : 1;

            m = king.X + 1;
            n = king.Y;
            if (IsValid(m, n) && board[m, n] != 2) board[m, n] = board[m, n] == 3 ? 4 : 1;

            m = king.X;
            n = king.Y - 1;
            if (IsValid(m, n) && board[m, n] != 2) board[m, n] = board[m, n] == 3 ? 4 : 1;

            m = king.X;
            n = king.Y + 1;
            if (IsValid(m, n) && board[m, n] != 2) board[m, n] = board[m, n] == 3 ? 4 : 1;

            m = king.X - 1;
            n = king.Y - 1;
            if (IsValid(m, n) && board[m, n] != 2) board[m, n] = board[m, n] == 3 ? 4 : 1;

            m = king.X - 1;
            n = king.Y + 1;
            if (IsValid(m, n) && board[m, n] != 2) board[m, n] = board[m, n] == 3 ? 4 : 1;

            m = king.X + 1;
            n = king.Y - 1;
            if (IsValid(m, n) && board[m, n] != 2) board[m, n] = board[m, n] == 3 ? 4 : 1;

            m = king.X + 1;
            n = king.Y + 1;
            if (IsValid(m, n) && board[m, n] != 2) board[m, n] = board[m, n] == 3 ? 4 : 1;
        }

        /// <summary>
        /// build white check board: 
        /// 0 = safe,
        /// 1 = kill,
        /// </summary>
        /// <param name="situation">black and white pieces</param>
        /// <param name="black">black situation</param>
        /// <returns></returns>
        static int[,] WhiteBoard(List<List<Piece>> situation, int[,] black)
        {
            var board = InitBoard();

            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    //no piece, or BOther
                    if (null == situation[y][x] || black[x, y] == 2) continue;

                    switch (situation[y][x].ChessType)
                    {
                        case PieceType.WhiteKing:
                            AssignWhiteKing(board, situation[y][x]);
                            break;
                        case PieceType.WhiteKnight:
                            AssignWhiteKnight(board, situation[y][x]);
                            break;
                        case PieceType.WhiteRook:
                            AssignWhiteRook(board, situation[y][x], black);
                            break;
                    }
                }
            }

            return board;
        }

        /// <summary>
        /// 8 cases
        /// </summary>
        /// <param name="board"></param>
        /// <param name="king"></param>
        private static void AssignWhiteKing(int[,] board, Piece king)
        {
            var m = king.X - 1;
            var n = king.Y;
            if (IsValid(m, n)) board[m, n] = 1;

            m = king.X + 1;
            n = king.Y;
            if (IsValid(m, n)) board[m, n] = 1;

            m = king.X;
            n = king.Y - 1;
            if (IsValid(m, n)) board[m, n] = 1;

            m = king.X;
            n = king.Y + 1;
            if (IsValid(m, n)) board[m, n] = 1;

            m = king.X - 1;
            n = king.Y - 1;
            if (IsValid(m, n)) board[m, n] = 1;

            m = king.X - 1;
            n = king.Y + 1;
            if (IsValid(m, n)) board[m, n] = 1;

            m = king.X + 1;
            n = king.Y - 1;
            if (IsValid(m, n)) board[m, n] = 1;

            m = king.X + 1;
            n = king.Y + 1;
            if (IsValid(m, n)) board[m, n] = 1;
        }

        /// <summary>
        /// 8 cases:
        /// </summary>
        /// <param name="board"></param>
        /// <param name="knight"></param>
        private static void AssignWhiteKnight(int[,] board, Piece knight)
        {
            var m = knight.X - 2;
            var n = knight.Y + 1;
            if (IsValid(m, n)) board[m, n] = 1;

            m = knight.X - 2;
            n = knight.Y - 1;
            if (IsValid(m, n)) board[m, n] = 1;

            m = knight.X + 2;
            n = knight.Y + 1;
            if (IsValid(m, n)) board[m, n] = 1;

            m = knight.X + 2;
            n = knight.Y - 1;
            if (IsValid(m, n)) board[m, n] = 1;

            m = knight.X - 1;
            n = knight.Y - 2;
            if (IsValid(m, n)) board[m, n] = 1;

            m = knight.X + 1;
            n = knight.Y - 2;
            if (IsValid(m, n)) board[m, n] = 1;

            m = knight.X - 1;
            n = knight.Y + 2;
            if (IsValid(m, n)) board[m, n] = 1;

            m = knight.X + 1;
            n = knight.Y + 2;
            if (IsValid(m, n)) board[m, n] = 1;
        }

        /// <summary>
        /// 4 cases,
        /// </summary>
        /// <param name="board"></param>
        /// <param name="rook"></param>
        /// <param name="black"></param>
        private static void AssignWhiteRook(int[,] board, Piece rook, int[,] black)
        {
            var m = rook.X;
            var n = rook.Y;
            for (int i = m + 1; i < 8; i++)
            {
                if (black[i, n] == 2 || black[i, n] == 3) break;

                if (black[i, n] == 4)
                {
                    board[i, n] = 1;
                    break;
                }

                board[i, n] = 1;
            }

            for (int i = m - 1; i >= 0; i--)
            {
                if (black[i, n] == 2 || black[i, n] == 3) break;

                if (black[i, n] == 4)
                {
                    board[i, n] = 1;
                    break;
                }

                board[i, n] = 1;
            }

            for (int j = n + 1; j < 8; j++)
            {
                if (black[m, j] == 2 || black[m, j] == 3) break;

                if (black[m, j] == 4)
                {
                    board[m, j] = 1;
                    break;
                }

                board[m, j] = 1;
            }

            for (int j = n - 1; j >= 0; j--)
            {
                if (black[m, j] == 2 || black[m, j] == 3) break;

                if (black[m, j] == 4)
                {
                    board[m, j] = 1;
                    break;
                }

                board[m, j] = 1;
            }
        }

        static bool IsValid(int x, int y)
        {
            return 0 <= x && x <= 7 && 0 <= y && y <= 7;
        }
        
        static int[,] InitBoard()
        {
            var board = new int[8, 8];
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    board[i, j] = 0;
            return board;
        } 

        /// <summary>
        /// 8 x 8 board only: y = outsideList, x = insideList,
        /// no piece slot is null, 
        /// each line only consider first 16 characters input, others are ignored.
        /// empty line is ignored, 
        /// only consider first 8 non-empty lines, others are ignored, 
        /// must has only one black king, 
        /// could have one white king, 
        /// </summary>
        /// <param name="lines"></param>
        /// <returns>null if error input</returns>
        static List<List<Piece>> BuildBoard(string[] lines)
        {
            if (lines.Length < 8) return null;//expect at least 8 lines,

            var hasBlackKing = false;
            var hasWhiteKing = false;
            var result = new List<List<Piece>>();
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;//ignore line, 

                if (line.Length < 16) return null;//expect at least 8 char,
                
                var list = new List<Piece>();
                int start = 0;
                while (start < 16)
                {
                    var code = line.Substring(start, 2);
                    switch (code)
                    {
                        case Empty:
                            list.Add(null);//no piece
                            break;
                        case Bking:
                            if (hasBlackKing) return null;//only one black king is expected
                            list.Add(new Piece(PieceType.BlackKing, list.Count, result.Count));
                            hasBlackKing = true;
                            break;
                        case BOther:
                            list.Add(new Piece(PieceType.BlackOther, list.Count, result.Count));
                            break;
                        case WRook:
                            list.Add(new Piece(PieceType.WhiteRook, list.Count, result.Count));
                            break;
                        case WKnight:
                            list.Add(new Piece(PieceType.WhiteKnight, list.Count, result.Count));
                            break;
                        case Wking:
                            if (hasWhiteKing) return null;//no more than one white king is expected
                            list.Add(new Piece(PieceType.WhiteKing, list.Count, result.Count));
                            hasWhiteKing = true;
                            break;
                        default:
                            return null;//error code
                    }
                    start += 2;
                }
                if (list.Count != 8) return null;
                result.Add(list);
            }//foreach line
            
            return result.Count == 8 && hasBlackKing ? result : null;
        }
    }//class
}
