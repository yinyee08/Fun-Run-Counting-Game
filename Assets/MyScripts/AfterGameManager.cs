using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class AfterGameManager : MonoBehaviour, InteractionListenerInterface
{
    [Tooltip("Camera used for screen ray-casting. This is usually the main camera.")]
	public Camera screenCamera;

    [Tooltip("Interaction manager instance, used to detect hand interactions. If left empty, it will be the first interaction manager found in the scene.")]
	private InteractionManager interactionManager;

    [Tooltip("Index of the player, tracked by the respective InteractionManager. 0 means the 1st player, 1 - the 2nd one, 2 - the 3rd one, etc.")]
    public int playerIndex = 0;

    [Tooltip("Whether the left hand interaction is allowed by the respective InteractionManager.")]
	public bool leftHandInteraction = true;

	[Tooltip("Whether the right hand interaction is allowed by the respective InteractionManager.")]
	public bool rightHandInteraction = true;
    
	private InteractionManager.HandEventType lastHandEvent = InteractionManager.HandEventType.None;
    private Vector3 screenNormalPos = Vector3.zero;

    public bool requestQuit = false;
    public bool reloadScene = false;
    public bool loadMainMenu = false;
    public bool requestPlayNext = false;

	//public GameObject WinMenu;
    //public GameObject ControllerKinect;
    public int sceneIndex;
    public int mainMenuIndex = 0;
	
    void Start()
	{
        // by default set the main-camera to be screen-camera
		if (screenCamera == null) 
		{
			screenCamera = Camera.main;
		}
		// get the interaction manager instance
		if(interactionManager == null)
		{
            //interactionManager = InteractionManager.Instance;
            interactionManager = GetInteractionManager();
        }
	}

    void Update() 
	{
			if(reloadScene)
			{
				// reset the objects as needed
				reloadScene = false;
                //WinMenu.SetActive(false);
                // ControllerKinect.GetComponent<SimpleGestureListener>().enabled = true;
                // ControllerKinect.GetComponent<InteractionManager>().enabled = false;
				SceneManager.LoadScene(sceneIndex);
				

			}

			if(loadMainMenu)
			{
				loadMainMenu = false;
                //Time.timeScale = 1f;
                Destroy(GetComponent<KinectManager>());
				Destroy(GetComponent<InteractionManager>());
                SceneManager.LoadScene(mainMenuIndex);
			}

			if(requestQuit)
			{
				requestQuit = false;
				Debug.Log("QUIT!");
        		Application.Quit();
			}
	}

       // tries to locate a proper interaction manager in the scene
    private InteractionManager GetInteractionManager()
    {
        // find the proper interaction manager
        MonoBehaviour[] monoScripts = FindObjectsOfType(typeof(MonoBehaviour)) as MonoBehaviour[];

        foreach (MonoBehaviour monoScript in monoScripts)
        {
            if ((monoScript is InteractionManager) && monoScript.enabled)
            {
                InteractionManager manager = (InteractionManager)monoScript;

                if (manager.playerIndex == playerIndex && manager.leftHandInteraction == leftHandInteraction && manager.rightHandInteraction == rightHandInteraction)
                {
                    return manager;
                }
            }
        }

        // not found
        return null;
    }

    public void HandGripDetected(long userId, int userIndex, bool isRightHand, bool isHandInteracting, Vector3 handScreenPos)
	{
		if (!isHandInteracting || !interactionManager)
			return;
		if (userId != interactionManager.GetUserID())
			return;

		lastHandEvent = InteractionManager.HandEventType.Grip;
		//isLeftHandDrag = !isRightHand;
		screenNormalPos = handScreenPos;
	}

	public void HandReleaseDetected(long userId, int userIndex, bool isRightHand, bool isHandInteracting, Vector3 handScreenPos)
	{
		if (!isHandInteracting || !interactionManager)
			return;
		if (userId != interactionManager.GetUserID())
			return;

		lastHandEvent = InteractionManager.HandEventType.Release;
		//isLeftHandDrag = !isRightHand;
		screenNormalPos = handScreenPos;
	}

	public bool HandClickDetected(long userId, int userIndex, bool isRightHand, Vector3 handScreenPos)
	{
		return true;
	}

    public void RequestToPlayAgain()
    {
        reloadScene = true;
        Debug.Log("reload: " + sceneIndex);
    }

	public void RequestToLoadMainMenu()
	{
        Debug.Log("load main menu");
		loadMainMenu = true;
	}

	public void RequestToQuit()
	{
		requestQuit = true;
	}

}