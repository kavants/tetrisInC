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
    // GRID MANAGER 
    public class GridManager
    {
        // Data ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        private static GridManager pInstance = null;

        public GridObject[,] grid;

        // Setting up Singleton Pattern ------------------------------------------------------------------------------------------------------------------------------------------

        public static void CreateGridManager()
        {
            if (pInstance == null)
            {
                pInstance = new GridManager();
            }
        }

        private static GridManager privGetInstance()
        {
            return pInstance;
        }

        protected GridManager()
        {
            // create grid

            grid = new GridObject[Constants.GAME_MAX_X + 1, Constants.GAME_MAX_Y + 1];

            for (int x = 0; x <= Constants.GAME_MAX_X; x++)
            {
                for (int y = 0; y <= Constants.GAME_MAX_Y; y++)
                {
                    grid[x, y] = new GridObject();
                }
            }
        }

        ~GridManager()
        {

        }

        // Add Piece To Grid ------------------------------------------------------------------------------------------------------------------------------------------

        public static void AddToGrid(Piece piece)
        {
            if (piece != null)
             {
                GridObject temp;
                for (int i = 0; i < 4; i++)
                {
                    if (piece.getXofBlock(i) <= Constants.GAME_MAX_X && piece.getYofBlock(i) <= Constants.GAME_MAX_Y)
                    {
                        temp = privGetInstance().grid[piece.getXofBlock(i), piece.getYofBlock(i)];
                        temp.Set(piece.pieceColor);
                    }
                }
            }
        }

        // Piece Intersects Grid -------------------------------------------------------------------------------------

        public static bool IntersectsGrid(Piece piece)
        {
            // if the block in current position intersects the grid, return true
            // if not, return false

            GridObject temp;
            for (int i = 0; i < 4; i++)
            {
                if (piece.getYofBlock(i) <= Constants.GAME_MAX_Y)
                {
                    temp = privGetInstance().grid[piece.getXofBlock(i), piece.getYofBlock(i)];
                    if (temp.status == GridObject.GridStatus.FILLED)
                        return true;
                }
            }
            return false;
        }

        // Piece is Landing -----------------

        public static bool Landing(Piece piece)
        {
            // if the block in current position intersects the grid, return true
            // if not, return false

            GridObject temp;
            int x;
            int y;

            for (int i = 0; i < 4; i++)
            {
                //if ((piece.getYofBlock(i) - 1 >= 0) && (piece.getYofBlock(i) <= Constants.GAME_MAX_Y))
                //{
                //    temp = privGetInstance().grid[piece.getXofBlock(i), piece.getYofBlock(i) - 1];
                //    if (temp.status == GridObject.GridStatus.FILLED)
                //        return true;
                //}
                x = piece.getXofBlock(i);
                y = piece.getYofBlock(i);

                if (y < 1)
                {
                    return true;
                }
                else if (y <= Constants.GAME_MAX_Y)
                {
                    temp = privGetInstance().grid[x, y - 1];
                    if (temp.status == GridObject.GridStatus.FILLED)
                        return true;
                }

            }
            return false;
        }

        // Look for End Game ----------------------------------------------------------------------------------------------------------------------------
        private void CheckTopRowForBlock()
        {
            for (int x = 0; x <= Constants.GAME_MAX_X; x++)
            {
                //Check if top row contains stationary block
                if (privGetInstance().grid[x, Constants.GAME_MAX_Y - 1].status != GridObject.GridStatus.EMPTY)
                {
                    privGetInstance().EndGame();
                }
            }
        }

        private void EndGame()
        {
            SoundManager.GameOver();
            GameManager.makeGameState_GAMEOVER();
        }

        // Look For Filled Rows  ------------------------------------------------------------------------------------------------------------------------------------------

        private void LookForFilledRows()
        {
            int numFilledRows = 0;
            int bottomFilledRow = -1;

            for (int y = 0; y <= Constants.GAME_MAX_Y; y++)
            {
                for (int x = 0; x <= Constants.GAME_MAX_X; x++)
                {
                    if (privGetInstance().grid[x, y].status == GridObject.GridStatus.EMPTY)
                        break;

                    if (x == Constants.GAME_MAX_X)
                    {
                        if (bottomFilledRow == -1) bottomFilledRow = y;
                        numFilledRows++;
                    }
                }
            }

            // update score based on how many filled rows
            if (numFilledRows!=0)
            {
                ScoreManager.FilledRow(numFilledRows);
                SoundManager.FilledRow();

                for (int i = 0; i < numFilledRows; i++)
                {
                    DeleteRow(bottomFilledRow);
                }
            }
        }

        // Delete Row ------------------------------------------------------------------------------------------------------------------------------------------

        private void DeleteRow(int rowIn)
        {
            // each row about rowIn will bump down a row
            // so basically, rowIn and every row above it becomes the row above it

            for (int y = rowIn; y < Constants.GAME_MAX_Y; y++)
                for (int x = 0; x <= Constants.GAME_MAX_X; x++)
                    privGetInstance().grid[x, y] = privGetInstance().grid[x, y + 1];

            // then top row just becomes empty

            for (int x=0; x <= Constants.GAME_MAX_X; x++)
                privGetInstance().grid[x, Constants.GAME_MAX_Y].Empty();
            
        }

        // Update ------------------------------------------------------------------------------------------------------------------------------------------

        public static void Update(GameManager.GameState stateIn)
        {
            switch (stateIn)
            {
                case GameManager.GameState.WELCOME:
                    privGetInstance().WelcomeUpdate();
                    break;
                case GameManager.GameState.PLAY:
                    privGetInstance().PlayUpdate();
                    break;
                case GameManager.GameState.GAMEOVER:
                    privGetInstance().GameOverUpdate();
                    break;
            }
        }

        private void WelcomeUpdate()
        {

        }

        private void PlayUpdate()
        {
           // look for filled rows
            privGetInstance().LookForFilledRows();

            // check top row for block (end game)
            privGetInstance().CheckTopRowForBlock();
        }

        private void GameOverUpdate()
        {

        }

        // Draw ------------------------------------------------------------------------------------------------------------------------------------------

        public static void Draw(GameManager.GameState stateIn)
        {
            switch (stateIn)
            {
                case GameManager.GameState.WELCOME:
                    privGetInstance().WelcomeDraw();
                    break;
                case GameManager.GameState.PLAY:
                    privGetInstance().PlayDraw();
                    break;
                case GameManager.GameState.GAMEOVER:
                    privGetInstance().GameOverDraw();
                    break;
            }
            
        }

        private void WelcomeDraw()
        {
            // do nothing
        }

        private void PlayDraw()
        {
            // go through 2d array and draw anything with status=filled

            for (int x = 0; x <= Constants.GAME_MAX_X; x++)
            {
                for (int y = 0; y <= Constants.GAME_MAX_Y; y++)
                {
                    if (privGetInstance().grid[x, y].status == GridObject.GridStatus.FILLED)
                    {
                        SOM.drawBox(x, y, privGetInstance().grid[x, y].color);
                    }
                }
            }
        }

        private void GameOverDraw()
        {
            for (int x = 0; x <= Constants.GAME_MAX_X; x++)
            {
                for (int y = 0; y <= Constants.GAME_MAX_Y; y++)
                {
                    if (privGetInstance().grid[x, y].status == GridObject.GridStatus.FILLED)
                    {
                        SOM.drawBox(x, y, privGetInstance().grid[x, y].color);
                    }
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------

        // Returns status of a space on the game grid given an x and y
        public static bool spaceIsEmpty(int x, int y)
        {

            GridObject space = GridManager.privGetInstance().grid[x, y];          
            
            if (space != null)
            {
                // This space is empty
                if (space.status == 0)
                {
                    return true;
                }
            }

            // else space is full       
            return false;
        }

        public static void ClearGrid()
        {
            for (int x = 0; x <= Constants.GAME_MAX_X; x++)
            {
                for (int y = 0; y <= Constants.GAME_MAX_Y; y++)
                {
                    privGetInstance().grid[x, y].Empty();
                }
            }
        }

    }
}
