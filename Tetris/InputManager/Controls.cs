using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class Controls
    {
        // controls

        public static void StartGame()
        {
            SoundManager.MusicOnOff();
            ScoreManager.ClearScore();
            GameManager.makeGameState_PLAY();
        }

        public static void MoveRight()
        {
            PieceManager.moveFallingPieceRight();
        }

        public static void MoveLeft()
        {
            PieceManager.moveFallingPieceLeft();
        }

        public static void SoftDrop()
        {
            PieceManager.moveFallingPieceSoftDrop();
        }

        public static void HardDrop()
        {
            PieceManager.moveFallingPieceHardDrop();
        }

        public static void Rotate_Clockwise()
        {
            PieceManager.RotateFallingPiece_Clockwise();
        }

        public static void Rotate_CounterClockwise()
        {
            PieceManager.RotateFallingPiece_CounterClockwise();
        }

        public static void MusicOnOff()
        {
            SoundManager.MusicOnOff();
        }

        // data


    }
}
