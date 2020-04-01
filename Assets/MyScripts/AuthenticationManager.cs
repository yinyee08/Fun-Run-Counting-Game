using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;

public class AuthenticationManager : MonoBehaviour {

	//public InputField loginEmail;
	//public InputField loginPassword;
	public Image profileImage;
	public GameObject signInButton;	
	public GameObject logoutButton;	
	public Text usernameField;
	
	// Awake function from Unity's MonoBehavior
	void Awake ()
	{
		//if (!FB.IsInitialized) {
			// Initialize the Facebook SDK
			FB.Init(InitCallback, OnHideUnity);
		//} else {
			// Already initialized, signal an app activation App Event
		//	FB.ActivateApp();
		//}
	}

	private void InitCallback ()
	{
		/*if (FB.IsInitialized) {
			// Signal an app activation App Event
			FB.ActivateApp();
			// Continue with Facebook SDK
			// ...
		} else {
			Debug.Log("Failed to Initialize the Facebook SDK");
		}

		if(FB.IsLoggedIn){
			Debug.Log("yes");
			FB.API("/me/picture?type=square&height=128&width=128", HttpMethod.GET, GetPicture);
		}*/
        if(FB.IsLoggedIn){
            Debug.Log("FB is logged in");
        }else{
            Debug.Log("FB is not logged in");
        }
	}

	private void OnHideUnity (bool isGameShown)
	{
		if (!isGameShown) {
			// Pause the game - we will need to hide
			Time.timeScale = 0;
		} else {
			// Resume the game - we're getting focus again
			Time.timeScale = 1;
		}
	}

	public void LoginWithFB() {
		List<string> perms = new List<string>(){"public_profile"}; //, "email"
		FB.LogInWithReadPermissions(perms, AuthCallback);
	}

	private void AuthCallback (ILoginResult result) {
		/*if (FB.IsLoggedIn) {
			// AccessToken class will have session details
			var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
			// Print current access token's User ID
			Debug.Log(aToken.UserId);
			// Print current access token's granted permissions
			foreach (string perm in aToken.Permissions) {
				Debug.Log(perm);
			}
		} else {
			Debug.Log("User cancelled login");
		}*/
        if(result.Error != null){
            Debug.Log(result.Error);
        }else{
            if(FB.IsLoggedIn){
                Debug.Log("FB is logged in");
            }else{
                Debug.Log("FB is not logged in");
            }
        }

		ToggleButtons(FB.IsLoggedIn);
	}

	private void ToggleButtons(bool hasLoggedIn) {
		if(hasLoggedIn){
			signInButton.SetActive(false);
			logoutButton.SetActive(true);

			FB.API("/me?fields=first_name", HttpMethod.GET, DisplayUsername);
			FB.API("/me/picture?type=square&height=128&width=128", HttpMethod.GET, GetPicture);

		}else{
			profileImage.sprite = null;
			usernameField.text = "";
			signInButton.SetActive(true);
			logoutButton.SetActive(false);
		}
	}

	private void DisplayUsername(IResult result) {
		if(result.Error != null){
            Debug.Log(result.Error);
        }else{
            usernameField.text = "Hi ! " + result.ResultDictionary["first_name"];
        }
	}

	private void GetPicture(IGraphResult result) {
		if(result.Error == null && result.Texture != null) {
			profileImage.sprite = Sprite.Create(result.Texture, new Rect(0,0,128,128), new Vector2());
		}
	}

	public void LogoutFromFB() {
		FB.LogOut();
		ToggleButtons(FB.IsLoggedIn);
	}
}
