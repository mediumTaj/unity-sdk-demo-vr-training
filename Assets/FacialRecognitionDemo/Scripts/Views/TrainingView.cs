﻿/**
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
using IBM.Watson.DeveloperCloud.Services.VisualRecognition.v3;
using IBM.Watson.DeveloperCloud.Utilities;
using System.Collections.Generic;

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
		[SerializeField]
		private GameObject m_TrainingSetPrefab;
		[SerializeField]
		private RectTransform m_TrainingSetsHolderRectTransform;

		private List<GameObject> m_ClassifierToTrainGameObjects = new List<GameObject>();
		private List<GameObject> m_TrainingSetGameObjects = new List<GameObject>();
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
		protected override void Awake()
		{
			base.Awake();
			EventManager.Instance.RegisterEventReceiver(Event.ON_TRAINING_SET_ADDED, OnTrainingSetAdded);
			EventManager.Instance.RegisterEventReceiver(Event.ON_TRAINING_SET_REMOVED, OnTrainingSetRemoved);
        }
		void OnEnable()
        {
            m_TrainClassifierButton.interactable = IsTrainClassifiersButtonActive();

            if(m_ClassifierToTrainGameObjects.Count == 0)
                PopulateClassifiersToTrain();
        }

		void OnDisable()
		{}
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

            Toggle newClassifierToggle = newClassifierGameObject.GetComponent<Toggle>();
            if (newClassifierToggle != null)
                newClassifierToggle.isOn = false;

			GetClassifiersPerClassifierVerbose newClassifierVerbose = new GetClassifiersPerClassifierVerbose();
			newClassifierVerbose.classifier_id = "new";
			newClassifierVerbose.name = "Train New Classifier";
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
		private bool IsTrainClassifiersButtonActive()
		{
			bool isActive = false;

			if (m_AppData.TrainingSets.Count == 1 && !m_AppData.ClassifierIDsToTrain.Contains("new"))
			{
				isActive = true;
			}
			else if (m_AppData.TrainingSets.Count >= 2)
			{
				isActive = true;
			}
			
			return isActive;
		}

		private void ClearTrainingSets()
		{
			

			while(m_TrainingSetGameObjects.Count > 0)
			{
				Destroy(m_TrainingSetGameObjects[0]);
				m_TrainingSetGameObjects.RemoveAt(0);
			}
		}

        private void ClearClassifiersToTrain()
        {
            m_AppData.ClassifierIDsToTrain.Clear();
            while (m_ClassifierToTrainGameObjects.Count > 0)
            {
                Destroy(m_ClassifierToTrainGameObjects[0]);
                m_ClassifierToTrainGameObjects.RemoveAt(0);
            }
        }

		#endregion

		#region Public Functions
		/// <summary>
		/// UI Click handler for options button clicked.
		/// </summary>
		public void OnOptionsButtonClicked()
		{
			ClearTrainingSets();
            ClearClassifiersToTrain();
			m_AppData.AppState = AppState.CONFIG;
		}

		/// <summary>
		/// UI Click handler for Select All button.
		/// </summary>
		public void OnSelectAllClassifiersButtonClicked()
		{
			foreach (GameObject go in m_ClassifierToTrainGameObjects)
			{
				Toggle toggle = go.GetComponent<Toggle>();
				if (toggle != null)
					toggle.isOn = true;
			}
		}
		/// <summary>
		/// UI click handler for Deselect All button.
		/// </summary>
		public void OnDeselectAllClassifiersButtonClicked()
		{
			foreach (GameObject go in m_ClassifierToTrainGameObjects)
			{
				Toggle toggle = go.GetComponent<Toggle>();
				if (toggle != null)
					toggle.isOn = false;
			}
		}

		/// <summary>
		/// UI Click handler for the Create Training Set Button.
		/// </summary>
		public void OnCreateTrainingSetButtonClicked()
		{
			//ClearData();
			m_AppData.AppState = AppState.CREATE_TRAINING_SET;
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

		private void OnTrainingSetAdded(object[] args)
		{
			if (args[0] is AppData.TrainingSet)
			{
				m_TrainClassifierButton.interactable = IsTrainClassifiersButtonActive();

				GameObject trainingSetPanelGameObject = Instantiate(m_TrainingSetPrefab, m_TrainingSetsHolderRectTransform) as GameObject;
				m_TrainingSetGameObjects.Add(trainingSetPanelGameObject);
				TrainingSetPanel trainingSetPanel = trainingSetPanelGameObject.GetComponent<TrainingSetPanel>();
				if (trainingSetPanel != null)
					trainingSetPanel.TrainingSet = args[0] as AppData.TrainingSet;
			}
		}

		private void OnTrainingSetRemoved(object[] args)
		{
			if (args[0] is AppData.TrainingSet)
			{
				m_TrainClassifierButton.interactable = IsTrainClassifiersButtonActive();

				Destroy(m_TrainingSetGameObjects[m_TrainingSetGameObjects.Count -1]);
				m_TrainingSetGameObjects.RemoveAt(m_TrainingSetGameObjects.Count - 1);
			}
		}
		#endregion
	}
}
