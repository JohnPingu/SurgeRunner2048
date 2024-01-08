using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaelKiller
{
    internal class Monstres
    {
        private string nom;
        private int mutation;
        private double degats;
        private double pv;
        private double pvMax;
        private double vitesse;
        private string couleur;

        public Monstres(string nom, double degats, double pv, double pvMax, double vitesse, string couleur, int mutation)
        {
            Nom = nom;
            Degats = degats;
            Pv = pv;
            PvMax = pvMax;
            Vitesse = vitesse;
            Couleur = couleur;
            Mutation = mutation;
        }
        public Monstres(string nom, double degats, double pv, double vitesse, string couleur)
        {
            Nom = nom;
            Degats = degats;
            Pv = pv;
            Vitesse = vitesse;
            Couleur = couleur;
            Mutation = 0;
        }

        public string Nom
        {
            get { return nom; }
            set 
            {
                if (String.IsNullOrEmpty(value)) throw new ArgumentException("Le nom doit être renseigné");
                nom = value; 
            }
        }

        public int Mutation
        {
            get { return mutation; }
            set 
            { 
                if(value < 0) throw new ArgumentOutOfRangeException("Le niveau de mutation doit être supérieur ou égal à 0");
                mutation = value; 
            }
        }

        public double Degats
        {
            get { return degats; }
            set 
            {
                if (value <= 0) throw new ArgumentOutOfRangeException("Les dégats doit être supérieur à 0");
                degats = value; 
            }
        }

        public double Pv
        {
            get { return pv; }
            set 
            { 
                if (value <= 0) throw new ArgumentOutOfRangeException("Les points de vie doivent être supérieur à 0");
                pv = value; 
            }
        }
        public double PvMax
        {
            get { return pvMax; }
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException("Les points de vie max doivent être supérieur à 0");
                pv = value;
            }
        }

        public double Vitesse
        {
            get { return vitesse; }
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException("La vitesse doit être supérieur à 0");
                vitesse = value; 
            }
        }

        public string Couleur
        {
            get { return couleur; }
            set 
            {
                if (!(value == "bleu" || value == "rouge" || value == "noir")) throw new ArgumentException("La couleur doit être bleu, rouge ou noir");
                couleur = value; 
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is Monstres monstres &&
                   mutation == monstres.mutation
                   && couleur == monstres.couleur
                   && nom == monstres.nom;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Nom, Degats, Pv, PvMax, Vitesse, Couleur, Mutation);
        }

        public override string? ToString()
        {
            return "Nom : " + nom + "\nDegats : " + degats + "\nPoints de Vie : " + pv + "\nPoints de Vie Max : " + pvMax + "\nVitesse: " + vitesse + "\nCouleur : " + couleur + "\nMutation : " + mutation;
        }
    }
}
