using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaelKiller
{
    internal class Armes
    {
        private string nom;
        private double degats;
        private double portee;
        private double vitesseAttaque;
        private int niveau;

        public Armes(string nom, double degats, double portee, double vitesseAttaque, int niveau)
        {
            Nom = nom;
            Degats = degats;
            Portee = portee;
            VitesseAttaque = vitesseAttaque;
            Niveau = niveau;
        }

        public string Nom
        {
            get 
            { 
                return nom; 
            }
            set 
            { 
                this.nom = value; 
            }
        }
        public double Degats
        {
            get 
            { 
                return degats; 
            }
            set 
            { 
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Les dégats doivent être supérieurs à 0");
                }
                else
                    this.degats = value; 
            }
        }
        public double Portee
        {
            get 
            { 
                return portee; 
            }
            set 
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("La portée doit être supérieure à 0");
                }
                else
                    this.portee = value;
            }
        }
        public double VitesseAttaque
        {
            get 
            { 
                return vitesseAttaque; 
            }
            set 
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("La vitesse d'attaque doit être supérieure à 0");
                }
                else
                    this.vitesseAttaque = value;
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
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException("Le niveau doit être d'au moins 1");
                }
                else
                    this.niveau = value; 
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is Armes armes &&
                   Nom == armes.Nom &&
                   Niveau == armes.Niveau;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Nom, Degats, Portee, VitesseAttaque, Niveau);
        }

        public override string ToString()
        {
            return "Nom : " + nom + "\nDégats : " + degats + "\nPortée : " + portee + "\nVitesse d'attaque : " + vitesseAttaque + "\nNiveau : " + niveau;
        }
    }
}
