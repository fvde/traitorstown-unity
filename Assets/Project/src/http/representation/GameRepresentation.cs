using System;
using System.Collections;
using System.Collections.Generic;
using Traitorstown.src.model;
using UnityEngine;

namespace Traitorstown.src.http.representation
{
    [Serializable]
    public class GameRepresentation
    {
        public long id;
        public GameStatus status;
        public int turn;
        public Role winner;
        public PlayerRepresentation[] players;

        public GameRepresentation(long id, GameStatus status, int turn, Role winner, PlayerRepresentation[] players)
        {
            this.id = id;
            this.status = status;
            this.turn = turn;
            this.winner = winner;
            this.players = players;
        }

        public Game ToGame()
        {
            return new Game((int)id, status, turn, winner, new List<Player>(
                new List<PlayerRepresentation>(players).ConvertAll<Player>(player => player.ToPlayer())));
        }
    }
}

