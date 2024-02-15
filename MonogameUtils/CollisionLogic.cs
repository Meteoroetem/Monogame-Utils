using Microsoft.Xna.Framework;

namespace MonogameUtils;

public struct Collision{
    bool collision;
    Vector2 directionVector;

    public Collision(Rectangle colliderA, Rectangle colliderB){
        bool xAxisOverlap = (colliderB.X < colliderA.X && colliderA.X < colliderB.X + colliderB.Width) ||
            (colliderB.X < colliderA.X + colliderA.Width && colliderA.X + colliderA.Width < colliderB.X + colliderB.Width);
        bool yAxisOverlap = (colliderB.Y < colliderA.Y && colliderA.Y < colliderB.Y + colliderB.Height) ||
            (colliderB.Y < colliderA.Y + colliderA.Height && colliderA.Y + colliderA.Height < colliderB.Y + colliderB.Height);

        if(xAxisOverlap && yAxisOverlap){
            collision = true;
            directionVector = (colliderA.Center - colliderB.Center).ToVector2();
        }
        else{
            collision = false;
            directionVector = Vector2.Zero;
        }
    }
}