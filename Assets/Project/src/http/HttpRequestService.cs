using System;
using System.Collections;
using System.Collections.Generic;
using Traitorstown.src;
using Traitorstown.src.http;
using Traitorstown.src.http.representation;
using Traitorstown.src.http.request;
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

        public IEnumerator register(string email, string password)
        {
            String requestPayload = JsonUtility.ToJson(new RegistrationRequest(email, password));
            Debug.Log(requestPayload);

            UnityWebRequest request = UnityWebRequest.Put(
                Configuration.API_URL + EndpointsResources.DELIMITER + EndpointsResources.USERS + EndpointsResources.DELIMITER + EndpointsResources.REGISTER,
                requestPayload);

            request.method = "POST";
            yield return makeRequest(request, response =>
            {
                UserRepresentation result = UserRepresentation.fromJSON(request.downloadHandler.text);

                if (result.token != null)
                {
                    token = result.token;
                    PlayerPrefs.SetString(TOKEN, token);
                    return result;
                }

                return result;
            });
        }

        public IEnumerator createNewGame()
        {
            UnityWebRequest request = UnityWebRequest.Put(Configuration.API_URL + EndpointsResources.DELIMITER + EndpointsResources.GAMES, "{}");
            request.method = "POST";
            yield return makeRequest(request);
        }

        private IEnumerator makeRequest(UnityWebRequest request, Func<object, object> responseHandler = null)
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
                    yield return responseHandler?.Invoke(request.downloadHandler.text);
                }
            }
        }
    }
}

