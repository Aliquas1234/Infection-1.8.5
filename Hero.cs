using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infection_1._8._5
{
    class Hero
    {
        public string name;
        public int health;
        public int maxHealth;
        public int atp;
        public int maxAtp;
        public int exp;
        public float expToNextLevel;
        public int level = 1;

        public float healOutput;
        public int damageOutput;

        public bool isAlive = true;

        public Hero(string newName)
        {
            name = newName;
            maxHealth = 100;
            health = 100;
            atp = 10;
            maxAtp = 10;
            exp = 0;
            expToNextLevel = 50;
            isAlive = true;
        }

        public void Update(GameTime gameTime)
        {
            if (health < 0)
                health = 0;
            if (health == 0)
                isAlive = false;
            if (health > 0)
                isAlive = true;

            maxHealth = level * 20 + 80;
        }
    }
}



