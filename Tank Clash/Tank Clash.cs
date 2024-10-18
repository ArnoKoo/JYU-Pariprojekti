using System;
using Jypeli;

/// @author Stefan
/// @version 07.10.2024
/// <summary>
/// 
/// </summary>
public class Tank_Clash : PhysicsGame
{
    public PhysicsObject pelaaja1;
    public PhysicsObject pelaaja2;
    
    Vector nopeusYlos = new Vector(0, 200);
    Vector nopeusAlas = new Vector(0, -200);

    public override void Begin() //Begin() on vissiin Update - Arska
    {
        LuoKentta();
        OhjainLogiikka();
    }

    void OhjainLogiikka()
    {
        //Pelaaja1
        Keyboard.Listen(Key.W, ButtonState.Down, TankkienNopeus, "P1 ylös", pelaaja1, nopeusYlos);
        Keyboard.Listen(Key.W, ButtonState.Released, TankkienNopeus, null, pelaaja1, Vector.Zero);
        Keyboard.Listen(Key.S, ButtonState.Down, TankkienNopeus, "P1 alas", pelaaja1, nopeusAlas);
        Keyboard.Listen(Key.S, ButtonState.Released, TankkienNopeus, null, pelaaja1, Vector.Zero);
        
        //Pelaaja2
        Keyboard.Listen(Key.Up, ButtonState.Down, TankkienNopeus, "P2 ylös", pelaaja2, nopeusYlos);
        Keyboard.Listen(Key.Up, ButtonState.Released, TankkienNopeus, null, pelaaja2, Vector.Zero);
        Keyboard.Listen(Key.Down, ButtonState.Down, TankkienNopeus, "P2 alas", pelaaja2, nopeusAlas);
        Keyboard.Listen(Key.Down, ButtonState.Released, TankkienNopeus, null, pelaaja2, Vector.Zero);
    }
    
    void LuoKentta() //Teoriassa vois luoda erikseen tankit mutta ei ole budjettia (aka jaksamista) moiselle - Arska
    {
        //P1
        pelaaja1 = new PhysicsObject(40, 40);
        pelaaja1.Shape = Shape.Rectangle;
        pelaaja1.Color = Color.Green;
        pelaaja1.X = -200; //Spawnipointti. Palataan tähän kohtaan silloin, kun tehdään respawn logiikkaa - Arska
        Add(pelaaja1);

        //P2
        pelaaja2 = new PhysicsObject(40, 40);
        pelaaja2.Shape = Shape.Rectangle;
        pelaaja2.Color = Color.Red;
        pelaaja2.X = 200; //Spawnipointti. Palataan tähän kohtaan silloin, kun tehdään respawn logiikkaa - Arska
        Add(pelaaja2);

        //Rajat
        Level.CreateBorders(1.0, false);
        Camera.ZoomToLevel();
    }

    void TankkienNopeus(PhysicsObject pelaaja, Vector nopeus)
    {
        pelaaja.Velocity = nopeus;
    }
}

public class Program
{
    public static void Main()
    {
        new Tank_Clash().Run();
    }
}