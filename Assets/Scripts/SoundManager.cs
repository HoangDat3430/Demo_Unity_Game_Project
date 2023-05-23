using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    [SerializeField] private AudioClipRefsSO audioClipRefsSO;

    public const string PLAYER_PREFS_SOUND_VOLUME = "SoundEffectsVolume";
    public static SoundManager Instance { get; private set; }

    private float playerMoveTimer;
    private float playerMoveTimerMax = .2f;
    private float volume = .5f;
    private void Awake()
    {
        playerMoveTimer = 0f;
        Instance = this;
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_VOLUME, volume);

    }
    private void Start()
    {
        DeliveryManager.Instance.OnDeliverySuccess += DeliveryManager_OnDeliverySuccess;
        DeliveryManager.Instance.OnDeliveryFail += DeliveryManager_OnDeliveryFail;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        BaseCounter.OnDrop += BaseCounter_OnDrop;
        TrashCounter.OnDroppingObject += TrashCounter_OnDroppingObject;
        Player.Instance.OnPickUp += Player_OnPickUp;
        GameHandler.Instance.OnStateChanged += GamHandler_GameOver;
    }

    private void GamHandler_GameOver(object sender, System.EventArgs e)
    {
        if(GameHandler.Instance.GetState() == GameHandler.State.gameOver)
        {
            if (DeliveryManager.Instance.GetRecipeSuccessAmount() >= 5)
            {
                PlaySound(audioClipRefsSO.gamePassed, Camera.main.transform.position, 0.3f);
            }
            else
            {
                PlaySound(audioClipRefsSO.gameUnpassed, Camera.main.transform.position, 0.3f);
            }
        }
    }

    private void Update()
    {
        if (Player.Instance.IsWalking())
        {
            playerMoveTimer += Time.deltaTime;
            if (playerMoveTimer >= playerMoveTimerMax)
            {
                PlaySound(audioClipRefsSO.footStep, Player.Instance.transform.position);
                playerMoveTimer = 0f;
            }
        }
    }
    
    public void PlayCountdownSound()
    {
        PlaySound(audioClipRefsSO.countdown, Vector3.zero);
    }
    public void PlayStartSound()
    {
        PlaySound(audioClipRefsSO.start, Vector3.zero);
    }
    public void PlayWarningSound(Vector3 position)
    {
        PlaySound(audioClipRefsSO.warning, position);
    }
    private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e)
    {
        CuttingCounter cuttingCounter = (CuttingCounter)sender;
        PlaySound(audioClipRefsSO.chop, cuttingCounter.transform.position);
    }

    private void BaseCounter_OnDrop(object sender, System.EventArgs e)
    {
        BaseCounter baseCounter = (BaseCounter)sender;
        PlaySound(audioClipRefsSO.objectDrop, baseCounter.transform.position);
    }

    private void Player_OnPickUp(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.objectPickup, Player.Instance.transform.position);
    }

    private void TrashCounter_OnDroppingObject(object sender, System.EventArgs e)
    {
        TrashCounter trashCounter = (TrashCounter)sender;
        PlaySound(audioClipRefsSO.trash, trashCounter.transform.position);
    }

    private void DeliveryManager_OnDeliveryFail(object sender, System.EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSO.deliveryFail, deliveryCounter.transform.position);
        PlaySound(audioClipRefsSO.triggerTrap, Camera.main.transform.position, 0.1f);
    }

    private void DeliveryManager_OnDeliverySuccess(object sender, System.EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSO.deliverySuccess, deliveryCounter.transform.position);
        PlaySound(audioClipRefsSO.tenMark, Camera.main.transform.position, 0.1f);
    }
    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volumeMultiplier = 1f)
    {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volumeMultiplier);
    }
    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * volume);
    }
    public void ChangeVolume()
    {
        volume += .1f;
        if(volume > 1.1f)
        {
            volume = 0f;
        }
        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return volume;
    }
}
