using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Traitorstown.src.model
{
    public class Game
    {
        public int Id { get; }
        public GameStatus Status { get; }
        public int Turn { get; }
        public Role Winner { get; }
        public List<Player> Players { get; }

        public Game(int id, GameStatus status, int turn, Role winner, List<Player> players)
        {
            Id = id;
            Status = status;
            Turn = turn;
            Winner = winner;
            Players = players;
        }

        public int GetReadyPlayerCount()
        {
            return Players.FindAll(player => player.Ready).Count;
        }
    }
}

