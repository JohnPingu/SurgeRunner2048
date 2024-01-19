using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaelKiller
{
    internal class Amélioration
    {
        private string type;
        private string nom;
        private string description;

        public Amélioration(string type, string nom, string description)
        {
            Type = type;
            Nom = nom;
            Description = description;
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
        public override bool Equals(object? obj)
        {
            return obj is Amélioration amélioration &&
                   Type == amélioration.Type &&
                   Nom == amélioration.Nom &&
                   Description == amélioration.Description;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type, Nom, Description);
        }
        public static bool IsNullOrEmpty([NotNullWhen(false)] Amélioration? value)
        {
            return (value == null || value.Nom == "") ? true : false;
        }
    }
}
