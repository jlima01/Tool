using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class ScreenShoot : MonoBehaviour
{
    int width, height, clipX, clipY, cameraWidth, cameraHeight;
	string folderName;
	public string itemId;
	private GameObject itemObj;
	private Transform spawn;
    public IEnumerator CreatePicture()
    {
    	yield return new WaitForEndOfFrame();

		if(cameraWidth > cameraHeight)
		{
			clipX = cameraWidth - cameraHeight;
		}
		else if(cameraHeight > cameraWidth)
		{
			clipY = cameraHeight - cameraWidth;
		}

    	//Texture2D texture = new Texture2D(cameraWidth - clipX, cameraHeight - clipY, TextureFormat.RGBA32, true); 
		Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, true); 
		
		for (int y = 0; y < texture.height; y++) {
             for (int x = 0; x < texture.width; x++) {
 
                 texture.SetPixel(x, y, Color.clear);
 
             }
         }
 
        texture.Apply();

    	//texture.ReadPixels(new Rect(clipX/2, clipY/2, cameraWidth - clipX, cameraHeight - clipY), 0, 0);
		texture.ReadPixels(new Rect((cameraWidth - width)/2, (cameraHeight - height)/2, cameraWidth - clipX, cameraHeight - clipY), 0, 0);
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

    public void TakeScreenShoot(int w, int h, int camW, int camH, string i, Transform sp, string name)
    {
		width = w;
		height = h;
		cameraWidth = camW;
		cameraHeight = camH;
		itemId = i;
		spawn = sp;
		folderName = name;
    	StartCoroutine("CreatePicture");

		//Debug.Log(transform.position);
    }
}