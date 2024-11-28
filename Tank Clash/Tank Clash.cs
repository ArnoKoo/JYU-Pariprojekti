using System;
using Jypeli;

/// @author Daniel Lobko & Arno Korhonen
/// @version 28.11.2024
/// <summary>
/// 
/// </summary>
///

public class Tank_Clash : PhysicsGame
{

    //HP
    private IntMeter pelaaja1HP;
    private IntMeter pelaaja2HP;

    //Kuvat ja soundEfektit
    private Image pelaaja1Kuva;
    private Image pelaaja2Kuva;
    private SoundEffect ampuminen;
    private SoundEffect osuminen;
    
    //Poweruppien setuppi
    private const int HPPowerupArvo = 1;
    private const double nopeusBoostiPituus = 10;
    private const double nopeusMultiplier = 100;
    
    public override void Begin() //Begin() on vissiin Update() - Arska
    {
        
        //Pitää ladata erikseen
        pelaaja1Kuva = LoadImage("P1Tank.png");
        pelaaja2Kuva = LoadImage("P2Tank.png");
        ampuminen = LoadSoundEffect("pew-pew-lame-sound-effect.wav");
        osuminen = LoadSoundEffect("homemadeoof-47509.wav");

        LuoKentta();
        PowerUpLooppi();

        //Pelaaja1 setuppi
        PhysicsObject tankki1 = LuoTankki(this, Level.Left+100, 0, Color.Red, 2);
        tankki1.Image = pelaaja1Kuva;
        tankki1.Size = new Vector(64, 64);
        
        //Pelaaja2 setuppi
        PhysicsObject tankki2 = LuoTankki(this, Level.Right-100, 0, Color.Blue, -2);
        tankki2.Image = pelaaja2Kuva;
        tankki2.Size = new Vector(64, 64);
        
        //HP mittaria varten
        pelaaja1HP = new IntMeter(5);
        pelaaja2HP = new IntMeter(5);
        
        //Siitä puheen ollen
        LuoPistelaskuri(Level.Left+100, pelaaja1HP);
        LuoPistelaskuri(Level.Right-100, pelaaja2HP);
        
        //Tää oli jottei omat luodit vahingoita.
        tankki1.Tag = "Pelaaja1";
        tankki2.Tag = "Pelaaja2";

        tankki1.KineticFriction = 1;
        tankki1.StaticFriction = 1;
        tankki1.LinearDamping = 2;
        tankki1.AngularDamping = 1;

        tankki2.KineticFriction = 1;
        tankki2.StaticFriction = 1;
        tankki2.LinearDamping = 2;
        tankki2.AngularDamping = 1;
        
        OhjainLogiikka(tankki1, tankki2);
    }

