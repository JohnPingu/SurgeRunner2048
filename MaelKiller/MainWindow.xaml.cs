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
        private const int CENTREX = 566;
        private const int CENTREY = 347;
        private const int INTERVALLETICK = 16;
        private bool gauche, droite, haut, bas, ruee, dispoRuee = false;
        private List<Rectangle> objetsSuppr = new List<Rectangle>();
        private DispatcherTimer intervalle = new DispatcherTimer();
        private int cdrRuee = 250, cdRuee;
        private Joueur joueur = new Joueur(25, 10, 30, 1);
        private Armes epeeNv1 = new Armes("Épée", 25, 15, 1.5, 1, "Une épée");
        private int cdArme1, cdArme2, cdrArme1, cdrArme2;

        public MainWindow()
        {
            InitializeComponent();
            Menu menu = new Menu();
            menu.ShowDialog();
            intervalle.Tick += MoteurJeu;
            intervalle.Interval = TimeSpan.FromMilliseconds(INTERVALLETICK);
            intervalle.Start();
            cdrArme1 = InitialisationVitesseAttaque(epeeNv1.VitesseAttaque);
            cdArme1 = cdrArme1;
        }
        private void FenetrePrincipale_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                gauche = true;
            }
            if (e.Key == Key.Right)
            {
                droite = true;
            }
            if (e.Key == Key.Up)
            {
                haut = true;
            }
            if (e.Key == Key.Down)
            {
                bas = true;
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
            Rect rectJoueur = new Rect(Canvas.GetLeft(rect_Joueur), Canvas.GetTop(rect_Joueur), rect_Joueur.Width, rect_Joueur.Height);
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
                    Canvas.SetLeft(rect_Joueur, Canvas.GetLeft(rect_Joueur) - joueur.Vitesse);
                }  
            }
            else if (droite == true && Canvas.GetLeft(Carte) > -3600)
            {
                Canvas.SetLeft(Carte, Canvas.GetLeft(Carte) - joueur.Vitesse);
                if (Canvas.GetLeft(rect_Joueur) < (CENTREX + 50))
                {
                    Canvas.SetLeft(rect_Joueur, Canvas.GetLeft(rect_Joueur) + joueur.Vitesse);
                }
            }
            if (haut == true && Canvas.GetTop(Carte) < 0)
            {
                Canvas.SetTop(Carte, Canvas.GetTop(Carte) + joueur.Vitesse);
                if (Canvas.GetTop(rect_Joueur) > (CENTREY - 50))
                {
                    Canvas.SetTop(rect_Joueur, Canvas.GetTop(rect_Joueur) - joueur.Vitesse);
                }
            }
            else if (bas == true && Canvas.GetTop(Carte) > -2600)
            {
                Canvas.SetTop(Carte, Canvas.GetTop(Carte) - joueur.Vitesse);
                if (Canvas.GetTop(rect_Joueur) < (CENTREY + 50))
                {
                    Canvas.SetTop(rect_Joueur, Canvas.GetTop(rect_Joueur) + joueur.Vitesse);
                }
            }
            if (!gauche && !droite)
            {
                if (Canvas.GetLeft(rect_Joueur) > CENTREX)
                {
                    Canvas.SetLeft(rect_Joueur, Canvas.GetLeft(rect_Joueur) - joueur.Vitesse/2);
                }
                else if (Canvas.GetLeft(rect_Joueur) < CENTREX)
                {
                    Canvas.SetLeft(rect_Joueur, Canvas.GetLeft(rect_Joueur) + joueur.Vitesse/2);
                }
            }
            if (!haut && !bas)
            {
                if (Canvas.GetTop(rect_Joueur) > CENTREY)
                {
                    Canvas.SetTop(rect_Joueur, Canvas.GetTop(rect_Joueur) - joueur.Vitesse/2);
                }
                else if (Canvas.GetTop(rect_Joueur) < CENTREY)
                {
                    Canvas.SetTop(rect_Joueur, Canvas.GetTop(rect_Joueur) + joueur.Vitesse/2);
                }
            }
            //------------------------------------------------//
            //ATTAQUE//
            //------------------------------------------------//
            cdArme1 -= 1;
            if (cdArme1 <= 0)
            {
                Attaque(epeeNv1, Canvas.GetLeft(rect_Joueur), Canvas.GetTop(rect_Joueur));
            }
        }
        private int InitialisationVitesseAttaque(double vitesseAttaque)
        {
            int cdrArme =(int)( 1000 / INTERVALLETICK / vitesseAttaque);
            return cdrArme;
        }
        private void Attaque(Armes arme, double xjoueur, double yjoueur)
        {

        }
    }
   
}
