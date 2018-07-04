using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ScrollerEngine.Scenes
{
    /// <summary>
    /// Worked on by Peter, Richard, Emmanuel
    /// Provides information about a single layer within the game.
    /// </summary>
    public class Layer
    {
        private Tile[,] _Tiles;
        private bool _Solid = false;
        private bool _TopCollisionOnly = false;
        private Vector2 _TileSize;

        /// <summary>
        /// Gets an array of all of the tiles within this level.
        /// This should be used only in rare situations, and never modified.
        /// </summary>
        public Tile[,] Tiles { get { return _Tiles; } }

        /// <summary>
        /// Indicates if this layer is solid. In other words, if users should collide with tiles on this level.
        /// </summary>
        public bool IsSolid
        {
            get { return _Solid; }
            set { _Solid = value; }
        }

        /// <summary>
        /// Gets or sets whether this layer is top collision only.
        /// </summary>
        public bool TopCollisionOnly
        {
            get { return _TopCollisionOnly; }
            set { _TopCollisionOnly = value; }
        }

        /// <summary>
        /// Gets the tile with the given X and Y coordinates.
        /// These are the coordinates within the Tileset, so for example the second tile would be {1, 0}.
        /// If the coordinates are outside this layer, null is returned.
        /// Null may also be returned if this particular tile is empty for this layer.
        /// </summary>
        public Tile GetTile(int X, int Y)
        {
            if (X < 0 || Y < 0 || X >= _Tiles.GetLength(0) || Y >= _Tiles.GetLength(1))
                return null;
            return _Tiles[X, Y];
        }

        /// <summary>
        /// Gets the tile at the specified world coordinates.
        /// </summary>
        public Tile GetTileAtPosition(Vector2 Position)
        {
            var TileCoords = Position / _TileSize;
            return GetTile((int)TileCoords.X, (int)TileCoords.Y);
        }

        /// <summary>
        /// Gets the tile at the specified world coordinates.
        /// </summary>
        public Tile GetTileAtPosition(Rectangle location)
        {
            var startCoords = new Vector2(location.X, location.Y) / _TileSize;
            var endCoords = new Vector2(location.X + location.Width, location.Y + location.Height) / _TileSize;
            for (int y = (int)startCoords.Y; y <= (int)endCoords.Y; y++)
            {
                for (int x = (int)startCoords.X; x <= (int)endCoords.X; x++)
                {
                    Tile tile = GetTile(x, y);
                    if (tile != null)
                        return tile;
                }
            }
            return null;
        }

        /// <summary>
        /// Creates a new layer from the given tiles and size.
        /// </summary>
        public Layer(Vector2 TileSize, Tile[,] Tiles)
        {
            this._Tiles = Tiles;
            this._TileSize = TileSize;
        }
    }
}
