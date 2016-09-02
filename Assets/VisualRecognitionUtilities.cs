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
using IBM.Watson.DeveloperCloud.Utilities;
using IBM.Watson.DeveloperCloud.Logging;
using IBM.Watson.DeveloperCloud.Services.VisualRecognition.v3;
using System.IO;
using UnityEngine;
using IBM.Watson.DeveloperCloud.Connection;


//public static byte[] CreateZip()
//{
//    Stream stream = new Stream();
//    GZipStream zipStream = new GZipStream(stream, CompressionLevel.Fastest);


//}


//  Set apiKey
public class VRCredentials
{
    VisualRecognition m_VisualRecognition = new VisualRecognition();
    void Start()
    {

    }

    public void SetVisualRecognitionAPIKey(string apiKey)
    {
        Config.CredentialInfo visualRecognitionCredentials = new Config.CredentialInfo();

        visualRecognitionCredentials.m_ServiceID = "VisualRecognitionV3";
        visualRecognitionCredentials.m_Apikey = apiKey;
        visualRecognitionCredentials.m_URL = "https://gateway-a.watsonplatform.net/visual-recognition/api";
        visualRecognitionCredentials.m_Note = "This ApiKey was added at runtime.";
            
        for (int i = 0; i < Config.Instance.Credentials.Count; i++)
        {
            if (Config.Instance.Credentials[i].m_ServiceID == visualRecognitionCredentials.m_ServiceID)
            {
                if (Config.Instance.Credentials[i].m_Apikey != visualRecognitionCredentials.m_Apikey)
                {
					Log.Debug("VisualRecognitionUtilities", "Deleting existing visual recognition APIKEY");
                    Config.Instance.Credentials.RemoveAt(i);
                }
                else
                {
                    Log.Debug("VisualRecognitionUtilities", "API Key matches - not replacing!");
					return;
                }
            }
        }

		Log.Debug("VisualRecognitionUtilities", "Adding visual recognition APIKEY | serviceID: {0} | APIKey: {1} | URL: {2} | Note: {3}", visualRecognitionCredentials.m_ServiceID, visualRecognitionCredentials.m_Apikey, visualRecognitionCredentials.m_URL, visualRecognitionCredentials.m_Note);

		Config.Instance.Credentials.Add(visualRecognitionCredentials);

		if (!Directory.Exists(Application.streamingAssetsPath))
			Directory.CreateDirectory(Application.streamingAssetsPath);
		File.WriteAllText(Application.streamingAssetsPath + "/Config.json", Config.Instance.SaveConfig());
		RESTConnector.FlushConnectors();
	}

    public void GetClassifiers()
    {
    if (!m_VisualRecognition.GetClassifiers(OnGetClassifiers))
        Log.Debug("VisualRecognitionUtilities", "Failed to get classifiers!");
    }

    public void OnGetClassifiers(GetClassifiersTopLevelBrief classifiers, string data)
    {

    }
}
