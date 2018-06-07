using System;
using System.IO;
using System.Collections;
using System.Reflection;
using ApiAiSDK;
using ApiAiSDK.Model;
using ApiAiSDK.Unity;
using Newtonsoft.Json;
using System.Net;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public delegate void NativeEventHandle(string[] results, float[] confidences);
public delegate void OnReceiveNativeResponde(string respond);
public class SpeechRecognizeController : MonoBehaviour
{
    public Text answerTextField;
    private ApiAiUnity apiAiUnity;
    private AudioSource aud;
    public AudioClip listeningSound;
    public OnReceiveNativeResponde d_NativeResponde;
    private readonly JsonSerializerSettings jsonSettings = new JsonSerializerSettings
    {
        NullValueHandling = NullValueHandling.Ignore,
    };
    private readonly Queue<Action> ExecuteOnMainThread = new Queue<Action>();

    // Use this for initialization
    IEnumerator Start()
    {
        // check access to the Microphone
        yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);
        if (!Application.HasUserAuthorization(UserAuthorization.Microphone))
            throw new NotSupportedException("Microphone using not authorized");
        ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) =>
        {
            return true;
        };
        const string ACCESS_TOKEN = "3485a96fb27744db83e78b8c4bc9e7b7";

        var config = new AIConfiguration(ACCESS_TOKEN, SupportedLanguage.English);

        apiAiUnity = new ApiAiUnity();
        apiAiUnity.Initialize(config);

        apiAiUnity.OnError += HandleOnError;
        apiAiUnity.d_OnNativeResult += HandleOnNativeResult;
    }

    void HandleOnNativeResult(string[] results, float[] confidences)
    {
        RunInMainThread(() =>
        {
            if (results != null && results.Length > 0)
            {
                answerTextField.text = results[0];
                d_NativeResponde(results[0]);
            }
            else
                Debug.LogError("Response is null");
        });
    }

    void HandleOnError(object sender, AIErrorEventArgs e)
    {
        RunInMainThread(() =>
        {
            Debug.LogException(e.Exception);
            Debug.Log(e.ToString());
            answerTextField.text = e.Exception.Message;
        });
    }
    void Update()
    {
        if (apiAiUnity != null)
            apiAiUnity.Update();
        // dispatch stuff on main thread
        while (ExecuteOnMainThread.Count > 0)
            ExecuteOnMainThread.Dequeue().Invoke();
    }

    private void RunInMainThread(Action action)
    {
        ExecuteOnMainThread.Enqueue(action);
    }
    public void StartNativeRecognition()
    {
        try
        {
            apiAiUnity.StartNativeRecognition();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }
}
