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
        private const double EVODEGATS = 25, EVOVITESSEATTAQUE = 1.1;
        private const int PERSOIDLE = 4, PERSORUN = 6;
        private const int MONSTRERUN = 4;
        private const int PERSOHURT = 2;
        private const int FRAMEATK = 50;
        
        private List<Rectangle> listeMonstreRect = new List<Rectangle>();
        private List<Monstres> listMonstre = new List<Monstres>();

        private List<Rectangle> listeProjectileRect = new List<Rectangle>();
        private List<Projectile> listeProjectile = new List<Projectile>();

        private int projectileINDEX = 0;

        private bool gauche, droite, haut, bas, ruee, dispoRuee = false, estAttaquant = false;
        private bool niveauSupp = false;
        private bool estEnHaut = false, estEnBas = false, estAGauche = false, estADroite = false;
        private List<Rectangle> objetsSuppr = new List<Rectangle>();
        private DispatcherTimer intervalle = new DispatcherTimer();
        private int cdrRuee = 250, cdRuee, compteRuee = 0;
        private Joueur joueur = new Joueur(25, 4, 30, 1);
        private Joueur baseJoueur = new Joueur(25, 4, 30, 1);
        private Armes arme1, arme2;
        private Supports support1, support2;
        private Amélioration amélioration1, amélioration2;
        private Secrets secret;
        private int cdArme1, cdArme2, cdrArme1, cdrArme2;
        private double xfleche, yfleche, lfleche, hfleche;
        private ImageBrush skinFleche = new ImageBrush();
        private ImageBrush drone = new ImageBrush();
        private char[] directionFleche = new char[2];
        private char[] directionSkin = new char[2];
        private int numMonstre = 1;
        private ImageBrush skinPerso = new ImageBrush();
        private int skinFrameCompte = 0;
        private double xpPourNvSup;

        private bool CameraEstEnMouvement = false;
        private double CameraMouvement = 0;

        private Random random = new Random();
        private int bonusArmes, bonusAug, typeBonus, bonus;
        private int niveauArme1 = 0, niveauArme2 = 0, niveauSupport1 = 0, niveauSupport2 = 0;
        private int cdInvincibilite;
        private bool joueurTouche = false;

        //-----------------------------------//
        //ARMES//
        //-----------------------------------//
        private Armes epee = new Armes("Epee", 25, 100, 1.5, 100, 1, "Une épée classique, solide et mortelle", true);
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
        private Armes[] listeArmes = new Armes[8];
        private string couleurGlobal = "bleu";

        //-----------------------------------//
        //SUPPORTS//
        //-----------------------------------//
        private Supports jambes = new Supports("Jambes Bioniques", 1, "vitesse", "Vos jambes à la pointe de la technologie vous permettent de vous déplacer plus vite !");
        private Supports[] tabJambes = new Supports[10];
        private Supports exosquelette = new Supports("Exosquelette", 1, "pv", "Un exosquelette ultra-résistant réduit considérablement les risque de mort");
        private Supports[] tabExosquelette = new Supports[10];
        private Supports nanoMachine = new Supports("Nano-Machine", 1, "regen", "De nanoscopiques automates remplacent vos globules rouges pour une capacité de guérison maximale");
        private Supports[] tabNanoMachine = new Supports[10];
        private Supports coeurOr = new Supports("Coeur en or", 1, "attraction", "Votre coeur métallique augmente l'efficacité avec laquelle vous gagnez de l'expérience");
        private Supports[] tabCoeurOr = new Supports[10];
        private Supports forgeage = new Supports("Forgeage adamantin", 1, "degats", "Votre maîtrise de la forge adamantine vous offre des armes de qualité supérieure aux dégats soutenus");
        private Supports[] tabForgeage = new Supports[10];
        private Supports revetement = new Supports("Revêtement Tachyon", 1, "vitesseAtk", "Le revêtement tachyonique de vos armes leur font atteindre des vitesses inégalées");
        private Supports[] tabRevêtement = new Supports[10];
        private Supports[] listeSupports = new Supports[6];

        //-----------------------------------//
        //AMELIORATIONS//
        //-----------------------------------//
        private Amélioration deferlement = new Amélioration("Arme", "Déferlement", "Un champ d'ondes électriques vous entoure");
        private Amélioration moteur = new Amélioration("Support", "Moteur Quantique", "La portée de vitre ruée est grandement améliorée");
        private Amélioration carburant = new Amélioration("Support","Carburant Quantique", "Votre ruée est plus efficiente, vous pouvez l'effectuer plus souvent");
        private Amélioration[] listeAmélioration = new Amélioration[3];

        //-----------------------------------//
        //SECRETS//
        //-----------------------------------//
        private Secrets surgeRunner;
        private Secrets fouetAdamantin;
        private Secrets lanceTachyon;
        private Secrets paladin;
        private Secrets goldenGun;
        private Secrets mechaHuman;
        private Secrets[] listeSecrets = new Secrets[3];
       
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
            if(menu.cbChoixArme.SelectedItem.ToString() == "System.Windows.Controls.ComboBoxItem : Epee")
            {
                arme1 = epee;
            }
            else if (menu.cbChoixArme.SelectedItem.ToString() == "System.Windows.Controls.ComboBoxItem : Lance")
            {
                arme1 = lance;
            }
            else if (menu.cbChoixArme.SelectedItem.ToString() == "System.Windows.Controls.ComboBoxItem : Fouet")
            {
                arme1 = fouet;
            }
            else if (menu.cbChoixArme.SelectedItem.ToString() == "System.Windows.Controls.ComboBoxItem : Hache")
            {
                arme1 = hache;
            }
            else if (menu.cbChoixArme.SelectedItem.ToString() == "System.Windows.Controls.ComboBoxItem : Revolver")
            {
                arme1 = revolver;
            }
            else if (menu.cbChoixArme.SelectedItem.ToString() == "System.Windows.Controls.ComboBoxItem : Fusil de Sniper")
            {
                arme1 = fusilSnip;
            }
            else if (menu.cbChoixArme.SelectedItem.ToString() == "System.Windows.Controls.ComboBoxItem : Fusil d'Assaut")
            {
                arme1 = fusilAssaut;
            }
            else if (menu.cbChoixArme.SelectedItem.ToString() == "System.Windows.Controls.ComboBoxItem : Canon")
            {
                arme1 = canon;
            }
            cdrArme1 = InitialisationVitesseAttaque(arme1.VitesseAttaque);
            cdArme1 = cdrArme1;
            if (menu.DialogResult == false) Application.Current.Shutdown();
            ChargementJeu();
            InitialisationAmelioration();
            intervalle.Tick += MoteurJeu;
            intervalle.Interval = TimeSpan.FromMilliseconds(INTERVALLETICK);
            intervalle.Start();
            directionFleche[0] = 'N';
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

            xpPourNvSup = joueur.XpPourNiveauSuivant;
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
        private void MiseAJourBarHp()
        {
            barHp.Minimum = 0;
            barHp.Maximum = joueur.PvMax;
            barHp.Value = joueur.Pv;
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

        private void GameOver()
        {

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
            SkinMonstre();
            
            VerificationCollisionMonstreJoueur();

            MiseAJourBarHp();
            MiseAJourBarXp();
            vitesseCam = (int)Math.Round(joueur.Vitesse / 2);
            VerifPosition();
            if (niveauSupp == true)
            {
                MiseEnPause();
                NiveauSupérieur();
                
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
            else
            {
                if (ruee == true)
                {
                    joueur.Vitesse = joueur.PorteeRuee;
                    compteRuee++;
                    if (compteRuee == 3)
                    {
                        ruee = false;
                        joueur.Vitesse = baseJoueur.Vitesse;
                    }
                }
            }

            //------------------------------------------------//
            //DEPLACEMENT//
            //------------------------------------------------//
            Deplacements();
            cameraEstEnMouvement();
            DeplacementMonstre();
            DeplacementProjectiles();

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
                Attaque(arme1, Canvas.GetLeft(rect_Joueur), Canvas.GetTop(rect_Joueur));
                frameAtk.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "ressources/img/Game/Armes/" + arme1.Nom + "/" + arme1.Nom + "_" + directionAtk[0] + directionAtk[1] + checkFrame + ".png"));
                VerifCollisionAtk(arme1);
            }
                if (arme1.EstMelee == true)
                {
                    frameAtk.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "ressources/img/Game/Armes/" + arme1.Nom + "/" + arme1.Nom + "_" + directionAtk[0] + directionAtk[1] + checkFrame + ".png"));

                }
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
            if (!Armes.IsNullOrEmpty(arme2))
            {
                cdArme2 -= 1;
                if (cdArme2 == 0)
                {
                    estAttaquant = true;
                }
                if (cdArme2 <= 0)
                {
                    checkFrame = cdArme2;
                    Attaque(arme2, Canvas.GetLeft(rect_Joueur), Canvas.GetTop(rect_Joueur));
                    frameAtk.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "ressources/img/Game/Armes/" + arme2.Nom + "/" + arme2.Nom + "_" + directionAtk[0] + directionAtk[1] + checkFrame + ".png"));
                    VerifCollisionAtk(arme2);
                    
                }
                if (cdArme2 == -9)
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
                    cdArme2 = cdrArme2;
                }
            }
            //------------------------------------------------//
            //EFFETS SUPPORTS//
            //------------------------------------------------//
            if (support1 == jambes)
            {
                joueur.Vitesse = baseJoueur.Vitesse + support1.Niveau;
            }
            else if (support2 == jambes)
            {
                joueur.Vitesse = baseJoueur.Vitesse + support2.Niveau;
            }
            if (support1 == exosquelette)
            {
                joueur.PvMax = baseJoueur.PvMax + 5 * support1.Niveau;
            }
            else if (support2 == exosquelette)
            {
                joueur.PvMax = baseJoueur.PvMax + 5 * support2.Niveau;
            }
            if (support1 == nanoMachine)
            {
                if (joueur.Pv + support1.Niveau < joueur.PvMax)
                {
                    joueur.Pv += support1.Niveau;
                }
                else joueur.Pv = joueur.PvMax;
            }
            else if (support2 == nanoMachine)
            {
                if (joueur.Pv + support2.Niveau < joueur.PvMax)
                {
                    joueur.Pv += support2.Niveau;
                }
                else joueur.Pv = joueur.PvMax;
            }
            Console.WriteLine(arme1);
        }

        private void VerifCollisionAtk(Armes arme)
        {
            for (int i = 0;i < listeMonstreRect.Count; i++)
            {
                foreach (Rectangle atk in monCanvas.Children.OfType<Rectangle>())
                {
                    if (atk.Tag == "attaque")
                    {
                        int xMonstre = (int)Canvas.GetLeft(listeMonstreRect[i]);
                        int yMonstre = (int)Canvas.GetTop(listeMonstreRect[i]);
                        int xAtk = (int)Canvas.GetLeft(atk);
                        int yAtk = (int)Canvas.GetTop(atk);
                        Rect rectMonstre = new Rect(xMonstre, yMonstre, listeMonstreRect[i].Width, listeMonstreRect[i].Height);
                        Rect rectAtk = new Rect(xAtk, yAtk, atk.Width, atk.Height);
                        if (rectAtk.IntersectsWith(rectMonstre))
                        {
                            foreach (Monstres monstre in listMonstre)
                            {
                                if (monstre.DegatsPossible == false)
                                {
                                    monstre.CompteDegats--;
                                }
                                else monstre.CompteDegats = FRAMEATK;
                                if (monstre.Index -1 == i)
                                {
                                    if (monstre.DegatsPossible == true)
                                    {
                                        monstre.Pv -= arme.Degats;
                                        if (monstre.Pv <= 0)
                                        {
                                            objetsSuppr.Add(listeMonstreRect[i]);
                                            joueur.GainExperience(monstre.Degats);
                                            VerificationNiveauSupp();
                                            rectMonstre.Offset(-2000,-2000);
                                            monstre.Pv = 1000000000000;
                                        }
                                        monstre.DegatsPossible = false;
                                    }
                                }
                                if (monstre.CompteDegats <= 0)
                                {
                                    monstre.DegatsPossible = true;
                                }
                                Console.WriteLine(monstre.Pv);
                            }
                        }
                    }
                }
            }
            foreach (Rectangle y in objetsSuppr)
            {
                monCanvas.Children.Remove(y);
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
                    Projectile projectile = new Projectile(10, arme.Degats);
                    projectile.DirectionBalleHB = directionFleche[0];
                    projectile.DirectionBalleGD = directionFleche[1];

                    ImageBrush skinBalle = new ImageBrush();
                    skinBalle.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "ressources/img/Game/Fleche/flecheBG.png"));
                    Rectangle projectileRect = new Rectangle
                    {
                        Width = 50,
                        Height = 50,
                        Fill = skinBalle
                    };
                    Canvas.SetTop(projectileRect, Canvas.GetTop(rect_Joueur) + rect_Joueur.Height/2);
                    Canvas.SetLeft(projectileRect, Canvas.GetLeft(rect_Joueur) + rect_Joueur.Width/2);
                    projectile.Index = projectileINDEX;
                    projectileINDEX++;
                    listeProjectile.Add(projectile);
                    listeProjectileRect.Add(projectileRect);
                    monCanvas.Children.Add(projectileRect);
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
        private void InitialisationArmes(Armes[] tabArmes, Armes arme)
        {
            tabArmes[0] = arme;
            for (int i = 1; i<tabArmes.Length; i++)
            {
                tabArmes[i] = new Armes(tabArmes[i - 1].Nom, tabArmes[i - 1].Degats + EVODEGATS, tabArmes[i - 1].Portee, tabArmes[i - 1].VitesseAttaque * EVOVITESSEATTAQUE, tabArmes[i - 1].Taille, tabArmes[i - 1].Niveau +1, tabArmes[i - 1].Description, tabArmes[i - 1].EstMelee, tabArmes[i - 1].VitesseProjectile, tabArmes[i - 1].Amplitude);
            }
        }
        private void InitialisationSupports(Supports[] tabSupports,Supports supports) 
        {
            tabSupports[0] = supports;
            for (int i = 1;i < tabSupports.Length;i++) 
            {
                tabSupports[i] = new Supports(tabSupports[i - 1].Nom, tabSupports[i - 1].Niveau + 1, tabSupports[i - 1].Multiplieur, tabSupports[i - 1].Description);
            }
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
            foreach (Monstres monstre in listMonstre)
            {
                int index = monstre.Index - 1;
                int Xmonstre = (int)Canvas.GetLeft(listeMonstreRect[index]);
                int Ymonstre = (int)Canvas.GetTop(listeMonstreRect[index]);
                int Xjoueur = (int)Canvas.GetLeft(rect_Joueur);
                int Yjoueur = (int)Canvas.GetTop(rect_Joueur);
                Rect monstrerect = new Rect(Xmonstre, Ymonstre, listeMonstreRect[index].Width, listeMonstreRect[index].Height);
                Rect joueurrect = new Rect(Xjoueur, Yjoueur, rect_Joueur.Width, rect_Joueur.Height);
                if (monstrerect.IntersectsWith(joueurrect))
                {
                    if (joueur.PeutPrendreDegats == true)
                    {
                        joueur.PeutPrendreDegats = false;
                        joueur.Pv -= monstre.Degats;
                        Console.Write("Joueur perd des HP :" + joueur.Pv);
                        if (joueur.Pv <= 0)
                        {
                            GameOver();
                            Console.Write("GAMEOVER");
                        }
                        break;
                    }
                }
                decompteDegatsJoueur--;
                if (decompteDegatsJoueur <= 0)
                {
                    joueur.PeutPrendreDegats = true;
                    decompteDegatsJoueur = 175;
                }
            }
        }

        private void DeplacementProjectiles()
        {
            foreach (Projectile projectile in listeProjectile)
            {
                int index = projectile.Index;
                double vitesseX = projectile.Vitesse;
                double vitesseY = projectile.Vitesse;
                if (projectile.DirectionBalleHB == 'H')
                {
                    vitesseY = projectile.Vitesse;
                    if (projectile.DirectionBalleGD == 'G')
                    {
                        vitesseX = -projectile.Vitesse;
                    } 
                    else if (projectile.DirectionBalleGD == 'D')
                    {
                        vitesseX = projectile.Vitesse;
                    } else
                    {
                        vitesseX = 0;
                    }
                
                } 
                else if (projectile.DirectionBalleHB == 'B')
                {
                    vitesseY = -projectile.Vitesse;
                    if (projectile.DirectionBalleGD == 'G')
                    {
                        vitesseX = -projectile.Vitesse;
                    }
                    else if (projectile.DirectionBalleGD == 'D')
                    {
                        vitesseX = projectile.Vitesse;
                    }
                    else
                    {
                        vitesseX = 0;
                    }
                } else
                {
                    vitesseY = 0;
                }

                Canvas.SetLeft(listeProjectileRect[index], Canvas.GetTop(listeProjectileRect[index]) + vitesseX);
                Canvas.SetTop(listeProjectileRect[index], Canvas.GetTop(listeProjectileRect[index]) + vitesseY);
            }
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
            double nbHasard = random.Next(1, 4);
            switch (nbHasard) {
                case 1:
                    Monstres robotDrone1 = new Monstres("drone1", 5, 50, 1, "bleu", 20);
                    robotDrone1.Couleur = couleurGlobal;
                    ApparitionMonstre(robotDrone1);
                    break;
                case 2:
                    Monstres robotDrone2 = new Monstres("drone2", 5, 50, 1, "bleu", 20);
                    robotDrone2.Couleur = couleurGlobal;
                    ApparitionMonstre(robotDrone2);
                    break;
                case 3:
                    Monstres robotDrone3 = new Monstres("drone3", 5, 50, 1, "bleu", 20);
                    robotDrone3.Couleur = couleurGlobal;
                    ApparitionMonstre(robotDrone3);
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

            if (monstre.Nom == "drone1") 
            {
                nouveauMonstreRect.Height = 96;
                nouveauMonstreRect.Width = 96;
            } else if (monstre.Nom == "drone2")
            {
                nouveauMonstreRect.Height = 96;
                nouveauMonstreRect.Width = 96;
            } else if (monstre.Nom == "drone3")
            {
                nouveauMonstreRect.Height = 72;
                nouveauMonstreRect.Width = 72;
            }

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
            monstre.CompteDegats = FRAMEATK;
        }

        private void PlacerNouveauMonstre(Rectangle monstre)
        {
            Random random = new Random();
            int rdm = random.Next(1,2);
            int coorX = 0;
            int coorY = 0;

            rdm = random.Next(1,3);

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
                if (joueur.PeutPrendreDegats == true)
                {
                    skinPerso.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "ressources/img/Game/Personnage/Perso/PersoIdle_" + directionSkin[1] + "_" + (skinFrameCompte / INTERVALLETICK % PERSOIDLE) + ".png"));
                } else
                {
                    skinPerso.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "ressources/img/Game/Personnage/Perso/PersoHurt_" + directionSkin[1] + "_" + (skinFrameCompte / INTERVALLETICK % PERSOHURT) + ".png"));
                }
            }
            else
            {
                if (joueur.PeutPrendreDegats == true)
                {
                    skinPerso.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "ressources/img/Game/Personnage/Perso/PersoRun_" + directionSkin[1] + "_" + (skinFrameCompte / 10 % PERSORUN) + ".png"));
                } else
                {
                    skinPerso.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "ressources/img/Game/Personnage/Perso/PersoHurt_" + directionSkin[1] + "_" + (skinFrameCompte / INTERVALLETICK % PERSOHURT) + ".png"));
                }
            }
        }

        private void SkinMonstre()
        {
            foreach(Monstres monstre in listMonstre)
            {
                ImageBrush skinrobot = new ImageBrush();
                int index = monstre.Index - 1;
                int nombreSkin = 0;
                if (monstre.Nom == "drone1")
                {
                    nombreSkin = 1;
                } else if (monstre.Nom == "drone2")
                {
                    nombreSkin = 2;
                } else if (monstre.Nom == "drone3")
                {
                    nombreSkin = 3;
                }
                if (Canvas.GetLeft(listeMonstreRect[index]) >= Canvas.GetLeft(rect_Joueur))
                {
                    skinrobot.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "ressources/img/Game/monstre/drone" + nombreSkin +"/walk/walk_g_" + ((skinFrameCompte / INTERVALLETICK % MONSTRERUN) + 1) + ".png"));
                } else
                {
                    skinrobot.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "ressources/img/Game/monstre/drone" + nombreSkin +"/walk/walk_d_" + ((skinFrameCompte / INTERVALLETICK % MONSTRERUN) + 1) + ".png"));
                }
                listeMonstreRect[index].Fill = skinrobot;
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
            //---------------------------------------------------//
            //ASSIGNATION DE L'ARME//
            //---------------------------------------------------//
            if (Armes.IsNullOrEmpty(arme1))
            {
                arme1 = new Armes(listeArmes[bonusArmes].Nom, listeArmes[bonusArmes].Degats, listeArmes[bonusArmes].Portee, listeArmes[bonusArmes].VitesseAttaque, listeArmes[bonusArmes].Taille, listeArmes[bonusArmes].Niveau, listeArmes[bonusArmes].Description, listeArmes[bonusArmes].EstMelee, listeArmes[bonusArmes].VitesseProjectile, listeArmes[bonusArmes].Amplitude);
                listeArmes[bonusArmes].Niveau++;
            }
            else if (!Armes.IsNullOrEmpty(arme1) && Armes.IsNullOrEmpty(arme2)) 
            {
                arme2 = new Armes(listeArmes[bonusArmes].Nom, listeArmes[bonusArmes].Degats, listeArmes[bonusArmes].Portee, listeArmes[bonusArmes].VitesseAttaque, listeArmes[bonusArmes].Taille, listeArmes[bonusArmes].Niveau, listeArmes[bonusArmes].Description, listeArmes[bonusArmes].EstMelee, listeArmes[bonusArmes].VitesseProjectile, listeArmes[bonusArmes].Amplitude);
                listeArmes[bonusArmes].Niveau++;
            }
            else if (!Armes.IsNullOrEmpty(arme1) && !Armes.IsNullOrEmpty(arme2))
                if (bonusArmes == 1) 
                {
                    for (int i = 0; i<listeArmes.Length; i++)
                    {
                        if (arme1.Nom == listeArmes[i].Nom)
                        {
                            arme1 = new Armes(listeArmes[i].Nom, listeArmes[i].Degats, listeArmes[i].Portee, listeArmes[i].VitesseAttaque, listeArmes[i].Taille, listeArmes[i].Niveau, listeArmes[i].Description, listeArmes[i].EstMelee, listeArmes[i].VitesseProjectile, listeArmes[i].Amplitude);
                            listeArmes[i].Niveau++;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < listeArmes.Length; i++)
                    {
                        if (arme2.Nom == listeArmes[i].Nom)
                        {
                            arme2 = new Armes(listeArmes[i].Nom, listeArmes[i].Degats, listeArmes[i].Portee, listeArmes[i].VitesseAttaque, listeArmes[i].Taille, listeArmes[i].Niveau, listeArmes[i].Description, listeArmes[i].EstMelee, listeArmes[i].VitesseProjectile, listeArmes[i].Amplitude);
                            listeArmes[i].Niveau++;
                        }
                    }
                }
            cdrArme1 = InitialisationVitesseAttaque(arme1.VitesseAttaque);
            cdArme1 = cdrArme1;
            if (!Armes.IsNullOrEmpty(arme2))
            {
                cdrArme2 = InitialisationVitesseAttaque(arme2.VitesseAttaque);
                cdArme2 = cdrArme2;
            }
            //---------------------------------------------------//
            //DISPARITION DU MENU//
            //---------------------------------------------------//
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
            //---------------------------------------------------//
            //ASSIGNATION DE L'ARME//
            //---------------------------------------------------//
            if (Armes.IsNullOrEmpty(arme1))
            {
                arme1 = new Armes(listeArmes[bonusAug].Nom, listeArmes[bonusAug].Degats, listeArmes[bonusAug].Portee, listeArmes[bonusAug].VitesseAttaque, listeArmes[bonusAug].Taille, listeArmes[bonusAug].Niveau, listeArmes[bonusAug].Description, listeArmes[bonusAug].EstMelee, listeArmes[bonusAug].VitesseProjectile, listeArmes[bonusAug].Amplitude);
                listeArmes[bonusAug].Niveau++;
            }
            cdrArme1 = InitialisationVitesseAttaque(arme1.VitesseAttaque);
            cdArme1 = cdrArme1;
            if (!Armes.IsNullOrEmpty(arme2))
            {
                cdrArme2 = InitialisationVitesseAttaque(arme2.VitesseAttaque);
                cdArme2 = cdrArme2;
            }
            //---------------------------------------------------//
            //DISPARITION DU MENU//
            //---------------------------------------------------//
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
            //---------------------------------------------------//
            //ASSIGNATION DE L'ARME//
            //---------------------------------------------------//
            if (Armes.IsNullOrEmpty(arme1))
            {
                arme1 = new Armes(listeArmes[bonus].Nom, listeArmes[bonus].Degats, listeArmes[bonus].Portee, listeArmes[bonus].VitesseAttaque, listeArmes[bonus].Taille, listeArmes[bonus].Niveau, listeArmes[bonus].Description, listeArmes[bonus].EstMelee, listeArmes[bonus].VitesseProjectile, listeArmes[bonus].Amplitude);
                listeArmes[bonus].Niveau++;
            }
            cdrArme1 = InitialisationVitesseAttaque(arme1.VitesseAttaque);
            cdArme1 = cdrArme1;
            if (!Armes.IsNullOrEmpty(arme2))
            {
                cdrArme2 = InitialisationVitesseAttaque(arme2.VitesseAttaque);
                cdArme2 = cdrArme2;
            }
            
            //---------------------------------------------------//
            //DISPARITION DU MENU//
            //---------------------------------------------------//
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
            surgeRunner = new Secrets("Surge Runner", "Le jeu n'a plus de secrets pour vous", epee, deferlement, jambes);
            fouetAdamantin = new Secrets("Orage Adamantin", "Votre fouet dépasse la solidité des matériaux connus et inflige des dégâts irréparables", fouet, forgeage);
            lanceTachyon = new Secrets("Frappes Tachyon", "Votre vitesse d'exécution surpasse même la lumière", lance, revetement);
            paladin = new Secrets("Paladin de l'annihilation", "Votre esprit a été corrompu par la puissance et vous ne cherchez plus que la destruction de toute chose", hache, exosquelette);
            goldenGun = new Secrets("Desert Eagle Doré mk.XII", "Tout bon jeu en a un, alors pourquoi ne pas le faire tirer dans toutes les directions ?", revolver, coeurOr);
            mechaHuman = new Secrets("Humain Augmenté Ultime", "Vos nano-Machines ont atteint la motion perpétuelle. Existe-t-il encore quelque-chose capable de vous tuer ?", carburant, nanoMachine);
            InitialisationArmes(tabEpee, epee); 
            InitialisationArmes(tabLance , lance);
            InitialisationArmes(tabFouet, fouet);
            InitialisationArmes(tabHache, hache);
            InitialisationArmes(tabRevolver, revolver);
            InitialisationArmes(tabFusilSnip, fusilSnip);
            InitialisationArmes(tabFusilAssaut, fusilAssaut);
            InitialisationArmes(tabCanon, canon);
            InitialisationSupports(tabJambes, jambes);
            InitialisationSupports(tabExosquelette, exosquelette);
            InitialisationSupports(tabNanoMachine, nanoMachine);
            InitialisationSupports(tabCoeurOr, coeurOr);
            InitialisationSupports(tabForgeage, forgeage);
            InitialisationSupports(tabRevêtement, revetement);
            listeArmes[0] = new Armes(tabEpee[1].Nom, tabEpee[1].Degats, tabEpee[1].Portee, tabEpee[1].VitesseAttaque, tabEpee[1].Taille, tabEpee[1].Niveau, tabEpee[1].Description, tabEpee[1].EstMelee, tabEpee[1].VitesseProjectile, tabEpee[1].Amplitude);
            listeArmes[1] = tabLance[1];
            listeArmes[2] = tabFouet[1];
            listeArmes[3] = tabHache[1];
            listeArmes[4] = tabRevolver[1];
            listeArmes[5] = tabFusilSnip[1];
            listeArmes[6] = tabFusilAssaut[1];
            listeArmes[7] = tabCanon[1];
            listeSupports[0] = tabJambes[1];
            listeSupports[1] = tabExosquelette[1];
            listeSupports[2] = tabNanoMachine[1];
            listeSupports[3] = tabCoeurOr[1];
            listeSupports[4] = tabForgeage[1];
            listeSupports[5] = tabRevêtement[1];
            listeAmélioration[0] = deferlement;
            listeAmélioration[1] = moteur;
            listeAmélioration[2] = carburant;
        }
        public void NiveauSupérieur()
        {
            listeArmes[0] = new Armes (tabEpee[listeArmes[0].Niveau].Nom, tabEpee[listeArmes[0].Niveau].Degats, tabEpee[listeArmes[0].Niveau].Portee, tabEpee[listeArmes[0].Niveau].VitesseAttaque, tabEpee[listeArmes[0].Niveau].Taille, tabEpee[listeArmes[0].Niveau].Niveau, tabEpee[listeArmes[0].Niveau].Description, tabEpee[listeArmes[0].Niveau].EstMelee, tabEpee[listeArmes[0].Niveau].VitesseProjectile, tabEpee[listeArmes[0].Niveau].Amplitude);
            listeArmes[1] = new Armes (tabLance[listeArmes[1].Niveau].Nom, tabLance[listeArmes[1].Niveau].Degats, tabLance[listeArmes[1].Niveau].Portee, tabLance[listeArmes[1].Niveau].VitesseAttaque, tabLance[listeArmes[1].Niveau].Taille, tabLance[listeArmes[1].Niveau].Niveau, tabLance[listeArmes[1].Niveau].Description, tabLance[listeArmes[1].Niveau].EstMelee, tabLance[listeArmes[1].Niveau].VitesseProjectile, tabLance[listeArmes[1].Niveau].Amplitude);
            listeArmes[2] = new Armes (tabFouet[listeArmes[2].Niveau].Nom, tabFouet[listeArmes[2].Niveau].Degats, tabFouet[listeArmes[2].Niveau].Portee, tabFouet[listeArmes[2].Niveau].VitesseAttaque, tabFouet[listeArmes[2].Niveau].Taille, tabFouet[listeArmes[2].Niveau].Niveau, tabFouet[listeArmes[2].Niveau].Description, tabFouet[listeArmes[2].Niveau].EstMelee, tabFouet[listeArmes[2].Niveau].VitesseProjectile, tabFouet[listeArmes[2].Niveau].Amplitude);
            listeArmes[3] = new Armes (tabHache[listeArmes[3].Niveau].Nom, tabHache[listeArmes[3].Niveau].Degats, tabHache[listeArmes[3].Niveau].Portee, tabHache[listeArmes[3].Niveau].VitesseAttaque, tabHache[listeArmes[3].Niveau].Taille, tabHache[listeArmes[3].Niveau].Niveau, tabHache[listeArmes[3].Niveau].Description, tabHache[listeArmes[3].Niveau].EstMelee, tabHache[listeArmes[3].Niveau].VitesseProjectile, tabHache[listeArmes[3].Niveau].Amplitude);
            listeArmes[4] = new Armes (tabRevolver[listeArmes[4].Niveau].Nom, tabRevolver[listeArmes[4].Niveau].Degats, tabRevolver[listeArmes[4].Niveau].Portee, tabRevolver[listeArmes[4].Niveau].VitesseAttaque, tabRevolver[listeArmes[4].Niveau].Taille, tabRevolver[listeArmes[4].Niveau].Niveau, tabRevolver[listeArmes[4].Niveau].Description, tabRevolver[listeArmes[4].Niveau].EstMelee, tabRevolver[listeArmes[4].Niveau].VitesseProjectile, tabRevolver[listeArmes[4].Niveau].Amplitude);
            listeArmes[5] = new Armes (tabFusilSnip[listeArmes[5].Niveau].Nom, tabFusilSnip[listeArmes[5].Niveau].Degats, tabFusilSnip[listeArmes[5].Niveau].Portee, tabFusilSnip[listeArmes[5].Niveau].VitesseAttaque, tabFusilSnip[listeArmes[5].Niveau].Taille, tabFusilSnip[listeArmes[5].Niveau].Niveau, tabFusilSnip[listeArmes[5].Niveau].Description, tabFusilSnip[listeArmes[5].Niveau].EstMelee, tabFusilSnip[listeArmes[5].Niveau].VitesseProjectile, tabFusilSnip[listeArmes[5].Niveau].Amplitude);
            listeArmes[6] = new Armes (tabFusilAssaut[listeArmes[6].Niveau].Nom, tabFusilAssaut[listeArmes[6].Niveau].Degats, tabFusilAssaut[listeArmes[6].Niveau].Portee, tabFusilAssaut[listeArmes[6].Niveau].VitesseAttaque, tabFusilAssaut[listeArmes[6].Niveau].Taille, tabFusilAssaut[listeArmes[6].Niveau].Niveau, tabFusilAssaut[listeArmes[6].Niveau].Description, tabFusilAssaut[listeArmes[6].Niveau].EstMelee, tabFusilAssaut[listeArmes[6].Niveau].VitesseProjectile, tabFusilAssaut[listeArmes[6].Niveau].Amplitude);
            listeArmes[7] = new Armes (tabCanon[listeArmes[7].Niveau].Nom, tabCanon[listeArmes[7].Niveau].Degats, tabCanon[listeArmes[7].Niveau].Portee, tabCanon[listeArmes[7].Niveau].VitesseAttaque, tabCanon[listeArmes[7].Niveau].Taille, tabCanon[listeArmes[7].Niveau].Niveau, tabCanon[listeArmes[7].Niveau].Description, tabCanon[listeArmes[7].Niveau].EstMelee, tabCanon[listeArmes[7].Niveau].VitesseProjectile, tabCanon[listeArmes[7].Niveau].Amplitude);
            listeSupports[0] = tabJambes[0];
            listeSupports[1] = tabExosquelette[0];
            listeSupports[2] = tabNanoMachine[0];
            listeSupports[3] = tabCoeurOr[0];
            listeSupports[4] = tabForgeage[0];
            listeSupports[5] = tabRevêtement[0];
            //----------------------------------//
            //ROLL AMELIORATIONS//
            //----------------------------------//

            if (Amélioration.IsNullOrEmpty(amélioration1))
            {
                bonus = random.Next(0, 2);
                TitreBonus3.Text = listeAmélioration[bonus].Nom;
            }
            else if (!Amélioration.IsNullOrEmpty(amélioration1) && Amélioration.IsNullOrEmpty(amélioration2))
            {
                do
                {
                    bonus = random.Next(0, 2);
                } while (listeAmélioration[bonus] == amélioration1);
                TitreBonus3.Text = listeAmélioration[bonus].Nom;
            }
            //----------------------------------//
            //ROLL SUPPORTS//
            //----------------------------------//
            if (Supports.IsNullOrEmpty(support1))
            {
                bonusAug = random.Next(0, 5);
                do
                {
                    bonusArmes = random.Next(0, 5);
                } while (bonusArmes == bonusAug);
                do
                {
                    bonus = random.Next(0, 5);
                } while(bonus == bonusAug || bonus == bonusArmes);
                TitreBonus1.Text = listeSupports[bonusArmes].Nom;
                TitreBonus2.Text = listeSupports[bonusAug].Nom;
                TitreBonus3.Text = listeSupports[bonus].Nom;
            }
            else if (!Supports.IsNullOrEmpty(support1) && Supports.IsNullOrEmpty(support2)) 
            {
                bonusAug = random.Next(0, 5);
                TitreBonus2.Text = listeSupports[bonusAug].Nom;
            }
            else if (!Supports.IsNullOrEmpty(support1) && !Supports.IsNullOrEmpty(support2))
            {
                bonusAug = random.Next(1, 2);
                if (bonusAug == 1)
                {
                    TitreBonus2.Text = support1.Nom;
                }
                else TitreBonus2.Text = support2.Nom;
            }
            else if (support1.Niveau == 10)
            {
                bonusAug = 2;
                TitreBonus2.Text = support2.Nom;
            }
            else if (support2.Niveau == 10)
            {
                bonusAug = 1;
                TitreBonus2.Text = support1.Nom;
            }
            //----------------------------------//
            //ROLL ARMES//
            //----------------------------------//
            if (Armes.IsNullOrEmpty(arme1))
            {
                bonusArmes = random.Next(0, 7);
                do
                {
                    bonusAug = random.Next(0, 7);
                } while (bonusAug == bonusArmes);
                do
                {
                    bonus = random.Next(0, 7);
                } while (bonus == bonusArmes || bonus == bonusAug);
                TitreBonus1.Text = listeArmes[bonusArmes].Nom;
                TitreBonus2.Text = listeArmes[bonusAug].Nom;
                TitreBonus3.Text = listeArmes[bonus].Nom;
            }
            else if (!Armes.IsNullOrEmpty(arme1) && Armes.IsNullOrEmpty(arme2)) 
            {
                bonusArmes = random.Next(0, 7);
                TitreBonus1.Text = listeArmes[bonusArmes].Nom;
            }
            else if (!Armes.IsNullOrEmpty(arme1) && !Armes.IsNullOrEmpty(arme2))
            {
                bonusArmes = random.Next(1, 2);
                if (bonusArmes == 1)
                {
                    TitreBonus1.Text = arme1.Nom;
                }
                else TitreBonus1.Text = arme2.Nom;
            }
            else if (arme1.Niveau == 10)
            {
                bonusArmes = 2;
                TitreBonus1.Text = arme2.Nom;
            }
            else if (arme2.Niveau == 10)
            {
                bonusArmes = 1;
                TitreBonus1.Text = arme1.Nom;
            }
            //----------------------------------//
            //ROLL SECRETS//
            //----------------------------------//
            if (listeArmes[0].Niveau == 10 && listeSupports[0].Niveau == 10 && (amélioration1 == deferlement || amélioration2 == deferlement) && Secrets.IsNullOrEmpty(secret)) 
            {
                TitreBonus3.Text = surgeRunner.Nom;
            }
            if (listeArmes[2].Niveau == 10 && listeSupports[4].Niveau == 10 && Secrets.IsNullOrEmpty(secret))
            {
                TitreBonus3.Text = fouetAdamantin.Nom;
            }
            if (listeArmes[1].Niveau == 10 && listeSupports[5].Niveau == 10 && Secrets.IsNullOrEmpty(secret))
            {
                TitreBonus3.Text = lanceTachyon.Nom;
            }
            if (listeArmes[3].Niveau == 10 && listeSupports[1].Niveau == 10 && Secrets.IsNullOrEmpty(secret))
            {
                TitreBonus3.Text = paladin.Nom;
            }
            if (listeArmes[4].Niveau == 10 && listeSupports[3].Niveau == 10 && Secrets.IsNullOrEmpty(secret))
            {
                TitreBonus3.Text = goldenGun.Nom;
            }
            if (listeSupports[2].Niveau == 10 && (amélioration1 == carburant || amélioration2 == carburant) && Secrets.IsNullOrEmpty(secret))
            {
                TitreBonus3.Text = mechaHuman.Nom;
            }
        }
        private void VerificationNiveauSupp()
        {
            if (joueur.XpPourNiveauSuivant != xpPourNvSup)
            {
                NiveauSupérieur();
                niveauSupp = true;
                MiseEnPause();
                niveauSupp = false;
                xpPourNvSup = joueur.XpPourNiveauSuivant;
            }
        }
    }
}
