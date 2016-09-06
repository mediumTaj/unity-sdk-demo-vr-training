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

using IBM.Watson.DeveloperCloud.Logging;
using IBM.Watson.DeveloperCloud.Services.VisualRecognition.v3;
using IBM.Watson.DeveloperCloud.Utilities;

namespace IBM.Watson.DeveloperCloud.Demos.FacialRecognition
{
	public class AppData
	{
		#region Constructor and Destructor
		/// <summary>
		/// AppData Constructor
		/// </summary>
		public AppData()
		{
			ClassifierIDs.Added += OnClassifierIDAdded;
			ClassifierIDs.Removed += OnClassifierIDRemoved;
			ClassifiersVerbose.Added += OnClassiferVerboseAdded;
			ClassifiersVerbose.Removed += OnClassifierVerboseRemoved;
		}

		~AppData()
		{
			ClassifierIDs.Added -= OnClassifierIDAdded;
			ClassifierIDs.Removed -= OnClassifierIDRemoved;
			ClassifiersVerbose.Added -= OnClassiferVerboseAdded;
			ClassifiersVerbose.Removed -= OnClassifierVerboseRemoved;
		}
		#endregion

		#region Instance
		/// <summary>
		/// Returns the singleton instance of AppData.
		/// </summary>
		public static AppData Instance { get { return Singleton<AppData>.Instance; } }
		#endregion

		#region Visual Recognition Classes
		/// <summary>
		/// List of Visual Recognition classes.
		/// </summary>
		public ObservedList<VRClass> VRClasses { get; set; }

		/// <summary>
		/// One Visual Recognition class.
		/// </summary>
		public class VRClass
		{
			/// <summary>
			/// Visual Recognition classname.
			/// </summary>
			public string className { get; set; }
			/// <summary>
			/// Visual Recognition Classifier Identifier.
			/// </summary>
			public string classifierID { get; set; }
			/// <summary>
			/// List of images as byteArrays.
			/// </summary>
			public ObservedList<byte[]> images = new ObservedList<byte[]>();

			/// <summary>
			/// VRClass Constructor.
			/// </summary>
			public VRClass()
			{
				images.Added += OnImageAdded;
				images.Removed += OnImageRemoved;
			}

			~VRClass()
			{
				images.Added -= OnImageAdded;
				images.Removed -= OnImageRemoved;
			}

			private void OnImageAdded()
			{
				Log.Debug("AppData", "Image added!");
				EventManager.Instance.SendEvent(Constants.ON_IMAGE_ADDED);
			}

			private void OnImageRemoved()
			{
				Log.Debug("AppData", "Image removed!");
				EventManager.Instance.SendEvent(Constants.ON_IMAGE_REMOVED);
			}
		}
		#endregion

		#region API Key
		/// <summary>
		/// The Visual Recognition APIKey
		/// </summary>
		public string APIKey
		{
			get { return m_APIKey; }
			set
			{
				m_APIKey = value;
				EventManager.Instance.SendEvent(Constants.ON_UPDATE_API_KEY);
			}
		}
		private string m_APIKey;
		#endregion

		#region Classifier IDs
		/// <summary>
		/// List of Classifier IDs
		/// </summary>
		public ObservedList<string> ClassifierIDs = new ObservedList<string>();

		private void OnClassifierIDAdded()
		{
			Log.Debug("AppData", "Classifier added!");
			EventManager.Instance.SendEvent(Constants.ON_CLASSIFIER_ADDED);
		}

		private void OnClassifierIDRemoved()
		{
			Log.Debug("AppData", "Classifier removed!");
			EventManager.Instance.SendEvent(Constants.ON_CLASSIFIER_ADDED);
		}
		#endregion

		#region ClassifiersBrief
		/// <summary>
		/// An object containing references to all of the classifiers with their names and identifiers.
		/// </summary>
		public GetClassifiersTopLevelBrief ClassifiersBrief
		{
			get { return m_ClassifiersBrief; }
			set
			{
				m_ClassifiersBrief = value;

				foreach (GetClassifiersPerClassifierBrief classifier in ClassifiersBrief.classifiers)
				{
					ClassifierIDs.Add(classifier.classifier_id);
				}

				EventManager.Instance.SendEvent(Constants.ON_CLASSIFIERS_UPDATED);
			}
		}
		private GetClassifiersTopLevelBrief m_ClassifiersBrief;
		#endregion

		#region ClassifiersVerbose
		/// <summary>
		/// List of Classifiers and their classes.
		/// </summary>
		public ObservedList<GetClassifiersPerClassifierVerbose> ClassifiersVerbose = new ObservedList<GetClassifiersPerClassifierVerbose>();

		private void OnClassiferVerboseAdded()
		{
			Log.Debug("AppData", "ClassifierVerbose added!");
			EventManager.Instance.SendEvent(Constants.ON_CLASSIFIER_VERBOSE_ADDED);
		}

		private void OnClassifierVerboseRemoved()
		{
			Log.Debug("AppData", "ClassifierVerbose removed!");
			EventManager.Instance.SendEvent(Constants.ON_CLASSIFIER_VERBOSE_REMOVED);
		}
		#endregion
	}
}
