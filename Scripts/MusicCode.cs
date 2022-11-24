using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicCode : MonoBehaviour
{
	public void Start()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
	}
	private AudioSource _audioSource;
	private void Awake()
	{
		DontDestroyOnLoad(transform.gameObject);
		_audioSource = GetComponent<AudioSource>();
	}

	public void PlayMusic()
	{
		if (_audioSource.isPlaying) return;
		_audioSource.Play();
	}

	public void StopMusic()
	{
		_audioSource.Stop();
	}
}