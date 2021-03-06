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
		// Esperar recursos serem desenhados na tela

    	yield return new WaitForEndOfFrame();

		// Caso opte por manter a resolução quadrada

		/* if(cameraWidth > cameraHeight)
		{
			clipX = cameraWidth - cameraHeight;
		}
		else if(cameraHeight > cameraWidth)
		{
			clipY = cameraHeight - cameraWidth;
		} */

    	//Texture2D texture = new Texture2D(cameraWidth - clipX, cameraHeight - clipY, TextureFormat.RGBA32, true); 

		//criar textura e limpar os pixels para fundo transparente

		Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, true); 
		
		for (int y = 0; y < texture.height; y++) {
             for (int x = 0; x < texture.width; x++) {
 
                 texture.SetPixel(x, y, Color.clear);
 
             }
         }
 
        texture.Apply();

		//Preencher pixels com captura da camera

    	//texture.ReadPixels(new Rect(0, 0, cameraWidth, cameraHeight), 0, 0);
		
		texture.ReadPixels(new Rect((cameraWidth - width)/2, (cameraHeight - height)/2, cameraWidth, cameraHeight), 0, 0);
    	texture.Apply();
		
		//Salvar imagem em png em pasta escolhida se existir

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

		//Destruir objeto criado

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