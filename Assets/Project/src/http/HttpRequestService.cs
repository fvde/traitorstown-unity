using BestHTTP.ServerSentEvents;
using System;
using System.Collections;
using System.Collections.Generic;
using Traitorstown.src;
using Traitorstown.src.http;
using Traitorstown.src.http.representation;
using Traitorstown.src.http.request;
using Traitorstown.src.model;
using UnityEngine;
using UnityEngine.Networking;

namespace Traitorstown.src.http
{
    public class HttpRequestService
    {
        public event EventHandler<RequestResponse> RequestUnsuccessful;
        private EventSource EventSource;
        public string token = null;
        private readonly string TOKEN = "token";

        private static HttpRequestService instance = new HttpRequestService();

        public static HttpRequestService Instance
        {
            get
            {
                return instance;
            }
        }

        private HttpRequestService()
        {
            if (PlayerPrefs.HasKey(TOKEN))
            {
                token = PlayerPrefs.GetString(TOKEN);
            }
        }

        public void RegisterForServerSentEventsForGame(int game)
        {
            if (EventSource != null && (EventSource.State == States.Open || EventSource.State == States.Connecting))
            {
                return;
            }

            EventSource = new EventSource(new Uri(Configuration.API_URL + "/messages/" + game));
            EventSource.InternalRequest.SetHeader(TOKEN, token);
            EventSource.Open();
            EventSource.OnOpen += HandleSSEConnect;
            EventSource.OnMessage += HandleSSEMessage;
            EventSource.OnError += HandleSSEError;
        }

        private void HandleSSEError(EventSource eventSource, string error)
        {
            Debug.Log("Error: " + error);
        }

        private void HandleSSEMessage(EventSource eventSource, Message message)
        {
            Debug.Log("Received message " + message);
        }

        private void HandleSSEConnect(EventSource eventSource)
        {
            Debug.Log("Successfully connected to " + eventSource);
        }

        public void CloseServerSentEventsConnection()
        {
            EventSource.Close();
        }

        public IEnumerator GetCurrentGame(int playerId, Action<Game> responseHandler)
        {
            yield return GameRequest(UnityWebRequest.Get(Configuration.API_URL + HttpResources.DELIMITER + HttpResources.PLAYERS + HttpResources.DELIMITER + playerId + HttpResources.DELIMITER + HttpResources.GAMES),
                "GET",
                responseHandler,
                null);
        }

        public IEnumerator CreateNewGame(Action<Game> responseHandler)
        {
            yield return GameRequest(UnityWebRequest.Post(Configuration.API_URL + HttpResources.DELIMITER + HttpResources.GAMES, "{}"),
                "POST",
                responseHandler,
                null);
        }

        public IEnumerator GetOpenGames(Action<List<Game>> responseHandler)
        {
            yield return GameRequest(UnityWebRequest.Get(Configuration.API_URL + HttpResources.DELIMITER + HttpResources.GAMES + "?status=" + GameStatus.OPEN),
                "GET",
                null,
                responseHandler);
        }

        public IEnumerator JoinGame(int gameId, int playerId, Action<Game> responseHandler)
        {
            yield return GameRequest(UnityWebRequest.Put(Configuration.API_URL + HttpResources.DELIMITER + HttpResources.GAMES + HttpResources.DELIMITER + gameId + HttpResources.DELIMITER + HttpResources.PLAYERS, JsonUtility.ToJson(new PlayerRequest(playerId))),
                "POST",
                responseHandler,
                null);
        }

        public IEnumerator LeaveGame(int gameId, int playerId, Action<Game> responseHandler)
        {
            yield return GameRequest(UnityWebRequest.Put(Configuration.API_URL + HttpResources.DELIMITER + HttpResources.GAMES + HttpResources.DELIMITER + gameId + HttpResources.DELIMITER + HttpResources.PLAYERS + HttpResources.DELIMITER + playerId, "{}"),
                "DELETE",
                responseHandler,
                null);
        }

        public IEnumerator SetReady(int gameId, int playerId, bool ready, Action<Game> responseHandler)
        {
            yield return GameRequest(UnityWebRequest.Put(Configuration.API_URL + HttpResources.DELIMITER + HttpResources.GAMES + HttpResources.DELIMITER + gameId + HttpResources.DELIMITER + HttpResources.PLAYERS + HttpResources.DELIMITER + playerId, JsonUtility.ToJson(new PlayerReadyRequest(ready))),
                "PUT",
                responseHandler,
                null);
        }

        public IEnumerator GetCards(int gameId, int playerId, Action<List<Card>> responseHandler)
        {
            yield return CardRequest(UnityWebRequest.Get(Configuration.API_URL + HttpResources.DELIMITER + HttpResources.GAMES + HttpResources.DELIMITER + gameId + HttpResources.DELIMITER + HttpResources.PLAYERS + HttpResources.DELIMITER + playerId + HttpResources.DELIMITER + HttpResources.CARDS),
                "GET",
                null,
                responseHandler);
        }

        public IEnumerator GetTurn(int gameId, int turnCounter, Action<Turn> responseHandler)
        {
            yield return TurnRequest(UnityWebRequest.Get(Configuration.API_URL + HttpResources.DELIMITER + HttpResources.GAMES + HttpResources.DELIMITER + gameId + HttpResources.DELIMITER + HttpResources.TURNS + HttpResources.DELIMITER + turnCounter),
                "GET",
                responseHandler,
                null);
        }

