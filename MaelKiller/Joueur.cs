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
        private int niveau;

        public Joueur(double pvMax, double vitesse, double porteeRuee, int niveau)
        {
            PvMax = pvMax;
            Vitesse = vitesse;
            PorteeRuee = porteeRuee;
            Niveau = niveau;
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
            return "Vie maximum : " + PvMax + "\nVitesse : " + vitesse + "\nNiveau : " + niveau;
        }
    }
}
