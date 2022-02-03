using UnityEngine;
using UnityEditor;
using System.IO;
public class FerramentaDeCapturaEditor : ScriptableWizard
{
	#region Variáveis ocultas
	Object[] itemsPrefabs;
	public GameObject camera, spawn;
	public static Ajustment ajustment;
	public static ItemType itemType;
	public int width, height;
	public float margin = 1, camPos = -3;
	float mult = 0, cameraPositionStaff = 1.75f, cameraPositionSword = 2, cameraPositionTool = 1.5f, cameraPositionBow = 1.75f;
	int i = 0;
	//float counter = 0;
	bool showBackgrounds = false;//, buttonPressed = false;
	public string nomeDaPasta = "IconesDosItens", prefabsPathName = "", swordPathName = "Items/Weapons/Two Hand Sword", toolsPathName = "Items/Weapons/Tools", staffPathName = "Items/Weapons/Staff", bowPathName = "Items/Weapons/Bow";
	string itemId = "item";
	Vector3 centerPos;

	#endregion

	#region  Criação da ferramenta

	[MenuItem("Tools/Ferramenta De Captura Editor")]

    public static void ShowWindow()
    {
    	GetWindow(typeof(FerramentaDeCapturaEditor));
    }

    private void OnGUI()
    {
        GUILayout.Label("Ferramenta De Captura", EditorStyles.boldLabel);

        DrawVariables();
    }

	#endregion

	#region Enum

	public enum Ajustment
	{
		Automatico,
		Manual,
	}
	public enum ItemType
	{
		Bow,
		Staff,
		Tool,
		Sword,
	}

	#endregion

	#region Desenhar variáveis
    private void DrawVariables()
    {
        #region Variaveis Visíveis

        EditorGUILayout.Space(8);

        camera = EditorGUILayout.ObjectField("Camera De Captura", camera, typeof(GameObject), true) as GameObject;
        spawn = EditorGUILayout.ObjectField("Posição De Captura ", spawn, typeof(GameObject), true) as GameObject;

		EditorGUILayout.Space(4);

		if(ajustment == Ajustment.Manual)
		{
			bowPathName = EditorGUILayout.TextField("Pasta dos itens Bow", bowPathName);
			staffPathName = EditorGUILayout.TextField("Pasta dos itens Staff", staffPathName);
			swordPathName = EditorGUILayout.TextField("Pasta dos itens Sword", swordPathName);
			toolsPathName = EditorGUILayout.TextField("Pasta dos itens Tools", toolsPathName);
		}
		else
		{
			prefabsPathName = EditorGUILayout.TextField("Pasta dos itens", prefabsPathName);
		}

		EditorGUILayout.Space(4);

		GUILayout.Label("Tipo de Ajuste", EditorStyles.boldLabel);

		EditorGUILayout.Space(4);

		ajustment = (Ajustment)EditorGUILayout.EnumPopup(ajustment);
		
        EditorGUILayout.Space(10);

        #endregion

        #region Resolução, Margem e Posição da Camera

		GUILayout.Label("Resolução Da Captura", EditorStyles.boldLabel);

		EditorGUILayout.Space(8);

		EditorGUILayout.BeginHorizontal();

		EditorGUILayout.BeginHorizontal();

		GUILayout.Label("Width", GUILayout.MaxWidth(50));
		width = EditorGUILayout.IntField(width, GUILayout.MaxWidth(50));
		GUILayout.Label("px", GUILayout.MaxWidth(25));

		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();

		GUILayout.Label("Height", GUILayout.MaxWidth(50));
		height = EditorGUILayout.IntField(height, GUILayout.MaxWidth(50));
		GUILayout.Label("px", GUILayout.MaxWidth(25));

		EditorGUILayout.EndHorizontal();

		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space(4);

        if(ajustment == Ajustment.Automatico)
		{
			EditorGUILayout.BeginHorizontal();

			GUILayout.Label("Margem", GUILayout.MaxWidth(75));

			margin = EditorGUILayout.Slider(margin, 0, 10, GUILayout.MaxWidth(125));

			EditorGUILayout.EndHorizontal();

			//prefabsPathName = "Items";
		}
		else
		{
			EditorGUILayout.BeginHorizontal();

			GUILayout.Label("Posição da Camera", GUILayout.MaxWidth(125));

			camPos = EditorGUILayout.Slider(camPos, 0, 10, GUILayout.MaxWidth(125));

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space(4);

			GUILayout.Label("Tipo de Item", EditorStyles.boldLabel);

			EditorGUILayout.Space(2);

			itemType = (ItemType)EditorGUILayout.EnumPopup(itemType, GUILayout.MaxWidth(75));

			switch(itemType)
			{
				case ItemType.Bow:
					camPos = cameraPositionBow;
					prefabsPathName = bowPathName;
				break;

				case ItemType.Staff:
					camPos = cameraPositionStaff;
					prefabsPathName = staffPathName;
				break;

				case ItemType.Sword:
					camPos = cameraPositionSword;
					prefabsPathName = swordPathName;
				break;

				case ItemType.Tool:
					camPos = cameraPositionTool;
					prefabsPathName = toolsPathName;
				break;
			}
		}

        #endregion

        #region Items Prefabs

        EditorGUILayout.Space(12);

        GUILayout.Label("Prefabs dos Items", EditorStyles.boldLabel);

		EditorGUILayout.Space(8);

		showBackgrounds = EditorGUILayout.Foldout(showBackgrounds, "ItemsPrefabs", true);
		
        if(showBackgrounds)
		{
			SetPrefabs();
		}

        EditorGUILayout.Space(8);

        #endregion

        #region Criar Pastas

        EditorGUILayout.Space(8);

        GUILayout.Label("Salvar imagens em: ", EditorStyles.boldLabel);

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("     Assets/", GUILayout.MaxWidth(60));
        nomeDaPasta = EditorGUILayout.TextField(nomeDaPasta, GUILayout.MaxWidth(100));

        if (GUILayout.Button("Criar Pasta", GUILayout.MaxWidth(75)))
        {
            OnWizardCreate();
        }

        EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space(8);

        GUILayout.Label("Carregar Prefabs de: ", EditorStyles.boldLabel);

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("     Resources/", GUILayout.MaxWidth(90));
        prefabsPathName = EditorGUILayout.TextField(prefabsPathName, GUILayout.MaxWidth(200));

        EditorGUILayout.EndHorizontal();

        #endregion

        #region Botões

        EditorGUILayout.Space(16);

        if (GUILayout.Button("Preencher Parâmetros"))
        {
            SetParameters();
			SetPrefabs();
        }

		if (GUILayout.Button("Gerar Imagens"))
		{
			SpawnItems();
		}

        if (GUILayout.Button("Resetar"))
        {
            Reset();
        }

		if (GUILayout.Button("Resetar Contador"))
        {
            ResetCounter();
        }

        #endregion

        #region Autor

        EditorGUILayout.Space(70);

        GUILayout.Label("Ferramenta desenvolvida por:  Jônatas Lima");

        #endregion
    }

