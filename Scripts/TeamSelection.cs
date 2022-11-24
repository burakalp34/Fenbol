using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeamSelection : MonoBehaviour
{
    public Camera mainCamera;
    public UnityEngine.UI.Image P1CountryImage, P2CountryImage;
    public UnityEngine.UI.Text P1CountryText, P2CountryText;
    public float cameraSize = 12f;
    public bool ZoomOut = false;
    public int P1CountryIndex = 0, P2CountryIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("AustriaP1".Replace("P1", "")); = "Austria"
        ZoomOut = false;
    }

    // Update is called once per frame
    void Update()
    {
        mainCamera.orthographicSize = cameraSize;
    }
    private void FixedUpdate()
    {
        if (ZoomOut) {
            cameraSize += Time.fixedDeltaTime * 6f;
        }
    }
    public void NextButton() {
        ZoomOut = true;
        PlayerPrefs.SetInt("Country1", P1CountryIndex);
        PlayerPrefs.SetInt("Country2", P2CountryIndex);
        SceneManager.LoadScene("PvP");
        //Debug.Log("Go to delay");
        //StartCoroutine(ContinueDelay());
    }
    IEnumerator ContinueDelay() {
        Debug.Log("Sure");
        yield return new WaitForSeconds(1.3f);
        SceneManager.LoadScene("PvP");
    }
    public void SelectCountry(UnityEngine.UI.Button clickedButton) {
        string CountryName = "", Player = "";
        int i = 2;
        for (i = 2; i < clickedButton.name.Length - 2; i++) {
            CountryName += clickedButton.name[i];
        }
        Player = clickedButton.name[i].ToString() + clickedButton.name[i + 1].ToString();
        if (Player == "P1") {
            P1CountryIndex = int.Parse(clickedButton.name[0].ToString() + clickedButton.name[1].ToString());
            P1CountryText.text = CountryName;
            P1CountryImage.sprite = clickedButton.image.sprite;
            P1CountryImage.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clickedButton.GetComponent<RectTransform>().rect.width * 1.5f);
            P1CountryImage.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clickedButton.GetComponent<RectTransform>().rect.height * 1.5f);
        }
        else if (Player == "P2")
        {
            P2CountryIndex = int.Parse(clickedButton.name[0].ToString() + clickedButton.name[1].ToString());
            P2CountryText.text = CountryName;
            P2CountryImage.sprite = clickedButton.image.sprite;
            P2CountryImage.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clickedButton.GetComponent<RectTransform>().rect.width * 1.5f);
            P2CountryImage.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clickedButton.GetComponent<RectTransform>().rect.height * 1.5f);
        }
    }
}