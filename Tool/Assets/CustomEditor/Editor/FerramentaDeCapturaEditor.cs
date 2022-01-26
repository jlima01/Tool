using UnityEngine;
using UnityEditor;
using System.IO;
public class FerramentaDeCapturaEditor : ScriptableWizard
{
	#region Variáveis ocultas
	Object[] itemsPrefabs;
	public GameObject camera, spawn;
	public int width, height;
	int i = 0;
	bool showBackgrounds = false;
	public string nomeDaPasta = "IconesDosItens", prefabsPathName = "";
	string itemId = "item";
	private bool itemCreated, spawning, OpenedFoldable;

	#endregion

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

    private void DrawVariables()
    {
        #region Variaveis Visíveis

        EditorGUILayout.Space(8);

        camera = EditorGUILayout.ObjectField("Camera De Captura", camera, typeof(GameObject), true) as GameObject;
        spawn = EditorGUILayout.ObjectField("Posição De Captura ", spawn, typeof(GameObject), true) as GameObject;

        EditorGUILayout.Space(24);

        #endregion

        #region Resolução

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

        #endregion

        #region Items Prefabs

        EditorGUILayout.Space(24);

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

		/* if (GUILayout.Button("Deletar Pasta", GUILayout.MaxWidth(90)))
        {
            OnWizardErase();
        } */

        EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space(8);

        GUILayout.Label("Carregar Prefabs de: ", EditorStyles.boldLabel);

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("     Resources/", GUILayout.MaxWidth(90));
        prefabsPathName = EditorGUILayout.TextField(prefabsPathName, GUILayout.MaxWidth(100));

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

        #endregion

        #region Autor

        EditorGUILayout.Space(100);

        GUILayout.Label("Ferramenta desenvolvida por:  Jônatas Lima");

        #endregion
    }
    #region Métodos
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
	/* void OnWizardErase()
    {
        //Passa longe disso! %$#@ Apagou toda pasta assets do projeto! &%$#@
		if(Directory.Exists("Assets" + "/" + nomeDaPasta))
		{
			bool deleteFolder = AssetDatabase.DeleteAsset("Assets" + "/" + nomeDaPasta);
		}
		else
		{
			Debug.Log("Directory Not Founded exception!");
		}
    } */
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
    private void GenerateImages()
    {
		if(camera.GetComponent<ScreenShoot>() != null)
    		camera.GetComponent<ScreenShoot>().TakeScreenShoot(width, height, itemId, spawn.transform, nomeDaPasta);
		else
		{
			Debug.Log("No ScreenShoot component!");
		}
    }
	private void SetParameters()
	{
		if(spawn == null)
		{
			spawn = new GameObject();
			spawn.name = "PosiçãoDeCapituraPadrão";
		}
		
		nomeDaPasta = "IconesTOS";
		prefabsPathName = "Items";
		camera = Camera.main.gameObject;

		if(camera.GetComponent<ScreenShoot>() == null)
		{
			camera.AddComponent<ScreenShoot>();
		}
		
		width = 1024;
		height = 1024;
	}
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
	private void Reset()
	{
		width = 0;
		height = 0;
		i = 0;
		camera = null;
		spawn = null;
		nomeDaPasta = "";
		prefabsPathName = "";
	}
	
	#endregion
}