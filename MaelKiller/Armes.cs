using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Printing.IndexedProperties;
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
        private string description;
        private bool estMelee;
        private double taille;
        private double vitesseProjectile;
        private double amplitude;

        public Armes(string nom, double degats, double portee, double vitesseAttaque, double taille, int niveau, string description, bool estMelee)
        {
            Nom = nom;
            Degats = degats;
            Portee = portee;
            VitesseAttaque = vitesseAttaque;
            Niveau = niveau;
            Description = description;
            EstMelee = estMelee;
            Taille = taille;
            VitesseProjectile = 0;
            Amplitude = 0;
        }
        public Armes(string nom, double degats, double portee, double vitesseAttaque, double taille, int niveau, string description, bool estMelee, double vitesseProjectile)
        {
            Nom = nom;
            Degats = degats;
            Portee = portee;
            VitesseAttaque = vitesseAttaque;
            Niveau = niveau;
            Description = description;
            EstMelee = estMelee;
            Taille = taille;
            VitesseProjectile = vitesseProjectile;
            Amplitude = 0;
        }
        public Armes(string nom, double degats, double portee, double amplitude, double vitesseAttaque, double taille, int niveau, string description, bool estMelee)
        {
            Nom = nom;
            Degats = degats;
            Portee = portee;
            Amplitude = amplitude;
            VitesseAttaque = vitesseAttaque;
            Niveau = niveau;
            Description = description;
            EstMelee = estMelee;
            Taille = taille;
            VitesseProjectile = 0;
        }
        public Armes(string nom, double degats, double portee, double vitesseAttaque, double taille, int niveau, string description, bool estMelee, double vitesseProjectile, double amplitude)
        {
            Nom = nom;
            Degats = degats;
            Portee = portee;
            VitesseAttaque = vitesseAttaque;
            Niveau = niveau;
            Description = description;
            EstMelee = estMelee;
            Taille = taille;
            VitesseProjectile = vitesseProjectile;
            Amplitude = amplitude;
        }
        public bool EstMelee
        {
            get
            {
                return estMelee;
            }
            set
            {
                this.estMelee = value;
            }
        }
        public string Nom
        {
            get 
            { 
                return nom; 
            }
            set 
            {
                if (String.IsNullOrEmpty(value)) throw new ArgumentException("Le nom doit être renseigné");
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

        public double VitesseProjectile
        {
            get
            {
                return vitesseProjectile;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("La vitesse d'attaque doit être supérieure à 0");
                }
                else
                    this.vitesseProjectile = value;
            }
        }

        public double Taille
        {
            get
            {
                return taille;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("La taille doit être supérieure à 0");
                }
                else
                    this.taille = value;
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
                    throw new ArgumentOutOfRangeException("Le niveau doit être d'au moins 0");
                }
                else
                    this.niveau = value; 
            }
        }
        public string Description
        {
            get
            {
                return description;
            }
            set 
            { 
                description = value; 
            }
        }
        public double Amplitude
        {
            get { return amplitude; }
            set
            {
                if (double.IsNaN(value))
                {
                    throw new ArgumentOutOfRangeException("Veuillez rentrer un chiffre");
                }
                amplitude = value;
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
            return HashCode.Combine(Nom, Degats, Portee, VitesseAttaque, Taille, Niveau, Description, VitesseProjectile);
        }

        public override string ToString()
        {
            return "Nom : " + Nom + "\nDégats : " + Degats + "\nPortée : " + Portee + "\nVitesse d'attaque : " + VitesseAttaque + "\nVitesse projectile : " + VitesseProjectile + "\nEst melée? : " + EstMelee + "\nNiveau : " + Niveau + "\n" + Description;
        }
        public static bool IsNullOrEmpty([NotNullWhen(false)] Armes? value)
        {
            return (value == null || value.Nom == "") ? true : false;
        }
    }
}
