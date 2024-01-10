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
        private bool gauche, droite, haut, bas, ruee, dispoRuee = false;
        private List<Rectangle> objetsSuppr = new List<Rectangle>();
        private DispatcherTimer intervalle = new DispatcherTimer();
        private int cdrRuee = 250, cdRuee;
        private Joueur joueur = new Joueur(25, 10, 30, 1);

        public MainWindow()
        {
            InitializeComponent();
            Menu menu = new Menu();
            menu.ShowDialog();
            if (menu.DialogResult == false) Application.Current.Shutdown();
            Chargement();
            intervalle.Tick += MoteurJeu;
            intervalle.Interval = TimeSpan.FromMilliseconds(16);
            intervalle.Start();
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
        }
    }
   
}
