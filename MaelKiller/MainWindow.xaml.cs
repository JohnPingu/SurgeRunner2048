using System;
using System.Collections.Generic;
using System.Linq;
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

namespace MaelKiller
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int PIXCARRE = 13;
        private const int CENTREX = 566;
        private const int CENTREY = 347;
        private const int INTERVALLETICK = 16;
        private const int LARGEURATKEPEE = 40;
        private const double EVODEGATS = 2, EVOVITESSEATTAQUE = 1.1;
        private bool gauche, droite, haut, bas, faceGauche = false, faceDroite = true, faceHaut = false, faceBas = false, ruee, dispoRuee = false;
        private List<Rectangle> objetsSuppr = new List<Rectangle>();
        private DispatcherTimer intervalle = new DispatcherTimer();
        private int cdrRuee = 250, cdRuee;
        private Joueur joueur = new Joueur(25, 10, 30, 1);
        private Armes epee = new Armes("Épée", 25, 15, 1.5, 1, "Une épée, solide et mortelle");
        private Armes[] tabEpee = new Armes[10];
        private Armes arme1, arme2;
        private int cdArme1, cdArme2, cdrArme1, cdrArme2;
        private double xfleche, yfleche, lfleche, hfleche;
        private ImageBrush skinFleche = new ImageBrush();

        public MainWindow()
        {
            InitializeComponent();
            Menu menu = new Menu();
            menu.ShowDialog();
            if (menu.DialogResult == false) Application.Current.Shutdown();
            Chargement();
            intervalle.Tick += MoteurJeu;
            intervalle.Interval = TimeSpan.FromMilliseconds(INTERVALLETICK);
            intervalle.Start();
            cdrArme1 = InitialisationVitesseAttaque(epee.VitesseAttaque);
            cdArme1 = cdrArme1;
            tabEpee = InitialisationArmes(epee);
        }

        private void Chargement()
        {
            ImageBrush brush1 = new ImageBrush();
            brush1.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "ressources/img/Game/FondMap.png"));
            Carte.Fill = brush1;
        }

        private void FenetrePrincipale_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                gauche = true;
                faceDroite = false;
                faceGauche = true;
            }
            else if (e.Key == Key.Right)
            {
                droite = true;
                faceGauche = false;
                faceDroite = true;
            }
            if (e.Key == Key.Up)
            {
                haut = true;
                faceBas = false;
                faceHaut = true;
            }
            else if (e.Key == Key.Down)
            {
                bas = true;
                faceHaut = false;
                faceBas = true;
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
                    Canvas.SetLeft(rect_Joueur, Canvas.GetLeft(rect_Joueur) - joueur.Vitesse/2);
                }  
            }
            else if (droite == true && Canvas.GetLeft(Carte) > -3600)
            {
                Canvas.SetLeft(Carte, Canvas.GetLeft(Carte) - joueur.Vitesse);
                if (Canvas.GetLeft(rect_Joueur) < (CENTREX + 50))
                {
                    Canvas.SetLeft(rect_Joueur, Canvas.GetLeft(rect_Joueur) + joueur.Vitesse/2);
                }
            }
            if (haut == true && Canvas.GetTop(Carte) < 0)
            {
                Canvas.SetTop(Carte, Canvas.GetTop(Carte) + joueur.Vitesse);
                if (Canvas.GetTop(rect_Joueur) > (CENTREY - 50))
                {
                    Canvas.SetTop(rect_Joueur, Canvas.GetTop(rect_Joueur) - joueur.Vitesse/2);
                }
            }
            else if (bas == true && Canvas.GetTop(Carte) > -2600)
            {
                Canvas.SetTop(Carte, Canvas.GetTop(Carte) - joueur.Vitesse);
                if (Canvas.GetTop(rect_Joueur) < (CENTREY + 50))
                {
                    Canvas.SetTop(rect_Joueur, Canvas.GetTop(rect_Joueur) + joueur.Vitesse/2);
                }
            }
            if (!gauche && !droite)
            {
                if (Canvas.GetLeft(rect_Joueur) > CENTREX)
                {
                    Canvas.SetLeft(rect_Joueur, Canvas.GetLeft(rect_Joueur) - joueur.Vitesse/2);
                    Canvas.SetLeft(Carte, Canvas.GetLeft(Carte) - joueur.Vitesse / 2);
                }
                else if (Canvas.GetLeft(rect_Joueur) < CENTREX)
                {
                    Canvas.SetLeft(rect_Joueur, Canvas.GetLeft(rect_Joueur) + joueur.Vitesse/2);
                    Canvas.SetLeft(Carte, Canvas.GetLeft(Carte) + joueur.Vitesse / 2);
                }
            }
            if (!haut && !bas)
            {
                if (Canvas.GetTop(rect_Joueur) > CENTREY)
                {
                    Canvas.SetTop(rect_Joueur, Canvas.GetTop(rect_Joueur) - joueur.Vitesse/2);
                    Canvas.SetTop(Carte, Canvas.GetTop(Carte) - joueur.Vitesse / 2);
                }
                else if (Canvas.GetTop(rect_Joueur) < CENTREY)
                {
                    Canvas.SetTop(rect_Joueur, Canvas.GetTop(rect_Joueur) + joueur.Vitesse/2);
                    Canvas.SetTop(Carte, Canvas.GetTop(Carte) + joueur.Vitesse / 2);
                }
            }
            //------------------------------------------------//
            //FLÊCHE//
            //------------------------------------------------//
            hfleche = PIXCARRE;
            lfleche = PIXCARRE;
            if (faceBas == true)
            {
                yfleche = Canvas.GetTop(rect_Joueur) + rect_Joueur.Height + joueur.Vitesse;
                if (faceGauche == true)
                {
                    skinFleche.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "ressources/img/Game/Fleche/flecheBG.png"));
                }
                else if(faceDroite == true) 
                {
                    skinFleche.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "ressources/img/Game/Fleche/flecheBD.png"));
                }
            }
            else if (faceHaut == true)
            {
                yfleche = Canvas.GetTop(rect_Joueur) + joueur.Vitesse;
            }
            else if (faceBas == false && faceHaut == false)
            {
                yfleche = Canvas.GetTop(rect_Joueur) + rect_Joueur.Height / 2;
            }
            if (faceDroite == true)
            {
                xfleche = Canvas.GetLeft(rect_Joueur) + rect_Joueur.Width + joueur.Vitesse;
            }
            else if (faceGauche == true)
            {
                xfleche = Canvas.GetLeft(rect_Joueur) + joueur.Vitesse;
            }
            else if (faceDroite == false && faceGauche == false)
            {
                xfleche = Canvas.GetLeft(rect_Joueur) + rect_Joueur.Width / 2;
            }
            Rect fleche = new Rect(xfleche, yfleche, lfleche, hfleche);

            //------------------------------------------------//
            //ATTAQUE//
            //------------------------------------------------//
            cdArme1 -= 1;
            if (cdArme1 <= 0)
            {
                Attaque(epee, Canvas.GetLeft(rect_Joueur), Canvas.GetTop(rect_Joueur));
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
            if (faceGauche == true)
            {
                xAtk = Canvas.GetLeft(rect_Joueur);
            }
            else
            {
                xAtk = Canvas.GetLeft(rect_Joueur) + rect_Joueur.Width;
            }
            if (faceHaut == true)
            {
                yAtk = Canvas.GetTop(rect_Joueur);
            }
            else if (faceBas == true)
            {
                yAtk = Canvas.GetTop(rect_Joueur) + rect_Joueur.Height;
            }
            else
            {
                yAtk = Canvas.GetTop(rect_Joueur) + rect_Joueur.Height / 2;
            }
            Rect attaque = new Rect(xAtk, yAtk, largeur, hauteur);
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
