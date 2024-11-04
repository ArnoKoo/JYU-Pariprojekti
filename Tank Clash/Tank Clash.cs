using System;
using Jypeli;

/// @author Stefan
/// @version 07.10.2024
/// <summary>
/// 
/// </summary>
public class Tank_Clash : PhysicsGame
{
    private IntMeter pelaaja1HP;
    private IntMeter pelaaja2HP;

    private Image pelaaja1Kuva;
    private Image pelaaja2Kuva;
    private SoundEffect ampuminen;
    private SoundEffect osuminen;
    public override void Begin() //Begin() on vissiin Update - Arska
    {
        pelaaja1Kuva = LoadImage("P1Tank.png");
        pelaaja2Kuva = LoadImage("P2Tank.png");
        ampuminen = LoadSoundEffect("pew-pew-lame-sound-effect.wav");
        osuminen = LoadSoundEffect("homemadeoof-47509.wav");

        LuoKentta();

        PhysicsObject tankki1 = LuoTankki(this, -200, 0, Color.Green, 2);
        tankki1.Image = pelaaja1Kuva;
        tankki1.Size = new Vector(128, 128); // Set size based on image

        PhysicsObject tankki2 = LuoTankki(this, 200, 0, Color.Red, -2);
        tankki2.Image = pelaaja2Kuva;
        tankki2.Size = new Vector(128, 128); // Set size based on image

        pelaaja1HP = new IntMeter(5);
        pelaaja2HP = new IntMeter(5);

        LuoPistelaskuri(-400, pelaaja1HP);
        LuoPistelaskuri(400, pelaaja2HP);

        tankki1.Tag = "Pelaaja1";
        tankki2.Tag = "Pelaaja2";

        tankki1.KineticFriction = 0.5;
        tankki1.StaticFriction = 0.5;
        tankki1.LinearDamping = 0.5;
        tankki1.AngularDamping = 1.0;

        tankki2.KineticFriction = 0.5;
        tankki2.StaticFriction = 0.5;
        tankki2.LinearDamping = 0.5;
        tankki2.AngularDamping = 1.0;

        Console.WriteLine($"Pelaaja 1 Image Size: {pelaaja1Kuva.Width}x{pelaaja1Kuva.Height}");
        Console.WriteLine($"Tank 1 Physics Size: {tankki1.Size.X}x{tankki1.Size.Y}");
        Console.WriteLine($"Pelaaja 2 Image Size: {pelaaja2Kuva.Width}x{pelaaja2Kuva.Height}");
        Console.WriteLine($"Tank 2 Physics Size: {tankki2.Size.X}x{tankki2.Size.Y}");
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

    public static PhysicsObject LuoTankki(PhysicsGame peli, double x, double y, Color väri, int paikka) // Funkio joka luo tankkeja, tälle tulee tarvetta, kun aletaan tekee koolisiota //skaalauksesta tuli täs hyödytön, ku skaalaan ne nyt kuvan mukaan -Arska
    {
        PhysicsObject tankki = new PhysicsObject(1, 1, Shape.Rectangle);
        tankki.Color = väri;
        tankki.X = x;
        tankki.Y = y;
        
        tankki.Angle = Angle.FromDegrees(90);
        
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
        Vector suunta = Vector.FromLengthAndAngle(1000.0, pelaaja.Angle);
        pelaaja.Push(suunta);
    }

    void Taakse(PhysicsObject pelaaja) //pelaajat liikkuu taakse
    {
        Vector suunta = Vector.FromLengthAndAngle(-1000.0, pelaaja.Angle);
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

        ampuminen.Play();
    }

    void AmmusOsui(PhysicsObject projektiili, PhysicsObject kohde) // Jos luoti osuu johonkin
    {
        if (kohde != projektiili.Tag) //olit oikeilla jäljillä, kiitos headstartist Dani - Arska
        {
            if (kohde.Tag == "Pelaaja1") //Jos osuu pelaajaan 1...
            {
                pelaaja1HP.Value--;
            }

            else if (kohde.Tag == "Pelaaja2") //Jos osuu pelaajaan 2...
            {
                pelaaja2HP.Value--;
            }
            osuminen.Play();
            Remove(projektiili); //huolimatta mitä käy, poistetaan projektiili
        }
    }

    void LuoPistelaskuri(double x, IntMeter pistelaskuri) //rewritasin tän - Arska
    {
        Label pistenaytto = new Label();
        pistenaytto.Title = "HP: ";
        pistenaytto.X = x;
        pistenaytto.Y = Screen.Top - 100;
        pistenaytto.TextColor = Color.Black;
        pistenaytto.Color = Color.White;

        pistenaytto.BindTo(pistelaskuri);
        Add(pistenaytto);
    }
}

public class Program
{ 
    public static void Main()
    { 
        new Tank_Clash().Run();
    }
}