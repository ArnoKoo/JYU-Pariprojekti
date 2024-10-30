using System;
using Jypeli;

/// @author Stefan
/// @version 07.10.2024
/// <summary>
/// 
/// </summary>
public class Tank_Clash : PhysicsGame
{

    public override void Begin() //Begin() on vissiin Update - Arska
    {
        LuoKentta();
        PhysicsObject tankki1 = LuoTankki(this, -200, 0, Color.Green, 2);
        PhysicsObject tankki2 = LuoTankki(this, 200, 0, Color.Red, -2);
        LuoPistelaskuri(-400);
        LuoPistelaskuri(400);
        OhjainLogiikka(tankki1, tankki2);
    }

    void OhjainLogiikka(PhysicsObject pelaaja1, PhysicsObject pelaaja2) 
    {
        //Pelaaja1
        Keyboard.Listen(Key.W, ButtonState.Down, Eteen, "P1 ylös", pelaaja1); //ylös
        Keyboard.Listen(Key.S, ButtonState.Down, Taakse, "P1 alas", pelaaja1); //alas

        Keyboard.Listen(Key.A, ButtonState.Down, TankkienRotaasi, "P1 vasen", pelaaja1, 5.0); //vasen
        Keyboard.Listen(Key.D, ButtonState.Down, TankkienRotaasi, "P1 vasen", pelaaja1, -5.0); //oikea
        
        Keyboard.Listen(Key.F, ButtonState.Pressed, CoolDown, "P1 ammu", pelaaja1); //ampuminen
        
        //Pelaaja2 
        Keyboard.Listen(Key.Up, ButtonState.Down, Eteen, "P2 ylös", pelaaja2); //ylös
        Keyboard.Listen(Key.Down, ButtonState.Down, Taakse, "P2 alas", pelaaja2); //alas
        
        Keyboard.Listen(Key.Left, ButtonState.Down, TankkienRotaasi, "P2 vasen", pelaaja2, 5.0); //vasen
        Keyboard.Listen(Key.Right, ButtonState.Down, TankkienRotaasi, "P2 vasen", pelaaja2, -5.0); //oikea
        
        Keyboard.Listen(Key.RightControl, ButtonState.Pressed, CoolDown, "P2 ammu", pelaaja2); //ampuminen
    }

    public static PhysicsObject LuoTankki(PhysicsGame peli, double x, double y, Color väri, int paikka) // Funkio joka luo tankkeja, tälle tulee tarvetta, kun aletaan tekee koolisiota
    {
        PhysicsObject tankki = new PhysicsObject(40, 40, Shape.Rectangle);
        tankki.Color = väri;
        tankki.X = x;
        tankki.Y = y;
        tankki.LinearDamping = 0.50;
        tankki.MaxVelocity = 100;
        tankki.Restitution = 0.0;
        peli.Add(tankki);

        return tankki;
    }
    private void LuoKentta() //Teoriassa vois luoda erikseen tankit mutta ei ole budjettia (aka jaksamista) moiselle - Arska  // on aikaa ja jaksamista
    {
        //Rajat
        Level.CreateBorders(1.0, false);
        Camera.ZoomToLevel();
        
        PhysicsObject vasenReuna = Level.CreateLeftBorder();
        vasenReuna.Restitution = 1.0;
        vasenReuna.KineticFriction = 0.0;
        vasenReuna.IsVisible = false;

        PhysicsObject oikeaReuna = Level.CreateRightBorder();
        oikeaReuna.Restitution = 1.0;
        oikeaReuna.KineticFriction = 0.0;
        oikeaReuna.IsVisible = false;

        PhysicsObject ylaReuna = Level.CreateTopBorder();
        ylaReuna.Restitution = 1.0;
        ylaReuna.KineticFriction = 0.0;
        ylaReuna.IsVisible = false;

        PhysicsObject alaReuna = Level.CreateBottomBorder();
        alaReuna.Restitution = 1.0;
        alaReuna.KineticFriction = 0.0;
        alaReuna.IsVisible = false;
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

    private const double CooldownDuration = 0.5;
    private bool isOnCooldown = false;

    void CoolDown(PhysicsObject pelaaja)
    {
        if (isOnCooldown)
        {
            return; 
        }

        PiuPiu(pelaaja);
        isOnCooldown = true;

        Timer.SingleShot(CooldownDuration, () => isOnCooldown = false);
    }
    void PiuPiu(PhysicsObject pelaaja)
    {
        PhysicsObject projektiili = new PhysicsObject(10, 10);
        projektiili.Shape = Shape.Circle;
        projektiili.Color = Color.Yellow;
        projektiili.Position = pelaaja.Position;
        
        Vector ampumaSuunta = Vector.FromLengthAndAngle(500.0, pelaaja.Angle);
        projektiili.Velocity = ampumaSuunta;
        projektiili.Restitution = 0; //pomppimista varten
        Add(projektiili);

        projektiili.Tag = pelaaja;

        AddCollisionHandler(projektiili, AmmusOsui);

    }

    void AmmusOsui(PhysicsObject projektiili, PhysicsObject kohde) // Jos luoti osuu johonkin
    {
        if (kohde != projektiili.Tag)
        {       
            Remove(projektiili);
        }
    }
    
    void LuoPistelaskuri(double x)
    {
        IntMeter pistelaskuri;

        pistelaskuri = new IntMeter(3);

        Label pistenaytto = new Label();
        pistenaytto.Title = "HP: ";
        pistenaytto.X =  x;
        pistenaytto.Y = Screen.Top - 100;
        pistenaytto.TextColor = Color.Black;
        pistenaytto.Color = Color.White;

        pistenaytto.BindTo(pistelaskuri);
        Add(pistenaytto);
    }

public class Program
{
    public static void Main()
    {
        new Tank_Clash().Run();
    }
}