    void PowerUpLooppi()
    {
            SpawnPowerUp();
            Timer.SingleShot(5, PowerUpLooppi);
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

    private void LuoKentta()
    {
        //Rajat
        double screenWidth = Screen.Width;
        double screenHeight = Screen.Height;
        
        PhysicsObject vasenReuna = Level.CreateLeftBorder();
        vasenReuna.X = -screenWidth / 2;
        vasenReuna.Height = screenHeight;
        vasenReuna.Restitution = 1.0;
        vasenReuna.KineticFriction = 0.0;
        vasenReuna.IsVisible = true;
        
        PhysicsObject oikeaReuna = Level.CreateRightBorder();
        oikeaReuna.X = screenWidth / 2;
        oikeaReuna.Height = screenHeight;
        oikeaReuna.Restitution = 1.0;
        oikeaReuna.KineticFriction = 0.0;
        oikeaReuna.IsVisible = true;
        
        PhysicsObject ylaReuna = Level.CreateTopBorder();
        ylaReuna.Y = screenHeight / 2;
        ylaReuna.Width = screenWidth;
        ylaReuna.Restitution = 1.0;
        ylaReuna.KineticFriction = 0.0;
        ylaReuna.IsVisible = true;
        
        PhysicsObject alaReuna = Level.CreateBottomBorder();
        alaReuna.Y = -screenHeight / 2;
        alaReuna.Width = screenWidth;
        alaReuna.Restitution = 1.0;
        alaReuna.KineticFriction = 0.0;
        alaReuna.IsVisible = true;
        
        //Seinät jotka on esteitä
        
        LuoSeina(-300, Level.Bottom+150, 20, 150);
        LuoSeina(0, 0, 30, 300);
        
        LuoSeina(100, 0, 150, 20);
        LuoSeina(300, Level.Top-150, 20, 150);
        
        LuoSeina(Level.Left+300, Level.Top-250, 50, 50);
        LuoSeina(Level.Right-270, Level.Bottom+220, 50, 50);
    }
    
    private void LuoSeina(double x, double y, double leveys, double korkeus) //Seinät mappia varte //Arska
    {
        PhysicsObject seina = new PhysicsObject(leveys, korkeus);
        seina.Position = new Vector(x, y);
        seina.Color = Color.Gray;
        seina.Restitution = 1.0;
        seina.KineticFriction = 0.0;
        seina.LinearDamping = double.PositiveInfinity;
        seina.AngularDamping = double.PositiveInfinity;
        seina.IsVisible = true;
        seina.Mass = double.PositiveInfinity; //hmm
        Add(seina);
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
            if (kohde.Tag.Equals("Pelaaja1")) //Jos osuu pelaajaan 1...
            {
                pelaaja1HP.Value--;
                osuminen.Play();
            }

            else if (kohde.Tag.Equals("Pelaaja2")) //Jos osuu pelaajaan 2...
            {
                pelaaja2HP.Value--;
                osuminen.Play();
            }
            Remove(projektiili); //huolimatta mitä käy, poistetaan projektiili
        }
        
        if (pelaaja1HP.Value == 0 || pelaaja2HP.Value == 0)
        {
            PelinLoppu();
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
    
    void SpawnPowerUp() //Logiikka poweruppien spawnaamisessa //Arska
    {
        Random rnd = new Random(); 
        double x  = rnd.Next(-600,600); 
        double y  = rnd.Next(-200,200);
        
        bool spawnHela = RandomGen.NextBool();

        PhysicsObject powerup = new PhysicsObject(20, 20);
        powerup.Position = RandomGen.NextVector(x,y);
        powerup.Shape = Shape.Circle;
        powerup.Color = spawnHela ? Color.Green : Color.Red;
        powerup.Tag = spawnHela ? "HPPowerUp" : "SpeedPowerUp";

        Add(powerup);
        osuminen.Play();

        AddCollisionHandler(powerup, PowerUpCollisionHandler);
    }

    void PowerUpCollisionHandler(PhysicsObject powerup, PhysicsObject tankki)
    {
        if (tankki.Tag.ToString() != "Pelaaja1" && tankki.Tag.ToString() != "Pelaaja2")
        {
            return;
        }

        if (powerup.Tag.ToString() == "HPPowerUp")
        {
            if (tankki.Tag.ToString() == "Pelaaja1")
            {
                pelaaja1HP.Value += HPPowerupArvo;
            }
            else if (tankki.Tag.ToString() == "Pelaaja2")
            {
                pelaaja2HP.Value += HPPowerupArvo;
            }
        }
        else if (powerup.Tag.ToString() == "SpeedPowerUp")
        {
            tankki.MaxVelocity *= nopeusMultiplier;

            Timer.SingleShot(nopeusBoostiPituus, () => tankki.MaxVelocity *= nopeusMultiplier);
        }

        if (!powerup.IsDestroyed)
        {
            Remove(powerup);
        }
    }
    void PelinLoppu()
    {
        Label tekstikentta = new Label(200, 25, "Peli loppui!");
        tekstikentta.Color = Color.White;
        tekstikentta.TextColor = Color.Black;
        tekstikentta.BorderColor = Color.Black;
        Add(tekstikentta);
        
        Timer.SingleShot(3.0, () =>
        {
            Environment.Exit(1);
        });
    }
}

public class Program
{ 
    public static void Main()
    { 
        new Tank_Clash().Run();
    }
}