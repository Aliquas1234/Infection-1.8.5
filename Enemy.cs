using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infection_1._8._5
{
    class Enemy
    {
        public int maxHealth;
        public int health;
        public int lowerDamage;
        public int upperDamage;
        public int damageOutput;
        public int healOutput;
        public string name;
        public int target;
        public int upperExp;
        public int lowerExp;
        public int nameX;
        public bool isBoss;
        Map map;
        public Sprite sprite;
        public SpriteBatch spriteBatch;
        public bool marked = false;
        public bool isAlive = true;

        Random rnd = new Random();

        public Enemy(Map newMap, Sprite newSprite, bool newIsBoss, SpriteBatch newspriteBatch)
        {
            map = newMap;
            isBoss = newIsBoss;
            sprite = newSprite;
            spriteBatch = newspriteBatch;

            #region Setting Boss stats

            if (isBoss)
            {
                switch (map.stage)
                {
                    case 1:
                        maxHealth = 175;
                        health = 175;
                        upperDamage = 35;
                        lowerDamage = 25;
                        name = "RhinoVirus-M";
                        upperExp = 100;
                        lowerExp = 100;
                        nameX = 260;
                        healOutput = 50;
                        break;

                    case 2:
                        maxHealth = 300;
                        health = 300;
                        upperDamage = 45;
                        lowerDamage = 35;
                        name = "Influenza-M";
                        upperExp = 200;
                        lowerExp = 200;
                        nameX = 265;
                        healOutput = 75;
                        break;

                    case 3:
                        maxHealth = 500;
                        health = 500;
                        upperDamage = 60;
                        lowerDamage = 45;
                        name = "E.Coli-M";
                        upperExp = 300;
                        lowerExp = 300;
                        nameX = 257;
                        healOutput = 90;
                        break;

                    case 4:
                        maxHealth = 750;
                        health = 750;
                        upperDamage = 70;
                        lowerDamage = 60;
                        name = "Malaria-M";
                        upperExp = 450;
                        lowerExp = 450;
                        nameX = 270;
                        healOutput = 100;
                        break;

                    case 5:
                        maxHealth = 900;
                        health = 900;
                        upperDamage = 85;
                        lowerDamage = 77;
                        name = "Cancer-M";
                        upperExp = 800;
                        lowerExp = 800;
                        nameX = 304;
                        healOutput = 125;
                        break;

                    default:
                        maxHealth = 9999;
                        health = 9999;
                        upperDamage = 999;
                        lowerDamage = 999;
                        name = "Virus.13";
                        upperExp = 1;
                        lowerExp = 1;
                        nameX = 300;
                        healOutput = 9999;
                        break;
                }
            }

            #endregion

            #region Stetting Enemy stats
            if (!isBoss)
            {
                switch (map.stage)
                {
                    case 1:
                        maxHealth = 50;
                        health = 50;
                        upperDamage = 20;
                        lowerDamage = 10;
                        name = "RhinoVirus";
                        upperExp = 20;
                        lowerExp = 8;
                        nameX = 290;
                        break;

                    case 2:
                        maxHealth = 100;
                        health = 100;
                        upperDamage = 35;
                        lowerDamage = 20;
                        name = "Influenza";
                        upperExp = 35;
                        lowerExp = 18;
                        nameX = 290;
                        break;

                    case 3:
                        maxHealth = 170;
                        health = 170;
                        upperDamage = 45;
                        lowerDamage = 30;
                        name = "E. Coli";
                        upperExp = 45;
                        lowerExp = 30;
                        nameX = 304;
                        break;

                    case 4:
                        maxHealth = 250;
                        health = 250;
                        upperDamage = 55;
                        lowerDamage = 40;
                        name = "Malaria";
                        upperExp = 60;
                        lowerExp = 40;
                        nameX = 300;
                        break;

                    case 5:
                        maxHealth = 400;
                        health = 400;
                        upperDamage = 75;
                        lowerDamage = 60;
                        name = "Cancer";
                        upperExp = 80;
                        lowerExp = 65;
                        nameX = 304;
                        break;

                    default:
                        maxHealth = 9999;
                        health = 9999;
                        upperDamage = 999;
                        lowerDamage = 999;
                        name = "Mysterious Virus";
                        upperExp = 1;
                        lowerExp = 1;
                        break;
                }
            }
            #endregion
        }
    }
}