	#endregion

    #region Métodos

	#region Preencher os parametros necessários
	private void SetParameters()
	{
		if(GameObject.FindGameObjectWithTag("Spawn") == null)
		{
			spawn = new GameObject();
			spawn.name = "PosiçãoDeCapituraPadrão";
			spawn.tag = "Spawn";
			//spawn.transform.rotation = Quaternion.Euler(0 , 0, -45);
		}
		else
		{
			spawn = GameObject.FindGameObjectWithTag("Spawn");
		}
		
		nomeDaPasta = "IconesTOS";
		prefabsPathName = "Items";
		camera = Camera.main.gameObject;

		if(camera.GetComponent<ScreenShoot>() == null)
		{
			camera.AddComponent<ScreenShoot>();
		}
		
		margin = 1;
		ajustment = Ajustment.Automatico;
		width = 1024;
		height = 1024;
	}

	#endregion

	#region Pegar os prefabs a serem geradas as imagens
	private void SetPrefabs()
	{
		EditorGUI.indentLevel++;
	
		if(prefabsPathName == null || prefabsPathName == "" || Resources.LoadAll(prefabsPathName, typeof(GameObject)) == null)
		{
			Debug.Log("Pasta inesistente ou vazia!");
		}
		else
		{
			itemsPrefabs = Resources.LoadAll(prefabsPathName, typeof(GameObject));
		}

		int arraySize = Mathf.Max(0, EditorGUILayout.IntField("Quantidade", itemsPrefabs.Length));

		for(int i = 0; i < itemsPrefabs.Length; i++)
		{
			itemsPrefabs[i] = EditorGUILayout.ObjectField("Element " + i, itemsPrefabs[i], typeof(GameObject), false) as GameObject;
		}

		EditorGUI.indentLevel--;
	}

