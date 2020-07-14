using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    // GRID OBJECT ------------------------------------------------------------------------------------------------------------------------------------------

    // these make up the 2d array that is our grid
    public class GridObject
    {
        // Enums ------------------------------------------------------------------------------------------------------------------------------------------
        public enum GridStatus
        {
            EMPTY = 0,
            FILLED = 1
        }

        // Data ------------------------------------------------------------------------------------------------------------------------------------------

        public DrawColor.Shade color;
        public GridStatus status;

        // Constructor ------------------------------------------------------------------------------------------------------------------------------------------

        public GridObject()
        {
            this.status = GridStatus.EMPTY;
            this.color = DrawColor.Shade.COLOR_BACKGROUND_CUSTOM;
        }

        // Empty ------------------------------------------------------------------------------------------------------------------------------------------

        public void Empty()
        {
            this.status = GridStatus.EMPTY;
            this.color = DrawColor.Shade.COLOR_BACKGROUND_CUSTOM;
        }

        // Set ------------------------------------------------------------------------------------------------------------------------------------------

        public void Set(DrawColor.Shade colorIn)
        {
            this.status = GridStatus.FILLED;
            this.color = colorIn;
        }
    }
}
