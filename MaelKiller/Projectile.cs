using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaelKiller
{
    internal class Projectile
    {
        private double vitesse;
        private double degats;
        private int index;
        private char directionBalleHB;
        private char directionBalleGD;
        public Projectile(double vitesse, double degtas)
        {
            Vitesse = vitesse;
            Degats = degtas;
            Index = 0;
        }

        public Projectile(double vitesse, double degtas, int index)
        {
            Vitesse = vitesse;
            Degats = degtas;
            Index = index;
        }

        public double Vitesse
        {
            get { return vitesse; }
            set { vitesse = value; }
        }

        public char DirectionBalleHB
        {
            get { return directionBalleHB; }
            set { directionBalleHB = value; }
        }

        public char DirectionBalleGD
        {
            get { return directionBalleGD; }
            set { directionBalleGD = value; }
        }

        public double Degats
        {
            get { return degats; }
            set { degats = value; }
        }

        public int Index
        {
            get { return index; }
            set { index = value; }
        }
        public override bool Equals(object? obj)
        {
            return obj is Projectile projectile &&
                   Vitesse == projectile.Vitesse &&
                   Degats == projectile.Degats;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Vitesse, Degats);
        }

        public override string? ToString()
        {
            return "Degats : " + Degats + "\nVitesse : " + Vitesse;
        }
    }
}
