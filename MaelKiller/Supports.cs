using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaelKiller
{
    internal class Supports
    {
		private string nom;
        private string multiplieur;
        private string description;

        public Supports(string nom, string multiplieur, string description)
        {
            Nom = nom;
            Multiplieur = multiplieur;
            description = description;
        }

        public string Nom
		{
			get { return nom; }
			set 
            {
                if (String.IsNullOrEmpty(value)) throw new ArgumentException("Le champ nom ne doit pas être vide");
                nom = value; 
            }
		}

        public string Multiplieur
        {
            get { return multiplieur; }
            set 
            {
                if (value != "vitesse" && value != "pv" && value != "regen" && value != "attraction") throw new ArgumentException("Le multiplieur doit être parmmis ces propositions: vitesse,pv,regen,attraction,");
                multiplieur = value; 
            }
        }
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
            }
        }

        public void ApplicationDesStats(Supports support, Joueur joueur)
        {
        
        }

        public override bool Equals(object? obj)
        {
            return obj is Supports supports &&
                   Nom == supports.Nom;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Nom, Multiplieur);
        }

        public override string? ToString()
        {
            return "Nom : " + nom + "\nMultiplieur : " + multiplieur + "\nDescription : " + description;
        }
    }
}
