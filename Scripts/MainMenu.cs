using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private MusicCode MC;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        MC = GameObject.FindGameObjectWithTag("Music").GetComponent<MusicCode>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartGame() {
        SceneManager.LoadScene("PvP");
    }
    public void QuitGame() {
        Application.Quit();
    }
    public void MainMenuButton() {
        SceneManager.LoadScene("MainMenu");
    }
    public void Credits() {
        SceneManager.LoadScene("Credits");
    }
    public void HowToPlay() {
        SceneManager.LoadScene("HowToPlay");
    }
    public void Settings() {
        SceneManager.LoadScene("Settings");
    }
    public void MainMenuIcon() {
        Application.OpenURL("https://procurementforhousing.co.uk/pfh-live-2018-themes/the-house-symbol/");
    }
    public void FootballIcon()
    {
        Application.OpenURL("http://www.publicdomainfiles.com/show_file.php?id=13488047876876");
    }
    public void FieldLink()
    {
        Application.OpenURL("https://commons.wikimedia.org/wiki/File:Football_field_105x68.PNG");
    }
    public void StadiumLink()
    {
        Application.OpenURL("https://www.gettyimages.be/detail/illustratie/soccer-game-royalty-free-illustraties/166078599?");
    }
    public void RestartIcon()
    {
        Application.OpenURL("https://www.pngall.com/restart-png/download/98487");
    }
    public void SoundIcon()
    {
        Application.OpenURL("https://www.kindpng.com/imgv/ioixmo_sound-png-icon-volume-png-transparent-png/");
    }
    public void PopWarnerLink() {
        Application.OpenURL("https://www.1001fonts.com/pop-warner-font.html");
    }
    public void OctinSportsLink()
    {
        Application.OpenURL("https://www.1001fonts.com/octin-sports-free-font.html");
    }
    public void GoalSound() {
        Application.OpenURL("https://freesound.org/people/D.jones/sounds/528799/");
    }
    public void KickSound()
    {
        Application.OpenURL("https://freesound.org/people/dersuperanton/sounds/433722/");
    }
    public void BackgroundMusic()
    {
        Application.OpenURL("https://freesound.org/people/sonically_sound/sounds/624886/");
    }
    public void MissedShotSound()
    {
        Application.OpenURL("https://freesound.org/people/avakas/sounds/263680/");
    }
    public void ChantingSound()
    {
        Application.OpenURL("https://freesound.org/people/habbis92/sounds/206003/");
    }
    public void WhistleSound()
    {
        Application.OpenURL("https://freesound.org/people/Rosa-Orenes256/sounds/538422/");
    }
}