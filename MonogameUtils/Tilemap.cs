using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameUtils;

public class Tilemap : List<List<Tile>>{
    /// <summary>
    /// The only constructor for the Tilemap class, it takes a text file and coverts it to a 2D array of Tiles using the provided key
    /// </summary>
    /// <param name="levelFile">The text file containing the level. It's important that all of the lines are at the same length</param>
    /// <param name="parserKey">A function that takes a character as a parameter and returns a Tile</param>
    public Tilemap(Stream levelFile, Func<char, Tile> parserKey){
        using StreamReader reader = new(levelFile);
        string line = reader.ReadLine();
        int tilemapHeight = (int)reader.BaseStream.Length/line.Length;
        char[][] levelCode = new char[tilemapHeight][];
        int cnt = 0;
        while (line != null){
            levelCode[cnt] = line.ToCharArray();
            cnt++;
            line = reader.ReadLine();
        }
        var tiles = levelCode.Select(lines => lines.Select(character => parserKey(character)).ToList()).ToList();
        AddRange(tiles);
        //* Add an area rectangle for each tile
        for(int row = 0; row < this.Count; row++){
            for(int col = 0; col < this[row].Count; col++){
                this[row][col] = new(this[row][col].Texture, this[row][col].Type){
                    Area = new(
                        col*this[row][col].Texture.Width,
                        row*this[row][col].Texture.Width,
                        this[row][col].Texture.Width,
                        this[row][col].Texture.Width)
                };
            }
        }
    }
    public Texture2D GetTexture2D(GraphicsDevice gd){
        var returnTexture = new Texture2D(
            gd, this[0].Count * this[0][0].Texture.Width, this.Count * this[0][0].Texture.Width);
        Color[] texturePixelData = new Color[this[0].Count * this[0][0].Texture.Width * this.Count * this[0][0].Texture.Width];

        Color[][][] sourceTilesPixels = new Color[this.Count][][];

        //* Foreach tile in the tilemap
        for(int row = 0; row < this.Count; row++){
            sourceTilesPixels[row] = new Color[this[0].Count][];
            for(int col = 0; col < this[row].Count; col++){
                sourceTilesPixels[row][col] = new Color[(int)Math.Pow(this[0][0].Texture.Width, 2)];
                this[row][col].Texture.GetData<Color>(sourceTilesPixels[row][col]);
            }
        }
        //* For each pixel in the complete Texture2D instance
        for(int i = 0; i < texturePixelData.Length; i++){
            /*int tileIndex = i / this[0][0].Texture.Width;
            int tileRow = tileIndex / this[0].Count;
            int tileCol = tileIndex % this[0].Count;*/
            int pixelRow = i / (this[0].Count*this[0][0].Texture.Width);
            int pixelCol = i % (this[0].Count*this[0][0].Texture.Width);
            int tileRow = pixelRow / this[0][0].Texture.Width;
            int tileCol = pixelCol / this[0][0].Texture.Width;
            int pixelTileRow = pixelRow % this[0][0].Texture.Width;
            int pixelTileCol = pixelCol % this[0][0].Texture.Width;

            texturePixelData[i] = sourceTilesPixels[tileRow][tileCol][pixelTileCol + pixelTileRow*this[0][0].Texture.Width];
        }
        returnTexture.SetData<Color>(texturePixelData);
        return returnTexture;
    }
}
