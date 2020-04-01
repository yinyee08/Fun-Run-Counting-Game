using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuChangeScene : MonoBehaviour, InteractionListenerInterface
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

    [Tooltip("Material used to outline the currently selected object.")]
	public Material selectedObjectMaterial;

    private Vector3 screenNormalPos = Vector3.zero;
	private InteractionManager.HandEventType lastHandEvent = InteractionManager.HandEventType.None;
    public bool loadNextScene = false;
    private int nextSceneIndex = 0;
    public bool requestPlay = false;
    public bool requestOption = false;
    public bool requestBack = false;
	public bool requestQuit = false;
	public bool requestOpenLeaderBoard = false;

	public GameObject MainMenu;
	public GameObject OptionMenu;
	public GameObject WriteName;
	public GameObject Leader_Board;
	public GameObject MainPanel;

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
		if(interactionManager != null && interactionManager.IsInteractionInited())
		{
			if(loadNextScene)
			{
				// reset the objects as needed
				loadNextScene = false;
				SceneManager.LoadScene(nextSceneIndex);
				Destroy(GetComponent<KinectManager>());
				Destroy(GetComponent<InteractionManager>());

			}

			if(requestPlay)
			{
				requestPlay = false;
				MainMenu.SetActive(false);
				WriteName.SetActive(true);
			}

			if(requestQuit)
			{
				requestQuit = false;
				Debug.Log("QUIT!");
        		Application.Quit();
			}

			if(requestOption)
			{
				requestOption = false;
				MainMenu.SetActive(false);
				OptionMenu.SetActive(true);
			}

			if(requestBack)
			{
				requestBack = false;
				MainMenu.SetActive(true);
				OptionMenu.SetActive(false);
				WriteName.SetActive(false);
			}

			if(requestOpenLeaderBoard)
			{
				requestOpenLeaderBoard = false;
				MainMenu.SetActive(false);
				MainPanel.SetActive(false);
				Leader_Board.GetComponent<Leaderboard>().OpenLeaderboard();
				Leader_Board.SetActive(true);
			}
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

    public void RequestToLoadNextScene(int sceneIndex)
    {
        loadNextScene = true;
        Debug.Log("Load next: " + loadNextScene);
        nextSceneIndex = sceneIndex;
    }

	public void RequestToPlay()
	{
		requestPlay = true;
	}

	public void RequestToQuit()
	{
		requestQuit = true;
	}

	public void RequestToOption()
	{
		requestOption = true;
	}

	public void RequestToBack()
	{
		requestBack = true;
	}

	public void RequestToOpenBoard()
	{
		requestOpenLeaderBoard = true;
	}

	public void RequestToCloseBoard()
	{
		MainMenu.SetActive(true);
		MainPanel.SetActive(true);
		Leader_Board.SetActive(false);
	}
}