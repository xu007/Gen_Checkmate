using System;

namespace Gen_Checkmate
{
    public enum PieceType
    {
        BlackKing,
        BlackOther,
        WhiteRook,
        WhiteKnight,
        WhiteKing
    }

    public class Piece
    {
        public PieceType ChessType { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Piece(PieceType type, int x, int y)
        {
            if (0 > x || x > 7)
                throw new ArgumentOutOfRangeException("x", "Invalid x position");
            if (0 > y || y > 7)
                throw new ArgumentOutOfRangeException("x", "Invalid y position");

            ChessType = type;
            X = x;
            Y = y;
        }
    }
}