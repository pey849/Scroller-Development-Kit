using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Xml;
using System.Reflection;
using ScrollerEngine;
using ScrollerEngine.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ScrollerEngine.Scenes
{
    /// <summary>
    /// Worked on by Peter, Richard, Emmanuel
    /// Provides data about a level, such as the tiles and entities within it.
    /// </summary>
    public class LevelData
    {
        /// <summary>
        /// Gets or sets the name for this level.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Indicates the size, in pixels, of each tile within the level.
        /// </summary>
        public Vector2 TileSize { get; private set; }

        /// <summary>
        /// Indicates the size, in pixels, of the map itself.
        /// </summary>
        public Vector2 MapSize { get; private set; }

        /// <summary>
        /// Returns an array of the layers contained within this level.
        /// </summary>
        public List<Layer> Layers { get; private set; }

        /// <summary>
        /// Returns an array of all the dynamic objects defined within this level.
        /// </summary>
        public LinkedList<Entity> DynamicObjects { get; private set; }
        
        /// <summary>
        /// Loads the data for a level using the Tiled Map Xml format.
        /// </summary>
        public static LevelData LoadLevel(string fileName)
        {
            // https://github.com/bjorn/tiled/wiki/TMX-Map-Format contains a description of the format.

            try
            {
                XmlDocument doc = new XmlDocument();
                try { doc.Load(fileName); }
                catch (Exception e1) { throw new Exception(e1.Message); }

                if (doc.GetElementsByTagName("map").Count != 1)
                    throw new FormatException("Expected a single map to be defined in a .tmx file.");
                var name = fileName.Split('/').Last().Split('.').First().Trim();
                var mapElements = doc.GetElementsByTagName("map").Item(0);
                var mapDetails = new MapDetails(mapElements);
                var textures = ParseTilesets(mapDetails);
                var layers = ParseLayers(mapDetails, textures);
                var entities = ParseObjects(mapDetails);
                LevelData result = new LevelData()
                {
                    Name = name,
                    TileSize = new Vector2(mapDetails.MapTileWidth, mapDetails.MapTileHeight),
                    MapSize = new Vector2(mapDetails.MapWidth, mapDetails.MapHeight),
                    Layers = layers,
                    DynamicObjects = entities
                };
                return result;
            }
            catch (Exception e1)
            {
                var message = string.Format("Scene: {0}\n{1}", fileName, e1.Message);
                throw new Exception(message);
            }
            throw new InvalidOperationException("LevelData.cs: This should never happen....");
        }

        private static List<TextureDetails> ParseTilesets(MapDetails details)
        {
            var textures = new List<TextureDetails>();
            foreach (XmlNode tilesetNode in details.MapElement.SelectNodes("tileset"))
            {
                var imageNode = tilesetNode.ChildNodes[0];
                string imageSource = imageNode.Attributes["source"].Value;
                string textureName = Path.GetFileNameWithoutExtension(imageSource);
                var texture = ScrollerBase.Instance.GlobalContent.LoadTexture2D("Tiles/" + textureName);
                int startGID = int.Parse(tilesetNode.Attributes["firstgid"].Value);
                int tileWidth = int.Parse(tilesetNode.Attributes["tilewidth"].Value);
                int tileHeight = int.Parse(tilesetNode.Attributes["tileheight"].Value);
                textures.Add(new TextureDetails()
                {
                    Texture = texture,
                    StartGID = startGID,
                    TileHeight = tileHeight,
                    TileWidth = tileWidth,
                    NumTilesHigh = texture.Height / tileHeight,
                    NumTilesWide = texture.Width / tileWidth
                });
            }
            textures = textures.OrderBy(c => c.StartGID).ToList();
            return textures;
        }

        private static List<Layer> ParseLayers(MapDetails details, List<TextureDetails> textures)
        {
            var layers = new List<Layer>();
            foreach (XmlNode layerNode in details.MapElement.SelectNodes("layer"))
            {
                var dataNode = layerNode.SelectNodes("data").Item(0);
                string compressionFormat = dataNode.Attributes["compression"].Value;
                string encodingFormat = dataNode.Attributes["encoding"].Value;
                if(!compressionFormat.Equals("gzip", StringComparison.InvariantCultureIgnoreCase) || !encodingFormat.Equals("base64", StringComparison.InvariantCultureIgnoreCase))
                    throw new FormatException("Currently the Tmx loader can only handled base-64 zlib tiles.");
                string base64Data = dataNode.InnerXml.Trim();
                byte[] compressedData = Convert.FromBase64String(base64Data);
                byte[] uncompressedData = new byte[1024]; //Must be a multiple of 4
                Tile[,] tiles = new Tile[details.MapNumTilesWide, details.MapNumTilesHigh];
                int mapIndex = 0;
                using (var gzipStream = new GZipStream(new MemoryStream(compressedData), CompressionMode.Decompress, false))
                {
                    while (true)
                    {
                        int bytesRead = gzipStream.Read(uncompressedData, 0, uncompressedData.Length);
                        if (bytesRead == 0)
                            break;
                        using (BinaryReader reader = new BinaryReader(new MemoryStream(uncompressedData)))
                        {
                            for (int i = 0; i < bytesRead; i += 4)
                            {
                                int gid = reader.ReadInt32();
                                int mapX = mapIndex % details.MapNumTilesWide;
                                int mapY = mapIndex / details.MapNumTilesWide;
                                mapIndex++;
                                if (gid == 0)
                                    continue;
                                var texture = textures.Last(c => c.StartGID <= gid);
                                int textureX = (gid - texture.StartGID) % texture.NumTilesWide;
                                int textureY = (gid - texture.StartGID) / texture.NumTilesWide;
                                Rectangle sourceRect = new Rectangle(textureX * texture.TileWidth, textureY * texture.TileHeight, texture.TileWidth, texture.TileHeight);
                                Rectangle location = new Rectangle(mapX * details.MapTileWidth, mapY * details.MapTileHeight, details.MapTileWidth, details.MapTileHeight);
                                Tile tile = new Tile(texture.Texture, sourceRect, location, new Vector2(mapX, mapY));
                                tiles[mapX, mapY] = tile;
                            }
                        }
                    }
                }

                Layer layer = new Layer(new Vector2(details.MapTileWidth, details.MapTileHeight), tiles);
                foreach (XmlNode propertiesNode in layerNode.SelectNodes("properties"))
                {
                    foreach (XmlNode property in propertiesNode.SelectNodes("property"))
                    {
                        //TODO: Consider separating this to a generic function.
                        string name = property.Attributes["name"].Value;
                        object value = property.Attributes["value"].Value;
                        PropertyInfo layerProperty = typeof(Layer).GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                        if(layerProperty == null)
                            throw new KeyNotFoundException("Layer does not contain a property named '" + name + "'.");
                        if(!layerProperty.PropertyType.IsAssignableFrom(value.GetType()))
                            value = Convert.ChangeType(value, layerProperty.PropertyType);
                        layerProperty.SetValue(layer, value, null);
                    }
                }
                layers.Add(layer);
            }

            return layers;
        }

        private static LinkedList<Entity> ParseObjects(MapDetails details)
        {
            LinkedList<Entity> entities = new LinkedList<Entity>();
            foreach (XmlNode objectGroupNode in details.MapElement.SelectNodes("objectgroup"))
            {
                foreach (XmlNode objectNode in objectGroupNode.SelectNodes("object"))
                {
                    ObjectType type = ReadObjectType(objectNode);
                    switch (type)
                    {
                        case ObjectType.Entity:
                            var entity = ParseEntity(objectNode);
                            entities.AddLast(entity);
                            break;
                        //TODO: can add more cases if need be.
                    }
                }
            }
            
            //TODO: pathing stuff here ?

            return entities;
        }

        private static Entity ParseEntity(XmlNode objectNode)
        {
            //TODO: determine if we want entities to be able to resize using Tiled.
            //      Essentially, could have a property in Entity (called IsFixedSize).
            //      For now, Tiled wont resize them.
            float width = float.Parse(objectNode.Attributes["width"].Value);
            float height = float.Parse(objectNode.Attributes["height"].Value);
            float x = float.Parse(objectNode.Attributes["x"].Value);
            float y = float.Parse(objectNode.Attributes["y"].Value);
            string entityType = objectNode.Attributes["type"].Value.Trim();
            string entityName = objectNode.Attributes["name"] == null ? null : objectNode.Attributes["name"].Value.Trim();
            var entity = ScrollerSerializer.CreateEntity(entityType);
            if (!string.IsNullOrWhiteSpace(entityName))
                entity.Name = entityName;
            entity.Position = new Vector2(x, y);
            entity.Size = new Vector2(width, height); //comment out this if we dont want tile to set the size.

            //TODO: Applies properties set through Tiled here.
            //      Only works for primitive types. Will expand if needed.
            foreach (XmlNode propertiesNode in objectNode.SelectNodes("properties"))
            {
                foreach (XmlNode propertyNode in propertiesNode.SelectNodes("property"))
                {
                    string name = propertyNode.Attributes["name"].Value.Trim();
                    object value = propertyNode.Attributes["value"].Value.Trim();
                    try
                    {
                        string[] namePropertySplit = name.Split('.');
                        if (namePropertySplit.Length != 2)
                            throw new FormatException(entityType + ": Expected object property name to be in the format of 'SpriteComponent.TextureName'.");
                        string componentName = namePropertySplit[0].Trim();
                        string propertyName = namePropertySplit[1].Trim();
                        var component = entity.Components[componentName];
                        PropertyInfo property = component.GetType().GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                        if (property == null)
                            throw new KeyNotFoundException(string.Format("{0} does not have {1} or {2}", entity.Name, componentName, propertyName));
                        if (!property.PropertyType.IsAssignableFrom(value.GetType()))
                            value = Convert.ChangeType(value, property.PropertyType);
                        property.SetValue(component, value, null);
                    }
                    catch (Exception e1)
                    {
                        var message = string.Format("Entity Name: {0}\nProperty: {1}\nValue: {2}\n\n{3}", entityName, name, value, e1.Message);
                        throw new Exception(message);
                    }
                }
            }

            return entity;
        }

        private static ObjectType ReadObjectType(XmlNode objectNode)
        {
            string typeAttrib = objectNode.Attributes["type"].Value;
            ObjectType type = ObjectType.Entity;
            Enum.TryParse<ObjectType>(typeAttrib, true, out type);
            return type;
        }

        private struct MapDetails
        {
            public int MapNumTilesWide;
            public int MapNumTilesHigh;
            public int MapTileWidth;
            public int MapTileHeight;
            public int MapWidth;
            public int MapHeight;
            public XmlNode MapElement;

            public MapDetails(XmlNode MapElement)
            {
                this.MapNumTilesWide = int.Parse(MapElement.Attributes["width"].Value);
                this.MapNumTilesHigh = int.Parse(MapElement.Attributes["height"].Value);
                this.MapTileWidth = int.Parse(MapElement.Attributes["tilewidth"].Value);
                this.MapTileHeight = int.Parse(MapElement.Attributes["tileheight"].Value);
                this.MapWidth = MapTileWidth * MapNumTilesWide;
                this.MapHeight = MapTileHeight * MapNumTilesHigh;
                this.MapElement = MapElement;
            }
        }

        private struct TextureDetails
        {
            public Texture2D Texture;
            public int StartGID;
            public int TileWidth;
            public int TileHeight;
            public int NumTilesWide;
            public int NumTilesHigh;
        }

        private enum ObjectType
        {
            Entity,
            //Can add more if needed. (EX: Path)
        }
    }
}
