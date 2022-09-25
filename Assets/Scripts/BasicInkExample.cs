using UnityEngine;
using UnityEngine.UI;
using System;
using Ink.Runtime;
using System.Collections.Generic;
using System.Collections;
using TMPro;

// This is a super bare bones example of how to play and display a ink story in Unity.
public class BasicInkExample : MonoBehaviour {
    public static event Action<Story> OnCreateStory;

	private GameObject buttons;

	private int last_choices = 0;
	
    void Awake () {
		// Remove the default message
		RemoveChildren();
		StartStory();
	}

	// Creates a new Story object with the compiled story which we can then play!
	void StartStory () {
		story = new Story (inkJSONAsset.text);
        if(OnCreateStory != null) OnCreateStory(story);
		RefreshView();
	}
	
	// This is the main function called every time the story changes. It does a few things:
	// Destroys all the old content and choices.
	// Continues over all the lines of text, then displays all the choices. If there are no choices, the story is finished!
	void RefreshView () {
		// Remove all the UI on screen
		RemoveChildren ();

		if (last_choices > 1 && story.currentText.Length > 0)
		{
			if (lineBreakPrefab != null)
			{
				GameObject newlinebreak = Instantiate(lineBreakPrefab);
				newlinebreak.transform.SetParent(canvas.transform, false);
			}
		}

		// Read all the content until we can't continue any more
		while (story.canContinue) {
			// Continue gets the next line of the story
			string text = story.Continue ();
			// This removes any white space from the text.
			text = text.Trim();
			// Display the text on screen!
			CreateContentView(text);
		}

		// Display all the choices, if there are any!
		buttons = Instantiate(buttonHolderPrefab);
		buttons.transform.SetParent(canvas.transform, false);
		last_choices = story.currentChoices.Count;
		if (story.currentChoices.Count > 0) {
			for (int i = 0; i < story.currentChoices.Count; i++) {
				Choice choice = story.currentChoices [i];
				Button button = CreateChoiceView (choice.text.Trim ());
				// Tell the button what to do when we press it
				button.onClick.AddListener (delegate {
					OnClickChoiceButton (choice);
				});
			}
		}
		// If we've read all the content and there's no choices, the story is finished!
		else {
			Button choice = CreateChoiceView("End of story.\nRestart?");
			choice.onClick.AddListener(delegate{
				StartStory();
			});
		}

		if (scrollrect != null)
		{
			StartCoroutine(ScrollToBottom());
		}
	}

	private IEnumerator ScrollToBottom()
	{
		//yield return new WaitForEndOfFrame();
		yield return new WaitForSeconds(0.1f);
		Canvas.ForceUpdateCanvases();
		scrollrect.verticalNormalizedPosition = 0f;
		Canvas.ForceUpdateCanvases();
	}

	// When we click the choice button, tell the story to choose that choice!
	void OnClickChoiceButton (Choice choice) {
		story.ChooseChoiceIndex (choice.index);
		RefreshView();
	}

	// Creates a textbox showing the the line of text
	void CreateContentView (string text) {
		if (text.Length > 0)
		{
			TextMeshProUGUI storyText = Instantiate(textPrefab) as TextMeshProUGUI;
			storyText.text = text;
			storyText.transform.SetParent(canvas.transform, false);
		}
	}

	// Creates a button showing the choice text
	Button CreateChoiceView (string text) {
		// Creates the button from a prefab
		Button choice = Instantiate (buttonPrefab) as Button;
		choice.transform.SetParent (buttons.transform, false);
		
		// Gets the text from the button prefab
		TextMeshProUGUI choiceText = choice.GetComponentInChildren<TextMeshProUGUI> ();
		choiceText.text = text;

		// Make the button expand to fit the text
		HorizontalLayoutGroup layoutGroup = choice.GetComponent <HorizontalLayoutGroup> ();
		layoutGroup.childForceExpandHeight = false;

		return choice;
	}

	// Destroys all the children of this gameobject (all the UI)
	void RemoveChildren () {
		/*int childCount = canvas.transform.childCount;
		//for (int i = childCount - 1; i >= 0; --i) {
			GameObject.Destroy (canvas.transform.GetChild (i).gameObject);
		}*/
		/*if (buttons != null)
		{
			for (int i = 0; i < buttons.Count; i++)
			{
				//buttons[i].GetComponent<ButtonControl>().DisolveOut();
				GameObject.Destroy(buttons[i]);
			}
		}*/
		GameObject.Destroy(buttons);
	}

	[SerializeField]
	private TextAsset inkJSONAsset = null;
	public Story story;

	[SerializeField]
	private GameObject canvas = null;
	[SerializeField]
	private ScrollRect scrollrect = null;

	// UI Prefabs
	[SerializeField]
	private TextMeshProUGUI textPrefab = null;
	[SerializeField]
	private Button buttonPrefab = null;
	[SerializeField]
	private GameObject buttonHolderPrefab = null;
	[SerializeField]
	private GameObject lineBreakPrefab = null;
}
