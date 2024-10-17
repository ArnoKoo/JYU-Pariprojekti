using System;
using Jypeli;

/// @author Stefan
/// @version 07.10.2024
/// <summary>
/// 
/// </summary>
public class Tank_Clash : PhysicsGame
{
    private Tank playerTank;

    public override void Begin()
    {
        // Initialize the player's tank
        playerTank = new Tank(40, 40, Color.Green);  // Create a green tank with size 40x40
        Add(playerTank.TankObject);                  // Add the tank's physics object to the game

        // Set the camera to follow the player's tank
        Camera.Follow(playerTank.TankObject);

        // Keyboard controls for moving the player's tank
        Keyboard.Listen(Key.Left, ButtonState.Down, () => playerTank.Move(new Vector(-1, 0)), null);
        Keyboard.Listen(Key.Right, ButtonState.Down, () => playerTank.Move(new Vector(1, 0)), null);
        Keyboard.Listen(Key.Up, ButtonState.Down, () => playerTank.Move(new Vector(0, 1)), null);
        Keyboard.Listen(Key.Down, ButtonState.Down, () => playerTank.Move(new Vector(0, -1)), null);

        // Set up game boundaries and background
        Level.CreateBorders();
        Level.Background.Color = Color.Gray;
        IsFullScreen = true;
    }
}

// Tank class as previously defined
public class Tank
{
    public PhysicsObject TankObject { get; private set; }
    private double speed = 1000;

    public Tank(double width, double height, Color color)
    {
        TankObject = new PhysicsObject(width, height);
        TankObject.Shape = Shape.Rectangle;
        TankObject.Color = color;
        TankObject.Restitution = 0.3;
    }

    public void Move(Vector direction)
    {
        TankObject.Push(direction * speed);
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        // Start the game by running the Tank_Clash game class
        new Tank_Clash().Run();
    }
}
