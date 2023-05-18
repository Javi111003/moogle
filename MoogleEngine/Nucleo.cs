namespace MoogleEngine;

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

//Clase que es el núcleo del proyecto, donde se realiza la consulta y se calcula el score de los documentos en base a esta 
public class Nucleo
{	
	private List<Documento> Documentos = new List<Documento>();//lista de los documentos
	public  List<string> ListadoPalabrasDocumentos = new List<string>();//lista de las palabras que aparecen en todos los documentos que se utilizará para buscar las sugerencias
	private string SugerenciaBusqueda = string.Empty;//sugerencia de la búsqueda
	
	//Constructor de la clase
	public Nucleo()
	{	List<string> archivos;//listado de los documentos
		
//Buscamos la ruta de los archivos
		
		string directorio = Directory.GetCurrentDirectory();
		char separador = directorio.Contains('\\') ? '\\' : '/'; // separador para windows o para linux
		if (directorio.Length>3)//si no es raiz del disco
   			directorio = directorio.Substring(0,directorio.LastIndexOf(separador)+1);//sacamos la subcadena hasta el ultimo '\' para subir al directorio anterior
		Herramientas.CaminoDocumentos =  directorio + "Content";//inicializamos el camino de los documentos
		

		Herramientas.PalabrasVacias = ListaStopwords.CreaListado();//se crea la lista de palabras vacías
		archivos = Herramientas.ListaArchivos(Herramientas.CaminoDocumentos);//lista de archivos
		for (int i =0; i< archivos.Count(); i++)
			Documentos.Add(new Documento(archivos[i]));
		//adicionamos a la lista de palabras en los documentos las palabra que hay en cada uno de ellos
		foreach(var doc in Documentos )//para cada documento
			foreach(var palabra in doc.Terminos)//para cada palabra
				if (!ListadoPalabrasDocumentos.Contains(palabra))//si no esta ya en la lista
					ListadoPalabrasDocumentos.Add(palabra);//la adicionamos

	} 
		
	//busca la sugerencia de una palabra que puede estar mal escrita en la consulta. Se toman las palabras que hay en los documentos como referencia
	//calculamos el porciento de similitud entre ellas y la que tenga mayor porciento será la sugerencia
	public static string BuscarSugerencia( string palabra, List<string> PalabrasdelosDoc )
    {              
        string resultado = string.Empty;//se inicia sin sugerencia a la palabra
        int distancia = int.MaxValue;//entre palabras 
        double tempporciento =0;//temporal para utilizar en el calculo del porciento de similitud
        double porciento =0;//porciento de similitud

        if ((palabra != null) && (PalabrasdelosDoc != null) && (palabra.Length !=0) && (PalabrasdelosDoc.Count !=0)){//si se cumplen las condiciones
            foreach(string temppalabra in PalabrasdelosDoc) {//pala cada una de las palabras en la lista de palabras de los documentos
                distancia = Herramientas.DistanciaDamerauLevenshtein(palabra, temppalabra);//calculamos la distancia entre palabras
                tempporciento = (1.0 - ((double)distancia / (double)Math.Max(palabra.Length, temppalabra.Length)));//calculamos el porciento de similitud entre las dos palabras

                if (tempporciento > porciento) {//si el porciento de similitud es mayor al que teníamos
                    porciento = tempporciento;//cambiamos el porciento de similitud
                    resultado = temppalabra;//guardamos la palabra como resultado
                }             
            }
        }
		
        return resultado;//palabra con mayor similitud a la incorrecta
    }
    
	//devuelve la sugerencia a la búsqueda
	public string Sugerencia
	{
		get{return this.SugerenciaBusqueda;}
	}
	

