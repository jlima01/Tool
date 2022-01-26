using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class ScreenShoot : MonoBehaviour
{
    int width, height;
	string folderName;
	public string itemId;
	private GameObject itemObj;
	private Transform spawn;
    public IEnumerator CreatePicture()
    {
    	yield return new WaitForEndOfFrame();
    	Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, true); 

    	texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
    	texture.Apply();

    	byte[] bytes = texture.EncodeToPNG();
		
		string path = Application.dataPath + "/" + folderName + "/" + itemId + ".png";

        if(Directory.Exists(Application.dataPath + "/" + folderName))
		{
			if(File.Exists(path))
			{
				File.WriteAllBytes(path, bytes);
			}
			else
			{
				Debug.Log("File not founded!");
			}
		}
		else
			{
				Debug.Log("Directory not founded!");
			}

    	DestroyImmediate(texture);

		if(spawn.transform.GetChild(0).gameObject != null)
		{
			itemObj = spawn.transform.GetChild(0).gameObject;
		}

		DestroyImmediate(itemObj);
    }

    public void TakeScreenShoot(int w, int h, string i, Transform sp, string name)
    {
		width = w;
		height = h;
		itemId = i;
		spawn = sp;
		folderName = name;
    	StartCoroutine("CreatePicture");
    }
}