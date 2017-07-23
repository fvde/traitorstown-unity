using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traitorstown.src.model;
using UnityEngine;

namespace Traitorstown.src.game
{
    public class GameState
    {
        private readonly string PLAYER_ID_IDENTIFIER = "playerId";
        private readonly string GAME_ID_IDENTIFIER = "gameId";
        private readonly string TURN_COUNTER_IDENTIFIER = "turnCounter";

        private static GameState instance = new GameState();

        public static GameState Instance
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

        public int? GameId
        {
            get
            {
                if (PlayerPrefs.HasKey(GAME_ID_IDENTIFIER))
                {
                    return PlayerPrefs.GetInt(GAME_ID_IDENTIFIER);

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
                    PlayerPrefs.SetInt(GAME_ID_IDENTIFIER, value.Value);
                }
                else
                {
                    PlayerPrefs.DeleteKey(GAME_ID_IDENTIFIER);
                }
            }
        }

        public int? TurnCounter
        {
            get
            {
                if (PlayerPrefs.HasKey(TURN_COUNTER_IDENTIFIER))
                {
                    return PlayerPrefs.GetInt(TURN_COUNTER_IDENTIFIER);

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
                    PlayerPrefs.SetInt(TURN_COUNTER_IDENTIFIER, value.Value);
                }
                else
                {
                    PlayerPrefs.DeleteKey(TURN_COUNTER_IDENTIFIER);
                }
            }
        }

        public List<Player> Players { get; set; }
        public List<Card> Cards { get; set; }
        public List<Resource> Resources { get; set; }

        private GameState()
        {
            Players = new List<Player>();
            Cards = new List<Card>();
            Resources = new List<Resource>();
        }

        public void Reset()
        {
            Players.Clear();
            Cards.Clear();
            Resources.Clear();
            PlayerId = null;
            GameId = null;
            TurnCounter = null;
        }
    }
}
