
using System;

namespace _2023_GC_A2_Partiel_POO.Level_2
{
    /// <summary>
    /// Définition des types dans le jeu
    /// </summary>
    public enum TYPE { NORMAL, WATER, FIRE, GRASS }

    public class TypeResolver
    {

        /// <summary>
        /// Récupère le facteur multiplicateur pour la résolution des résistances/faiblesses
        /// WATER faible contre GRASS, resiste contre FIRE
        /// FIRE faible contre WATER, resiste contre GRASS
        /// GRASS faible contre FIRE, resiste contre WATER
        /// </summary>
        /// <param name="attacker">Type de l'attaque (le skill)</param>
        /// <param name="receiver">Type de la cible</param>
        /// <returns>
        /// Normal returns 1 if attacker or receiver
        /// 0.8 if resist
        /// 1.0 if same type
        /// 1.2 if vulnerable
        /// </returns>
        public static float GetFactor(TYPE attacker, TYPE receiver)
        {
            float factor = 1.0f;

            switch(receiver)
            {
                case TYPE.NORMAL:
                    factor = 1.0f;
                    break;
                case TYPE.WATER:
                    switch (attacker)
                    {
                        case TYPE.FIRE:
                            factor = 0.8f;
                            break;
                        case TYPE.GRASS:
                            factor = 1.2f;
                            break;
                    }
                    break;
                case TYPE.FIRE:
                    switch (attacker)
                    {
                        case TYPE.WATER:
                            factor = 1.2f;
                            break;
                        case TYPE.GRASS:
                            factor = 0.8f;
                            break;
                    }
                    break;
                case TYPE.GRASS:
                    switch (attacker)
                    {
                        case TYPE.FIRE:
                            factor = 1.2f;
                            break;
                        case TYPE.WATER:
                            factor = 0.8f;
                            break;
                    }
                    break;
            }
            return factor;
        }
    }
}
