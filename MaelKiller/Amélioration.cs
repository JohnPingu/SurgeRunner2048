using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaelKiller
{
    internal class Amélioration
    {
        private string type;
        private string nom;

        public Amélioration(string type, string nom)
        {
            Type = type;
            Nom = nom;
        }

        public string Type 
        {
            get
            {
               return type;
            }
            set
            {
                type = value;
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
                nom = value;
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is Amélioration amélioration &&
                   Type == amélioration.Type &&
                   Nom == amélioration.Nom;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type, Nom);
        }

    }
}
