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
	public class Event
	{
		public const string ON_HAS_STARTED_UPDATED = "OnHasStartedUpdated";
		public const string ON_UPDATE_APP_STATE = "OnUpdateAppState";
		public const string ON_CLASSIFIER_ADDED = "OnClassifierAdded";
		public const string ON_CLASSIFIER_REMOVED = "OnClassifierRemoved";
		public const string ON_IMAGE_ADDED = "OnImageAdded";
		public const string ON_IMAGE_REMOVED = "OnImageRemoved";
		public const string ON_API_KEY_UPDATED = "OnUpdateAPIKey";
		public const string ON_CLASSIFIERS_UPDATED = "OnClassifiersUpdated";
		public const string ON_CLASSIFIER_VERBOSE_ADDED = "OnClassiferVerboseAdded";
		public const string ON_CLASSIFIER_VERBOSE_REMOVED = "OnClassiferVerboseRemoved";
		public const string CHECK_API_KEY = "CheckAPIKey";
		public const string API_KEY_CHECKED = "APIKeyChecked";
		public const string ON_API_KEY_VALIDATED = "OnAPIKeyValidated";
		public const string ON_API_KEY_INVALIDATED = "OnAPIKeyInvalidated";
		public const string ON_REQUEST_CLASSIFIER_DELETE_CONFIRMATION = "OnRequestClassifierDeleteConfirmation";
	}

	public class AppState
	{
		public const int NONE = -1;
		public const int START = 0;
		public const int CONFIG = 1;
		public const int PHOTO = 2;
		public const int CLASSIFY_RESULT = 3;
		public const int ADD_TO_TRAINING = 4;
		public const int TRAINING = 5;
		public const int TRAINING_SUCCESS = 6;
		public const int TRAINING_FAILURE = 7;
		public const int TRAIN = 8;
		public const int TRAIN_OPTIONS = 9;
		public const int TRAIN_PHOTO = 10;
		public const int RESULT_PHOTOS = 11;
	}
}
