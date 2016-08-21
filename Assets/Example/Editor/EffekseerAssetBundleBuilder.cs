using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class EffekseerAssetBundleBuilder
{
	// Bundles直下のディレクトリをAssetBundleとしてビルドする
	[MenuItem("AssetBundles/Build by Effekseer")]
	static void BuildAssetBundles()
	{
		string bundleRoot = Application.dataPath + "/" + "Bundles";

		string[] bundlePath = Directory.GetDirectories(bundleRoot, "*", SearchOption.TopDirectoryOnly);
		AssetBundleBuild[] builds = new AssetBundleBuild[bundlePath.Length];
		for (int i = 0; i < bundlePath.Length; i++) {
			builds[i] = new AssetBundleBuild();
			bundlePath[i] = bundlePath[i].Replace("\\", "/");
			string bundleName = Path.GetFileName(bundlePath[i]);

			string[] assetPaths = Directory.GetFiles(bundlePath[i], "*.*", SearchOption.AllDirectories);
			List<string> assetList = new List<string>();
			List<string> assetExtList = new List<string>();
			for (int j = 0; j < assetPaths.Length; j++) {
				string path = assetPaths[j];
				if (Path.GetExtension(path) == ".meta") {
					continue;
				}
				path = path.Replace("\\", "/");
				path = path.Replace(bundlePath[i] + "/", "");
				assetExtList.Add(Path.GetExtension(path));
				path = path.Substring(0, path.LastIndexOf("."));
				assetList.Add(path);
			}
			string[] assetNames = assetList.ToArray();
			string[] assetExts = assetExtList.ToArray();

			//Assetの配列を作成
			Object[] assets = new Object[assetNames.Length];
			for (int j = 0; j < assetNames.Length; j++) {
				string path = "Assets/Bundles/" + bundleName + "/" + assetNames[j] + assetExts[j];
				assets[j] = AssetDatabase.LoadAssetAtPath(
					path, typeof(UnityEngine.Object));
				Debug.Log(path);
			}
			
            /*
			//AssetBundleを出力
			string outputPath = Application.streamingAssetsPath + "/" + bundleName;
			BuildPipeline.BuildAssetBundleExplicitAssetNames(
				assets, assetNames, outputPath,
				BuildAssetBundleOptions.ChunkBasedCompression);
	        */	
        }
	}
}
