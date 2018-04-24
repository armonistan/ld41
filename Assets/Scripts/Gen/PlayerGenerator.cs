﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAbilities;
using SCG.General;

public class PlayerGenerator : MonoBehaviour {

    static string[] firstNames =
    {
        "TOMMY",
        "JIM",
        "JERRY",
        "RAY",
        "KEN",
        "HAROLD",
        "BUDDY",
        "CHUCK",
        "FRANK",
        "GEORGE",
        "HARDY",
        "DON",
        "ODELL",
        "BUCK",
        "DICK",
        "WILLARD",
        "TONY",
        "LEON",
        "ELDON",
        "SPEEDY"
    };

    static string[] lastNames =
    {
        "WISEAU",
        "HALPERT",
        "SEINFELD",
        "CARR",
        "BARRY",
        "DANENHAUER",
        "ABRUZZESE",
        "BROWN",
        "D'AMATO",
        "FERGUSON",
        "GUY",
        "GLICK",
        "HAMMOND",
        "JONES",
        "KELLOGG",
        "LAROSE",
        "MCNAMARA",
        "NANCE",
        "O'DONNELL",
        "PITTS",
        "QUAYLE",
        "RILEY",
        "SATCHER",
        "SIMKUS",
        "THOMAS",
        "WOOD"
    };

    static MarkovNameGenerator firstNameGen = new MarkovNameGenerator(firstNames, 2, 4);
    static MarkovNameGenerator lastNameGen = new MarkovNameGenerator(lastNames, 2, 6);

    public void generate(GameObject passOn)
    {
        PointAllocation points = generateStats(5000f, (int)Random.Range(0, 15));
        int basePrice = 100000;
        PlayerAbilities.Abilities ability = AbilitiesEnum.getRandom();
        PlayerStats stats = new PlayerStats(generateName(), generateNumber(), points.speed, points.bulk, points.style, (int) (basePrice * points.cost_multiplier * AbilitiesEnum.GetCostMultiplier(ability)), ability, getHeight(), getWeight());
        PlayerCard maybeCard = passOn.GetComponent<PlayerCard>();
        if(maybeCard != null)
        {
            maybeCard.renderCard(stats);
        }
    }

    static public PlayerStats generate(int price)
    {
        int basePrice;
        PointAllocation points;
        Abilities ability;

        if (price < 100000)
        {
            basePrice = 0;
            points = generateStats(1f, 0);
            ability = Abilities.FreeAgent;
        } else
        {
            basePrice = 100000;
            ability = AbilitiesEnum.getRandom();
            float maxMultiplier = (price * 1.0f) / (basePrice * 1.4f);
            points = generateStats(maxMultiplier, (int)Random.Range(0, 15));
        }

        PlayerStats stats = new PlayerStats(generateName(), generateNumber(), points.speed, points.bulk, points.style, (int)(basePrice * points.cost_multiplier * AbilitiesEnum.GetCostMultiplier(ability)), ability, getHeight(), getWeight());
        return stats;
    }

    private static string generateName()
    {
        return firstNameGen.NextName.ToUpper() + " " + lastNameGen.NextName.ToUpper();
    }

    private static string generateNumber()
    {
        return ((int)Random.Range(0, 420)).ToString();
    }
	
    private static PointAllocation generateStats(float maxMultiplier, int points)
    {
        return new PointAllocation(maxMultiplier, points);
    }

    private static string getHeight()
    {
        return (((int)Random.Range(2, 8)).ToString() + "\'" + ((int)Random.Range(0, 11)).ToString() + "\"");
    }

    private static string getWeight()
    {
        return ((int)Random.Range(100, 450)).ToString() + " lbs";
    }
     
	private class PointAllocation
    {
        public int speed = 2;
        public int bulk = 2;
        public int style = 2;
        public float cost_multiplier = 1.0f;

        public PointAllocation(float maxMultiplier, int points)
        {
            int max_points = 15;

            // allocates all points into the stats with equal probability
            for(int i = 0; i < points && i < max_points && cost_multiplier < maxMultiplier; i++)
            {
                float bucket = Random.value;
                if (bucket < .33)
                {
                    if (speed < 7)
                    {
                        speed++;
                        cost_multiplier *= 1 + (float) speed / 7;
                    }
                    else
                    {
                        max_points++;
                        points++;
                    }
                } else if (bucket < .66)
                {
                    if (bulk < 7)
                    {
                        bulk++;
                        cost_multiplier *= 1 + (float) bulk / 7;
                    }
                    else
                    {
                        max_points++;
                        points++;
                    }
                } else 
                {
                    if (style < 7)
                    {
                        style++;
                        cost_multiplier *= 1 + (float) style / 7;
                    } else
                    {
                        max_points++;
                        points++;
                    }
                }
            }
        }
    }
}
