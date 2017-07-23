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

        public IEnumerator getCurrentGame(int playerId, Action<Game> responseHandler)
        {
            yield return gameRequest(UnityWebRequest.Get(Configuration.API_URL + HttpResources.DELIMITER + HttpResources.PLAYERS + HttpResources.DELIMITER + playerId + HttpResources.DELIMITER + HttpResources.GAMES),
                "GET",
                responseHandler,
                null);
        }

        public IEnumerator createNewGame(Action<Game> responseHandler)
        {
            yield return gameRequest(UnityWebRequest.Post(Configuration.API_URL + HttpResources.DELIMITER + HttpResources.GAMES, "{}"),
                "POST",
                responseHandler,
                null);
        }

        public IEnumerator getOpenGames(Action<List<Game>> responseHandler)
        {
            yield return gameRequest(UnityWebRequest.Get(Configuration.API_URL + HttpResources.DELIMITER + HttpResources.GAMES + "?status=" + GameStatus.OPEN),
                "GET",
                null,
                responseHandler);
        }

        public IEnumerator joinGame(int gameId, int playerId, Action<Game> responseHandler)
        {
            yield return gameRequest(UnityWebRequest.Put(Configuration.API_URL + HttpResources.DELIMITER + HttpResources.GAMES + HttpResources.DELIMITER + gameId + HttpResources.DELIMITER + HttpResources.PLAYERS, JsonUtility.ToJson(new PlayerRequest(playerId))),
                "POST",
                responseHandler,
                null);
        }

        public IEnumerator leaveGame(int gameId, int playerId, Action<Game> responseHandler)
        {
            yield return gameRequest(UnityWebRequest.Put(Configuration.API_URL + HttpResources.DELIMITER + HttpResources.GAMES + HttpResources.DELIMITER + gameId + HttpResources.DELIMITER + HttpResources.PLAYERS + HttpResources.DELIMITER + playerId, "{}"),
                "DELETE",
                responseHandler,
                null);
        }

        public IEnumerator setReady(int gameId, int playerId, bool ready, Action<Game> responseHandler)
        {
            yield return gameRequest(UnityWebRequest.Put(Configuration.API_URL + HttpResources.DELIMITER + HttpResources.GAMES + HttpResources.DELIMITER + gameId + HttpResources.DELIMITER + HttpResources.PLAYERS + HttpResources.DELIMITER + playerId, JsonUtility.ToJson(new PlayerReadyRequest(ready))),
                "PUT",
                responseHandler,
                null);
        }

        public IEnumerator getCards(int gameId, int playerId, Action<List<Card>> responseHandler)
        {
            yield return cardRequest(UnityWebRequest.Get(Configuration.API_URL + HttpResources.DELIMITER + HttpResources.GAMES + HttpResources.DELIMITER + gameId + HttpResources.DELIMITER + HttpResources.PLAYERS + HttpResources.DELIMITER + playerId + HttpResources.DELIMITER + HttpResources.CARDS),
                "GET",
                null,
                responseHandler);
        }

        public IEnumerator getTurn(int gameId, int turnCounter, Action<Turn> responseHandler)
        {
            yield return turnRequest(UnityWebRequest.Get(Configuration.API_URL + HttpResources.DELIMITER + HttpResources.GAMES + HttpResources.DELIMITER + gameId + HttpResources.DELIMITER + HttpResources.TURNS + HttpResources.DELIMITER + turnCounter),
                "GET",
                responseHandler,
                null);
        }

        public IEnumerator register(string email, string password, Action<int> responseHandler)
        {
            yield return userRequest(HttpResources.DELIMITER + HttpResources.USERS + HttpResources.DELIMITER + HttpResources.REGISTER,
                            JsonUtility.ToJson(new RegistrationRequest(email, password)),
                            responseHandler);
        }

        public IEnumerator login(string email, string password, Action<int> responseHandler)
        {
            yield return userRequest(HttpResources.DELIMITER + HttpResources.USERS + HttpResources.DELIMITER + HttpResources.LOGIN,
                            JsonUtility.ToJson(new LoginRequest(email, password)),
                            responseHandler);
        }

        private IEnumerator gameRequest(UnityWebRequest request, string httpVerb, Action<Game> responseHandler, Action<List<Game>> multipleResponseHandler)
        {
            request.method = httpVerb;
            yield return makeRequest(request, response =>
            {
                if (responseHandler != null)
                {
                    GameRepresentation result = JsonUtility.FromJson<GameRepresentation>(request.downloadHandler.text);
                    responseHandler(result.toGame());
                }

                if (multipleResponseHandler != null)
                {
                    List<GameRepresentation> results = new List<GameRepresentation>(JsonHelper.getJsonArray<GameRepresentation>(request.downloadHandler.text));
                    multipleResponseHandler(results.ConvertAll(game => game.toGame()));
                }
            });
        }

        private IEnumerator cardRequest(UnityWebRequest request, string httpVerb, Action<Card> responseHandler, Action<List<Card>> multipleResponseHandler)
        {
            request.method = httpVerb;
            yield return makeRequest(request, response =>
            {
                if (responseHandler != null)
                {
                    CardRepresentation card = JsonUtility.FromJson<CardRepresentation>(request.downloadHandler.text);
                    responseHandler(card.toCard());
                }

                if (multipleResponseHandler != null)
                {
                    List<CardRepresentation> results = new List<CardRepresentation>(JsonHelper.getJsonArray<CardRepresentation>(request.downloadHandler.text));
                    multipleResponseHandler(results.ConvertAll(card => card.toCard()));
                }
            });
        }

        private IEnumerator turnRequest(UnityWebRequest request, string httpVerb, Action<Turn> responseHandler, Action<List<Turn>> multipleResponseHandler)
        {
            request.method = httpVerb;
            yield return makeRequest(request, response =>
            {
                if (responseHandler != null)
                {
                    TurnRepresentation result = JsonUtility.FromJson<TurnRepresentation>(request.downloadHandler.text);
                    responseHandler(result.toTurn());
                }

                if (multipleResponseHandler != null)
                {
                    List<TurnRepresentation> results = new List<TurnRepresentation>(JsonHelper.getJsonArray<TurnRepresentation>(request.downloadHandler.text));
                    multipleResponseHandler(results.ConvertAll(turn => turn.toTurn()));
                }
            });
        }

        private IEnumerator userRequest(string path, string payload, Action<int> responseHandler)
        {
            UnityWebRequest request = UnityWebRequest.Put(
                Configuration.API_URL + path,
                payload);

            request.method = "POST";
            yield return makeRequest(request, response =>
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

        private IEnumerator makeRequest(UnityWebRequest request, Action<object> responseHandler = null)
        {
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
            }
            else
            {
                Debug.Log("Response: " + request.responseCode + (request.downloadHandler != null ? " Details: " + request.downloadHandler.text : ""));
                if (request.responseCode == 200)
                {
                    responseHandler?.Invoke(request.downloadHandler.text);
                }
            }
        }
    }
}

