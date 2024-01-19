﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaelKiller
{
    internal class Secrets
    {
        private string nom;
        private Armes conditionArme;
        private Amélioration conditionAmelioration;
        private Supports conditionSupports;
        private string description;

        public Secrets(string nom, string description, Armes conditionArme, Supports conditionSupports)
        {
            Nom = nom;
            Description = description;
            ConditionArme = conditionArme;
            ConditionSupports = conditionSupports;
        }

        public Secrets(string nom, string description, Armes conditionArme, Amélioration conditionAmelioration)
        {
            Nom = nom;
            Description = description;
            ConditionArme = conditionArme;
            ConditionAmelioration = conditionAmelioration;
        }

        public Secrets(string nom, string description, Amélioration conditionAmelioration, Supports conditionSupports)
        {
            Nom = nom;
            Description = description;
            ConditionAmelioration = conditionAmelioration;
            ConditionSupports = conditionSupports;
        }

        public Secrets(string nom, string description, Armes conditionArme, Amélioration conditionAmelioration, Supports conditionSupports) : this(nom, description, conditionArme, conditionAmelioration)
        {
            ConditionSupports = conditionSupports;
        }

        public string Nom { get => nom; set => nom = value; }
        public string Description { get => description; set => description = value; }
        internal Armes ConditionArme { get => conditionArme; set => conditionArme = value; }
        internal Amélioration ConditionAmelioration { get => conditionAmelioration; set => conditionAmelioration = value; }
        internal Supports ConditionSupports { get => conditionSupports; set => conditionSupports = value; }
    }

}