using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaelKiller
{
    internal class Joueur
    {
        private double pv;
        private double pvMax;
        private double vitesse;
        private double porteeRuee;
        private double xp;
        private double xpPourNiveauSuivant;
        private int niveau;
        private bool estMort;
        private const double CONSTANTE_MULTIPLIEUR_NIVEAU = 1.5;

        public Joueur(double pvMax, double vitesse, double porteeRuee, int niveau)
        {
            PvMax = pvMax;
            Vitesse = vitesse;
            PorteeRuee = porteeRuee;
            Niveau = niveau;
            Pv = pvMax;
            Xp = xp;
            XpPourNiveauSuivant = 100;
            estMort = false;
        }
        public double Pv
        {
            get
            {
                return pv;
            }
            set
            {
                pv = value;
            }
        }
        public double PvMax
        {
            get
            {
                return pvMax;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Les points de vie maximums doivent être supérieurs à 0");
                }
                pvMax = value;
            }
        }
        public double Xp
        {
            get
            {
                return xp;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("L'experience doit être égal ou supérieur à 0");
                }
                xp = value;
            }
        }

        public double XpPourNiveauSuivant
        {
            get
            {
                return xpPourNiveauSuivant;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("L'xp pour le niveau suivant doit être supérieur à 0");
                }
                xpPourNiveauSuivant = value;
            }
        }
        public double Vitesse
        {
            get
            {
                return vitesse;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("La vitesse doit être supérieure à 0");
                }
                vitesse = value;
            }
        }
        public double PorteeRuee
        {
            get
            {
                return porteeRuee;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("La portée de la ruée doit être supérieure à 0");
                }
                porteeRuee = value;
            }
        }

        public bool EstMort
        {
            get
            {
                return EstMort;
            }
            set
            {
                estMort = value;
            }
        }
        public int Niveau
        {
            get
            {
                return niveau;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Le niveau doit être supérieur à 0");
                }
                niveau = value;
            }
        }

        private void GainExperience(double xp)
        {
            double res;
            res = this.Xp + xp;
            if (res >= this.XpPourNiveauSuivant)
            {
                this.XpPourNiveauSuivant = this.XpPourNiveauSuivant * CONSTANTE_MULTIPLIEUR_NIVEAU;
                this.Xp = 0;
#if DEBUG
                Console.WriteLine("xp pour niveau suivant: " + XpPourNiveauSuivant);
#endif
            } else
            {
                this.Xp += res;
            }

        }

        private void PrendreDegats(double degats)
        {
            double res;
            res = this.Pv - degats;
            if (res <= 0)
            {
                this.EstMort = true;
            } else
            {
                this.Pv = res;
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is Joueur joueur &&
                   Pv == joueur.Pv &&
                   PvMax == joueur.PvMax &&
                   Vitesse == joueur.Vitesse &&
                   PorteeRuee == joueur.PorteeRuee &&
                   Niveau == joueur.Niveau;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Pv, PvMax, Vitesse, Niveau);
        }

        public override string? ToString()
        {
            return "Vie maximum : " + PvMax + "\nVitesse : " + Vitesse + "\nNiveau : " + Niveau + "\nXp : " + Xp + "\nXp pour niveau suivant : " + XpPourNiveauSuivant;
        }
    }
}
