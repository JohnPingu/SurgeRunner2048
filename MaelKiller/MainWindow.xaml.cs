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



        private const int PIXCARRE = 26;
        private const int CENTREX = 566;
        private const int CENTREY = 347;
        private const int INTERVALLETICK = 16;
        private const int LARGEURATKEPEE = 80;
        private const double EVODEGATS = 2, EVOVITESSEATTAQUE = 1.1;
        private bool gauche, droite, haut, bas, ruee, dispoRuee = false, estAttaquant = false;
        private List<Rectangle> objetsSuppr = new List<Rectangle>();
        private DispatcherTimer intervalle = new DispatcherTimer();
        private int cdrRuee = 250, cdRuee;
        private Joueur joueur = new Joueur(25, 10, 30, 1);
        private Armes epee = new Armes("Épée", 25, 50, 1.5, 1, 25, "Une épée, solide et mortelle", false);
        private Armes[] tabEpee = new Armes[10];
        private Armes arme1, arme2;
        private int cdArme1, cdArme2, cdrArme1, cdrArme2;
        private double xfleche, yfleche, lfleche, hfleche;
        private ImageBrush skinFleche = new ImageBrush();
        private char[] directionFleche = new char[2];
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
            cdrArme1 = InitialisationVitesseAttaque(epee.VitesseAttaque);
            cdArme1 = cdrArme1;
            tabEpee = InitialisationArmes(epee);
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
            Console.WriteLine(seconde);

        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            increment++;
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
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Start();
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
            if (gauche == true && Canvas.GetLeft(Carte) < 0)
            {
                Canvas.SetLeft(Carte, Canvas.GetLeft(Carte) + joueur.Vitesse);
                if (Canvas.GetLeft(rect_Joueur) > (CENTREX - 50))
                {
                    Canvas.SetLeft(rect_Joueur, Canvas.GetLeft(rect_Joueur) - joueur.Vitesse / 2);
                }
            }
            else if (droite == true && Canvas.GetLeft(Carte) > -3600)
            {
                Canvas.SetLeft(Carte, Canvas.GetLeft(Carte) - joueur.Vitesse);
                if (Canvas.GetLeft(rect_Joueur) < (CENTREX + 50))
                {
                    Canvas.SetLeft(rect_Joueur, Canvas.GetLeft(rect_Joueur) + joueur.Vitesse / 2);
                }
            }
            if (haut == true && Canvas.GetTop(Carte) < 0)
            {
                Canvas.SetTop(Carte, Canvas.GetTop(Carte) + joueur.Vitesse);
                if (Canvas.GetTop(rect_Joueur) > (CENTREY - 50))
                {
                    Canvas.SetTop(rect_Joueur, Canvas.GetTop(rect_Joueur) - joueur.Vitesse / 2);
                }
            }
            else if (bas == true && Canvas.GetTop(Carte) > -2600)
            {
                Canvas.SetTop(Carte, Canvas.GetTop(Carte) - joueur.Vitesse);
                if (Canvas.GetTop(rect_Joueur) < (CENTREY + 50))
                {
                    Canvas.SetTop(rect_Joueur, Canvas.GetTop(rect_Joueur) + joueur.Vitesse / 2);
                }
            }
            if (!gauche && !droite)
            {
                if (Canvas.GetLeft(rect_Joueur) > CENTREX)
                {
                    Canvas.SetLeft(rect_Joueur, Canvas.GetLeft(rect_Joueur) - joueur.Vitesse / 2);
                    Canvas.SetLeft(Carte, Canvas.GetLeft(Carte) - joueur.Vitesse / 2);
                }
                else if (Canvas.GetLeft(rect_Joueur) < CENTREX)
                {
                    Canvas.SetLeft(rect_Joueur, Canvas.GetLeft(rect_Joueur) + joueur.Vitesse / 2);
                    Canvas.SetLeft(Carte, Canvas.GetLeft(Carte) + joueur.Vitesse / 2);
                }
            }
            if (!haut && !bas)
            {
                if (Canvas.GetTop(rect_Joueur) > CENTREY)
                {
                    Canvas.SetTop(rect_Joueur, Canvas.GetTop(rect_Joueur) - joueur.Vitesse / 2);
                    Canvas.SetTop(Carte, Canvas.GetTop(Carte) - joueur.Vitesse / 2);
                }
                else if (Canvas.GetTop(rect_Joueur) < CENTREY)
                {
                    Canvas.SetTop(rect_Joueur, Canvas.GetTop(rect_Joueur) + joueur.Vitesse / 2);
                    Canvas.SetTop(Carte, Canvas.GetTop(Carte) + joueur.Vitesse / 2);
                }
            }
            //------------------------------------------------//
            //FLÊCHE//
            //------------------------------------------------//
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
                Attaque(epee, Canvas.GetLeft(rect_Joueur), Canvas.GetTop(rect_Joueur));
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
            double xAtk, yAtk, largeur = arme.Portee, hauteur = LARGEURATKEPEE;
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
                    xAtk = Canvas.GetLeft(rect_Joueur) + rect_Joueur.Width / 2;
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
                    yAtk = Canvas.GetTop(rect_Joueur) + rect_Joueur.Height / 2;
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
                    xAtk = Canvas.GetLeft(rect_Joueur) + rect_Joueur.Width / 2;
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
                    yAtk = Canvas.GetTop(rect_Joueur) + rect_Joueur.Height / 2;
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
    }
}
