

using IBM.Watson.DeveloperCloud.Services.VisualRecognition.v3;
using IBM.Watson.DeveloperCloud.Utilities;
using System.Collections.Generic;
/**
* Copyright 2015 IBM Corp. All Rights Reserved.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*      http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*
*/
using UnityEngine;
using UnityEngine.UI;

namespace IBM.Watson.DeveloperCloud.Demos.FacialRecognition
{
	public class TrainingView : View
	{

		#region Private Data
		[SerializeField]
		private Button m_CreateTrainingSetButton;
		[SerializeField]
		private Button m_TrainClassifierButton;
		[SerializeField]
		private RectTransform m_TrainingSetsRectTransform;
		[SerializeField]
		private RectTransform m_ClassifiersToTrainRectTransform;
		[SerializeField]
		private GameObject m_ClassifierToTrainPrefab;
		[SerializeField]
		private Button m_SelectAllButton;
		[SerializeField]
		private Button m_DeslectAllButton;

		private List<GameObject> m_ClassifierToTrainGameObjects = new List<GameObject>();
		#endregion

		#region Public Properties
		#endregion

		#region Constructor and Destructor
		/// <summary>
		/// The TrainingView Constrtuctor.
		/// </summary>
		public TrainingView()
		{
			if (!m_ViewStates.Contains(AppState.TRAIN))
				m_ViewStates.Add(AppState.TRAIN);
		}
		#endregion

		#region Awake / Start / Enable / Disable
		void OnEnable()
		{
			PopulateClassifiersToTrain();
			//EventManager.Instance.RegisterEventReceiver(Event.ON_CLASSIFIER_TO_TRAIN_ADDED, OnClassifierToTrainAdded);
			//EventManager.Instance.RegisterEventReceiver(Event.ON_CLASSIFIER_TO_TRAIN_REMOVED, OnClassifierToTrainRemoved);
		}

		void OnDisable()
		{
			//EventManager.Instance.UnregisterEventReceiver(Event.ON_CLASSIFIER_TO_TRAIN_ADDED, OnClassifierToTrainAdded);
			//EventManager.Instance.UnregisterEventReceiver(Event.ON_CLASSIFIER_TO_TRAIN_REMOVED, OnClassifierToTrainRemoved);
		}
		#endregion

		#region Private Functions
		private void PopulateClassifiersToTrain()
		{
			if (m_AppData.ClassifierIDsToClassifyWith == null || m_AppData.ClassifierIDsToClassifyWith.Count == 0)
				return;

			foreach (string classifierID in m_AppData.ClassifierIDsToClassifyWith)
			{
				if (classifierID != "default")
				{
					GameObject classifierToTrainGameObject = Instantiate(m_ClassifierToTrainPrefab, m_ClassifiersToTrainRectTransform) as GameObject;
					ClassifierToTrain classifierToTrain = classifierToTrainGameObject.GetComponent<ClassifierToTrain>();
					m_ClassifierToTrainGameObjects.Add(classifierToTrainGameObject);
					foreach (GetClassifiersPerClassifierVerbose classifierVerbose in m_AppData.ClassifiersVerbose)
						if (classifierVerbose.classifier_id == classifierID)
						{
							m_AppData.ClassifierIDsToTrain.Add(classifierID);
							classifierToTrain.ClassifierVerbose = classifierVerbose;
							break;
						}
				}
			}

			GameObject newClassifierGameObject = Instantiate(m_ClassifierToTrainPrefab, m_ClassifiersToTrainRectTransform) as GameObject;
			ClassifierToTrain newClassifierToTrain = newClassifierGameObject.GetComponent<ClassifierToTrain>();
			m_AppData.ClassifierIDsToTrain.Add("new");
			GetClassifiersPerClassifierVerbose newClassifierVerbose = new GetClassifiersPerClassifierVerbose();
			newClassifierVerbose.classifier_id = "new";
			newClassifierVerbose.name = "New Classifier";
			newClassifierVerbose.classes = null;
			newClassifierToTrain.ClassifierVerbose = newClassifierVerbose;
			m_ClassifierToTrainGameObjects.Add(newClassifierGameObject);
		}

		//private bool IsSelectAllButtonEnabled()
		//{
		//	return m_AppData.ClassifierIDsToTrain.Count < m_ClassifierToTrainGameObjects.Count;
		//}

		//private bool IsDeselectAllButtonEnabled()
		//{
		//	return m_AppData.ClassifierIDsToTrain.Count > 0;
		//}
		#endregion

		#region Public Functions
		/// <summary>
		/// UI Click handler for options button clicked.
		/// </summary>
		public void OnOptionsButtonClicked()
		{
			m_AppData.ClassifierIDsToTrain.Clear();
			while (m_ClassifierToTrainGameObjects.Count > 0)
			{
				Destroy(m_ClassifierToTrainGameObjects[0]);
				m_ClassifierToTrainGameObjects.RemoveAt(0);
			}

			m_AppData.AppState = AppState.CONFIG;
		}

		public void OnSelectAllClassifiersButtonClicked()
		{
			foreach (GameObject go in m_ClassifierToTrainGameObjects)
			{
				Toggle toggle = go.GetComponent<Toggle>();
				if (toggle != null)
					toggle.isOn = true;
			}
		}

		public void OnDeselectAllClassifiersButtonClicked()
		{
			foreach (GameObject go in m_ClassifierToTrainGameObjects)
			{
				Toggle toggle = go.GetComponent<Toggle>();
				if (toggle != null)
					toggle.isOn = false;
			}
		}
		#endregion

		#region Event Handlers
		//private void OnClassifierToTrainAdded(object[] args)
		//{
		//	m_SelectAllButton.interactable = IsSelectAllButtonEnabled();
		//}

		//private void OnClassifierToTrainRemoved(object[] args)
		//{
		//	m_DeslectAllButton.interactable = IsDeselectAllButtonEnabled();
		//}
		#endregion
	}
}
