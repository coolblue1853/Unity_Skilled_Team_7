using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Mixer Settings")] // 인스펙터에서 이 변수들을 그룹으로 묶어 시각적으로 구분
    public AudioMixer audioMixer; // 오디오 믹서 참조 (BGM/SFX 볼륨 제어용)

    [Header("Background Music")]
    public AudioSource bgmSource; // BGM 재생용 오디오 소스
    public AudioClip[] bgmClips;  // 다양한 BGM 클립을 배열로 관리

    [Header("Sound Effects (Auto Register)")]
    public AudioSource sfxSource; // 효과음 재생용 오디오 소스
    private Dictionary<string, AudioClip> sfxDict = new Dictionary<string, AudioClip>(); // 이름-클립 매핑 딕셔너리

    void Awake()
    {
        // 싱글톤 패턴 적용
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 오브젝트 유지
            LoadAllSFX(); // 효과음 자동 등록 실행
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Resources/Audio/SFX 폴더 내 모든 효과음을 로드하여 딕셔너리에 등록
    void LoadAllSFX()
    {
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio/SFX");
        foreach (AudioClip clip in clips)
        {
            if (!sfxDict.ContainsKey(clip.name))
            {
                sfxDict.Add(clip.name, clip);
                Debug.Log($"SFX 로드 완료: {clip.name}");
            }
        }
    }

    // BGM을 인덱스로 재생
    public void PlayBGM(int index)
    {
        if (index >= 0 && index < bgmClips.Length)
        {
            bgmSource.clip = bgmClips[index];
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    // 이름으로 효과음 재생
    public void PlaySFX(string name)
    {
        if (sfxDict.TryGetValue(name, out AudioClip clip))
        {
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"SFX '{name}' 을(를) 찾을 수 없습니다.");
        }
    }

    // BGM 볼륨 설정 (0.0 ~ 1.0 범위 → dB로 변환)
    public void SetBGMVolume(float value)
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);
    }

    // SFX 볼륨 설정 (0.0 ~ 1.0 범위 → dB로 변환)
    public void SetSFXVolume(float value)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);
    }
}
