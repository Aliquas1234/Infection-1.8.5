using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Infection_1._8._5
{
    enum Gamestate { Start, Title, Overworld, Battle, Pause, Gameover, End };
    enum Items { Antibiotic, Glucose, Regenerator, FlawlessDNA, CorruptDNA }

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region Variable Library

        //Graphics setup
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Key waiting
        bool enterWaiting;

        //Gamestate
        Gamestate gameState;
        bool inPauseMenu;
        bool inTeamMenu;
        bool inItemMenu;
        bool battleState1;
        bool battleState2;
        bool battleState3;
        bool battleState4;
        bool battleState5;
        bool battleState6;
        int battleMeasageNumber;
        bool tutorial = true;
        int tutorialNumber = 1;
        int tutorialSubNumber = 1;
        bool youHealedText;
        int endText;

        //Backgrounds 
        Background backGround;
        Background topWall;
        Background bottomWall;
        Background fader;
        Background blackscreen;

        //Enemy
        Enemy enemy;
        Sprite RhinoVirus;
        Sprite Influenza;
        Sprite EColi;
        Sprite Malaria;
        Sprite Cancer;
        Sprite RhinoVirusM;
        Sprite InfluenzaM;
        Sprite EColiM;
        Sprite MalariaM;
        Sprite CancerM;

        //Map
        Map map;

        //Themes
        Song mainTheme;
        Song overworldTheme;
        TimeSpan mainReset;
        TimeSpan overworldReset;

        //Player
        Character player;
        Hero TCell;
        Hero BCell;
        Hero Macrophage;
        int Identified;

        //Overworld bosses
        Sprite OverWorldRhinoVirus;
        Sprite OverWorldInfluenza;
        Sprite OverWorldEcoli;
        Sprite OverWorldMalaria;
        Sprite OverWorldCancer;

        //Health bar
        Sprite healthbar;
        float x;

        //RBC
        List<RedBloodcell> redBloodcells1;
        List<RedBloodcell> redBloodcells2;
        List<RedBloodcell> redBloodcells3;
        List<RedBloodcell> redBloodcells4;

        //Fonts
        SpriteFont CourierNew;

        //UIs
        UserInterface battleUI1;
        UserInterface battleUI2;
        UserInterface pauseUI;
        Sprite Textbox;
        Background titleScreen;

        //Logos
        Texture2D AliquisTexture;
        Rectangle AliquisRectangle;

        //reference
        Random rnd;
        int screenWidth;
        int screenHieght;
        int pausetime;
        bool visible;
        bool firsttime;
        int turnNumber;
        bool first;

        //Colors/display
        Color baseColor;
        Color backgroundColor;
        bool fadein = true;
        bool fadeout = false;

        #endregion

        public Game1()
        {
            //graphics setup
            graphics = new GraphicsDeviceManager(this);

            //file reader setup
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            //Gamestate
            gameState = new Gamestate();
            gameState = Gamestate.Start;

            //Room & stage maps
            map = new Map();

            //Refernces
            rnd = new Random();
            screenWidth = GraphicsDevice.Viewport.Width;
            screenHieght = GraphicsDevice.Viewport.Height;

            //Song
            mainTheme = Content.Load<Song>("InfectionOverWorld2");
            overworldTheme = Content.Load<Song>("InfectionMain");
            mainReset = TimeSpan.FromSeconds(18.5);
            overworldReset = TimeSpan.FromSeconds(13.5);

            //overworld backdrops
            topWall = new Background(Content.Load<Texture2D>("bloodvesselwall"), new Rectangle(0, 0, screenWidth, screenHieght / 8), new Vector2(0, 0), Color.Salmon);
            bottomWall = new Background(Content.Load<Texture2D>("bloodvesselwallb"), new Rectangle(0, 0, screenWidth, screenHieght / 8), new Vector2(0, 425), Color.Salmon);
            backGround = new Background(Content.Load<Texture2D>("bloodvesselback"), new Rectangle(0, 0, screenWidth, screenHieght / 8 * 7), new Vector2(0, 60), Color.DarkRed);
            blackscreen = new Background(Content.Load<Texture2D>("youhealed"), new Rectangle(0, 0, screenWidth, screenHieght), new Vector2(0, 0), baseColor);

            //Fonts
            CourierNew = Content.Load<SpriteFont>("Courier New");

            //UIs
            battleUI1 = new UserInterface(Content.Load<Texture2D>("Battle UI 1"), new Rectangle(0, 0, screenWidth, screenHieght), new Vector2(0, 0), Color.White, Content, 2);
            battleUI2 = new UserInterface(Content.Load<Texture2D>("Battle UI 1"), new Rectangle(0, 0, screenWidth, screenHieght), new Vector2(0, 0), Color.White, Content, 3);
            pauseUI = new UserInterface(Content.Load<Texture2D>("Pause UI 1"), new Rectangle(0, 0, screenWidth, screenHieght), new Vector2(0, 0), Color.White, Content, 1);
            titleScreen = new Background(Content.Load<Texture2D>("Title UI 1"), new Rectangle(0, 0, screenWidth, screenHieght), new Vector2(0, 0), Color.White);
            Textbox = new Sprite(Content.Load<Texture2D>("Textbox1"), new Rectangle(0, 0, screenWidth, screenHieght / 4), new Vector2(0, screenHieght / 8 * 6), Color.White);
            fader = new Background(Content.Load<Texture2D>("Fader"), new Rectangle(0, 0, screenWidth, screenHieght), new Vector2(0, 0), baseColor);
            healthbar = new Sprite(Content.Load<Texture2D>("healthbar78"), new Rectangle(0, 0, 250, 10), new Vector2(195, 48), Color.White);

            #region Red Blood Cell Mapping

            //map 1
            redBloodcells1 = new List<RedBloodcell>();
            redBloodcells1.Add(new RedBloodcell(Content.Load<Texture2D>("RedBloodcell1"), new Rectangle(0, 0, 20, 20), new Vector2(400, 100), Color.White));
            redBloodcells1.Add(new RedBloodcell(Content.Load<Texture2D>("RedBloodcell1"), new Rectangle(0, 0, 20, 20), new Vector2(100, 300), Color.White));
            redBloodcells1.Add(new RedBloodcell(Content.Load<Texture2D>("RedBloodcell1"), new Rectangle(0, 0, 20, 20), new Vector2(700, 400), Color.White));
            redBloodcells1.Add(new RedBloodcell(Content.Load<Texture2D>("RedBloodcell1"), new Rectangle(0, 0, 20, 20), new Vector2(200, 200), Color.White));
            redBloodcells1.Add(new RedBloodcell(Content.Load<Texture2D>("RedBloodcell1"), new Rectangle(0, 0, 20, 20), new Vector2(600, 150), Color.White));
            redBloodcells1.Add(new RedBloodcell(Content.Load<Texture2D>("RedBloodcell1"), new Rectangle(0, 0, 20, 20), new Vector2(300, 315), Color.White));
            redBloodcells1.Add(new RedBloodcell(Content.Load<Texture2D>("RedBloodcell1"), new Rectangle(0, 0, 20, 20), new Vector2(50, 100), Color.White));

            //map 2
            redBloodcells2 = new List<RedBloodcell>();
            redBloodcells2.Add(new RedBloodcell(Content.Load<Texture2D>("RedBloodcell1"), new Rectangle(0, 0, 20, 20), new Vector2(100, 100), Color.White));
            redBloodcells2.Add(new RedBloodcell(Content.Load<Texture2D>("RedBloodcell1"), new Rectangle(0, 0, 20, 20), new Vector2(200, 300), Color.White));
            redBloodcells2.Add(new RedBloodcell(Content.Load<Texture2D>("RedBloodcell1"), new Rectangle(0, 0, 20, 20), new Vector2(300, 400), Color.White));
            redBloodcells2.Add(new RedBloodcell(Content.Load<Texture2D>("RedBloodcell1"), new Rectangle(0, 0, 20, 20), new Vector2(400, 200), Color.White));
            redBloodcells2.Add(new RedBloodcell(Content.Load<Texture2D>("RedBloodcell1"), new Rectangle(0, 0, 20, 20), new Vector2(500, 150), Color.White));
            redBloodcells2.Add(new RedBloodcell(Content.Load<Texture2D>("RedBloodcell1"), new Rectangle(0, 0, 20, 20), new Vector2(600, 315), Color.White));
            redBloodcells2.Add(new RedBloodcell(Content.Load<Texture2D>("RedBloodcell1"), new Rectangle(0, 0, 20, 20), new Vector2(700, 100), Color.White));

            //map 3
            redBloodcells3 = new List<RedBloodcell>();
            redBloodcells3.Add(new RedBloodcell(Content.Load<Texture2D>("RedBloodcell1"), new Rectangle(0, 0, 20, 20), new Vector2(120, 100), Color.White));
            redBloodcells3.Add(new RedBloodcell(Content.Load<Texture2D>("RedBloodcell1"), new Rectangle(0, 0, 20, 20), new Vector2(205, 400), Color.White));
            redBloodcells3.Add(new RedBloodcell(Content.Load<Texture2D>("RedBloodcell1"), new Rectangle(0, 0, 20, 20), new Vector2(310, 200), Color.White));
            redBloodcells3.Add(new RedBloodcell(Content.Load<Texture2D>("RedBloodcell1"), new Rectangle(0, 0, 20, 20), new Vector2(425, 75), Color.White));
            redBloodcells3.Add(new RedBloodcell(Content.Load<Texture2D>("RedBloodcell1"), new Rectangle(0, 0, 20, 20), new Vector2(530, 150), Color.White));
            redBloodcells3.Add(new RedBloodcell(Content.Load<Texture2D>("RedBloodcell1"), new Rectangle(0, 0, 20, 20), new Vector2(608, 300), Color.White));
            redBloodcells3.Add(new RedBloodcell(Content.Load<Texture2D>("RedBloodcell1"), new Rectangle(0, 0, 20, 20), new Vector2(790, 500), Color.White));

            //map 4
            redBloodcells4 = new List<RedBloodcell>();
            redBloodcells4.Add(new RedBloodcell(Content.Load<Texture2D>("RedBloodcell1"), new Rectangle(0, 0, 20, 20), new Vector2(100, 100), Color.White));
            redBloodcells4.Add(new RedBloodcell(Content.Load<Texture2D>("RedBloodcell1"), new Rectangle(0, 0, 20, 20), new Vector2(200, 315), Color.White));
            redBloodcells4.Add(new RedBloodcell(Content.Load<Texture2D>("RedBloodcell1"), new Rectangle(0, 0, 20, 20), new Vector2(300, 150), Color.White));
            redBloodcells4.Add(new RedBloodcell(Content.Load<Texture2D>("RedBloodcell1"), new Rectangle(0, 0, 20, 20), new Vector2(400, 75), Color.White));
            redBloodcells4.Add(new RedBloodcell(Content.Load<Texture2D>("RedBloodcell1"), new Rectangle(0, 0, 20, 20), new Vector2(500, 400), Color.White));
            redBloodcells4.Add(new RedBloodcell(Content.Load<Texture2D>("RedBloodcell1"), new Rectangle(0, 0, 20, 20), new Vector2(600, 300), Color.White));
            redBloodcells4.Add(new RedBloodcell(Content.Load<Texture2D>("RedBloodcell1"), new Rectangle(0, 0, 20, 20), new Vector2(700, 100), Color.White));

            #endregion

            //Player
            player = new Character(Content.Load<Texture2D>("WhiteBloodcell"), new Rectangle(10, screenHieght / 2, 25, 25), new Vector2(10, screenHieght / 2), Color.White, backGround);
            TCell = new Hero("T-Cell");
            BCell = new Hero("B-Cell");
            Macrophage = new Hero("Macrophage");

            //Logos
            AliquisTexture = Content.Load<Texture2D>("Aliquis logo 3");
            baseColor.R = 1; baseColor.B = 1; baseColor.G = 1;
            AliquisRectangle = new Rectangle(GraphicsDevice.Viewport.Width / 2 - 115, GraphicsDevice.Viewport.Height / 2 - 100, 225, 200);

            //Colors
            baseColor = new Color(255, 255, 255);
            backgroundColor = new Color(0, 0, 0);

            //Enemies sprites
            RhinoVirus = new Sprite(Content.Load<Texture2D>("RhinoVirus2"), new Rectangle(255, 130, 150, 120), new Vector2(255, 130), Color.White);
            Influenza = new Sprite(Content.Load<Texture2D>("Influenza2"), new Rectangle(245, 120, 150, 150), new Vector2(245, 120), Color.White);
            EColi = new Sprite(Content.Load<Texture2D>("E.Coli2"), new Rectangle(216, 130, 220, 100), new Vector2(216, 130), Color.White);
            Malaria = new Sprite(Content.Load<Texture2D>("Malaria1"), new Rectangle(245, 130, 150, 75), new Vector2(245, 130), Color.White);
            Cancer = new Sprite(Content.Load<Texture2D>("Cancer1"), new Rectangle(245, 130, 150, 80), new Vector2(245, 130), Color.White);

            //Overworld Mutants
            OverWorldRhinoVirus = new Sprite(Content.Load<Texture2D>("RhinoVirus_M"), new Rectangle(245, 130, 30, 30), new Vector2(245, 130), Color.White);
            OverWorldInfluenza = new Sprite(Content.Load<Texture2D>("Influenza_M"), new Rectangle(500, 200, 30, 30), new Vector2(245, 130), Color.White);
            OverWorldEcoli = new Sprite(Content.Load<Texture2D>("E.Coli_M"), new Rectangle(300, 380, 40, 21), new Vector2(245, 130), Color.White);
            OverWorldMalaria = new Sprite(Content.Load<Texture2D>("Malaria1"), new Rectangle(700, 360, 40, 20), new Vector2(245, 130), Color.White);
            OverWorldCancer = new Sprite(Content.Load<Texture2D>("Cancer_M"), new Rectangle(500, 130, 90, 90), new Vector2(245, 130), Color.White);

            //Mutant sprites
            RhinoVirusM = new Sprite(Content.Load<Texture2D>("RhinoVirus_M"), new Rectangle(245, 130, 150, 120), new Vector2(245, 130), Color.White);
            InfluenzaM = new Sprite(Content.Load<Texture2D>("Influenza_M"), new Rectangle(245, 120, 150, 150), new Vector2(245, 120), Color.White);
            EColiM = new Sprite(Content.Load<Texture2D>("E.Coli_M"), new Rectangle(200, 130, 250, 130), new Vector2(200, 130), Color.White);
            MalariaM = new Sprite(Content.Load<Texture2D>("Malaria_M"), new Rectangle(200, 130, 150, 75), new Vector2(200, 130), Color.White);
            CancerM = new Sprite(Content.Load<Texture2D>("Cancer_M"), new Rectangle(160, 80, 350, 240), new Vector2(160, 80), Color.White);

            //base initializer
            base.Initialize();
        }

        protected override void LoadContent()
        {
            //SpriteBatches
            spriteBatch = new SpriteBatch(GraphicsDevice);

        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            switch (gameState)
            {
                #region Gamestate.Loading
                case Gamestate.Start:

                    //fades in
                    if (fadein)
                    {
                        baseColor.R += 1;
                        baseColor.B += 1;
                        baseColor.G += 1;

                        if (IsDivisable(gameTime.TotalGameTime.Milliseconds, 7))
                            AliquisRectangle.Y--;
                    }

                    //stops fade in and pauses
                    if (baseColor.R >= 254 && baseColor.B >= 254 && baseColor.G >= 254)
                    {
                        fadeout = true;
                        fadein = false;
                        pausetime++;
                    }

                    //fades out
                    if (pausetime >= 150 && fadeout)
                    {
                        pausetime = 0;
                        gameState = Gamestate.Title;

                        MediaPlayer.Play(mainTheme);
                        MediaPlayer.IsRepeating = true;

                        fadeout = false;

                        if (TCell.exp >= 1 || TCell.level > 1)
                        {
                            Exit();
                        }
                    }

                    break;
                #endregion

                #region Gamestate.Title
                case Gamestate.Title:

                    titleScreen.Update(gameTime);

                    #region text blinking
                    if (pausetime == 0)
                    {
                        visible = true;
                    }

                    if (pausetime == 60)
                    {
                        visible = false;
                    }

                    if (visible)
                        pausetime++;

                    if (!visible)
                        pausetime--;
                    #endregion

                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                        enterWaiting = true;
                    if (Keyboard.GetState().IsKeyUp(Keys.Enter) && enterWaiting)
                    {
                        gameState = Gamestate.Overworld;

                        MediaPlayer.Play(overworldTheme);
                        MediaPlayer.Volume = .35f;

                        tutorial = true;

                        enterWaiting = false;
                    }

                    break;
                #endregion

                #region Gamestate.Overworld
                case Gamestate.Overworld:
                    if (!tutorial)
                    {
                        //updating player
                        switch (map.stage)
                        {
                            case 1:
                                player.Update(gameTime, map, OverWorldRhinoVirus);
                                break;

                            case 2:
                                player.Update(gameTime, map, OverWorldInfluenza);
                                break;

                            case 3:
                                player.Update(gameTime, map, OverWorldEcoli);
                                break;

                            case 4:
                                player.Update(gameTime, map, OverWorldMalaria);
                                break;

                            case 5:
                                player.Update(gameTime, map, OverWorldCancer);
                                break;
                        }
                    }

                    #region tutorials
                    else
                    {
                        if (first)
                        {
                            first = false;

                            switch (map.stage)
                            {
                                case 1:
                                    player.Update(gameTime, map, OverWorldRhinoVirus);
                                    break;

                                case 2:
                                    player.Update(gameTime, map, OverWorldInfluenza);
                                    break;

                                case 3:
                                    player.Update(gameTime, map, OverWorldEcoli);
                                    break;

                                case 4:
                                    player.Update(gameTime, map, OverWorldMalaria);
                                    break;

                                case 5:
                                    player.Update(gameTime, map, OverWorldCancer);
                                    break;
                            }
                        }

                        tutorialNumber = map.stage;

                        //getting enter
                        if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                            enterWaiting = true;
                        if (Keyboard.GetState().IsKeyUp(Keys.Enter) && enterWaiting)
                        {
                            tutorialSubNumber++;
                            enterWaiting = false;
                        }

                        //if tutorial1
                        if (tutorialNumber == 1)
                        {
                            if (tutorialSubNumber == 4)
                                tutorial = false;
                        }
                        if (tutorialNumber == 2)
                        {
                            if (tutorialSubNumber == 4)
                                tutorial = false;
                        }
                        if (tutorialNumber == 3)
                        {
                            if (tutorialSubNumber == 2)
                                tutorial = false;
                        }
                        if (tutorialNumber == 4)
                        {
                            if (tutorialSubNumber == 2)
                                tutorial = false;
                        }
                        if (tutorialNumber == 5)
                        {
                            if (tutorialSubNumber == 4)
                                tutorial = false;
                        }
                    }
                    #endregion

                    backGround.Update(gameTime);
                    topWall.Update(gameTime);
                    bottomWall.Update(gameTime);
                    Textbox.Update(gameTime);

                    #region Boss encounter

                    if (player.bossFight)
                    {

                        switch (map.stage)
                        {
                            case 1:
                                enemy = new Enemy(map, RhinoVirusM, true, spriteBatch);
                                break;
                            case 2:
                                enemy = new Enemy(map, InfluenzaM, true, spriteBatch);
                                break;
                            case 3:
                                enemy = new Enemy(map, EColiM, true, spriteBatch);
                                break;
                            case 4:
                                enemy = new Enemy(map, MalariaM, true, spriteBatch);
                                break;
                            case 5:
                                enemy = new Enemy(map, CancerM, true, spriteBatch);
                                break;
                        }

                        gameState = Gamestate.Battle;
                        battleState1 = true;
                        battleState2 = false;
                        battleState3 = false;
                        battleState4 = false;
                        battleState5 = false;
                        battleState6 = false;
                        Identified = 0;
                        turnNumber = 1;
                        TCell.atp = TCell.maxAtp;
                        BCell.atp = BCell.maxAtp;
                        Macrophage.atp = Macrophage.maxAtp;
                        GamePad.SetVibration(PlayerIndex.One, .5f, .5f);

                    }

                    #endregion

                    #region red blood cell updating
                    if (map.room == 1)
                    {
                        foreach (RedBloodcell r in redBloodcells1)
                            r.Update(gameTime);
                    }
                    if (map.room == 2)
                    {
                        foreach (RedBloodcell r in redBloodcells2)
                            r.Update(gameTime);
                    }
                    if (map.room == 3)
                    {
                        foreach (RedBloodcell r in redBloodcells3)
                            r.Update(gameTime);
                    }
                    if (map.room == 4)
                    {
                        foreach (RedBloodcell r in redBloodcells4)
                            r.Update(gameTime);
                    }
                    #endregion

                    #region Screen Wrapping
                    //going left
                    if (player.position.X <= 1 && map.room != 1)
                    {
                        map.room--;
                        player.position = new Vector2(750, player.position.Y);
                    }
                    //bone marrow
                    else if (player.position.X <= 1 && map.room <= 1)
                    {

                        player.position = new Vector2(1, player.position.Y);

                        youHealedText = true;

                        //you healed screen
                        if (youHealedText)
                        {
                            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                                enterWaiting = true;
                            if (Keyboard.GetState().IsKeyUp(Keys.Enter) && enterWaiting)
                            {
                                enterWaiting = false;
                                youHealedText = false;
                                player.position.X = 2f;
                                TCell.health = TCell.maxHealth;
                                BCell.health = BCell.maxHealth;
                                Macrophage.health = Macrophage.maxHealth;
                            }
                        }
                    }
                    //going right
                    if (player.position.X >= screenWidth - player.texture.Width && map.room != 4)
                    {
                        map.room++;
                        player.position = new Vector2(50, player.position.Y);
                    }
                    //trying to go right in room 4
                    else if (player.position.X >= screenWidth - player.texture.Width && map.room == 4)
                        player.position = new Vector2(screenWidth - player.texture.Width, player.position.Y);

                    #endregion

                    #region Random encounter
                    //rng encounter
                    if (player.encounterRate == 1) // activated state = 1, deactivated state = 1000
                    {
                        player.encounterRate = 0;

                        switch (map.stage)
                        {
                            case 1:
                                enemy = new Enemy(map, RhinoVirus, false, spriteBatch);
                                break;
                            case 2:
                                enemy = new Enemy(map, Influenza, false, spriteBatch);
                                break;
                            case 3:
                                enemy = new Enemy(map, EColi, false, spriteBatch);
                                break;
                            case 4:
                                enemy = new Enemy(map, Malaria, false, spriteBatch);
                                break;
                            case 5:
                                enemy = new Enemy(map, Cancer, false, spriteBatch);
                                break;
                        }

                        gameState = Gamestate.Battle;
                        battleState1 = true;
                        battleState2 = false;
                        battleState3 = false;
                        battleState4 = false;
                        battleState5 = false;
                        battleState6 = false;
                        Identified = 0;
                        turnNumber = 1;
                        TCell.atp = TCell.maxAtp;
                        BCell.atp = BCell.maxAtp;
                        Macrophage.atp = Macrophage.maxAtp;
                        GamePad.SetVibration(PlayerIndex.One, .5f, .5f);
                    }

                    #endregion

                    #region pause
                    //open pause menu
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        gameState = Gamestate.Pause;
                        inPauseMenu = true;
                    }

                    #endregion

                    break;
                #endregion

                #region Gamestate.Battle
                case Gamestate.Battle:

                    enemy.sprite.Update(gameTime);
                    fader.Update(gameTime);
                    healthbar.Update(gameTime);

                    #region Main battle menu
                    if (battleState1)
                    {
                        battleUI1.Update(gameTime);
                        firsttime = true;

                        //reset damage outputs and heal outputs
                        enemy.damageOutput = 0;
                        enemy.healOutput = 0;
                        TCell.damageOutput = 0;
                        TCell.healOutput = 0;
                        BCell.damageOutput = 0;
                        BCell.healOutput = 0;
                        Macrophage.damageOutput = 0;
                        Macrophage.healOutput = 0;

                        //regular enemy dies
                        if (!enemy.isAlive && !enemy.isBoss)
                        {
                            enemy = null;
                            gameState = Gamestate.Overworld;
                        }

                        //boss dies
                        else if (!enemy.isAlive && enemy.isBoss && enemy.name != "Cancer-M")
                        {
                            map.stage++;
                            map.room = 1;
                            player.position = new Vector2(10, screenHieght / 2);
                            tutorial = true;
                            tutorialNumber = map.stage;
                            tutorialSubNumber = 1;
                            first = true;
                            gameState = Gamestate.Overworld;
                            enemy = null;
                        }
                        else if (!enemy.isAlive && enemy.isBoss && enemy.name == "Cancer-M")
                        {
                            gameState = Gamestate.End;
                            endText = 1;
                        }

                        //input
                        if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                            enterWaiting = true;

                        if (Keyboard.GetState().IsKeyUp(Keys.Enter) && battleUI1.cursorPosition == 3 && enterWaiting)
                        {
                            if (!enemy.isBoss)
                                gameState = Gamestate.Overworld;

                            enterWaiting = false;
                        }


                        if (Keyboard.GetState().IsKeyUp(Keys.Enter) && battleUI1.cursorPosition == 1 && enterWaiting)
                        {
                            battleState1 = false;
                            battleState2 = true;
                            enterWaiting = false;
                        }

                        if (Keyboard.GetState().IsKeyUp(Keys.Enter) && battleUI1.cursorPosition == 2 && enterWaiting)
                        {
                            battleState1 = false;
                            battleState6 = true;
                            enterWaiting = false;
                        }
                    }
                    #endregion

                    #region TCell's turn

                    if (battleState2)
                    {
                        if (TCell.isAlive)
                        {
                            battleUI2.Update(gameTime);
                            battleUI2.cursor.Update(gameTime);

                            //input
                            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                                enterWaiting = true;

                            //basic attack
                            if (Keyboard.GetState().IsKeyUp(Keys.Enter) && battleUI2.cursorPosition == 1 && enterWaiting)
                            {
                                TCell.damageOutput = TCell.level * 2 + Identified;
                                battleState2 = false;
                                battleState3 = true;
                                enterWaiting = false;
                            }
                            //Identify attack
                            if (Keyboard.GetState().IsKeyUp(Keys.Enter) && battleUI2.cursorPosition == 2 && enterWaiting)
                            {
                                if (TCell.atp >= 5)
                                {
                                    Identified = TCell.level / 2;
                                    if (Identified < 1)
                                        Identified = 1;

                                    TCell.atp -= 5;
                                    battleState2 = false;
                                    battleState3 = true;
                                }

                                enterWaiting = false;
                            }
                            //heal move
                            if (Keyboard.GetState().IsKeyUp(Keys.Enter) && battleUI2.cursorPosition == 3 && enterWaiting)
                            {
                                if (TCell.atp >= 4)
                                {
                                    TCell.healOutput = TCell.maxHealth * .5f;
                                    TCell.atp -= 4;
                                    battleState2 = false;
                                    battleState3 = true;
                                }
                                enterWaiting = false;
                            }
                            //Use Item
                            if (Keyboard.GetState().IsKeyUp(Keys.Enter) && battleUI2.cursorPosition == 4 && enterWaiting)
                            {
                                battleState2 = false;
                                battleState3 = true;
                                enterWaiting = false;
                            }
                        }

                        //if tcell == dead
                        else if (!TCell.isAlive)
                        {
                            battleState2 = false;
                            battleState3 = true;
                        }
                    }
                    #endregion

                    #region BCell's turn
                    if (battleState3)
                    {
                        if (BCell.isAlive)
                        {
                            battleUI2.Update(gameTime);
                            battleUI2.cursor.Update(gameTime);

                            //input
                            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                                enterWaiting = true;

                            //basic attack
                            if (Keyboard.GetState().IsKeyUp(Keys.Enter) && battleUI2.cursorPosition == 1 && enterWaiting)
                            {
                                BCell.damageOutput = BCell.level * 2 + Identified;
                                battleState3 = false;
                                battleState4 = true;
                                enterWaiting = false;
                            }
                            //Antibody attack
                            if (Keyboard.GetState().IsKeyUp(Keys.Enter) && battleUI2.cursorPosition == 2 && enterWaiting)
                            {
                                if (BCell.atp >= 3)
                                {
                                    enemy.marked = true;

                                    BCell.atp -= 3;
                                    battleState3 = false;
                                    battleState4 = true;
                                }

                                enterWaiting = false;
                            }

                            //All-out attack move
                            if (Keyboard.GetState().IsKeyUp(Keys.Enter) && battleUI2.cursorPosition == 3 && enterWaiting)
                            {
                                if (BCell.atp >= 3)
                                {
                                    BCell.damageOutput = BCell.level * 3 + Identified;
                                    BCell.atp -= 3;
                                    battleState3 = false;
                                    battleState4 = true;
                                }
                                enterWaiting = false;
                            }
                            //Use Item
                            if (Keyboard.GetState().IsKeyUp(Keys.Enter) && battleUI2.cursorPosition == 4 && enterWaiting)
                            {
                                battleState3 = false;
                                battleState4 = true;
                                enterWaiting = false;
                            }
                        }
                        else if (!BCell.isAlive)
                        {
                            battleState3 = false;
                            battleState4 = true;
                        }
                    }
                    #endregion

                    #region Macrophage's turn

                    if (battleState4)
                    {
                        if (Macrophage.isAlive)
                        {
                            battleUI2.Update(gameTime);
                            battleUI2.cursor.Update(gameTime);

                            //input
                            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                                enterWaiting = true;

                            //basic attack
                            if (Keyboard.GetState().IsKeyUp(Keys.Enter) && battleUI2.cursorPosition == 1 && enterWaiting)
                            {
                                Macrophage.damageOutput = Macrophage.level * 2 + Identified;
                                battleState4 = false;
                                battleState5 = true;
                                enterWaiting = false;
                            }
                            //Devour attack
                            if (Keyboard.GetState().IsKeyUp(Keys.Enter) && battleUI2.cursorPosition == 2 && enterWaiting)
                            {
                                if (Macrophage.atp >= 7 && enemy.marked)
                                {
                                    Macrophage.damageOutput = Macrophage.level * 8 + Identified;

                                    Macrophage.atp -= 7;
                                    battleState4 = false;
                                    battleState5 = true;
                                }

                                enterWaiting = false;
                            }

                            //heal move
                            if (Keyboard.GetState().IsKeyUp(Keys.Enter) && battleUI2.cursorPosition == 3 && enterWaiting)
                            {
                                if (Macrophage.atp >= 4)
                                {
                                    Macrophage.healOutput = Macrophage.level * .5f;
                                    Macrophage.atp -= 4;
                                    battleState4 = false;
                                    battleState5 = true;
                                }
                                enterWaiting = false;
                            }
                            //Use Item
                            if (Keyboard.GetState().IsKeyUp(Keys.Enter) && battleUI2.cursorPosition == 4 && enterWaiting)
                            {
                                battleState4 = false;
                                battleState5 = true;
                                enterWaiting = false;
                            }
                        }

                        else if (!Macrophage.isAlive)
                        {
                            battleState4 = false;
                            battleState5 = true;
                        }
                    }
                    #endregion

                    #region Enemy's turn & damage calculated
                    if (battleState5)
                    {
                        if (firsttime)
                        {
                            turnNumber++;

                            //enemy damage chosen
                            enemy.damageOutput = rnd.Next(enemy.lowerDamage, enemy.upperDamage);

                            //enemy target chosen
                            //ooo
                            if (TCell.isAlive && BCell.isAlive && Macrophage.isAlive)
                                enemy.target = rnd.Next(1, 4);
                            //oox
                            if (TCell.isAlive && BCell.isAlive && !Macrophage.isAlive)
                                enemy.target = rnd.Next(1, 3);
                            //xoo
                            if (!TCell.isAlive && BCell.isAlive && Macrophage.isAlive)
                                enemy.target = rnd.Next(2, 2);
                            //oxo
                            if (TCell.isAlive && !BCell.isAlive && Macrophage.isAlive)
                                enemy.target = rnd.Next(5, 7);
                            //oxx
                            if (TCell.isAlive && !BCell.isAlive && !Macrophage.isAlive)
                                enemy.target = 1;
                            //xox
                            if (!TCell.isAlive && BCell.isAlive && !Macrophage.isAlive)
                                enemy.target = 2;
                            //xxo
                            if (!TCell.isAlive && !BCell.isAlive && Macrophage.isAlive)
                                enemy.target = 3;
                            //xxx
                            if (!TCell.isAlive && !BCell.isAlive && !Macrophage.isAlive)
                                enemy.target = 4;

                            //damage calculator
                            enemy.health -= TCell.damageOutput;
                            enemy.health -= BCell.damageOutput;
                            enemy.health -= Macrophage.damageOutput;

                            //enemy dies
                            if (enemy.health <= 0)
                            {
                                enemy.isAlive = false;

                                //exp awarded
                                if (TCell.isAlive)
                                    TCell.exp += rnd.Next(enemy.lowerExp, enemy.upperExp);
                                if (BCell.isAlive)
                                    BCell.exp += rnd.Next(enemy.lowerExp, enemy.upperExp);
                                if (Macrophage.isAlive)
                                    Macrophage.exp += rnd.Next(enemy.lowerExp, enemy.upperExp);

                                //lv. increases
                                if (TCell.exp >= TCell.expToNextLevel)
                                {
                                    TCell.level++;
                                    TCell.exp = 0;
                                    TCell.expToNextLevel = (int)TCell.expToNextLevel * .3f;
                                    TCell.expToNextLevel = (int)TCell.expToNextLevel;
                                }
                                if (BCell.exp >= BCell.expToNextLevel)
                                {
                                    BCell.level++;
                                    BCell.exp = 0;
                                    BCell.expToNextLevel = (int)BCell.expToNextLevel * .3f;
                                    BCell.expToNextLevel = (int)BCell.expToNextLevel;
                                }
                                if (Macrophage.exp >= Macrophage.expToNextLevel)
                                {
                                    Macrophage.level++;
                                    Macrophage.exp = 0;
                                    Macrophage.expToNextLevel = (int)Macrophage.expToNextLevel * .3f;
                                    Macrophage.expToNextLevel = (int)Macrophage.expToNextLevel;
                                }
                            }

                            //enemy's damage done
                            if (enemy.isAlive)
                            {
                                if (enemy.target == 4)
                                    enemy.health += enemy.healOutput;
                                if (enemy.target == 1 || enemy.target == 5)
                                    TCell.health -= enemy.damageOutput;
                                if (enemy.target == 2)
                                    BCell.health -= enemy.damageOutput;
                                if (enemy.target == 3 || enemy.target == 6)
                                    Macrophage.health -= enemy.damageOutput;
                            }

                            TCell.health += (int)TCell.healOutput;
                            BCell.health += (int)BCell.healOutput;
                            Macrophage.health += (int)Macrophage.healOutput;

                            //health bounderies
                            if (enemy.health < 0)
                                enemy.health = 0;
                            if (TCell.health < 0)
                                TCell.health = 0;
                            if (BCell.health < 0)
                                BCell.health = 0;
                            if (Macrophage.health < 0)
                                Macrophage.health = 0;
                            if (enemy.health > enemy.maxHealth)
                                enemy.health = enemy.maxHealth;
                            if (TCell.health > TCell.maxHealth)
                                TCell.health = TCell.maxHealth;
                            if (BCell.health > BCell.maxHealth)
                                BCell.health = BCell.maxHealth;
                            if (Macrophage.health > Macrophage.maxHealth)
                                Macrophage.health = Macrophage.maxHealth;

                            //hero undates
                            TCell.Update(gameTime);
                            BCell.Update(gameTime);
                            Macrophage.Update(gameTime);

                            //
                            battleMeasageNumber = 1;
                            firsttime = false;
                        }

                        //gameover
                        if (!TCell.isAlive && !BCell.isAlive && !Macrophage.isAlive)
                        {
                            gameState = Gamestate.Gameover;
                            enemy.target = 0;
                        }

                        //display measages
                        if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                            enterWaiting = true;

                        if (Keyboard.GetState().IsKeyUp(Keys.Enter) && enterWaiting && battleMeasageNumber != 4)
                        {
                            battleMeasageNumber++;
                            enterWaiting = false;
                        }

                        if (battleMeasageNumber > 4)
                            battleMeasageNumber = 4;

                        //moving back to battlestate1
                        if (Keyboard.GetState().IsKeyUp(Keys.Enter) && battleMeasageNumber == 4 && enterWaiting)
                        {
                            //atp regen
                            if (TCell.atp < 10)
                                TCell.atp += 1;
                            if (BCell.atp < 10)
                                BCell.atp += 1;
                            if (Macrophage.atp < 10)
                                Macrophage.atp += 1;

                            battleState5 = false;
                            battleState1 = true;
                            enterWaiting = false;
                        }
                    }
                    #endregion

                    #region checking enemy
                    if (battleState6)
                    {
                        battleUI1.cursor.position.Y = 290;
                        battleUI1.cursor.Update(gameTime);

                        if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                            enterWaiting = true;

                        if (Keyboard.GetState().IsKeyUp(Keys.Enter) && enterWaiting)
                        {
                            battleState1 = true;
                            battleState6 = false;
                            enterWaiting = false;
                        }
                    }
                    #endregion

                    break;
                #endregion

                #region Gamestate.Pause

                case Gamestate.Pause:

                    #region InPauseMenu

                    if (inPauseMenu)
                    {
                        pauseUI.Update(gameTime);

                        //push
                        if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                            enterWaiting = true;

                        //release
                        if (Keyboard.GetState().IsKeyUp(Keys.Enter) && enterWaiting)
                        {
                            enterWaiting = false;

                            switch (pauseUI.cursorPosition)
                            {
                                case 1:
                                    pauseUI.cursor.position = new Vector2(328, 383);
                                    inTeamMenu = true;
                                    inPauseMenu = false;
                                    break;

                                case 2:
                                    inItemMenu = true;
                                    inPauseMenu = false;
                                    break;

                                case 3:
                                    gameState = Gamestate.Overworld;
                                    break;

                                case 4:
                                    Exit();
                                    break;
                            }
                        }
                    }
                    #endregion

                    #region InTeamMenu
                    //In Team Menu
                    if (inTeamMenu)
                    {
                        pauseUI.cursor.position = new Vector2(328, 383);
                        pauseUI.cursor.Update(gameTime);

                        //push
                        if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                            enterWaiting = true;

                        //release
                        if (Keyboard.GetState().IsKeyUp(Keys.Enter) && enterWaiting)
                        {
                            inTeamMenu = false;
                            inPauseMenu = true;
                            enterWaiting = false;
                        }
                    }
                    #endregion

                    #region InItemMenu

                    if (inItemMenu)
                    {
                        //push
                        if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                            enterWaiting = true;

                        //release
                        if (Keyboard.GetState().IsKeyUp(Keys.Enter) && enterWaiting)
                        {
                            inTeamMenu = false;
                            inPauseMenu = true;
                            enterWaiting = false;
                        }
                    }
                    #endregion

                    break;

                #endregion

                #region Ganestate.GameOver
                case Gamestate.Gameover:

                    Exit();

                    break;
                #endregion

                #region Gamestate.End
                case Gamestate.End:

                    MediaPlayer.Stop();

                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                        enterWaiting = true;
                    if (Keyboard.GetState().IsKeyUp(Keys.Enter) && enterWaiting)
                    {
                        enterWaiting = false;
                        endText++;
                    }

                    if (endText >= 6)
                    {
                        gameState = Gamestate.Start;
                        baseColor.R = 1; baseColor.B = 1; baseColor.G = 1;
                        AliquisRectangle = new Rectangle(GraphicsDevice.Viewport.Width / 2 - 115, GraphicsDevice.Viewport.Height / 2 - 100, 225, 200);
                        fadein = true;
                    }
                    break;
                    #endregion
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(backgroundColor);

            //drawing spriteBatch
            spriteBatch.Begin();

            #region Gamestate.Loading

            if (gameState == Gamestate.Start)
            {
                if (baseColor.R >= 2 && baseColor.B >= 2 && baseColor.G >= 2)
                    spriteBatch.Draw(AliquisTexture, AliquisRectangle, baseColor);
            }

            #endregion

            #region Gamestate.Title
            if (gameState == Gamestate.Title)
            {
                titleScreen.Draw(spriteBatch);

                spriteBatch.DrawString(CourierNew, "Ethan Genser", new Vector2(5, 465), Color.SlateGray, 0f, new Vector2(0, 0), .35f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(CourierNew, "V. 0.4.8.4", new Vector2(750, 465), Color.SlateGray, 0f, new Vector2(0, 0), .35f, SpriteEffects.None, 0f);

                if (visible)
                    spriteBatch.DrawString(CourierNew, "Press Enter", new Vector2(358, 270), Color.LightGray, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
            }
            #endregion

            #region Gamestate.Overworld
            if (gameState == Gamestate.Overworld)
            {
                if (!youHealedText)
                {
                    //general
                    GraphicsDevice.Clear(Color.DarkRed);
                    backGround.Draw(spriteBatch);
                    bottomWall.Draw(spriteBatch);
                    topWall.Draw(spriteBatch);
                    player.Draw(spriteBatch);

                    #region drawing red blood cells
                    switch (map.room)
                    {
                        //room 1 redbloodcells
                        case 1:
                            foreach (RedBloodcell r in redBloodcells1)
                                r.Draw(spriteBatch);
                            break;

                        //room 2 redbloodcells
                        case 2:
                            foreach (RedBloodcell r in redBloodcells2)
                                r.Draw(spriteBatch);
                            break;

                        //room 3 redbloodcells
                        case 3:
                            foreach (RedBloodcell r in redBloodcells3)
                                r.Draw(spriteBatch);
                            break;

                        //room 4 redbloodcells
                        case 4:
                            foreach (RedBloodcell r in redBloodcells4)
                                r.Draw(spriteBatch);

                            switch (map.stage)
                            {
                                case 1:
                                    OverWorldRhinoVirus.Draw(spriteBatch);
                                    break;
                                case 2:
                                    OverWorldInfluenza.Draw(spriteBatch);
                                    break;
                                case 3:
                                    OverWorldEcoli.Draw(spriteBatch);
                                    break;
                                case 4:
                                    OverWorldMalaria.Draw(spriteBatch);
                                    break;
                                case 5:
                                    OverWorldCancer.Draw(spriteBatch);
                                    break;
                            }
                            break;
                    }
                    #endregion

                    #region tutorials
                    if (tutorial)
                    {
                        switch (tutorialNumber)
                        {
                            case 1:
                                if (tutorialSubNumber == 1)
                                {
                                    TextBoxMethod(true, 1, Color.Black, "Welcome to the world of Infection, young T-Cell!", spriteBatch, Textbox, CourierNew, screenHieght);
                                    TextBoxMethod(false, 2, Color.Black, "I see you and your friends, B-Cell and Macrophage, have arived just in time", spriteBatch, Textbox, CourierNew, screenHieght);
                                    TextBoxMethod(false, 3, Color.Black, "to help fight off the pathogen! ", spriteBatch, Textbox, CourierNew, screenHieght);
                                }
                                if (tutorialSubNumber == 2)
                                {
                                    TextBoxMethod(true, 1, Color.Black, "I have heard rumors that there is a pathogen with mutated DNA.", spriteBatch, Textbox, CourierNew, screenHieght);
                                    TextBoxMethod(false, 2, Color.Black, "If you can kill that, the rest of the pathogen will probably leave.", spriteBatch, Textbox, CourierNew, screenHieght);
                                    TextBoxMethod(false, 3, Color.Black, "Now hurry! We don't have time to lose!", spriteBatch, Textbox, CourierNew, screenHieght);
                                }
                                if (tutorialSubNumber == 3)
                                {
                                    TextBoxMethod(true, 1, Color.DarkRed, "-Stage 1- ", spriteBatch, Textbox, CourierNew, screenHieght);
                                    TextBoxMethod(false, 2, Color.DarkRed, "Infection: The Common Cold", spriteBatch, Textbox, CourierNew, screenHieght);
                                    TextBoxMethod(false, 3, Color.DarkRed, "Pathogen: RhinoVirus (Virus)", spriteBatch, Textbox, CourierNew, screenHieght);
                                }
                                break;

                            case 2:
                                if (tutorialSubNumber == 1)
                                {
                                    TextBoxMethod(true, 1, Color.Black, "Great job with that Mutant RhinoVirus. It apears as though my", spriteBatch, Textbox, CourierNew, screenHieght);
                                    TextBoxMethod(false, 2, Color.Black, "theory was correct. Killing the Mutant pathogen stops the other", spriteBatch, Textbox, CourierNew, screenHieght);
                                    TextBoxMethod(false, 3, Color.Black, "pathogen from receiving the DNA resistant to medicine.", spriteBatch, Textbox, CourierNew, screenHieght);
                                }
                                if (tutorialSubNumber == 2)
                                {
                                    TextBoxMethod(true, 1, Color.Black, "You should hurry now, the infections are getting more and more", spriteBatch, Textbox, CourierNew, screenHieght);
                                    TextBoxMethod(false, 2, Color.Black, "severe...", spriteBatch, Textbox, CourierNew, screenHieght);
                                }
                                if (tutorialSubNumber == 3)
                                {
                                    TextBoxMethod(true, 1, Color.DarkRed, "-Stage 2- ", spriteBatch, Textbox, CourierNew, screenHieght);
                                    TextBoxMethod(false, 2, Color.DarkRed, "Infection: The Flu", spriteBatch, Textbox, CourierNew, screenHieght);
                                    TextBoxMethod(false, 3, Color.DarkRed, "Pathogen: Influenza (Virus)", spriteBatch, Textbox, CourierNew, screenHieght);
                                }
                                break;

                            case 3:
                                if (tutorialSubNumber == 1)
                                {
                                    TextBoxMethod(true, 1, Color.DarkRed, "-Stage 3- ", spriteBatch, Textbox, CourierNew, screenHieght);
                                    TextBoxMethod(false, 2, Color.DarkRed, "Infection: E. Coli infection", spriteBatch, Textbox, CourierNew, screenHieght);
                                    TextBoxMethod(false, 3, Color.DarkRed, "Pathogen: E. Coli (Bacteria)", spriteBatch, Textbox, CourierNew, screenHieght);
                                }
                                break;

                            case 4:
                                if (tutorialSubNumber == 1)
                                {
                                    TextBoxMethod(true, 1, Color.DarkRed, "-Stage 4- ", spriteBatch, Textbox, CourierNew, screenHieght);
                                    TextBoxMethod(false, 2, Color.DarkRed, "Infection: Malaria infection", spriteBatch, Textbox, CourierNew, screenHieght);
                                    TextBoxMethod(false, 3, Color.DarkRed, "Pathogen: Malaria (Parasite)", spriteBatch, Textbox, CourierNew, screenHieght);
                                }
                                break;
                            case 5:
                                if (tutorialSubNumber == 1)
                                {
                                    TextBoxMethod(true, 1, Color.Black, "Oh no...", spriteBatch, Textbox, CourierNew, screenHieght);
                                    TextBoxMethod(false, 2, Color.Black, "I.. I never thought it would come to this...", spriteBatch, Textbox, CourierNew, screenHieght);
                                }
                                if (tutorialSubNumber == 2)
                                {
                                    TextBoxMethod(true, 1, Color.Black, "You have to go!", spriteBatch, Textbox, CourierNew, screenHieght);
                                    TextBoxMethod(false, 2, Color.Black, "Now!", spriteBatch, Textbox, CourierNew, screenHieght);
                                }
                                if (tutorialSubNumber == 3)
                                {
                                    TextBoxMethod(true, 1, Color.DarkRed, "-Stage 5- ", spriteBatch, Textbox, CourierNew, screenHieght);
                                    TextBoxMethod(false, 2, Color.DarkRed, "Infection: Cancer", spriteBatch, Textbox, CourierNew, screenHieght);
                                    TextBoxMethod(false, 3, Color.DarkRed, "Pathogen: ...", spriteBatch, Textbox, CourierNew, screenHieght);
                                }
                                break;
                        }
                    }
                    #endregion
                }

                #region Overworld measages (not tutorial)

                if (youHealedText)
                {
                    blackscreen.Draw(spriteBatch);
                    spriteBatch.DrawString(CourierNew, "Your team fully healed in the bone marrow.", new Vector2(200, 200), Color.White, 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0f);
                }
                #endregion
            }
            #endregion

            #region Gamestate.Battle
            if (gameState == Gamestate.Battle)
            {
                GraphicsDevice.Clear(Color.DarkRed);

                battleUI1.Draw(spriteBatch);
                enemy.sprite.Draw(spriteBatch);
                fader.Draw(spriteBatch);

                spriteBatch.DrawString(CourierNew, enemy.name, new Vector2(enemy.nameX, 23), Color.Black, 0f, new Vector2(0, 0), .35f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(CourierNew, "T-Cell", new Vector2(89, 343), Color.Black, 0f, new Vector2(0, 0), .35f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(CourierNew, "B-Cell", new Vector2(295, 343), Color.Black, 0f, new Vector2(0, 0), .35f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(CourierNew, "Macrophage", new Vector2(495, 343), Color.Black, 0f, new Vector2(0, 0), .35f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(CourierNew, "H P:", new Vector2(40, 380), Color.Black, 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(CourierNew, "ATP:", new Vector2(40, 437), Color.Black, 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(CourierNew, "H P:", new Vector2(255, 380), Color.Black, 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(CourierNew, "ATP:", new Vector2(255, 437), Color.Black, 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(CourierNew, "H P:", new Vector2(455, 380), Color.Black, 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(CourierNew, "ATP:", new Vector2(455, 437), Color.Black, 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(CourierNew, TCell.health + " / " + TCell.maxHealth, new Vector2(100, 380), Color.Black, 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(CourierNew, TCell.atp + " / 10", new Vector2(100, 437), Color.Black, 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(CourierNew, BCell.health + " / " + BCell.maxHealth, new Vector2(305, 380), Color.Black, 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(CourierNew, BCell.atp + " / 10", new Vector2(305, 437), Color.Black, 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(CourierNew, Macrophage.health + " / " + Macrophage.maxHealth, new Vector2(505, 380), Color.Black, 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(CourierNew, Macrophage.atp + " / 10", new Vector2(505, 437), Color.Black, 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(CourierNew, "Turn number", new Vector2(674, 388), Color.Black, 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(CourierNew, turnNumber.ToString(), new Vector2(720, 413), Color.Black, 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0f);

                #region healthbar

                x = 1;
                x = (float)enemy.health / (float)enemy.maxHealth;
                x = x * 250;

                healthbar.rectangle.Width = (int)x;
                healthbar.Draw(spriteBatch);

                #endregion

                #region What will you do
                //What will you do
                if (battleState1)
                {
                    battleUI1.cursor.Draw(spriteBatch);

                    spriteBatch.DrawString(CourierNew, "Fight", new Vector2(703, 70), Color.Black, 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(CourierNew, "Check", new Vector2(703, 180), Color.Black, 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(CourierNew, "Flee", new Vector2(703, 290), Color.Black, 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(CourierNew, "What will you do", new Vector2(667, 9), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                }
                #endregion

                #region Hero1 fights
                //Hero1 fights
                if (battleState2)
                {
                    battleUI2.cursor.Draw(spriteBatch);

                    spriteBatch.DrawString(CourierNew, "Attack", new Vector2(703, 70), Color.Black, 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(CourierNew, "Cost: 0 ATP", new Vector2(669, 90), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(CourierNew, "Identify", new Vector2(703, 140), Color.Black, 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(CourierNew, "Cost: 5 ATP", new Vector2(669, 160), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(CourierNew, "Heal", new Vector2(703, 220), Color.Black, 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(CourierNew, "Cost: 4 ATP", new Vector2(669, 240), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(CourierNew, "Item", new Vector2(703, 290), Color.Black, 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(CourierNew, "T-Cell", new Vector2(708, 9), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                }
                #endregion

                #region Hero2 fights
                //Hero2 fights
                if (battleState3)
                {
                    battleUI2.cursor.Draw(spriteBatch);

                    spriteBatch.DrawString(CourierNew, "Attack", new Vector2(703, 70), Color.Black, 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(CourierNew, "Cost: 0 ATP", new Vector2(669, 90), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(CourierNew, "Antibodies", new Vector2(703, 140), Color.Black, 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(CourierNew, "Cost: 3 ATP", new Vector2(669, 160), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(CourierNew, "Fever", new Vector2(703, 220), Color.Black, 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(CourierNew, "Cost: 3 ATP", new Vector2(669, 240), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(CourierNew, "Item", new Vector2(703, 290), Color.Black, 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(CourierNew, "B-Cell", new Vector2(708, 9), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                }
                #endregion

                #region Hero3 fights
                //Hero3 fights
                if (battleState4)
                {
                    battleUI2.cursor.Draw(spriteBatch);

                    spriteBatch.DrawString(CourierNew, "Attack", new Vector2(703, 70), Color.Black, 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(CourierNew, "Cost: 0 ATP", new Vector2(669, 90), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(CourierNew, "Devour", new Vector2(703, 140), Color.Black, 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(CourierNew, "Cost: 7 ATP", new Vector2(669, 160), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(CourierNew, "Heal", new Vector2(703, 220), Color.Black, 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(CourierNew, "Cost: 4 ATP", new Vector2(669, 240), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(CourierNew, "Item", new Vector2(703, 290), Color.Black, 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(CourierNew, "Macrophage", new Vector2(692, 9), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                }
                #endregion

                #region Battle messages displayed
                //Enemy attacks & damage is displayed
                if (battleState5)
                {
                    //tcell
                    if (battleMeasageNumber == 1 && !TCell.isAlive)
                    {
                        spriteBatch.DrawString(CourierNew, "T-Cell cannot", new Vector2(669, 120), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(CourierNew, "attack.", new Vector2(669, 160), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                    }
                    else if (battleMeasageNumber == 1 && TCell.damageOutput > 0)
                    {
                        spriteBatch.DrawString(CourierNew, "T-Cell did", new Vector2(669, 120), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(CourierNew, TCell.damageOutput + " damage.", new Vector2(669, 160), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                    }
                    else if (battleMeasageNumber == 1 && TCell.healOutput > 0)
                    {
                        spriteBatch.DrawString(CourierNew, "T-Cell healed", new Vector2(669, 120), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(CourierNew, TCell.healOutput + " HP.", new Vector2(669, 160), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                    }
                    else if (battleMeasageNumber == 1)
                    {
                        spriteBatch.DrawString(CourierNew, "The enemy was", new Vector2(669, 120), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(CourierNew, "recognised by", new Vector2(669, 160), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(CourierNew, "T-Cell.", new Vector2(669, 200), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                    }

                    //bcell
                    if (battleMeasageNumber == 2 && !BCell.isAlive)
                    {
                        spriteBatch.DrawString(CourierNew, "B-Cell cannot", new Vector2(669, 120), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(CourierNew, "attack.", new Vector2(669, 160), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                    }

                    else if (battleMeasageNumber == 2 && BCell.damageOutput > 0)
                    {
                        spriteBatch.DrawString(CourierNew, "B-Cell did", new Vector2(669, 120), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(CourierNew, BCell.damageOutput + " damage.", new Vector2(669, 160), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                    }

                    else if (battleMeasageNumber == 2 && enemy.marked)
                    {
                        spriteBatch.DrawString(CourierNew, "B-Cell marked", new Vector2(669, 120), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(CourierNew, "the enemy with", new Vector2(669, 160), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(CourierNew, "antibodies.", new Vector2(669, 200), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                    }

                    //macrophage
                    if (battleMeasageNumber == 3 && !Macrophage.isAlive)
                    {
                        spriteBatch.DrawString(CourierNew, "Macrophage", new Vector2(669, 120), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(CourierNew, "cannot attack.", new Vector2(669, 160), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                    }
                    else if (battleMeasageNumber == 3 && Macrophage.damageOutput > 0)
                    {
                        spriteBatch.DrawString(CourierNew, "Macrophage did", new Vector2(669, 120), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(CourierNew, Macrophage.damageOutput + " damage.", new Vector2(669, 160), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                    }
                    if (battleMeasageNumber == 3 && Macrophage.healOutput > 0)
                    {
                        spriteBatch.DrawString(CourierNew, "Macrophage", new Vector2(669, 120), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(CourierNew, "healed " + Macrophage.healOutput + " HP.", new Vector2(669, 160), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                    }

                    //enemy
                    if (battleMeasageNumber == 4 && !enemy.isAlive)
                    {
                        spriteBatch.DrawString(CourierNew, "You have", new Vector2(669, 120), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(CourierNew, "defeated the", new Vector2(669, 160), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(CourierNew, enemy.name + "!", new Vector2(669, 200), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                    }
                    if (battleMeasageNumber == 4 && enemy.damageOutput > 0 && enemy.isAlive)
                    {
                        spriteBatch.DrawString(CourierNew, enemy.name + " did", new Vector2(669, 120), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(CourierNew, enemy.damageOutput + " damage to", new Vector2(669, 160), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                        if (enemy.target == 1 || enemy.target == 5)
                            spriteBatch.DrawString(CourierNew, "T-Cell.", new Vector2(669, 200), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                        if (enemy.target == 2)
                            spriteBatch.DrawString(CourierNew, "B-Cell.", new Vector2(669, 200), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                        if (enemy.target == 3 || enemy.target == 6)
                            spriteBatch.DrawString(CourierNew, "Macrophage.", new Vector2(669, 200), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                    }
                    if (battleMeasageNumber == 4 && enemy.target == 4 && enemy.isAlive)
                    {
                        spriteBatch.DrawString(CourierNew, enemy.name, new Vector2(669, 120), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(CourierNew, "healed " + enemy.healOutput, new Vector2(669, 160), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(CourierNew, "HP. " + enemy.healOutput, new Vector2(669, 200), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                    }
                }
                #endregion

                #region Checking enemy
                //check
                if (battleState6)
                {
                    spriteBatch.DrawString(CourierNew, "Checking enemy...", new Vector2(668, 9), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);

                    spriteBatch.DrawString(CourierNew, "Name: " + enemy.name, new Vector2(669, 50), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(CourierNew, "HP: " + enemy.health + "/" + enemy.maxHealth, new Vector2(669, 100), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(CourierNew, "Back", new Vector2(669, 290), Color.Black, 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0f);

                    switch (map.stage)
                    {
                        case 1:
                            spriteBatch.DrawString(CourierNew, "It seems to like", new Vector2(669, 150), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                            spriteBatch.DrawString(CourierNew, "being called", new Vector2(669, 170), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                            spriteBatch.DrawString(CourierNew, "'UnicornVirus'", new Vector2(669, 190), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                            break;

                        case 2:
                            spriteBatch.DrawString(CourierNew, "It's a self-", new Vector2(669, 150), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                            spriteBatch.DrawString(CourierNew, "proclaimed", new Vector2(669, 170), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                            spriteBatch.DrawString(CourierNew, "master in", new Vector2(669, 190), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                            spriteBatch.DrawString(CourierNew, "the art of", new Vector2(669, 210), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                            spriteBatch.DrawString(CourierNew, "Kung Flu.", new Vector2(669, 230), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                            break;

                        case 3:
                            spriteBatch.DrawString(CourierNew, "It prefers", new Vector2(669, 150), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                            spriteBatch.DrawString(CourierNew, "it's hamburgers", new Vector2(669, 170), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                            spriteBatch.DrawString(CourierNew, "on the rare", new Vector2(669, 190), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                            spriteBatch.DrawString(CourierNew, "side.", new Vector2(669, 210), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                            break;

                        case 4:
                            spriteBatch.DrawString(CourierNew, "It's making an", new Vector2(669, 150), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                            spriteBatch.DrawString(CourierNew, "bizarre face", new Vector2(669, 170), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                            spriteBatch.DrawString(CourierNew, "at you.", new Vector2(669, 190), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                            break;

                        case 5:
                            spriteBatch.DrawString(CourierNew, "Your worst", new Vector2(669, 150), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                            spriteBatch.DrawString(CourierNew, "nightmare...", new Vector2(669, 170), Color.Black, 0f, new Vector2(0, 0), .4f, SpriteEffects.None, 0f);
                            break;
                    }
                }
                #endregion

            }
            #endregion

            #region Gamestate.Pause

            if (gameState == Gamestate.Pause)
            {
                pauseUI.Draw(spriteBatch);

                pauseUI.cursor.Draw(spriteBatch);

                if (inPauseMenu)
                {
                    spriteBatch.DrawString(CourierNew, "Team", new Vector2(364, 75), Color.Black, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(CourierNew, "Items", new Vector2(358, 175), Color.Black, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(CourierNew, "Back to game", new Vector2(290, 275), Color.Black, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(CourierNew, "Exit", new Vector2(366, 375), Color.Black, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                }
                if (inTeamMenu)
                {
                    spriteBatch.DrawString(CourierNew, TCell.name + ":   Lv. " + TCell.level + "    HP: " + TCell.health + "/" + TCell.maxHealth + "     Exp: " + TCell.exp + "/" + TCell.expToNextLevel + " ", new Vector2(155, 75), Color.Black, 0f, new Vector2(0, 0), .7f, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(CourierNew, BCell.name + ":   Lv. " + BCell.level + "    HP: " + BCell.health + "/" + BCell.maxHealth + "     Exp: " + BCell.exp + "/" + BCell.expToNextLevel + " ", new Vector2(155, 175), Color.Black, 0f, new Vector2(0, 0), .7f, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(CourierNew, Macrophage.name + ":   Lv. " + Macrophage.level + "    HP: " + Macrophage.health + "/" + Macrophage.maxHealth + "     Exp: " + Macrophage.exp + "/" + Macrophage.expToNextLevel + " ", new Vector2(120, 275), Color.Black, 0f, new Vector2(0, 0), .7f, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(CourierNew, "Exit", new Vector2(366, 375), Color.Black, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                }
            }

            #endregion

            #region Gamestate.End

            if (gameState == Gamestate.End)
                switch (endText)
                {
                    case 1:
                        spriteBatch.DrawString(CourierNew, "I can't beleive it! H-how did you defeat it?", new Vector2(200, 200), Color.White, 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0f);
                        break;
                    case 2:
                        spriteBatch.DrawString(CourierNew, "       You have done the impossible!", new Vector2(200, 200), Color.White, 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0f);
                        break;
                    case 3:
                        spriteBatch.DrawString(CourierNew, "        You have defeated cancer.", new Vector2(200, 200), Color.White, 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0f);
                        break;
                    case 4:
                        spriteBatch.DrawString(CourierNew, "             Congratulations...", new Vector2(200, 200), new Color(255, 0, 0), 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0f);
                        break;
                    case 5:
                        spriteBatch.DrawString(CourierNew, "             Congratulations...", new Vector2(200, 200), new Color(255, 0, 0), 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(CourierNew, "         ...and thanks for playing!", new Vector2(200, 230), new Color(255, 0, 0), 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0f);
                        spriteBatch.DrawString(CourierNew, "                      -Ethan Genser", new Vector2(200, 270), new Color(255, 0, 0), 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0f);
                        break;
                }

            #endregion

            spriteBatch.End();

            #region G.D.S.

            //Base Drawer
            base.Draw(gameTime);

            #endregion

        }

        //Textbox custom method
        static void TextBoxMethod(bool clear, int speechY, Color color, string meassage, SpriteBatch spriteBatch, Sprite Textbox, SpriteFont CourierNew, int screenHieght)
        {
            //X coords for text to start
            int speechX = 20;

            //Y coord options
            switch (speechY)
            {
                case 1:
                    speechY = screenHieght / 8 * 6 + 20;
                    break;
                case 2:
                    speechY = screenHieght / 8 * 6 + 50;
                    break;
                case 3:
                    speechY = screenHieght / 8 * 6 + 80;
                    break;
            }

            //clears old text by redrawing textbox
            if (clear)
                Textbox.Draw(spriteBatch);

            //prints text
            string[] MeassageArray = meassage.Select(x => x.ToString()).ToArray();

            foreach (string character in MeassageArray)
            {
                spriteBatch.DrawString(CourierNew, character, new Vector2(speechX, speechY), color, 0f, Vector2.Zero, .4f, SpriteEffects.None, 0f);
                speechX += 10;
            }
        }

        //Divisable custom method
        static bool IsDivisable(int value, int denominator)
        {
            return value % denominator == 0;
        }
    }
}

// Programming by: Ethan Genser
// Sprites & Visuals by: Ethan Genser
// Music made by: Brennan Sprague