        public IEnumerator PlayCard(int gameId, int turnCounter, int cardId, int targetPlayer, Action callback)
        {
            yield return MakeRequest(UnityWebRequest.Put(Configuration.API_URL + HttpResources.DELIMITER + HttpResources.GAMES + HttpResources.DELIMITER + gameId + HttpResources.DELIMITER + HttpResources.TURNS + HttpResources.DELIMITER + turnCounter + HttpResources.DELIMITER + HttpResources.CARDS, JsonUtility.ToJson(new CardRequest(cardId, targetPlayer))),
                "POST",
                null, 
                callback);
        }

        public IEnumerator Register(string email, string password, Action<int> responseHandler)
        {
            yield return UserRequest(HttpResources.DELIMITER + HttpResources.USERS + HttpResources.DELIMITER + HttpResources.REGISTER,
                            JsonUtility.ToJson(new RegistrationRequest(email, password)),
                            responseHandler);
        }

        public IEnumerator Login(string email, string password, Action<int> responseHandler)
        {
            yield return UserRequest(HttpResources.DELIMITER + HttpResources.USERS + HttpResources.DELIMITER + HttpResources.LOGIN,
                            JsonUtility.ToJson(new LoginRequest(email, password)),
                            responseHandler);
        }

        private IEnumerator GameRequest(UnityWebRequest request, string httpVerb, Action<Game> responseHandler, Action<List<Game>> multipleResponseHandler)
        {
            yield return MakeRequest(request, httpVerb, response =>
            {
                if (responseHandler != null)
                {
                    GameRepresentation result = JsonUtility.FromJson<GameRepresentation>(request.downloadHandler.text);
                    responseHandler(result.ToGame());
                }

                if (multipleResponseHandler != null)
                {
                    List<GameRepresentation> results = new List<GameRepresentation>(JsonHelper.getJsonArray<GameRepresentation>(request.downloadHandler.text));
                    multipleResponseHandler(results.ConvertAll(game => game.ToGame()));
                }
            });
        }

        private IEnumerator CardRequest(UnityWebRequest request, string httpVerb, Action<Card> responseHandler, Action<List<Card>> multipleResponseHandler)
        {
            yield return MakeRequest(request, httpVerb, response =>
            {
                if (responseHandler != null)
                {
                    CardRepresentation card = JsonUtility.FromJson<CardRepresentation>(request.downloadHandler.text);
                    responseHandler(card.ToCard());
                }

                if (multipleResponseHandler != null)
                {
                    List<CardRepresentation> results = new List<CardRepresentation>(JsonHelper.getJsonArray<CardRepresentation>(request.downloadHandler.text));
                    multipleResponseHandler(results.ConvertAll(card => card.ToCard()));
                }
            });
        }

        private IEnumerator TurnRequest(UnityWebRequest request, string httpVerb, Action<Turn> responseHandler, Action<List<Turn>> multipleResponseHandler)
        {
            yield return MakeRequest(request, httpVerb, response =>
            {
                if (responseHandler != null)
                {
                    TurnRepresentation result = JsonUtility.FromJson<TurnRepresentation>(request.downloadHandler.text);
                    responseHandler(result.ToTurn());
                }

                if (multipleResponseHandler != null)
                {
                    List<TurnRepresentation> results = new List<TurnRepresentation>(JsonHelper.getJsonArray<TurnRepresentation>(request.downloadHandler.text));
                    multipleResponseHandler(results.ConvertAll(turn => turn.ToTurn()));
                }
            });
        }

        private IEnumerator UserRequest(string path, string payload, Action<int> responseHandler)
        {
            UnityWebRequest request = UnityWebRequest.Put(
                Configuration.API_URL + path,
                payload);

            yield return MakeRequest(request, "POST", response =>
            {
                UserRepresentation result = JsonUtility.FromJson<UserRepresentation>(request.downloadHandler.text);

                if (result.token != null)
                {
                    token = result.token;
                    PlayerPrefs.SetString(TOKEN, token);
                }

                responseHandler((int)result.id);
            });
        }

        private IEnumerator MakeRequest(UnityWebRequest request, string httpVerb, Action<object> responseHandler = null, Action callback = null)
        {
            request.method = httpVerb;

            if (token != null)
            {
                request.SetRequestHeader(TOKEN, token);
            }

            request.SetRequestHeader("Content-Type", "application/json;charset=UTF-8");

            if (request.uploadHandler != null)
            {
                request.uploadHandler.contentType = "application/json;charset=UTF-8";
            }

            yield return request.Send();

            if (request.isNetworkError)
            {
                Debug.Log("Error: " + request.error);
                RequestUnsuccessful?.Invoke(this, new RequestResponse(-1, request.error));
            }
            else
            {
                Debug.Log("Response: " + request.responseCode + (request.downloadHandler != null ? " Details: " + request.downloadHandler.text : ""));
                if (request.responseCode == 200)
                {
                    responseHandler?.Invoke(request.downloadHandler.text);
                    callback?.Invoke();
                } else
                {
                    RequestUnsuccessful?.Invoke(this, new RequestResponse(request.responseCode, request.downloadHandler?.text));
                }
            }
        }
    }
}

