using Assets.src.gen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Gen
{
    public static class PlanGenerator
    {
        public static readonly int GENERATOR_POOL = 100;
        public static readonly int MIN_MOVES = 10;
        public static readonly int MAX_MOVES = 50;
        public static readonly int YARDS_PER_MOVE = 10;
        public static readonly int MIN_ENEMIES_PER_MOVE = 1;
        public static readonly int MAX_ENEMIES_PER_MOVE = 5;

        private static readonly Random rand = new Random();

        public static List<Move> CreatePlan()
        {
            var plans = new List<List<Move>>(GENERATOR_POOL);

            for (int p = 0; p < GENERATOR_POOL; p++)
            {
                plans.Add(createRandomPlan());
            }

            //TODO: Assess plans
            return plans[rand.Next(plans.Count)];
        }

        private static List<Move> createRandomPlan()
        {
            var size = rand.Next(MIN_MOVES, MAX_MOVES);
            var plan = new List<Move>(size);

            for (int m = 2; m < size + 2; m++)
            {
                plan.Add(
                    new Move()
                    {
                        Enemies = createRandomMove(),
                        Yard = m * YARDS_PER_MOVE
                    }
                );
            }

            return plan;
        }

        private static EnemyType[] createRandomMove()
        {
            var enemyTypes = Enum.GetValues(typeof(EnemyType)) as EnemyType[];
            return Enumerable.Range(MIN_ENEMIES_PER_MOVE, rand.Next(MAX_ENEMIES_PER_MOVE - MIN_ENEMIES_PER_MOVE + 1)).Select((int e) => enemyTypes[rand.Next(enemyTypes.Length)]).ToArray();
        }
    }
}
