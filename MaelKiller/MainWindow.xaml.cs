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
        
        private List<Rectangle> listeMonstreRect = new List<Rectangle>();
        private List<Monstres> listMonstre = new List<Monstres>();

        private bool gauche, droite, haut, bas, ruee, dispoRuee = false, estAttaquant = false;
        private bool estEnHaut = false, estEnBas = false, estAGauche = false, estADroite = false;
        private List<Rectangle> objetsSuppr = new List<Rectangle>();
        private DispatcherTimer intervalle = new DispatcherTimer();
        private int cdrRuee = 250, cdRuee;
        private Joueur joueur = new Joueur(25, 10, 30, 1);
        private Armes arme1, arme2;
        private int cdArme1, cdArme2, cdrArme1, cdrArme2;
        private double xfleche, yfleche, lfleche, hfleche;
        private ImageBrush skinFleche = new ImageBrush();
        private ImageBrush drone = new ImageBrush();
        private char[] directionFleche = new char[2];
        private int numMonstre = 1;

        //-----------------------------------//
        //ARMES//
        //-----------------------------------//
        private Armes epee = new Armes("Épée", 25, 100, 1.5, 100, 1, "Une épée classique, solide et mortelle", true);
        private Armes[] tabEpee = new Armes[10];
        private Armes lance = new Armes("Lance", 30, 200, 1.2, 50, 1, "Une lance de soldat, utile pour tenir les adversaires à distance", true);
        private Armes[] tabLance = new Armes[10];
        private Armes fouet = new Armes("Fouet", 10, 150, 1.75, 100, 1, "Le fouet est une arme inhabituelle, mais fort utile pour les ennemis nombreux", true);
        private Armes[] tabFouet = new Armes[10];
        private Armes hache = new Armes("Hache", 50, 50, 1, 100, 1, "Une hache avec peut de portée, masi des dégâts conséquents au corps-à-coprs", true);
        private Armes[] tabHache = new Armes[10];

        private string couleurGlobal = "bleu";

        private Monstres robot = new Monstres("robot", 5, 20, 30, "bleu", 20);

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
            if (seconde % 2 == 0)
            {
                GenerationMonstreHasard();
                Console.WriteLine("NoveauMonstre");
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
            brush1.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "ressources/img/Game/FondMap.png"));
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
            }
            else if (e.Key == Key.Right)
            {
                droite = true;
                directionFleche[1] = 'D';
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
            MiseAJourBarXp();
            vitesseCam = (int)Math.Round(joueur.Vitesse / 2);
            VerifPosition();
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
                checkFrame = cdArme1 / 3;
                frameAtk.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "ressources/img/Game/Atk/Atk" + checkFrame + ".png"));
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
            double xAtk, yAtk, largeur = arme.Portee, hauteur = arme.Taille;
            if (estAttaquant == true)
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
                    xAtk = Canvas.GetLeft(rect_Joueur) + (rect_Joueur.Width / 2) - (largeur / 2);
                }
                if (directionFleche[0] == 'H')
                {
                    yAtk = Canvas.GetTop(rect_Joueur) - hauteur;
                }
                else if (directionFleche[0] == 'B')
                {
                    yAtk = Canvas.GetTop(rect_Joueur) + rect_Joueur.Height;
                }
                else
                {
                    yAtk = Canvas.GetTop(rect_Joueur) + (rect_Joueur.Height / 2) - (hauteur / 2);
                }
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
                    xAtk = Canvas.GetLeft(rect_Joueur) + (rect_Joueur.Width / 2) - (largeur / 2);
                }
                if (directionAtk[0] == 'H')
                {
                    yAtk = Canvas.GetTop(rect_Joueur) - hauteur;
                }
                else if (directionAtk[0] == 'B')
                {
                    yAtk = Canvas.GetTop(rect_Joueur) + rect_Joueur.Height;
                }
                else
                {
                    yAtk = Canvas.GetTop(rect_Joueur) + (rect_Joueur.Height / 2) - (hauteur / 2);
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
#if DEBUG 
            Console.WriteLine("x : " + xAtk + " y : " + yAtk);
            Console.WriteLine("Fleche : " + directionFleche[0] + " " + directionFleche[1]);
            Console.WriteLine("Atk : " + directionAtk[0] + " " + directionAtk[1]);
            Console.WriteLine("Haut : " + haut + "\nBas : " + bas + "\nGauche : " + gauche + "\nDroite : " + droite);
#endif
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
                foreach (Rectangle rectmonstre in listeMonstreRect)
                {
                    Console.WriteLine("Monstre:" + numMonstre);
                    int i = 1;
                    Canvas.SetTop(rectmonstre, Canvas.GetLeft(rectmonstre) + joueur.Vitesse + listMonstre[i].Vitesse);
                }
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
                }
                else if (Canvas.GetLeft(rect_Joueur) < CENTREX - vitesseCam && Canvas.GetLeft(Carte) + vitesseCam <= 0 )
                {
                    Canvas.SetLeft(rect_Joueur, Canvas.GetLeft(rect_Joueur) + vitesseCam);
                    Canvas.SetLeft(Carte, Canvas.GetLeft(Carte) + vitesseCam);
                }
            }
            if (!haut && !bas && Canvas.GetTop(rect_Joueur) - joueur.Vitesse >= 0 && Canvas.GetTop(rect_Joueur) + rect_Joueur.Height + joueur.Vitesse <= 800)
            {
                if (Canvas.GetTop(rect_Joueur) > CENTREY + vitesseCam && Canvas.GetTop(Carte) - vitesseCam >= -2400)
                {
                    Canvas.SetTop(rect_Joueur, Canvas.GetTop(rect_Joueur) - vitesseCam);
                    Canvas.SetTop(Carte, Canvas.GetTop(Carte) - vitesseCam);
                }
                else if (Canvas.GetTop(rect_Joueur) < CENTREY - vitesseCam && Canvas.GetTop(Carte) + vitesseCam <= 0)
                {
                    Canvas.SetTop(rect_Joueur, Canvas.GetTop(rect_Joueur) + vitesseCam);
                    Canvas.SetTop(Carte, Canvas.GetTop(Carte) + vitesseCam);
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
            Monstres nouveauMonstre = new Monstres();
            double nbHasard = random.Next(1, 3);
            switch (nbHasard) {
                case 1:
                    nouveauMonstre = robot;
                    nouveauMonstre.Couleur = couleurGlobal;
                    ApparitionMonstre(nouveauMonstre);
                    break;
                case 2:
                    nouveauMonstre = robot;
                    nouveauMonstre.Couleur = couleurGlobal;
                    ApparitionMonstre(nouveauMonstre);
                    break;
                case 3:
                    nouveauMonstre = robot;
                    nouveauMonstre.Couleur = couleurGlobal;
                    ApparitionMonstre(nouveauMonstre);
                    break;
            }
            
        }
        private void ApparitionMonstre(Monstres monstre)
        {
            string numserie = "Monstre" + numMonstre.ToString();
            Rectangle nouveauMonstreRect = new Rectangle
            {
                Name = numserie,
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
    }
}
