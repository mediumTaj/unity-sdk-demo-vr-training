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
using System.Collections;
using System;
using System.IO;
using Ionic.Zip;

namespace IBM.Watson.DeveloperCloud.Demos.FacialRecognition
{
    static public class Utils
    {
		/// <summary>
		/// The delegate for loading a file, used by TrainClassifier().
		/// </summary>
		/// <param name="filename">The filename to load.</param>
		/// <returns>Should return a byte array of the file contents or null of failure.</returns>
		public delegate byte[] LoadFileDelegate(string filename);
		/// <summary>
		/// Set this property to overload the internal file loading of this class.
		/// </summary>
		public static LoadFileDelegate LoadFile { get; set; }

        /// <summary>
        /// Creates image files and zip files and returns zip byte array data.
        /// </summary>
        /// <param name="trainingSet">Training set to create the zip file for.</param>
        /// <returns>Returns .zip file data.</returns>
		static public byte[] GetZipByteArray(AppData.TrainingSet trainingSet)
        {
            if (trainingSet.imagesData.Count == 0)
                throw new Utilities.WatsonException("The specified training set does not have associated image data!");
            if (string.IsNullOrEmpty(trainingSet.className))
                throw new ArgumentNullException("className");

            string className = trainingSet.className;
            string directoryPath = Path.Combine(AppData.Instance.VisualRecognitionTrainingDataPath, className);

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            for (int i = 0; i < trainingSet.imagesData.Count; i++)
            {
                byte[] imageData = trainingSet.imagesData[i];
                File.WriteAllBytes(directoryPath + i + ".png", imageData);
            }

            using (ZipFile zipFile = new ZipFile())
            {
                zipFile.AddDirectory(FindDirectory(directoryPath, className));
                zipFile.Save(className + ".zip");
            }

            byte[] zipData = null;

            if (LoadFile != null)
            {
                zipData = LoadFile(className + ".zip");
            }
            else
            {
#if !UNITY_WEBPLAYER
                zipData = File.ReadAllBytes(className + ".zip");
#endif
            }

            return zipData;
        }

        private static string FindDirectory(string check, string name)
        {
            foreach (var d in Directory.GetDirectories(check))
            {
                string dir = d.Replace("\\", "/");        // normalize the slashes
                if (dir.EndsWith(name))
                    return d;

                string found = FindDirectory(d, name);
                if (found != null)
                    return found;
            }

            return null;
        }
    }
}
