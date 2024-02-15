using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MonogameUtils;

public class Level
{
	public readonly string Name;
	public readonly byte Id;
	public readonly Tilemap Tilemap;
	public readonly Texture2D tilemapTexture;
	public Rectangle CameraRect;
	public Level(Tilemap tilemap, Rectangle cameraRect, GraphicsDevice gd){
		Tilemap = tilemap;
		CameraRect = cameraRect;
		tilemapTexture = Tilemap.GetTexture2D(gd);
	}
	public void Draw(ref SpriteBatch spriteBatch, GraphicsDeviceManager graphics){
		int tileWidth = Tilemap[0][0].Texture.Width; //Width of the Texture
		int tileDrawWidth = CameraRect.Width/tileWidth; //How many tiles should be drawn for each row
		int tileDrawHeight = CameraRect.Width/tileWidth; //How many tiles should be drawn for each column
		int actualTileWidth = graphics.PreferredBackBufferWidth/tileDrawWidth; //Scaling the texture 
		int actualTileHeight = graphics.PreferredBackBufferWidth/tileDrawHeight;
		int startTileRow = CameraRect.Y/tileWidth;
		int startTileColumn = CameraRect.X/tileDrawWidth;

		for (int i = startTileRow; i <= startTileRow+tileDrawHeight; i++){
			for (int j = startTileColumn; j <= startTileColumn+tileDrawWidth; j++){
				try{
					spriteBatch.Draw(
						Tilemap[i][j].Texture,
						new Rectangle(actualTileWidth*(j-startTileColumn),actualTileHeight*(i-startTileRow),
							actualTileWidth,
							actualTileHeight),
						Color.White);
				}
				catch (ArgumentOutOfRangeException){
					break;
				}
			}
		}
	}
}
