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
        private bool gauche, droite, haut, bas = false;
        private List<Rectangle> objetsSuppr = new List<Rectangle>();
        private DispatcherTimer intervalle = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            intervalle.Tick += MoteurJeu;
            intervalle.Interval = TimeSpan.FromMilliseconds(16);
        }
        private void MoteurJeu(object sender, EventArgs e)
        {
            Rect joueur = new Rect(Canvas.GetLeft(rect_Joueur), Canvas.GetTop(rect_Joueur), rect_Joueur.Width, rect_Joueur.Height);
        }
    }
   
}
