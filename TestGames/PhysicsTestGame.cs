using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonogameUtils;

namespace TestGames;

public class PhysicsPlayer : Player{
    public Vector2 velocity = new(0, 0.5f);
    public Vector2 acceleration = new(0, 0);
    public PhysicsPlayer(Texture2D _spriteSheet, Rectangle[] _idleFrames, Rectangle[] _rightFrames, Rectangle[] _leftFrames) : base(_spriteSheet, _idleFrames, _rightFrames, _leftFrames){}
    public new void NextFrame(GameTime gameTime){
        base.NextFrame(gameTime);
        velocity += acceleration;
        Transform(velocity);
    }
    public Collision IsCollidingWith(Rectangle otherCollider){
        return new(Area, otherCollider);
    }
}

public class PhysicsTestGame : Game
{
    private GraphicsDeviceManager _g;
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
    Level physicsTestLevel;
	PhysicsPlayer sunflower;

    public PhysicsTestGame()
    {
        _g = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        using Stream levelStream = TitleContainer.OpenStream("Content/Levels/PhysicsTestLevel.lvl");
        physicsTestLevel = new(
            new(
                levelStream,
                TestLevelKey
            ),
            new(0,0,18*16,18*8),
            GraphicsDevice
        );


        sunflower = new(
            Content.Load<Texture2D>("Short_Sunflower_Sprite_Sheet"),
            new Rectangle[2]{
                new(0,0,11,20), new(0,21,11,20)},
            new Rectangle[2]{
                new(12,0,11,20), new(12,21,11,20)
            },
            new Rectangle[2]{
                new(24,0,11,20), new(24,21,11,20)
            }){
            	SpriteScale = 5
        	};

        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        // TODO: Add your update logic here
        bool leftKeyPressed = Keyboard.GetState().IsKeyDown(Keys.Left);
        bool rightKeyPressed = Keyboard.GetState().IsKeyDown(Keys.Right);
        bool upKeyPressed = Keyboard.GetState().IsKeyDown(Keys.Up);
        bool downKeyPressed = Keyboard.GetState().IsKeyDown(Keys.Down);
        //bool spaceBarPressed = Keyboard.GetState().IsKeyDown(Keys.Space);

		sunflower.acceleration.X = sunflower.velocity.X < 0 ? 1 : sunflower.velocity.X > 0 ? -1 : 0;

		if(rightKeyPressed)
        {
            if(leftKeyPressed){
				
                sunflower.CurrentAnimation = "Idle";
			}
            else
            {
                sunflower.CurrentAnimation = "Right";
                sunflower.velocity.X = 1;
            }
        }
        else if(leftKeyPressed)
        {
            sunflower.CurrentAnimation = "Left";
            sunflower.velocity.X = -1;
        }
        else{
            sunflower.CurrentAnimation = "Idle";
        }

		
		/*testLevel.CameraRect.X += leftKeyPressed && testLevel.CameraRect.X >= 5 ? -5 : (rightKeyPressed ? +5 : 0);
		testLevel.CameraRect.Y += upKeyPressed  && testLevel.CameraRect.Y >= 5 ? -5 : (downKeyPressed ? +5 : 0);*/
		

		if (sunflower.Area.Location.Y >= _g.PreferredBackBufferHeight-sunflower.Area.Height){
			sunflower.velocity.Y = 0;
		}

        //if(sunflower.IsCollidingWith())

        sunflower.NextFrame(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        spriteBatch.Begin(samplerState:SamplerState.PointClamp);
        spriteBatch.Draw(physicsTestLevel.tilemapTexture,
            new Rectangle(0,0,_g.PreferredBackBufferWidth,_g.PreferredBackBufferHeight),
            physicsTestLevel.CameraRect, Color.White);
        spriteBatch.Draw(sunflower.SpriteSheet, sunflower.Area, 
            sunflower.CurrentSprite, Color.White);
		spriteBatch.End();

        base.Draw(gameTime);
    }
}