	public SearchItem[] RealizarConsulta(string consulta)
	{	
		Query ConsultaRealizada = new Query(consulta);//se crea objeto para la consulta
		List<Documento> DocumentosConsulta = new List<Documento>();//lista donde se almacenaran los documentos sobres los que se realizara la consulta
		bool[] documentonecesario = new bool[Documentos.Count];//arreglo para definir si un documento es necesario o no para la consulta
		string[] fraseconsulta;//arreglo donde se almacenaran los terminos de la consulta para buscarlos en el texto
		string palabra = string.Empty;
		string lema = string.Empty;
		SearchItem[] resultado;//resultado a devolver


		SugerenciaBusqueda = "";//inicializamos la sugerencia de la búsqueda vacía
		
		fraseconsulta = new string[ConsultaRealizada.Terminos.Count];//inicializamos el arreglo
		//los copiamos para el arreglo
		int k = 0; 
		foreach (var termino in ConsultaRealizada.Terminos)
        {

            fraseconsulta[k] =termino.Key;
			k++;
        }

		//del total de documentos, buscamos los documentos que incluiremos o no en la consulta
		//si un documento no tiene ninguno de los terminos y pasamos a calcular su peso, crearía columnas vacias en la matriz

		for ( int i = 0; i < Documentos.Count; i++)//para cada documento
			documentonecesario[i] = Documentos[i].ExistenTerminos(fraseconsulta);//vemos si existe al menos un término o no ha ninguno
		//procesamos lor terminos prohibidos que no deben aparecer en los documentos y los obligatorios que deben aparecer en todos
		for ( int i = 0; i < Documentos.Count; i++){//para cada uno de los documentos
				foreach (string termino in ConsultaRealizada.TerminosProhibidos)//para cada uno de los términos prohibidos
					if (Documentos[i].ExisteTermino(termino))//existe el termino en el documento
						documentonecesario[i] = false;//no se buscara en este documento
			
				foreach (string termino in ConsultaRealizada.TerminosObligatorios)//para cada uno de los términos obligatorios
					if (!Documentos[i].ExisteTermino(termino))//no existe el término en el documento
						documentonecesario[i] = false;//no se buscara en este documento
		}	
		//creamos la lista de los documentos sobre los que se realizará la consulta
		for ( int i = 0; i < documentonecesario.Length; i++)//para todos los documentos
			if (documentonecesario[i])//si aparece como verdadero
				DocumentosConsulta.Add(Documentos[i]);//se adiciona a la lista
		
		//creamos el vector TF de la consulta. la cantidad de elementos serán la cantidad de terminos de la consulta y para cada par de términos que se quiera saber la distancia se le agrega un elememnto
		int[] vectorconsulta = new int[ConsultaRealizada.Terminos.Count()+ConsultaRealizada.TerminosCercanos.Count()];//cantidad de términos consulta + cantidad de par de términos por distancia
		//creamos matriz TF de los documentos, que va a tener una fila por cada documento y una columna por cada elemento del vector TF de la consulta
		int[,] vectoresdocumentos = new int[DocumentosConsulta.Count, ConsultaRealizada.Terminos.Count()+ConsultaRealizada.TerminosCercanos.Count()];
		
		
		//damos valores a los elementos del vector TF de la consulta
		k = 0; 
		foreach (var termino in ConsultaRealizada.Terminos) {//para cada uno de los términos de la consulta

            vectorconsulta[k] =termino.Value;//ponemos la cantidad de veces que aparece el término

			k++;
        }
		foreach (var termino in ConsultaRealizada.TerminosCercanos) {//para cada uno de los pares de términos de la consulta
            vectorconsulta[k] =1;//ponemos la distancia entre ellos en la consulta a 1
			k++;
        }
		
		//damos valores a los elementos de la matriz TF de los documentos
		int j = 0;
		foreach(var doc in DocumentosConsulta ){//para cada uno de los documentos
			k = 0; 
			foreach (var termino in ConsultaRealizada.Terminos){//para cada uno de los terminos de la consulta
				vectoresdocumentos[j,k] = doc.CuentaTermino(termino.Key);//ponemos la cantidad de veces que aparece el término en el documento
				k++;
			}
			foreach (var termino in ConsultaRealizada.TerminosCercanos){//para cada uno de los pares de términos de la consulta
				vectoresdocumentos[j,k] = doc.DistanciaMinTerminos(termino.X,termino.Y);//ponemos la distancia mínima entre ellos en el documento
				k++;
			}
			j++;
		}

		//Relizaremos la revisión de la matriz TF de los documentos antes de pasarla al calculo de los pesos
		
		//paso 1: ver que colunmas estan en cero y si hay alguna buscamos si la palabra esta mal escrita y nos da el sistema una sugerencia
		//calculamos la frecuencia del nuevo termino en los documentos
		//paso 2: ver que colunmas estan en cero y si hay alguna buscamos la raiz de la palabra
		//intentamos buscar una sugerencia referente a la raiz de la palabra y devolverla multiplicada por tres para un menor peso
		//paso 3:  ver que colunmas estan en cero y eliminar para no invalidar el calculo

		//califica el termino segun el paso en el que esta 
		int[] estadotermino = new int[ConsultaRealizada.Terminos.Count()];
		
		for(k=0; k< estadotermino.Length;k++)//para cada elemento
			estadotermino[k] = 0;	//lo iniciamos en 0

		int sumacolumna =0;	//variable para guarda la suma de la columna		
		for(k = 0; k < ConsultaRealizada.Terminos.Count; k++ ){//para cada termino
			sumacolumna=0;
			for(j = 0; j < vectoresdocumentos.GetLength(0); j++ ){//sumamos los valor de la columna
				sumacolumna = sumacolumna + vectoresdocumentos[j,k];
			}
			if (sumacolumna==0) //si la suma es 0, lo marcamos para paso 1
				estadotermino[k] = 1;
		}

		//paso 1
		for ( int i = 0; i < estadotermino.Length; i++){//para cada termino
			if (estadotermino[i] == 1) {//si esta marcado como paso 1
				palabra = BuscarSugerencia(fraseconsulta[i], ListadoPalabrasDocumentos);//buscamos sugerencia de la palabra
				if (palabra.Length>0){//si hay sugerencia
					sumacolumna =0;//inicializamos la suma
					j = 0;
					foreach(var doc in DocumentosConsulta ){//para cada documento
						vectoresdocumentos[j,i] = doc.CuentaTermino(palabra);//ponemos las veces que aparece 
						sumacolumna = sumacolumna +vectoresdocumentos[j,i];//calculamos la suma de la columna
						j++;
					}
					if (sumacolumna == 0)//si la suma de la colunma sigue siendo 0
						estadotermino[i] = 2;//lo marcamos como paso 2
					else if (SugerenciaBusqueda.Length == 0)
					{ //guardamos la sugerencia para ser mostrada con el resultado
						SugerenciaBusqueda = palabra;
						fraseconsulta[i] = palabra;//sustituimos en la consulta , la palabra mal escrita por la sugerencia
					}
					else
						SugerenciaBusqueda = SugerenciaBusqueda + ", " + palabra;//si hay mas de una sugerencia las concatenamos
				}
				else
					estadotermino[i] = 2;// si no hay sugerencia lo marcamos para el paso 2	
			}
				
		}

		//paso 2
		
		for ( int i = 0; i < estadotermino.Length; i++){//para cada termino
			if (estadotermino[i] == 2) {//si esta marcado como paso 2
				lema = Stemming.Stemmear(fraseconsulta[i]); //sacamos la raiz de la palabra
				if (lema.Length>0){//si hay raiz
					sumacolumna =0;//inicializamos la suma
					j = 0;
					foreach(var doc in DocumentosConsulta ){//para cada documento
						vectoresdocumentos[j,i] = (doc.CuentaLema(lema)*2);//ponemos las veces que aparece multiplicada por dos para que el peso sea menor que el de la palabra
						sumacolumna = sumacolumna +vectoresdocumentos[j,i];//calculamos la suma de la columna
						j++;
					}

					if (sumacolumna == 0)   //si la suma de la colunma sigue siendo 0
						estadotermino[i] = 3;//lo marcamos como paso 3
					else if (SugerenciaBusqueda.Length == 0)
					{ //guardamos la sugerencia para ser mostrada con el resultado
						SugerenciaBusqueda = BuscarSugerencia(lema, ListadoPalabrasDocumentos);
						fraseconsulta[i] = palabra;//sustituimos en la consulta , la palabra mal escrita por la sugerencia
					}
					else
						SugerenciaBusqueda = SugerenciaBusqueda + ", " + palabra;//si hay mas de una sugerencia las concatenamos
				}
				else
					estadotermino[i] = 3;//lo marcamos como paso 3
			}
				
		}

		//paso 3=No queda otra alternativa que remover las columnas en 0 para proceder con el calculo

		List<int> columnasaborrar = new List<int>();//inicializamos la lista de las columnas que vamos a borrar

		for ( int i = 0; i < estadotermino.Length; i++)//para cada termino
			if(estadotermino[i] == 3)//si esta marcado como paso 4
				columnasaborrar.Add(i);//adicionamos la columna a borrar
		
		if (columnasaborrar.Count > 0){//si hay columnas a borrar
			vectoresdocumentos = Matrix.RemoveAllColumnas( vectoresdocumentos, columnasaborrar);
			vectorconsulta = Matrix.EliminaCeldas( vectorconsulta, columnasaborrar);
		}

		double[] pesos = CalcularPesosDocumentos(vectorconsulta, vectoresdocumentos);//calculamos los pesos de los documentos

		SearchItem[] items = new SearchItem[DocumentosConsulta.Count];//Resultados 

		for ( int i = 0; i < DocumentosConsulta.Count; i++){//para cada documento
			string oracionconsulta = DocumentosConsulta[i].ExtraerOracion(fraseconsulta);//buscamos la oración donde aparezcan los terminos
			//marcamos las palabras en las oraciones
			foreach(string palabraamarcar in fraseconsulta)
				oracionconsulta = oracionconsulta.Replace(palabraamarcar, "«"+palabraamarcar +"»");//lo marcamos

			items[i] = new SearchItem(DocumentosConsulta[i].Nombre.Substring(DocumentosConsulta[i].Nombre.LastIndexOf("\\")+1), oracionconsulta, (float) pesos[i]);//damos valores al item del resultado
		}
		resultado = items.OrderByDescending(x => x.Score).ToArray();//organizamos 
		return resultado;//retornamos los resultados
		
	}
	//Calcula el peso de los documentos según modelo vectorial
	//Entrada: arreglo con la frecuencia de los terminos de la consulta y matriz con la frecuencia de los terminos de los documentos
	private double[] CalcularPesosDocumentos(int[] arrConsultaTFVector, int[,] arrDocumentoTFVector)

