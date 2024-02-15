using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonogameUtils;

namespace TestGames;

public class TilemapTestGame : Game
{
	private readonly GraphicsDeviceManager _g;
	private SpriteBatch spriteBatch;
	public Tile TestLevelKey(char c) => c switch{
		'.' => new(new(GraphicsDevice,18,18), TileType.blank),
		'<' => new(Content.Load<Texture2D>("Tiles/Cloud-Left"), TileType.platform),
		'_' => new(Content.Load<Texture2D>("Tiles/Cloud-Middle"), TileType.platform),
		'>' => new(Content.Load<Texture2D>("Tiles/Cloud-Right"), TileType.platform),
		'~' => new(Content.Load<Texture2D>("Tiles/Grass-Top"), TileType.solid),
		'#' => new(Content.Load<Texture2D>("Tiles/Dirt"), TileType.solid),
		_ => throw new ArgumentOutOfRangeException(nameof(c), $"Not expected char value: {c}")
	};
	Level testLevel;

	public TilemapTestGame(){
		_g = new GraphicsDeviceManager(this);
		IsMouseVisible = true;
	}

	protected override void Initialize(){
		// TODO: Add your initialization logic here

		base.Initialize();
	}

	protected override void LoadContent(){
		spriteBatch = new SpriteBatch(GraphicsDevice);
		Content.RootDirectory = "Content";
        using Stream levelStream = TitleContainer.OpenStream("Content/Levels/TestLevel.lvl");
        testLevel = new(
            new(
                levelStream,
                TestLevelKey),
            new(0,0,18*16,18*8),
			GraphicsDevice);
    }

	protected override void Update(GameTime gameTime){
		bool leftKeyPressed = Keyboard.GetState().IsKeyDown(Keys.Left);
        bool rightKeyPressed = Keyboard.GetState().IsKeyDown(Keys.Right);
        bool upKeyPressed = Keyboard.GetState().IsKeyDown(Keys.Up);
        bool downKeyPressed = Keyboard.GetState().IsKeyDown(Keys.Down);
        //bool spaceBarPressed = Keyboard.GetState().IsKeyDown(Keys.Space);
		//* right if right key pressed, left if left key pressed. else nothing
		testLevel.CameraRect.X += 10 * (rightKeyPressed ^ leftKeyPressed ? (rightKeyPressed ? 1 : -1) : 0);

		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime){
		GraphicsDevice.Clear(Color.CornflowerBlue);

		// TODO: Add your drawing code here
		spriteBatch.Begin(samplerState:SamplerState.PointClamp);
		//testLevel.Draw(ref spriteBatch, _graphics);
		spriteBatch.Draw(testLevel.tilemapTexture,
			new Rectangle(0,0,_g.PreferredBackBufferWidth,_g.PreferredBackBufferHeight),
			testLevel.CameraRect, Color.White);
		spriteBatch.End();
		base.Draw(gameTime);
	}
}
