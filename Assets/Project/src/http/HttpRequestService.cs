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

        private HttpRequestService()
        {
            if (PlayerPrefs.HasKey(TOKEN))
            {
                token = PlayerPrefs.GetString(TOKEN);
            }
        }

        public static HttpRequestService Instance
        {
            get
            {
                return instance;
            }
        }

        public IEnumerator createNewGame(Action<Game> responseHandler)
        {
            yield return gameRequest(EndpointsResources.DELIMITER + EndpointsResources.GAMES,
                "POST",
                "{}",
                responseHandler);
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

        private IEnumerator gameRequest(string path, string httpVerb, string payload, Action<Game> responseHandler)
        {
            UnityWebRequest request = UnityWebRequest.Put(
                Configuration.API_URL + path,
                payload);

            request.method = httpVerb;
            yield return makeRequest(request, response =>
            {
                GameRepresentation result = GameRepresentation.fromJSON(request.downloadHandler.text);
                responseHandler(new Game(result.id));
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

                responseHandler(new Player(result.id));
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
                Debug.Log("Response: " + request.responseCode + " Details: " + request.downloadHandler.text);
                if (request.responseCode == 200)
                {
                    responseHandler?.Invoke(request.downloadHandler.text);
                }
            }
        }
    }
}

