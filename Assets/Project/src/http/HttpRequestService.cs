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
            yield return gameRequest(UnityWebRequest.Get(Configuration.API_URL + EndpointsResources.DELIMITER + EndpointsResources.PLAYERS + EndpointsResources.DELIMITER + playerId + EndpointsResources.DELIMITER + EndpointsResources.GAMES),
                "GET",
                responseHandler,
                null);
        }

        public IEnumerator createNewGame(Action<Game> responseHandler)
        {
            yield return gameRequest(UnityWebRequest.Post(Configuration.API_URL + EndpointsResources.DELIMITER + EndpointsResources.GAMES, "{}"),
                "POST",
                responseHandler,
                null);
        }

        public IEnumerator getOpenGames(Action<List<Game>> responseHandler)
        {
            yield return gameRequest(UnityWebRequest.Get(Configuration.API_URL + EndpointsResources.DELIMITER + EndpointsResources.GAMES + "?status=" + GameStatus.OPEN),
                "GET",
                null,
                responseHandler);
        }

        public IEnumerator joinGame(int gameId, int playerId, Action<Game> responseHandler)
        {
            yield return gameRequest(UnityWebRequest.Put(Configuration.API_URL + EndpointsResources.DELIMITER + EndpointsResources.GAMES + EndpointsResources.DELIMITER + gameId + EndpointsResources.DELIMITER + EndpointsResources.PLAYERS, JsonUtility.ToJson(new PlayerRequest(playerId))),
                "POST",
                responseHandler,
                null);
        }

        public IEnumerator leaveGame(int gameId, int playerId, Action<Game> responseHandler)
        {
            yield return gameRequest(UnityWebRequest.Put(Configuration.API_URL + EndpointsResources.DELIMITER + EndpointsResources.GAMES + EndpointsResources.DELIMITER + gameId + EndpointsResources.DELIMITER + EndpointsResources.PLAYERS + EndpointsResources.DELIMITER + playerId, "{}"),
                "DELETE",
                responseHandler,
                null);
        }

        public IEnumerator register(string email, string password, Action<Player> responseHandler)
        {
            yield return userRequest(EndpointsResources.DELIMITER + EndpointsResources.USERS + EndpointsResources.DELIMITER + EndpointsResources.REGISTER,
                            JsonUtility.ToJson(new RegistrationRequest(email, password)),
                            responseHandler);
        }

        public IEnumerator login(string email, string password, Action<Player> responseHandler)
        {
            yield return userRequest(EndpointsResources.DELIMITER + EndpointsResources.USERS + EndpointsResources.DELIMITER + EndpointsResources.LOGIN,
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
                    responseHandler(new Game((int)result.id, result.status));
                }

                if (multipleResponseHandler != null)
                {
                    List<GameRepresentation> results = new List<GameRepresentation>(JsonHelper.getJsonArray<GameRepresentation>(request.downloadHandler.text));
                    multipleResponseHandler(results.ConvertAll(game => new Game((int)game.id, game.status)));
                }
            });
        }

        private IEnumerator userRequest(string path, string payload, Action<Player> responseHandler)
        {
            UnityWebRequest request = UnityWebRequest.Put(
                Configuration.API_URL + path,
                payload);

            request.method = "POST";
            yield return makeRequest(request, response =>
            {
                UserRepresentation result = UserRepresentation.fromJSON(request.downloadHandler.text);

                if (result.token != null)
                {
                    token = result.token;
                    PlayerPrefs.SetString(TOKEN, token);
                }

                responseHandler(new Player((int)result.id));
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