	{
		double[,] arrDocumentosTFVector = new double[arrDocumentoTFVector.GetLength(0), arrDocumentoTFVector.GetLength(1)];
			for (int i = 0; i < arrDocumentoTFVector.GetLength(0); i++)
        {
			for (int j = 0; j < arrDocumentoTFVector.GetLength(1); j++)
            {
				arrDocumentosTFVector[i, j] = arrDocumentoTFVector[i, j];
            }
        }
		double[] arrConsultaTFIDF = new double[arrConsultaTFVector.Length];//arreglo que va a contener el TF-IDF de la consulta
		double[] arrDocumentosIDFVector;//arreglo para el vector IDF de los documentos
		double[,] arrDocumentosTFIDFVector; ;//arreglo para la matriz TF-IDF 
		double[] arrSIMVector; ;//arreglo de los pesos de los documentos(vector similitud)
		int CantidadTerminos = arrConsultaTFVector.Length;//cantidad de terminos en la consulta
		int CantidadDocumentos = arrDocumentosTFVector.GetLength(0);//cantidad de documentos

		// inicializar arreglos
		arrSIMVector = new double[CantidadDocumentos];
		arrDocumentosIDFVector = new double[CantidadTerminos];
		arrDocumentosTFIDFVector = new double[CantidadDocumentos, CantidadTerminos];

		//calculo del vector IDF de los documentos
		for (int i = 0; i < CantidadTerminos; i++)
		{//para cada uno de los terminos de la consulta
			int frecuencia = 0;//inicializamos la frecuencia
			for (int j = 0; j < CantidadDocumentos; j++)
			{//para cada uno de los documentos
				if (arrDocumentosTFVector[j, i] > 0)//si el termino aparece en el documento
					frecuencia++;//icrementamos la frecuencia

			}
			arrDocumentosIDFVector[i] = Math.Log10(((double)(CantidadDocumentos + 1) / (double)frecuencia)+1.1); //calculamos el valor de IDF del termino en el documento
			arrConsultaTFIDF[i] = (double)arrConsultaTFVector[i] * (double)arrDocumentosIDFVector[i];
		}
		//Calculo de la matriz TF-IDF
		for (int i = 0; i < CantidadDocumentos; i++)
		{//para cada documento
			for (int j = 0; j < CantidadTerminos; j++)
			{ //para cada uno de los terminos
				arrDocumentosTFIDFVector[i, j] = (double)(arrDocumentosTFVector[i, j] * arrDocumentosIDFVector[j]);//calculamos el valor de TF-IDF
			}

		}
		//calculo de la similitud de los documentos con la consulta que son los pesos de cada documento
		double numerator;//valores temporales
		double denominator;
		double querydenominator;
		for (int i = 0; i < CantidadDocumentos; i++)
		{//para cada documento
			numerator = 0;//inicializamos a 0 los temporales
			denominator = 0;
			querydenominator = 0;
			for (int j = 0; j < CantidadTerminos; j++)
			{//para cada uno de los terminos
				numerator = numerator + ((double)arrDocumentosTFIDFVector[i, j] * (double)arrConsultaTFIDF[j]);//sumamos valores
				denominator = denominator + Math.Pow((double)arrDocumentosTFIDFVector[i, j], 2);
				querydenominator = querydenominator + Math.Pow((double)arrConsultaTFIDF[j], 2);
			}
			arrSIMVector[i] = numerator / (Math.Sqrt((double)denominator) * Math.Sqrt((double)querydenominator));//calculamos el peso del documento
		}


		return arrSIMVector;//devolvemos los pesos o score de los documentos
	}
}

