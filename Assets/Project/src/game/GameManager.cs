using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traitorstown.src.game.state;
using UnityEngine;

namespace Traitorstown.src.game
{
    public class GameManager : MonoBehaviour
    {
        public GameState GameState;
        private GameStorage GameStorage = GameStorage.Instance;

        void Start()
        {
            GameState = new StartingGame();
            UpdateState();
            GameStorage.StateChanged += HandleStateChanged;
        }

        public void Play()
        {
            GameStorage.Reset();
            GameState = new LookingForGame();
            UpdateState();
        }

        public void UpdateState()
        {
            GameState = GameState.Transition(GameStorage, this);
        }

        private void HandleStateChanged(object sender, EventArgs e)
        {
            UpdateState();
        }

        internal void CreateNewGame()
        {
            StartCoroutine(GameService.Instance.CreateNewGame());
        }

        public void GetCurrentGame()
        {
            StartCoroutine(GameService.Instance.GetCurrentGame());
        }

        public void GetOpenGames()
        {
            StartCoroutine(GameService.Instance.GetOpenGames());
        }

        public void JoinGame(int gameId)
        {
            StartCoroutine(GameService.Instance.JoinGame(gameId));
        }

        public void LeaveGame()
        {
            StartCoroutine(GameService.Instance.LeaveGame());
        }

        public void SetReady()
        {
            StartCoroutine(GameService.Instance.SetReady());
        }

        public void SetNotReady()
        {
            StartCoroutine(GameService.Instance.SetNotReady());
        }

        public void GetCards()
        {
            StartCoroutine(GameService.Instance.LeaveGame());
        }

        public void Register(string username)
        {
            StartCoroutine(UserService.Instance.Register(username));
        }

        public void Login(string username)
        {
            StartCoroutine(UserService.Instance.Login(username));
        }
    }
}
