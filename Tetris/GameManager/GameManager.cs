using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// GAME MANAGER ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

// this is the bigtime overarching class that holds the PieceManager, GridManager, etc

// employs the SINGLETON PATTERN (http://csharpindepth.com/Articles/General/Singleton.aspx)
// to ensure that no duplicates of this class are made

namespace Tetris
{
    public class GameManager
    {
        // State Enum ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        
        public enum GameState
        {
            WELCOME,
            PLAY,
            GAMEOVER
        }
            
        // Data ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        private static GameManager pInstance = null;
        public GameState currState;

        // Setting up Singleton Pattern ------------------------------------------------------------------------------------------------------------------------------------------

        public static void CreateGameManager()
        {
            if (pInstance == null)
            {
                pInstance = new GameManager();
            }
        }

        private static GameManager privGetInstance()
        {
            return pInstance;
        }

        protected GameManager()
        {
            this.currState = GameState.WELCOME;
        }

        ~GameManager()
        {

        }

        // Wrapping the methods from Game.cs ------------------------------------------------------------------------------------------------------------------------------------------

        public static void LoadContent()
        {
            // create all the individual managers

            InputManager.CreateInputManager();
            GridManager.CreateGridManager();
            PieceManager.CreatePieceManager();
            ScoreManager.CreateScoreManager();
            SoundManager.CreateSoundManager();
        }

        public static void Update()
        {
            // update all the individual managers

            InputManager.Update(privGetInstance().currState);
            PieceManager.Update(privGetInstance().currState);
            GridManager.Update(privGetInstance().currState);
            ScoreManager.Update(privGetInstance().currState);
            SoundManager.Update(privGetInstance().currState);
        }

        public static void Draw()
        {
            ScoreManager.Draw(privGetInstance().currState);
            PieceManager.Draw(privGetInstance().currState);
            GridManager.Draw(privGetInstance().currState);
        }

        public static void UnloadContent()
        {
            // todo: clean out all the managers? 
        }

        public static void EndGame()
        {

        }

        // Switching States ------------------------------------------------------------------------------------------------------------------------------------------

        public static void makeGameState_PLAY()
        {
            privGetInstance().currState = GameState.PLAY;

            // clear grid 
            GridManager.ClearGrid();
            // clear score
        }

        public static void makeGameState_GAMEOVER()
        {
            privGetInstance().currState = GameState.GAMEOVER;
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------

    }
}
