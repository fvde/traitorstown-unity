using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traitorstown.src.game.state;
using Traitorstown.src.http;
using UnityEngine;

namespace Traitorstown.src.game
{
    public class GameManager : MonoBehaviour
    {
        public GameState GameState;

        void Start()
        {
            GameState = new StartingGame();
            HttpRequestService.Instance.RequestUnsuccessful += HandleRequestUnsuccessful;
        }

        public void Update()
        {
            if (GameState.IsReadyToTransition(Time.deltaTime))
            {
                GameState = GameState.Transition(GameStorage.Instance, this);
            }
        }

        public void Play()
        {
            GameStorage.Instance.Reset();
            GameState = new LookingForGame();
        }

        public void EndGame()
        {
            GameStorage.Instance.Reset();
            GameObjectFactory.Instance.DestroyAll();
            GameService.Instance.EndGame();
        }

        private void HandleStateChanged(object sender, EventArgs e)
        {

        }

        private void HandleRequestUnsuccessful(object sender, RequestResponse e)
        {
            Debug.Log("Request failed with respone " + e.ToString());
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
            StartCoroutine(GameService.Instance.GetCards());
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
