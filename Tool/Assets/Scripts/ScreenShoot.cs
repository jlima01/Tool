using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class ScreenShoot : MonoBehaviour
{
    int width, height, clipX, clipY;
	string folderName;
	public string itemId;
	private GameObject itemObj;
	private Transform spawn;
    public IEnumerator CreatePicture()
    {
    	yield return new WaitForEndOfFrame();

		if(width > height)
		{
			clipX = width - height;
		}
		else if(height > width)
		{
			clipY = height - width;
		}
    	Texture2D texture = new Texture2D(width - clipX, height - clipY, TextureFormat.RGBA32, true); 

    	texture.ReadPixels(new Rect(clipX/2, clipY/2, width - clipX, height - clipY), 0, 0);
    	texture.Apply();

    	byte[] bytes = texture.EncodeToPNG();
		
		string path = Application.dataPath + "/" + folderName + "/" + itemId + ".png";

        if(Directory.Exists(Application.dataPath + "/" + folderName))
		{
			File.WriteAllBytes(path, bytes);
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