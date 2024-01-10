using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MaelKiller
{
    /// <summary>
    /// Logique d'interaction pour Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        public bool extremeDiff = false;
        public Menu()
        {
            InitializeComponent();
            ImageBrush brush1 = new ImageBrush();
            brush1.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "ressources/img/Menu/MENU.png"));
            rectFond.Fill = brush1;
            ImageBrush brush2 = new ImageBrush();
            brush2.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "ressources/img/Menu/boutonPlay.png"));
            butPlay.Background = brush2;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            if (cbiDiffExtreme.IsSelected = true)
            {
                extremeDiff = true; 
            }
            this.Close();
        }
                
    }
}
