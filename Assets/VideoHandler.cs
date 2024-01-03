using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

public class VideoHandler : MonoBehaviour
{
    
    #region Unity Hierarchy

    private VideoPlayer _videoPlayer;

    #endregion
    
    [SerializeField] private SettingsScriptableObject settingsScriptableObject;
    
    #region Unity Events

    public UnityEvent OnVideoStart;
    
    public UnityEvent OnVideoStop;
    
    #endregion
    
    #region Init Methods

    private void Awake()
    {
        _videoPlayer = GetComponent<VideoPlayer>();
        enabled = false;
    }
    
    #endregion

    #region Logic Methods

    public void StartVideo()
    {
        _videoPlayer.Play();
        OnVideoStart?.Invoke();
        enabled = true;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // TODO : Warning if the videoPlayer has attribut WaitForFirstFrame true
        // _videoPlayer.isPlaying will be false, and we don't want this behavior
        if (!_videoPlayer.isPlaying)
        {
            OnVideoStop?.Invoke();
            enabled = false;
        }
    }

    #endregion
    
}
