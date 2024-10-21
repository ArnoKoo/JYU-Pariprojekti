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

    public override void Begin() //Begin() on vissiin Update - Arska
    {
        LuoKentta();
        OhjainLogiikka();
    }

    void OhjainLogiikka()
    {
        //Pelaaja1
        Keyboard.Listen(Key.W, ButtonState.Down, Eteen, "P1 ylös", pelaaja1); //ylös
        Keyboard.Listen(Key.S, ButtonState.Down, Taakse, "P1 alas", pelaaja1); //alas

        Keyboard.Listen(Key.A, ButtonState.Down, TankkienRotaasi, "P1 vasen", pelaaja1, -5.0); //vasen
        Keyboard.Listen(Key.D, ButtonState.Down, TankkienRotaasi, "P1 vasen", pelaaja1, -5.0); //oikea
        
        Keyboard.Listen(Key.F, ButtonState.Pressed, PiuPiu, "P1 ammu", pelaaja1); //ampuminen
        
        //Pelaaja2
        Keyboard.Listen(Key.Up, ButtonState.Down, Eteen, "P2 ylös", pelaaja2); //ylös
        Keyboard.Listen(Key.Down, ButtonState.Down, Taakse, "P2 alas", pelaaja2); //alas
        
        Keyboard.Listen(Key.Left, ButtonState.Down, TankkienRotaasi, "P2 vasen", pelaaja2, -5.0); //vasen
        Keyboard.Listen(Key.Right, ButtonState.Down, TankkienRotaasi, "P2 vasen", pelaaja2, -5.0); //oikea
        
        Keyboard.Listen(Key.RightControl, ButtonState.Pressed, PiuPiu, "P2 ammu", pelaaja2); //ampuminen
    }
    
    void LuoKentta() //Teoriassa vois luoda erikseen tankit mutta ei ole budjettia (aka jaksamista) moiselle - Arska
    {
        //P1
        pelaaja1 = new PhysicsObject(40, 40);
        pelaaja1.Shape = Shape.Rectangle;
        pelaaja1.Color = Color.Green;
        pelaaja1.X = -200; //Spawnipointti. Palataan tähän kohtaan silloin, kun tehdään respawn logiikkaa - Arska
        pelaaja1.LinearDamping = 0.95;
        pelaaja1.MaxVelocity = 300;
        pelaaja1.Restitution = 0.2; 
        Add(pelaaja1);

        //P2
        pelaaja2 = new PhysicsObject(40, 40);
        pelaaja2.Shape = Shape.Rectangle;
        pelaaja2.Color = Color.Red;
        pelaaja2.X = 200; //Spawnipointti. Palataan tähän kohtaan silloin, kun tehdään respawn logiikkaa - Arska
        pelaaja2.LinearDamping = 0.95;
        pelaaja2.MaxVelocity = 300;
        pelaaja2.Restitution = 0.2; 
        Add(pelaaja2);

        //Rajat
        Level.CreateBorders(1.0, false);
        Camera.ZoomToLevel();
    }

    void Eteen(PhysicsObject pelaaja) //pelaajat liikkuu eteenpäin
    {
        Vector suunta = Vector.FromLengthAndAngle(500.0, pelaaja.Angle);
        pelaaja.Push(suunta);
    }
    
    void Taakse(PhysicsObject pelaaja) //pelaajat liikkuu taakse
    {
        Vector suunta = Vector.FromLengthAndAngle(-500.0, pelaaja.Angle);
        pelaaja.Push(suunta);
    }

    void TankkienRotaasi(PhysicsObject pelaaja, double asteenMuutokset)
    {
        pelaaja.Angle += Angle.FromDegrees(asteenMuutokset);
    }

    void PiuPiu(PhysicsObject pelaaja)
    {
        PhysicsObject projektiili = new PhysicsObject(10, 10);
        projektiili.Shape = Shape.Circle;
        projektiili.Color = Color.Yellow;
        projektiili.Position = pelaaja.Position;
        
        Vector ampumaSuunta = Vector.FromLengthAndAngle(500.0, pelaaja.Angle);
        projektiili.Velocity = ampumaSuunta;
        projektiili.Restitution = 0.3; //pomppimista varten
        Add(projektiili);
    }
}

public class Program
{
    public static void Main()
    {
        new Tank_Clash().Run();
    }
}