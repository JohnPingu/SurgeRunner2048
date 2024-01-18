using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Diagnostics.Eventing.Reader;
using System.Transactions;

namespace MaelKiller
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool pause = false;
        private int seconde = 0;
        private int minute = 0;
        private int tempsSeconde = 0;
        DispatcherTimer timer = new DispatcherTimer();
        TimeSpan tempsEcoule = new TimeSpan();
        private DateTime DebutChrono = new DateTime();
        private int increment = 0;
        private int vitesseCam;

        private const int PIXCARRE = 26;
        private const int CENTREX = 566;
        private const int CENTREY = 347;
        private const int INTERVALLETICK = 15;
        private const double EVODEGATS = 2, EVOVITESSEATTAQUE = 1.1;
        private const int PERSOIDLE = 4, PERSORUN = 6;
        
        private List<Rectangle> listeMonstreRect = new List<Rectangle>();
        private List<Monstres> listMonstre = new List<Monstres>();

        private bool gauche, droite, haut, bas, ruee, dispoRuee = false, estAttaquant = false;
        private bool niveauSupp = true;
        private bool estEnHaut = false, estEnBas = false, estAGauche = false, estADroite = false;
        private double directionProjectile = 0;
        private List<Rectangle> objetsSuppr = new List<Rectangle>();
        private DispatcherTimer intervalle = new DispatcherTimer();
        private int cdrRuee = 250, cdRuee;
        private Joueur joueur = new Joueur(25, 4, 30, 1);
        private Armes arme1, arme2;
        private int cdArme1, cdArme2, cdrArme1, cdrArme2;
        private double xfleche, yfleche, lfleche, hfleche;
        private ImageBrush skinFleche = new ImageBrush();
        private ImageBrush drone = new ImageBrush();
        private char[] directionFleche = new char[2];
        private char[] directionSkin = new char[2];
        private int numMonstre = 1;
        private ImageBrush skinPerso = new ImageBrush();
        private int skinFrameCompte = 0;

        private bool CameraEstEnMouvement = false;
        private double CameraMouvement = 0;

        //-----------------------------------//
        //ARMES//
        //-----------------------------------//
        private Armes epee = new Armes("Épée", 25, 100, 1.5, 100, 1, "Une épée classique, solide et mortelle", true);
        private Armes[] tabEpee = new Armes[10];
        private Armes lance = new Armes("Lance", 30, 200, 1.2, 50, 1, "Une lance de soldat, utile pour tenir les adversaires à distance", true);
        private Armes[] tabLance = new Armes[10];
        private Armes fouet = new Armes("Fouet", 10, 150, 1.75, 100, 1, "Le fouet est une arme inhabituelle, mais utile pour les ennemis nombreux", true);
        private Armes[] tabFouet = new Armes[10];
        private Armes hache = new Armes("Hache", 50, 50, 1, 100, 1, "Une hache avec peu de portée, mais des dégâts conséquents au corps-à-coprs", true);
        private Armes[] tabHache = new Armes[10];
        private Armes revolver = new Armes("Revolver", 50, 750, 0.75, 20, 1, "Une puissance de feu bienvenue pour se battre de loin, mais lente à l'utilisation", false, 10);
        private Armes[] tabRevolver = new Armes[10];
        private Armes fusilSnip = new Armes("Fusil de précision", 75, 900, 0.5, 20, 1, "Une arme de très longue portée, qui en paie le prix. A metre entre les mains d'experts", false, 10);
        private Armes[] tabFusilSnip = new Armes[10];
        private Armes fusilAssaut = new Armes("Fusil d'Assaut", 5, 700, 2, 20, 1, "L'arme de prédilection des soldats faisant face à de grands groupes d'ennemis", false, 10);
        private Armes[] tabFusilAssaut = new Armes[10];
        private Armes canon = new Armes("Canon à main", 150, 800, 0.25, 100, 1, "On ne fait plus dans la dentelle. Préparez vos propre frappes d'artiellerie directement depuis le front", false, 2);
        private Armes[]tabCanon = new Armes[10];
        private Amélioration[] ameliorations = new Amélioration[4];
        private Armes[] listeArmes = new Armes[8];
        private string couleurGlobal = "bleu";

        //-----------------------------------//
        //SUPPORTS//
        //-----------------------------------//
        private Supports jambes = new Supports("Jambes Bioniques", 1, "vitesse", "Vos jambes à la pointe de la technologie vous permettent de vous déplacer plus vite !");
        private Supports exosquelette = new Supports("Exosquelette", 1, "pv", "Un exosquelette ultra-résistant réduit considérablement les risque de mort");
        private Supports nanoMachine = new Supports("Nano-Machine", 1, "regen", "De nanoscopiques automates remplacent vos globules rouges pour une capacité de guérison maximale");
        private Supports coeurOr = new Supports("Coeur en or", 1, "attraction", "Votre coeur métallique crée un champmagnétique permettant d'attirer l'expérience de plus loin");
        private Supports forgeage = new Supports("Forgeage adamantin", 1, "degats", "Votre maîtrise de la forge adamantine vous offre des armes de qualité supérieure aux dégats soutenus");
        private Supports revetement = new Supports("Revêtement Tachyon", 1, "vitesseAtk", "Le revêtement tachyonique de vos armes leur font atteindre des vitesses inégalées");
        private Supports[] listeSupports = new Supports[6];

        //-----------------------------------//
        //AMELIORATIONS//
        //-----------------------------------//
        private Amélioration déferlement = new Amélioration("Arme", "Déferlement", "Un champ d'ondes électriques vous entoure");

        private Monstres robot = new Monstres("robot", 5, 20, 6, "bleu", 20);

        private void ProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private char[] directionAtk = new char[2];
        private ImageBrush frameAtk = new ImageBrush();
        private int checkFrame;

        public MainWindow()
        {
            InitializeComponent();
            Menu menu = new Menu();
            menu.ShowDialog();
            if (menu.DialogResult == false) Application.Current.Shutdown();
            ChargementJeu();
            InitialisationAmelioration();
            intervalle.Tick += MoteurJeu;
            intervalle.Interval = TimeSpan.FromMilliseconds(INTERVALLETICK);
            intervalle.Start();
            cdrArme1 = InitialisationVitesseAttaque(lance.VitesseAttaque);
            cdArme1 = cdrArme1;
            tabEpee = InitialisationArmes(epee);
            tabLance = InitialisationArmes(lance);
            tabFouet = InitialisationArmes(fouet);
            tabHache = InitialisationArmes(hache);
            directionFleche[1] = 'D';
            directionSkin[1] = 'D';

            Rectangle fleche = new Rectangle
            {
                Tag = "flecheAtk",
                Height = PIXCARRE,
                Width = PIXCARRE,
                Fill = Brushes.Purple,
                Stroke = Brushes.Transparent,
            };
            xfleche = Canvas.GetLeft(rect_Joueur) + rect_Joueur.Width + joueur.Vitesse;
            yfleche = Canvas.GetTop(rect_Joueur) + rect_Joueur.Height / 2;
            monCanvas.Children.Add(fleche);
            skinFleche.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "ressources/img/Game/Fleche/flechedroite.png"));
            skinPerso.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "ressources/img/Game/Personnage/Perso/PersoIdle_D_1.png"));
            rect_Joueur.Fill = skinPerso;
        }

        private void CalculTemps()
        {
            /*tempsEcoule = DateTime.Now - DebutChrono;
            minute = tempsEcoule.Minutes;
            seconde = tempsEcoule.Seconds;*/

            minute = (int)Math.Ceiling((double)(increment / 60));
            seconde = increment - (minute * 60);

        }
        private void MinuterieTick(object sender, EventArgs e)
        {
            increment++;
            //joueur.GainExperience(10);
            MiseAJourCouleur();
            if (seconde % 3 == 0)
            {
                GenerationMonstreHasard();
            }
            Console.WriteLine(increment);
        }

        private void MiseAJourTemps()
        {
            string minuteTexte = minute.ToString();
            string secondeTexte = seconde.ToString();
            if (minute >= 0 && minute <= 9)
            {
                minuteTexte = "0" + minute;
            }
            if (seconde >= 0 && seconde <= 9)
            {
                secondeTexte = "0" + seconde;
            }
            chrono.Content = minuteTexte + ":" + secondeTexte;
        }

        private void ChargementJeu()
        {
            DebutChrono = DateTime.Now;
            ImageBrush brush1 = new ImageBrush();
            brush1.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "ressources/img/Game/map.png"));
            Carte.Fill = brush1;
            timer.Tick += MinuterieTick;
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Start();
        }

        private void MiseAJourBarXp()
        {
            barXP.Minimum = 0;
            barXP.Maximum = joueur.XpPourNiveauSuivant;
            barXP.Value = joueur.Xp;
        }

        

        private void MiseAJourCouleur()
        {
            switch (minute)
            {
                case < 3:
                    couleurGlobal = "bleu";
                    break;
                case < 5:
                    couleurGlobal = "rouge";
                    break;
                case > 5:
                    couleurGlobal = "noir";
                    break;
            }
        }

        

        private void FenetrePrincipale_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                gauche = true;
                directionFleche[1] = 'G';
                directionSkin[1] = 'G';
            }
            else if (e.Key == Key.Right)
            {
                droite = true;
                directionFleche[1] = 'D';
                directionSkin[1] = 'D';
            }
            if (e.Key == Key.Up)
            {
                haut = true;
                directionFleche[0] = 'H';
            }
            else if (e.Key == Key.Down)
            {
                bas = true;
                directionFleche[0] = 'B';
            }
            if (e.Key == Key.Space)
            {
                if (dispoRuee = true)
                {
                    ruee = true;
                    dispoRuee = false;
                }
            }
            if (e.Key == Key.Escape)
            {
                pause = true;
                MiseEnPause();
            }
        }
        private void FenetrePrincipale_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                gauche = false;
            }
            if (e.Key == Key.Right)
            {
                droite = false;
            }
            if (e.Key == Key.Up)
            {
                haut = false;
            }
            if (e.Key == Key.Down)
            {
                bas = false;
            }
            if (e.Key == Key.Space)
            {
                ruee = false;
            }
        }
        private void MoteurJeu(object sender, EventArgs e)
        {
            skinFrameCompte++;
            SkinPersonnage();
            MiseAJourBarXp();
            vitesseCam = (int)Math.Round(joueur.Vitesse / 2);
            VerifPosition();
            if (niveauSupp == true)
            {
                NiveauSupérieur();
                MiseEnPause();
            }
            //------------------------------------------------//
            //JOUEUR//
            //------------------------------------------------//
            Rect rectJoueur = new Rect(Canvas.GetLeft(rect_Joueur), Canvas.GetTop(rect_Joueur), rect_Joueur.Width, rect_Joueur.Height);
            if (pause == false)
            {
                CalculTemps();
                MiseAJourTemps();
            }


            //------------------------------------------------//
            //RUEE//
            //------------------------------------------------//
            if (dispoRuee == false)
            {
                cdRuee -= 1;
                if (cdRuee == 0)
                {
                    dispoRuee = true;
                    cdRuee = cdrRuee;
                }
            }

            //------------------------------------------------//
            //DEPLACEMENT//
            //------------------------------------------------//
            Deplacements();
            cameraEstEnMouvement();
            DeplacementMonstre();

            //------------------------------------------------//
            //FLÊCHE//
            //------------------------------------------------//

            VerifDirection();

            //------------------------------------------------//
            //ATTAQUE//
            //------------------------------------------------//
            estAttaquant = false;
            cdArme1 -= 1;
            if (cdArme1 == 0)
            {
                estAttaquant = true;
            }
            if (cdArme1 <= 0)
            {
                checkFrame = cdArme1;
                frameAtk.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "ressources/img/Game/Armes/Lance/Lance_G" + checkFrame + ".png"));
                Attaque(lance, Canvas.GetLeft(rect_Joueur), Canvas.GetTop(rect_Joueur));
            }
            if (cdArme1 == -9) 
            {
                foreach (Rectangle x in monCanvas.Children.OfType<Rectangle>())
                {
                    if (x is Rectangle && (string)x.Tag == "attaque")
                    {
                        objetsSuppr.Add(x);
                    }
                }
                foreach (Rectangle y in objetsSuppr)
                {
                    monCanvas.Children.Remove(y);
                }
                cdArme1 = cdrArme1;
            }

        }

        private int InitialisationVitesseAttaque(double vitesseAttaque)
        {
            int cdrArme =(int)( 1000 / INTERVALLETICK / vitesseAttaque);
            return cdrArme;
        }
        private void Attaque(Armes arme, double xjoueur, double yjoueur)
        {
            double xAtk = 0, yAtk = 0, largeur = arme.Portee, hauteur = arme.Taille;
            if (estAttaquant == true)
            {
                if (arme.EstMelee == true)
                {
                    if (arme.Amplitude != 0)
                    {
                        xAtk = Canvas.GetLeft(rect_Joueur) + rect_Joueur.Width / 2 - arme.Amplitude;
                        yAtk = Canvas.GetTop(rect_Joueur) + rect_Joueur.Height / 2 - arme.Amplitude;
                        largeur = arme.Amplitude * 2;
                        hauteur = arme.Amplitude * 2;
                    }
                    else
                    {
                        directionAtk[0] = directionFleche[0];
                        directionAtk[1] = directionFleche[1];
                        if (directionFleche[1] == 'G')
                        {
                            xAtk = Canvas.GetLeft(rect_Joueur) - largeur;
                        }
                        else if (directionFleche[1] == 'D')
                        {
                            xAtk = Canvas.GetLeft(rect_Joueur) + rect_Joueur.Width;
                        }
                        else
                        {
                            largeur = arme.Taille;
                            xAtk = Canvas.GetLeft(rect_Joueur) + (rect_Joueur.Width / 2) - (largeur / 2);
                        }
                        if (directionFleche[0] == 'H')
                        {
                            hauteur = arme.Portee;
                            yAtk = Canvas.GetTop(rect_Joueur) - hauteur;
                        }
                        else if (directionFleche[0] == 'B')
                        {
                            hauteur = arme.Portee;
                            yAtk = Canvas.GetTop(rect_Joueur) + rect_Joueur.Height;
                        }
                        else
                        {
                            yAtk = Canvas.GetTop(rect_Joueur) + (rect_Joueur.Height / 2) - (hauteur / 2);
                        }
                    }
                }
                else
                {
                    TirArmeDistance();
                }
                
            }
            else
            {
                if (arme.EstMelee == true)
                {
                    if (arme.Amplitude != 0)
                    {
                        xAtk = Canvas.GetLeft(rect_Joueur) + rect_Joueur.Width / 2 - arme.Amplitude;
                        yAtk = Canvas.GetTop(rect_Joueur) + rect_Joueur.Height / 2 - arme.Amplitude;
                    }
                    else
                    {
                        if (directionAtk[1] == 'G')
                        {
                            xAtk = Canvas.GetLeft(rect_Joueur) - largeur;
                        }
                        else if (directionAtk[1] == 'D')
                        {
                            xAtk = Canvas.GetLeft(rect_Joueur) + rect_Joueur.Width;
                        }
                        else
                        {
                            largeur = arme.Taille;
                            xAtk = Canvas.GetLeft(rect_Joueur) + (rect_Joueur.Width / 2) - (largeur / 2);
                        }
                        if (directionAtk[0] == 'H')
                        {
                            hauteur = arme.Portee;
                            yAtk = Canvas.GetTop(rect_Joueur) - hauteur;
                        }
                        else if (directionAtk[0] == 'B')
                        {
                            hauteur = arme.Portee;
                            yAtk = Canvas.GetTop(rect_Joueur) + rect_Joueur.Height;
                        }
                        else
                        {
                            yAtk = Canvas.GetTop(rect_Joueur) + (rect_Joueur.Height / 2) - (hauteur / 2);
                        }
                    }
                }
                Rect attaque = new Rect(xAtk, yAtk, largeur, hauteur);
                Rectangle atk = new Rectangle
                {
                    Tag = "attaque",
                    Width = largeur,
                    Height = hauteur,
                    Fill = frameAtk,
                };
                Canvas.SetLeft(atk, xAtk);
                Canvas.SetTop(atk, yAtk);
                monCanvas.Children.Add(atk);
            }
            
#if DEBUG 
            Console.WriteLine("x : " + xAtk + " y : " + yAtk);
            Console.WriteLine("Fleche : " + directionFleche[0] + " " + directionFleche[1]);
            Console.WriteLine("Atk : " + directionAtk[0] + " " + directionAtk[1]);
            Console.WriteLine("Haut : " + haut + "\nBas : " + bas + "\nGauche : " + gauche + "\nDroite : " + droite);
#endif
        }
        private void TirArmeDistance()
        {

        }
        private Armes[] InitialisationArmes(Armes arme)
        {
            Armes[] tabArme;
            tabArme = new Armes[10];
            tabArme[0] = arme;
            for (int i = 1; i < 10; i++)
            {
                tabArme[i] = arme;
                tabArme[i].Niveau = arme.Niveau + i;
                tabArme[i].Degats = arme.Degats * EVODEGATS * tabArme[i].Niveau;
                tabArme[i].VitesseAttaque = arme.VitesseAttaque * EVOVITESSEATTAQUE * tabArme[i].Niveau;
            }
            return tabArme;
        }
        private void Deplacements()
        {
            //---------------------------------------------------------//
            //DEPLACEMENT VERS LA GAUCHE//
            //---------------------------------------------------------//
            if (gauche == true && estAGauche == false && estADroite == false)
            {
                Canvas.SetLeft(Carte, Canvas.GetLeft(Carte) + joueur.Vitesse);
                CameraEstEnMouvement = true;

                if (Canvas.GetLeft(rect_Joueur) > (CENTREX - 50))
                {
                    Canvas.SetLeft(rect_Joueur, Canvas.GetLeft(rect_Joueur) - vitesseCam);
                }
            }
            else if (gauche == true && estAGauche == true)
            {
                if (Canvas.GetLeft(rect_Joueur) - joueur.Vitesse >= 0)
                {
                    Canvas.SetLeft(rect_Joueur, Canvas.GetLeft(rect_Joueur) - joueur.Vitesse);
                }
                else Canvas.SetLeft(rect_Joueur, -Canvas.GetLeft(Carte));
            }
            else if (gauche == true && estADroite == true && Canvas.GetLeft(rect_Joueur) > CENTREX)
            {
                Canvas.SetLeft(rect_Joueur, Canvas.GetLeft(rect_Joueur) - joueur.Vitesse);
            }
            else if (gauche == true && estADroite == true && Canvas.GetLeft(rect_Joueur) <= CENTREX)
            {
                Canvas.SetLeft(Carte, Canvas.GetLeft(Carte) + joueur.Vitesse);
                CameraEstEnMouvement = true;
                if (Canvas.GetLeft(rect_Joueur) > (CENTREX - 50))
                {
                    Canvas.SetLeft(rect_Joueur, Canvas.GetLeft(rect_Joueur) + vitesseCam);
                }
            }
            //---------------------------------------------------------//
            //DEPLACEMENT VERS LA DROITE//
            //---------------------------------------------------------//
            else if (droite == true && estAGauche == false && estADroite == false)
            {
                Canvas.SetLeft(Carte, Canvas.GetLeft(Carte) - joueur.Vitesse);
                CameraEstEnMouvement = true;
                if (Canvas.GetLeft(rect_Joueur) < (CENTREX + 50))
                {
                    Canvas.SetLeft(rect_Joueur, Canvas.GetLeft(rect_Joueur) + vitesseCam);
                }
            }
            else if (droite == true && estADroite == true)
            {
                if (Canvas.GetLeft(rect_Joueur) + joueur.Vitesse <= fenetrePrincipale.ActualWidth - rect_Joueur.Width)
                {
                    Canvas.SetLeft(rect_Joueur, Canvas.GetLeft(rect_Joueur) + joueur.Vitesse);
                }
                else Canvas.SetLeft(rect_Joueur, fenetrePrincipale.ActualWidth - rect_Joueur.Width);
            }
            else if (droite == true && estAGauche == true && Canvas.GetLeft(rect_Joueur) < CENTREX)
            {
                Canvas.SetLeft(rect_Joueur, Canvas.GetLeft(rect_Joueur) + joueur.Vitesse);
            }
            else if (droite == true && estAGauche == true && Canvas.GetLeft(rect_Joueur) >= CENTREX)
            {
                Canvas.SetLeft(Carte, Canvas.GetLeft(Carte) - joueur.Vitesse);
                CameraEstEnMouvement = true;
                if (Canvas.GetLeft(rect_Joueur) < (CENTREX + 50))
                {
                    Canvas.SetLeft(rect_Joueur, Canvas.GetLeft(rect_Joueur) + vitesseCam);
                }
            }
            //---------------------------------------------------------//
            //DEPLACEMENT VERS LE HAUT//
            //---------------------------------------------------------//
            if (haut == true && estEnHaut == false && estEnBas == false)
            {
                Canvas.SetTop(Carte, Canvas.GetTop(Carte) + joueur.Vitesse);
                CameraEstEnMouvement = true;
                if (Canvas.GetTop(rect_Joueur) > (CENTREY - 50))
                {
                    Canvas.SetTop(rect_Joueur, Canvas.GetTop(rect_Joueur) - vitesseCam);
                }
            }
            else if (haut == true && estEnHaut == true)
            {
                if (Canvas.GetTop(rect_Joueur) - joueur.Vitesse >= 0)
                {
                    Canvas.SetTop(rect_Joueur, Canvas.GetTop(rect_Joueur) - joueur.Vitesse);
                }
                else Canvas.SetTop(rect_Joueur, -Canvas.GetTop(Carte));
            }
            else if (haut == true && estEnBas == true && Canvas.GetTop(rect_Joueur) > CENTREY)
            {
                Canvas.SetTop(rect_Joueur, Canvas.GetTop(rect_Joueur) - joueur.Vitesse);
            }
            else if (haut == true && estEnBas == true && Canvas.GetTop(rect_Joueur) <= CENTREY)
            {
                Canvas.SetTop(Carte, Canvas.GetTop(Carte) + joueur.Vitesse);
                CameraEstEnMouvement = true;
                if (Canvas.GetTop(rect_Joueur) > (CENTREY - 50))
                {
                    Canvas.SetTop(rect_Joueur, Canvas.GetTop(rect_Joueur) - vitesseCam);
                }
            }
            //---------------------------------------------------------//
            //DEPLACEMENT VERS LE BAS//
            //---------------------------------------------------------//
            else if (bas == true && estEnBas == false && estEnHaut == false)
            {
                Canvas.SetTop(Carte, Canvas.GetTop(Carte) - joueur.Vitesse);
                CameraEstEnMouvement = true;
                if (Canvas.GetTop(rect_Joueur) < (CENTREY + 50))
                {
                    Canvas.SetTop(rect_Joueur, Canvas.GetTop(rect_Joueur) + vitesseCam);
                }
            }
            else if (bas == true && estEnBas == true)
            {
                if (Canvas.GetTop(rect_Joueur) + joueur.Vitesse <= 770 - rect_Joueur.Height)
                {
                    Canvas.SetTop(rect_Joueur, Canvas.GetTop(rect_Joueur) + joueur.Vitesse);
                }
                else Canvas.SetTop(rect_Joueur, fenetrePrincipale.ActualHeight - rect_Joueur.Height);
            }
            else if (bas == true && estEnHaut == true && Canvas.GetTop(rect_Joueur) < CENTREY)
            {
                Canvas.SetTop(rect_Joueur, Canvas.GetTop(rect_Joueur) + joueur.Vitesse);
            }
            else if (bas == true && estEnHaut == true && Canvas.GetTop(rect_Joueur) >= CENTREY)
            {
                Canvas.SetTop(Carte, Canvas.GetTop(Carte) - joueur.Vitesse);
                CameraEstEnMouvement = true;
                if (Canvas.GetTop(rect_Joueur) < (CENTREY + 50))
                {
                    Canvas.SetTop(rect_Joueur, Canvas.GetTop(rect_Joueur) + vitesseCam);
                }
            }
            //---------------------------------------------------------//
            //RECENTRAGE CAMERA//
            //---------------------------------------------------------//
            if (!gauche && !droite && Canvas.GetLeft(rect_Joueur) - joueur.Vitesse >= 0 && Canvas.GetLeft(rect_Joueur) + joueur.Vitesse + rect_Joueur.Width <= 1200)
            {
                if (Canvas.GetLeft(rect_Joueur) > CENTREX + vitesseCam && Canvas.GetLeft(Carte) - vitesseCam >= -3600)
                {
                    Canvas.SetLeft(rect_Joueur, Canvas.GetLeft(rect_Joueur) - vitesseCam);
                    Canvas.SetLeft(Carte, Canvas.GetLeft(Carte) - vitesseCam);
                    CameraEstEnMouvement = true;
                }
                else if (Canvas.GetLeft(rect_Joueur) < CENTREX - vitesseCam && Canvas.GetLeft(Carte) + vitesseCam <= 0 )
                {
                    Canvas.SetLeft(rect_Joueur, Canvas.GetLeft(rect_Joueur) + vitesseCam);
                    Canvas.SetLeft(Carte, Canvas.GetLeft(Carte) + vitesseCam);
                    CameraEstEnMouvement = true;
                }
            }
            if (!haut && !bas && Canvas.GetTop(rect_Joueur) - joueur.Vitesse >= 0 && Canvas.GetTop(rect_Joueur) + rect_Joueur.Height + joueur.Vitesse <= 800)
            {
                if (Canvas.GetTop(rect_Joueur) > CENTREY + vitesseCam && Canvas.GetTop(Carte) - vitesseCam >= -2400)
                {
                    Canvas.SetTop(rect_Joueur, Canvas.GetTop(rect_Joueur) - vitesseCam);
                    Canvas.SetTop(Carte, Canvas.GetTop(Carte) - vitesseCam);
                    CameraEstEnMouvement = true;
                }
                else if (Canvas.GetTop(rect_Joueur) < CENTREY - vitesseCam && Canvas.GetTop(Carte) + vitesseCam <= 0)
                {
                    Canvas.SetTop(rect_Joueur, Canvas.GetTop(rect_Joueur) + vitesseCam);
                    Canvas.SetTop(Carte, Canvas.GetTop(Carte) + vitesseCam);
                    CameraEstEnMouvement = true;
                } 
            }

            if(!haut && !bas && !gauche && !droite)
            {
               CameraEstEnMouvement = false;
            }
        }
        private void cameraEstEnMouvement() 
        {
            if(CameraEstEnMouvement == false)
            {
                CameraMouvement = 0;
            } else
            {
                CameraMouvement = vitesseCam;
            }
        }
        
        private void VerificationCollisionMonstreJoueur()
        {
            foreach (Rectangle monstrerectangle in listeMonstreRect)
            {
                int Xmonstre = (int)Canvas.GetLeft(monstrerectangle);
                int Ymonstre = (int)Canvas.GetTop(monstrerectangle);
                int Xjoueur = (int)Canvas.GetTop(rect_Joueur);
                int Yjoueur = (int)Canvas.GetTop(rect_Joueur);
                Rect monstrerect = new Rect(Xmonstre,Ymonstre,monstrerectangle.Width, monstrerectangle.Height);
                Rect joueurrect = new Rect(Xjoueur, Yjoueur, rect_Joueur.Width, rect_Joueur.Height);
                bool collision = monstrerect.IntersectsWith(joueurrect);
                if (collision)
                {
                    CollisionAvecJoueur();
                }
            }
        }
        private void CollisionAvecJoueur()
        {

        }
        
        private void DeplacementMonstre()
        {
            foreach (Monstres monstre in listMonstre)
            {
                int i = monstre.Index;
                double diffX = Math.Abs(Canvas.GetLeft(rect_Joueur) - Canvas.GetLeft(listeMonstreRect[i - 1]));
                double diffY = Math.Abs(Canvas.GetTop(rect_Joueur) - Canvas.GetTop(listeMonstreRect[i - 1]));
                double vitesseX = monstre.Vitesse;
                double vitesseY = monstre.Vitesse;
                double frameMax;
                
                if (diffX >= diffY)
                {
                    frameMax = diffX / monstre.Vitesse;
                    vitesseY = diffY / frameMax;
                }
                else
                {
                    frameMax = diffY / monstre.Vitesse;
                    vitesseX = diffX / frameMax;
                }
                if (CameraEstEnMouvement)
                {
                    if (!(Canvas.GetLeft(rect_Joueur) < Canvas.GetLeft(listeMonstreRect[i - 1]) + 2 && Canvas.GetLeft(rect_Joueur) > Canvas.GetLeft(listeMonstreRect[i - 1]) - 2))
                    {
                        if (gauche && !droite && !estAGauche && !estADroite)
                        {
                            if (Canvas.GetLeft(listeMonstreRect[i - 1]) + monstre.Vitesse > Canvas.GetLeft(rect_Joueur))
                            {
                                vitesseX -= CameraMouvement*1.5;
                            }
                            else
                            {
                                vitesseX += CameraMouvement*1.5;
                            }
                        }
                        else if (droite && !gauche && !estAGauche && !estADroite)
                        {
                            if (Canvas.GetLeft(listeMonstreRect[i - 1]) + monstre.Vitesse > Canvas.GetLeft(rect_Joueur))
                            {
                                vitesseX += CameraMouvement*1.5;
                            }
                            else
                            {
                                vitesseX -= CameraMouvement * 1.5;
                            }
                        }
                    }
                    if(!(Canvas.GetTop(rect_Joueur) < Canvas.GetTop(listeMonstreRect[i - 1]) + 6 && Canvas.GetTop(rect_Joueur) > Canvas.GetTop(listeMonstreRect[i - 1]) - 6))
                    {
                        if (haut && !bas && !estEnBas && !estEnHaut)
                        {
                            if (Canvas.GetTop(listeMonstreRect[i - 1]) + monstre.Vitesse > Canvas.GetTop(rect_Joueur) + monstre.Vitesse)
                            {
                                vitesseY -= CameraMouvement;
                            }
                            else
                            {
                                vitesseY += CameraMouvement;
                            }
                        }
                        else if (bas && !haut && !estEnHaut && !estEnBas)
                        {
                            if (Canvas.GetTop(listeMonstreRect[i - 1]) + monstre.Vitesse > Canvas.GetTop(rect_Joueur) + monstre.Vitesse)
                            {
                                vitesseY += CameraMouvement;
                            }
                            else
                            {
                                vitesseY -= CameraMouvement;
                            }
                        }
                    }
                }
                if (Canvas.GetLeft(rect_Joueur) < Canvas.GetLeft(listeMonstreRect[i - 1]) + 6 && Canvas.GetLeft(rect_Joueur) > Canvas.GetLeft(listeMonstreRect[i - 1]) - 6 && Canvas.GetTop(rect_Joueur) < Canvas.GetTop(listeMonstreRect[i - 1]) + 6 && Canvas.GetTop(rect_Joueur) > Canvas.GetTop(listeMonstreRect[i - 1]) - 6)
                {
                    continue;
                }
                else if (Canvas.GetLeft(listeMonstreRect[i - 1]) + monstre.Vitesse > Canvas.GetLeft(rect_Joueur))
                {
                    Canvas.SetLeft(listeMonstreRect[i - 1], Canvas.GetLeft(listeMonstreRect[i - 1]) - vitesseX);
                }
                else
                {
                    Canvas.SetLeft(listeMonstreRect[i - 1], Canvas.GetLeft(listeMonstreRect[i - 1]) + vitesseX);
                }

                if (Canvas.GetLeft(rect_Joueur) < Canvas.GetLeft(listeMonstreRect[i - 1]) + 6 && Canvas.GetLeft(rect_Joueur) > Canvas.GetLeft(listeMonstreRect[i - 1]) - 6 && Canvas.GetTop(rect_Joueur) < Canvas.GetTop(listeMonstreRect[i - 1]) + 6 && Canvas.GetTop(rect_Joueur) > Canvas.GetTop(listeMonstreRect[i - 1]) - 6)
                {
                    continue;
                }
                else if (Canvas.GetTop(listeMonstreRect[i - 1]) + monstre.Vitesse > Canvas.GetTop(rect_Joueur) + monstre.Vitesse)
                {
                    Canvas.SetTop(listeMonstreRect[i - 1], Canvas.GetTop(listeMonstreRect[i - 1]) - vitesseY);
                }
                else
                {
                    Canvas.SetTop(listeMonstreRect[i - 1], Canvas.GetTop(listeMonstreRect[i - 1]) + vitesseY);
                }

            }
        }
        private void VerifPosition()
        {
            if (Canvas.GetLeft(Carte) >= 0)
            {
                estAGauche = true;
            }
            else { estAGauche = false; }
            if (Canvas.GetLeft(Carte) <= -3600)
            {
                estADroite = true;
            }
            else { estADroite= false; }
            if (Canvas.GetTop(Carte) >=0)
            {
                estEnHaut = true;
            }
            else { estEnHaut = false; }
            if (Canvas.GetTop(Carte) <= -2400)
            {
                estEnBas = true;
            }
            else { estEnBas = false; }
        }
        private void VerifDirection()
        {
            if (directionFleche[0] == 'H')
            {
                if (haut == false)
                {
                    if (gauche == true || droite == true)
                    {
                        directionFleche[0] = 'N';
                    }
                }
            }
            else if (directionFleche[0] == 'B')
            {
                if (bas == false)
                {
                    if (gauche == true || droite == true)
                    {
                        directionFleche[0] = 'N';
                    }
                }
            }
            if (directionFleche[1] == 'G')
            {
                if (gauche == false)
                {
                    if (bas == true || haut == true)
                    {
                        directionFleche[1] = 'N';
                    }
                }
            }
            else if (directionFleche[1] == 'D')
            {
                if (droite == false)
                {
                    if (bas == true || haut == true)
                    {
                        directionFleche[1] = 'N';
                    }
                }
            }

            foreach (Rectangle x in monCanvas.Children.OfType<Rectangle>())
            {
                if (x is Rectangle && (string)x.Tag == "flecheAtk")
                {
                    objetsSuppr.Add(x);
                }
            }
            foreach (Rectangle y in objetsSuppr)
            {
                monCanvas.Children.Remove(y);
            }
            if (directionFleche[0] == 'B')
            {
                yfleche = Canvas.GetTop(rect_Joueur) + rect_Joueur.Height + joueur.Vitesse;
                if (directionFleche[1] == 'G')
                {
                    xfleche = Canvas.GetLeft(rect_Joueur) - PIXCARRE - joueur.Vitesse;
                    skinFleche.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "ressources/img/Game/Fleche/flecheBG.png"));
                }
                else if (directionFleche[1] == 'D')
                {
                    xfleche = Canvas.GetLeft(rect_Joueur) + rect_Joueur.Width + joueur.Vitesse;
                    skinFleche.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "ressources/img/Game/Fleche/flecheBD.png"));
                }
                else
                {
                    xfleche = Canvas.GetLeft(rect_Joueur) + (rect_Joueur.Width / 2) - (PIXCARRE / 2);
                    skinFleche.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "ressources/img/Game/Fleche/flechebas.png"));
                }
            }
            else if (directionFleche[0] == 'H')
            {
                yfleche = Canvas.GetTop(rect_Joueur) - joueur.Vitesse - PIXCARRE;
                if (directionFleche[1] == 'G')
                {
                    xfleche = Canvas.GetLeft(rect_Joueur) - PIXCARRE - joueur.Vitesse;
                    skinFleche.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "ressources/img/Game/Fleche/flecheHG.png"));
                }
                else if (directionFleche[1] == 'D')
                {
                    xfleche = Canvas.GetLeft(rect_Joueur) + rect_Joueur.Width + joueur.Vitesse;
                    skinFleche.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "ressources/img/Game/Fleche/flecheHD.png"));
                }
                else
                {
                    xfleche = Canvas.GetLeft(rect_Joueur) + (rect_Joueur.Width / 2) - (PIXCARRE / 2);
                    skinFleche.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "ressources/img/Game/Fleche/flechehaut.png"));
                }

            }
            else
            {
                yfleche = Canvas.GetTop(rect_Joueur) + (rect_Joueur.Height / 2) - (PIXCARRE / 2);
                if (directionFleche[1] == 'G')
                {
                    xfleche = Canvas.GetLeft(rect_Joueur) - PIXCARRE - joueur.Vitesse;
                    skinFleche.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "ressources/img/Game/Fleche/flechegauche.png"));
                }
                else if (directionFleche[1] == 'D')
                {
                    xfleche = Canvas.GetLeft(rect_Joueur) + rect_Joueur.Width + joueur.Vitesse;
                    skinFleche.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "ressources/img/Game/Fleche/flechedroite.png"));
                }

            }
            Rectangle fleche = new Rectangle
            {
                Tag = "flecheAtk",
                Height = PIXCARRE,
                Width = PIXCARRE,
                Fill = skinFleche,
            };
            Canvas.SetLeft(fleche, xfleche);
            Canvas.SetTop(fleche, yfleche);
            monCanvas.Children.Add(fleche);
        }
        private void GenerationMonstreHasard()
        {
            Random random = new Random();
            double nbHasard = random.Next(1, 3);
            switch (nbHasard) {
                case 1:
                    Monstres robot = new Monstres("robot", 5, 20, 2, "bleu", 20);
                    robot.Couleur = couleurGlobal;
                    ApparitionMonstre(robot);
                    break;
                case 2:
                    Monstres nouveauMonstre2 = new Monstres("robot", 5, 20, 2, "bleu", 20);
                    nouveauMonstre2.Couleur = couleurGlobal;
                    ApparitionMonstre(nouveauMonstre2);
                    break;
                case 3:
                    Monstres nouveauMonstre3 = new Monstres("robot", 5, 20, 2, "bleu", 20);
                    nouveauMonstre3.Couleur = couleurGlobal;
                    ApparitionMonstre(nouveauMonstre3);
                    break;
            }
            
        }
        private void ApparitionMonstre(Monstres monstre)
        {
            monstre.Index = numMonstre;
            Rectangle nouveauMonstreRect = new Rectangle
            {
                Tag = "Monstre",
                Height = 105,
                Width = 60,
            };

            if (monstre.Nom == "robot") 
            {
                nouveauMonstreRect.Height = 96;
                nouveauMonstreRect.Width = 96;
                drone.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "ressources/img/Game/Monstre/Drone/Idle/1.png"));
                nouveauMonstreRect.Fill = drone;
            };

            if (monstre.Couleur == "rouge")
            {
                monstre.PvMax *= 1.5;
                monstre.Degats *= 1.5;
            } 
            else if(monstre.Couleur == "noir")
            {
                monstre.PvMax *= 2.5;
                monstre.Degats *= 2.5;
            };
            PlacerNouveauMonstre(nouveauMonstreRect);
            listeMonstreRect.Add(nouveauMonstreRect);
            monCanvas.Children.Add(nouveauMonstreRect);
            listMonstre.Add(monstre);
            numMonstre++;
        }

        private void PlacerNouveauMonstre(Rectangle monstre)
        {
            Random random = new Random();
            int rdm = random.Next(1,2);
            int coorX = 0;
            int coorY = 0;

            rdm = random.Next(1,4);

            switch (rdm)
            {
                case 1:
                    coorX = -40;
                    coorY = random.Next(0, 800);
                    break;
                case 2:
                    coorX = 1240;
                    coorY = random.Next(0, 800);
                    break;
                case 3:
                    coorX = random.Next(0, 1200);
                    coorY = -40;
                    break;
                case 4:
                    coorX = random.Next(0, 1200);
                    coorY = 840;
                    break;
            }

            Canvas.SetTop(monstre, coorY);
            Canvas.SetLeft(monstre, coorX);
        }
        private bool verificationCollisions(Rectangle rect)
        {
            return false;
        }
        private void SkinPersonnage()
        {
            if (!gauche && !droite && !haut && !bas)
            {
                skinPerso.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "ressources/img/Game/Personnage/Perso/PersoIdle_" + directionSkin[1] + "_" + (skinFrameCompte / INTERVALLETICK % PERSOIDLE) + ".png")); 
            }
            else
            {
                skinPerso.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "ressources/img/Game/Personnage/Perso/PersoRun_" + directionSkin[1] + "_" + (skinFrameCompte / 10 % PERSORUN) + ".png"));
            }
        }
        private void MiseEnPause()
        {
            timer.Stop();
            intervalle.Stop();
            if (niveauSupp == true)
            {
                foreach (Rectangle x in monCanvas.Children.OfType<Rectangle>())
                {
                    if ((string)x.Tag == "NVSUP" && x is Rectangle)
                    {
                        x.Visibility = Visibility.Visible;
                    }
                }
                foreach (TextBlock x in monCanvas.Children.OfType<TextBlock>())
                {
                    if ((string)x.Tag == "NVSUP" && x is TextBlock)
                    {
                        x.Visibility = Visibility.Visible;
                    }
                }
            }
            else if (pause == true)
            {
                foreach (Rectangle x in monCanvas.Children.OfType<Rectangle>())
                {
                    if ((string)x.Tag == "Pause" && x is Rectangle)
                    {
                        x.Visibility = Visibility.Visible;
                    }
                }
                foreach (TextBlock x in monCanvas.Children.OfType<TextBlock>())
                {
                    if ((string)x.Tag == "Pause" && x is TextBlock)
                    {
                        x.Visibility = Visibility.Visible;
                    }
                }
            }
        }
        private void Reprendre_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            foreach (Rectangle x in monCanvas.Children.OfType<Rectangle>())
            {
                if ((string)x.Tag == "Pause" && x is Rectangle)
                {
                    x.Visibility = Visibility.Hidden;
                }
            }
            foreach (TextBlock x in monCanvas.Children.OfType<TextBlock>())
            {
                if ((string)x.Tag == "Pause" && x is TextBlock)
                {
                    x.Visibility = Visibility.Hidden;
                }
            }
            pause = false;
            timer.Start();
            intervalle.Start();
        }
        private void Quitter_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Environment.Exit(0);
        }
        private void Bonus1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            foreach (Rectangle x in monCanvas.Children.OfType<Rectangle>())
            {
                if ((string)x.Tag == "NVSUP" && x is Rectangle)
                {
                    x.Visibility = Visibility.Hidden;
                }
            }
            foreach (TextBlock x in monCanvas.Children.OfType<TextBlock>())
            {
                if ((string)x.Tag == "NVSUP" && x is TextBlock)
                {
                    x.Visibility = Visibility.Hidden;
                }
            }
            niveauSupp = false;
            timer.Start();
            intervalle.Start();
        }
        private void Bonus2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            foreach (Rectangle x in monCanvas.Children.OfType<Rectangle>())
            {
                if ((string)x.Tag == "NVSUP" && x is Rectangle)
                {
                    x.Visibility = Visibility.Hidden;
                }
            }
            foreach (TextBlock x in monCanvas.Children.OfType<TextBlock>())
            {
                if ((string)x.Tag == "NVSUP" && x is TextBlock)
                {
                    x.Visibility = Visibility.Hidden;
                }
            }
            niveauSupp = false;
            timer.Start();
            intervalle.Start();
        }

        private void Bonus3_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            foreach (Rectangle x in monCanvas.Children.OfType<Rectangle>())
            {
                if ((string)x.Tag == "NVSUP" && x is Rectangle)
                {
                    x.Visibility = Visibility.Hidden;
                }
            }
            foreach (TextBlock x in monCanvas.Children.OfType<TextBlock>())
            {
                if ((string)x.Tag == "NVSUP" && x is TextBlock)
                {
                    x.Visibility = Visibility.Hidden;
                }
            }
            niveauSupp = false;
            timer.Start();
            intervalle.Start();
        }
        private void InitialisationAmelioration()
        {
            listeArmes[0] = epee;
            listeArmes[1] = lance;
            listeArmes[2] = fouet;
            listeArmes[3] = hache;
            listeArmes[4] = revolver;
            listeArmes[5] = fusilSnip;
            listeArmes[6] = fusilAssaut;
            listeArmes[7] = canon;
        }
        public void NiveauSupérieur()
        {
            Random random = new Random();
            int bonusArmes, bonusAug, bonus;
            if (Armes.IsNullOrEmpty(arme1))
            {
                bonusArmes = random.Next(0,3);
                bonusAug = random.Next(0,3);
                while (bonusAug == bonusArmes)
                {
                    bonusAug = random.Next(0, 3);
                }
                bonus = random.Next(0, 3);
                while(bonus == bonusArmes || bonus == bonusAug)
                {
                    bonus = random.Next(0, 3);
                }
            }
            else if (!Armes.IsNullOrEmpty(arme1) && Armes.IsNullOrEmpty(arme2))
            {
                bonusArmes = random.Next(0, 3);
                while (listeArmes[bonusArmes].Nom == arme1.Nom)
                {
                    bonusArmes = random.Next(0, 3);
                }
                bonusAug = random.Next(3, listeArmes.Length);
                bonus = random.Next(0, listeArmes.Length);
                while (bonus == bonusAug || listeArmes[bonus].Nom == arme1.Nom)
                {
                    bonus = random.Next(0, listeArmes.Length);
                }
            }
            else
            {
                bonusArmes = random.Next(3, listeArmes.Length);
                bonusAug = random.Next(3, listeArmes.Length);
                while (bonusAug == bonusArmes)
                {
                    bonusAug = random.Next(3, listeArmes.Length);
                }
                bonus = random.Next(3, listeArmes.Length);
                while (bonus == bonusAug || bonus == bonusArmes)
                {
                    bonus = random.Next(3, listeArmes.Length);
                }
            }
            TitreBonus1.Text = listeArmes[bonusArmes].Nom;
        }
    }
}