	#endregion

	#region Método para criar pasta para salvar imagens
    void OnWizardCreate()
    {
		if(nomeDaPasta == "" || nomeDaPasta == null)
		{
			nomeDaPasta = "IconesPNG";
			string primaryFolder = AssetDatabase.CreateFolder("Assets", nomeDaPasta);
		}
		else
		{
			string primaryFolder = AssetDatabase.CreateFolder("Assets", nomeDaPasta);
		}
	}
	
	#endregion

	#region Gerar itens e apagá-los depois dos prints
	private void SpawnItems()
	{
		if(itemsPrefabs == null)
		{
			Debug.Log("Nenhum item/prefab adicionado!");
			return;
		}

		GenerateItens(i);
	}
	private void GenerateItens(int counter)
	{
		if(itemsPrefabs[counter] != null && spawn != null)
		{
			Instantiate(itemsPrefabs[counter], spawn.transform.position, spawn.transform.rotation, spawn.transform);
			itemId = itemsPrefabs[counter].name;

			GameObject item = itemsPrefabs[counter] as GameObject;

			Transform itemOb = spawn.transform.GetChild(0);

			if(ajustment == Ajustment.Automatico)
			{
				if(item.GetComponentInChildren<MeshFilter>() != null)
				{
					Mesh mesh = item.GetComponentInChildren<MeshFilter>().sharedMesh;
					
					float maxExtent = mesh.bounds.extents.magnitude;
					float minDistance = (maxExtent * margin) / Mathf.Sin(Mathf.Deg2Rad * Camera.main.fieldOfView / 2f);
					Camera.main.transform.position = itemOb.transform.position - Vector3.forward * minDistance;
					Camera.main.nearClipPlane = minDistance - maxExtent;

				}
				else if(item.GetComponentInChildren<SkinnedMeshRenderer>() != null)
				{
					Mesh mesh = item.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;
					
					float maxExtent = mesh.bounds.extents.magnitude;
					float minDistance = (maxExtent * margin) / Mathf.Sin(Mathf.Deg2Rad * Camera.main.fieldOfView / 2f);
					Camera.main.transform.position = itemOb.transform.position - Vector3.forward * minDistance;
					Camera.main.nearClipPlane = minDistance - maxExtent;
				}
			}
			else
			{
				if(item.GetComponentInChildren<MeshFilter>() != null)
				{
					Mesh mesh = item.GetComponentInChildren<MeshFilter>().sharedMesh;

					//Camera.main.transform.position = new Vector3(mesh.bounds.center.x, mesh.bounds.center.y, -camPos);
					Camera.main.transform.position = new Vector3(0, 0, -camPos);
				}
				else if(item.GetComponentInChildren<SkinnedMeshRenderer>() != null)
				{
					Mesh mesh = item.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;

					//Camera.main.transform.position = new Vector3(mesh.bounds.center.x, mesh.bounds.center.y, -camPos);
					Camera.main.transform.position = new Vector3(0, 0, -camPos);
				}
				
			}
			
			GenerateImages();

			i++;

			if(i > itemsPrefabs.Length - 1)
			{
				i = 0;
			}
		}
		else
		{
			Debug.Log("No items or spawn to generate items!");
		}
	}

	#endregion

	#region  Gerar e salvar imagens em PNG
    private void GenerateImages()
    {
		if(camera.GetComponent<ScreenShoot>() != null)
		{
			camera.GetComponent<ScreenShoot>().TakeScreenShoot(width, height, Camera.main.pixelWidth, Camera.main.pixelHeight, itemId, spawn.transform, nomeDaPasta);
		}	
		else
		{
			Debug.Log("No ScreenShoot component!");
		}
    }

	#endregion

	#region Resetar parâmetros
	private void Reset()
	{
		width = 0;
		height = 0;
		margin = 0;
		i = 0;
		camera = null;
		spawn = null;
		ajustment = Ajustment.Automatico;
		camPos = 0;
		
		if(GameObject.FindGameObjectWithTag("Spawn") != null)
		{
			DestroyImmediate(GameObject.FindGameObjectWithTag("Spawn"));
		}
		
		nomeDaPasta = "";
		prefabsPathName = "";
	}
	private void ResetCounter()
	{
		i = 0;
	}

	#endregion
	
	#endregion
}