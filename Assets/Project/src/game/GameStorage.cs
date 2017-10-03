using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traitorstown.src.game.state;
using Traitorstown.src.model;
using UnityEngine;

namespace Traitorstown.src.game
{
    public class GameStorage
    {
        public event EventHandler GameUpdated;
        private readonly string USERNAME_IDENTIFIER = "username";
        private readonly string PLAYER_ID_IDENTIFIER = "playerId";

        private static GameStorage instance = new GameStorage();

        public static GameStorage Instance
        {
            get
            {
                return instance;
            }
        }

        public int? PlayerId
        {
            get
            {
                if (PlayerPrefs.HasKey(PLAYER_ID_IDENTIFIER))
                {
                    return PlayerPrefs.GetInt(PLAYER_ID_IDENTIFIER);

                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (value.HasValue)
                {
                    PlayerPrefs.SetInt(PLAYER_ID_IDENTIFIER, value.Value);
                }
                else
                {
                    PlayerPrefs.DeleteKey(PLAYER_ID_IDENTIFIER);
                }
            }
        }

        public String UserName
        {
            get
            {
                return PlayerPrefs.GetString(USERNAME_IDENTIFIER);
            }
            set
            {
                if (value != null)
                {
                    PlayerPrefs.SetString(USERNAME_IDENTIFIER, value);
                }
                else
                {
                    PlayerPrefs.DeleteKey(USERNAME_IDENTIFIER);
                }
            }
        }

        public int? GameId { get; private set; }
        public GameState GameState { get; set; }
        public List<Game> OpenGames { get; set; }
        public List<Player> Players { get; private set; }
        public List<Card> Cards { get; set; }
        public List<Resource> Resources { get; private set; }
        private Game game;
        public Game Game
        {
            get { return game; }
            set
            {
                game = value;
                if (value != null)
                {
                    GameId = value.Id;
                    Players = value.Players;
                    Resources = value.Players.Find(p => p.Id == PlayerId).Resources;
                    GameUpdated?.Invoke(this, null);
                }
            }
        }

        private GameStorage()
        {
            Reset();
        }

        public void Reset()
        {
            GameState = new StartingGame();
            Players = new List<Player>();
            Cards = new List<Card>();
            Resources = new List<Resource>();
            OpenGames = new List<Game>();
            Game = null;
            GameId = null;
        }

        public void ResetUser()
        {
            UserName = null;
            PlayerId = null;
        }
    }
}
