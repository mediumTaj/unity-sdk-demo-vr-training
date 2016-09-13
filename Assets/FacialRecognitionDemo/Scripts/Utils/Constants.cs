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

namespace IBM.Watson.DeveloperCloud.Demos.FacialRecognition
{
	/// <summary>
	/// Event contstants.
	/// </summary>
	public class Event
	{
		/// <summary>
		/// The application has started.
		/// </summary>
		public const string ON_HAS_STARTED_UPDATED = "OnHasStartedUpdated";
		/// <summary>
		/// The application state has updated.
		/// </summary>
		public const string ON_UPDATE_APP_STATE = "OnUpdateAppState";
		/// <summary>
		/// A classifier was added.
		/// </summary>
		public const string ON_CLASSIFIER_ADDED = "OnClassifierAdded";
		/// <summary>
		/// A classifier was removed.
		/// </summary>
		public const string ON_CLASSIFIER_REMOVED = "OnClassifierRemoved";
		/// <summary>
		/// An image was added.
		/// </summary>
		public const string ON_IMAGE_ADDED = "OnImageAdded";
		/// <summary>
		/// An image was removed.
		/// </summary>
		public const string ON_IMAGE_REMOVED = "OnImageRemoved";
		/// <summary>
		/// The API Key was updated.
		/// </summary>
		public const string ON_API_KEY_UPDATED = "OnUpdateAPIKey";
		/// <summary>
		/// The classifiers were updated.
		/// </summary>
		public const string ON_CLASSIFIERS_UPDATED = "OnClassifiersUpdated";
		/// <summary>
		/// A verbose classifier was updated.
		/// </summary>
		public const string ON_CLASSIFIER_VERBOSE_ADDED = "OnClassiferVerboseAdded";
		/// <summary>
		/// A verbose classifier was removed.
		/// </summary>
		public const string ON_CLASSIFIER_VERBOSE_REMOVED = "OnClassiferVerboseRemoved";
		/// <summary>
		/// The API Key is being validated.
		/// </summary>
		public const string CHECK_API_KEY = "CheckAPIKey";
		/// <summary>
		/// The API Key is finished validating.
		/// </summary>
		public const string API_KEY_CHECKED = "APIKeyChecked";
		/// <summary>
		/// The API Key was validated.
		/// </summary>
		public const string ON_API_KEY_VALIDATED = "OnAPIKeyValidated";
		/// <summary>
		/// The API Key is invalid.
		/// </summary>
		public const string ON_API_KEY_INVALIDATED = "OnAPIKeyInvalidated";
		/// <summary>
		/// Delete a classifier.
		/// </summary>
		public const string ON_REQUEST_CLASSIFIER_DELETE_CONFIRMATION = "OnRequestClassifierDeleteConfirmation";
		/// <summary>
		/// Add a classifier to the classify list.
		/// </summary>
		public const string ON_CLASSIFIER_ID_TO_CLASSIFY_WITH_ADDED = "OnClassifierToClassifyWithAdded";
		/// <summary>
		/// Remove a classifier from the classify list.
		/// </summary>
		public const string ON_CLASSIFIER_ID_TO_CLASSIFY_WITH_REMOVED = "OnClassifierToClassifyWithRemoved";
		/// <summary>
		/// Add an endpoint to the endpoint list.
		/// </summary>
		public const string ON_ENDPOINT_ADDED = "OnEndpointAdded";
		/// <summary>
		/// Remove an endpoint from the endpoint list.
		/// </summary>
		public const string ON_ENDPOINT_REMOVED = "OnEndpointRemoved";
		/// <summary>
		/// The WebCamera dimensions have been updated.
		/// </summary>
        public const string ON_WEB_CAMERA_DIMENSIONS_UPDATED = "OnWebCameraDimensionsUpdated";
		/// <summary>
		/// The classification result has been received.
		/// </summary>
		public const string ON_CLASSIFICATION_RESULT = "OnClassificationResult";
		/// <summary>
		/// The detect faces result has been received.
		/// </summary>
		public const string ON_DETECT_FACES_RESULT = "OnDetectFacesResult";
		/// <summary>
		/// The recognize text result has been received.
		/// </summary>
		public const string ON_RECOGNIZE_TEXT_RESULT = "OnRecognizeTextResult";
		/// <summary>
		/// The image to classify has been received.
		/// </summary>
		public const string ON_IMAGE_TO_CLASSIFY = "OnImageToClassify";
		/// <summary>
		/// The scale factor between the WebCameraWidget and the RawImage displaying the texture has been updated.
		/// </summary>
		public const string ON_UPDATE_SCALE_FACTOR = "OnUpdateScaleFactor";
		/// <summary>
		/// The number of cameras on this device was updated.
		/// </summary>
		public const string ON_NUMBER_OF_WEB_CAMERAS_UPDATED = "OnNumberOfWebCamerasUpdated";
		public const string ON_WEB_CAMERA_INDEX_UPDATED = "OnWebCameraIndexUpdated";
	}

	/// <summary>
	/// Application states.
	/// </summary>
	public class AppState
	{
		/// <summary>
		/// No app state.
		/// </summary>
		public const int NONE = -1;
		/// <summary>
		/// The start screen.
		/// </summary>
		public const int START = 0;
		/// <summary>
		/// The config screen.
		/// </summary>
		public const int CONFIG = 1;
		/// <summary>
		/// The photo screen.
		/// </summary>
		public const int PHOTO = 2;
		/// <summary>
		/// The results screen.
		/// </summary>
		public const int CLASSIFY_RESULT = 3;
		/// <summary>
		/// The update screen.
		/// </summary>
		public const int UPDATE = 4;
		/// <summary>
		/// The training screen.
		/// </summary>
		public const int TRAIN = 5;
	}

	/// <summary>
	/// Visual Recognition Enpoints.
	/// </summary>
	public class Endpoint
	{
		/// <summary>
		/// No endpoint.
		/// </summary>
		public const int NONE = -1;
		/// <summary>
		/// Classify.
		/// </summary>
		public const int CLASSIFY = 0;
		/// <summary>
		/// DetectFaces.
		/// </summary>
		public const int DETECT_FACES = 1;
		/// <summary>
		/// RecognizeText.
		/// </summary>
		public const int RECOGNIZE_TEXT = 2;
	}
}
