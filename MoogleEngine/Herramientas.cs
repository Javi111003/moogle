namespace MoogleEngine;

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

//Clase Útiles
//Definición de propiedades y metodos que son utilizados en todo el programa
public static class Herramientas
{
	//Tipo de datos de pares de palabras
	public struct ParesPalabras
	{	//Constructor
    	public ParesPalabras(string x, string y)
    	{
        	X = x;//inicializar X
        	Y = y;//inicializar Y
    	}

    	public string X { get; }//devuelve valor de X
    	public string Y { get; }//devuelve valor de Y
	}    
    public static string CaminoDocumentos = ""; //Camino donde se encuentran los archivos de los documentos
	//Lista de caracteres separadores de palabras
    public static List<char> Separadores = new List<char>() { ' ', '.', ',', ';', ':', '¡', '!', '¿', '?', '(', ')', '{', '}', '[', ']', '\\', '\"', '\'', '«', '»', '#', '$', '%', '&', '*', '+', '-', '/', '<', '=', '—', '–','>', '÷', '@', '^', '_', '`', '|', '~', '¦' };
    //Lista de vocales acentuadas
	public static List<char> VocalesAcentuadas = new List<char>(){'ó', 'í', 'á', 'é', 'ú', 'ü', 'Ó', 'Í', 'Á', 'É', 'Ú', 'Ü', 'à', 'è', 'ì', 'ò', 'ù', 'À', 'È', 'Ì', 'Ò', 'Ù', 'â',
                                                                  'ê', 'î', 'ô', 'û', 'Â', 'Ê', 'Î', 'Ô', 'Û', 'ä', 'ë', 'ï', 'ö', 'Ä', 'Ë', 'Ï', 'Ö', 'å', 'Å', 'ã', 'Ã', 'õ', 'Õ'};
	//Lista de vocales equivalentes a las acentuadas sin acentos
    public static char[] VocalesSinAcentos ={'o', 'i', 'a', 'e', 'u', 'u', 'O', 'I', 'A', 'E', 'U', 'U', 'a', 'e', 'i', 'o', 'u', 'A', 'E', 'I', 'O', 'U', 'a',
                                            'e', 'i', 'o', 'u', 'A', 'E', 'I', 'O', 'U', 'a', 'e', 'i', 'o', 'A', 'E', 'I', 'O', 'a', 'A', 'a', 'A', 'o', 'O'};
	
	//Lista de palabras vacías
    public static List<string> PalabrasVacias = new List<string>();
	//Lista de artículos del español
    public  static List<string> Articulos = new List<string>() {"el", "la", "los", "las", "un", "una", "unos", "unas"};
    //Lista de preposiciones del español
	public static List<string> Preposiciones= new List<string>() {"a", "ante", "bajo", "cabe", "con", "contra", "de", "desde", "en", "entre", "hacia", "hasta", "para", "por", "según", "sin", "so", "sobre", "tras", "versus", "cabe", "so"};
    //Lista de conjunciones del español
	public static List<string> Conjunciones= new List<string>() {"a", "así", "aún ", "aunque", "bien", "como", "con", "cuando", "de", "e", "en", "lo", "luego", "mas", "ni", "no", "o", "para", "pero", "por", "porque", "pues", "puesto", "que", "si", "sin", "sino", "tal", "tan", "tanto", "u", "y"};
    //Lista de pronombres del español
	public static List<string> Pronombres= new List<string>() {"yo", "me", "mí", "conmigo", "nosotros", "nos", "nosotras", "tú", "te", "ti", "contigo", "usted", "vos", "vosotros", "vosotras", "os", "ustedes", "él", "lo", "le", "se", "sí", "consigo", "ella", "la", "ello", "ellos", "ellas", "los", "las", "les", "se", "sí", "consigo","mi"};
    //Lista de adverbios del español
    public static List<string> Adverbios= new List<string>() {"aquí", "allí", "ahí", "allá", "acá", "arriba", "abajo", "cerca", "lejos", "delante", "detrás", "encima", "debajo", "enfrente", "atrás", "alrededor" ,"antes", "después", "luego", "pronto", "tarde", "temprano", "todavía", "aún", "ya", "ayer", "hoy", "mañana", "siempre", "nunca", "jamás", 
                                                              "anoche", "enseguida", "ahora", "mientras", "bien", "mal", "regular", "despacio", "deprisa", "así", "tal", "como", "aprisa", "adrede", "peor", "mejor", "muy", "poco", "mucho", "bastante", "más", "menos", "algo", "demasiado", 
                                                              "casi", "solo", "tan", "tanto", "todo", "nada", "sí", "también", "cierto", "claro", "exacto", "obvio", "asimismo", "no", "jamás", "nunca", "tampoco", "quizás", "acaso", "cuándo", "cómo", "cuánto", "dónde", 
                                                              "cuando", "como", "cuanto", "donde", "aún", "inclusive", "además", "incluso", "viceversa", "siquiera",  "próximamente", "prontamente", "anteriormente", "fielmente", "estupendamente", "fácilmente", "negativamente", "responsablemente", 
                                                              "solamente", "aproximadamente", "ciertamente", "efectivamente", "verdaderamente", "seguramente", " primeramente", "últimamente", "probablemente", "posiblemente", "solamente", "únicamente", "mismamente", "propiamente", "precisamente", "concretamente", "contrariamente", "consecuentemente"};
    //Lista de vocales
	public static List<char> Vocales = new List<char>() { 'a', 'e', 'i', 'o', 'u' };
    //Lista de operadores de búsqueda que aparecen en las consultas
	public static List<char> OperadoresBusqueda = new List<char>() { '!', '^', '~', '*' };
    //Lista de letras
	public static List<char> Letras = new List<char>() { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'À', 'Á', 'Â', 'Ã', 'Ä', 'Å', 'È', 'É', 'Ê', 'Ë', 'Ì', 'Í', 'Î', 'Ï', 'Ð', 'Ñ', 'Ò', 'Ó', 'Ô', 'Õ', 'Ö', 'Ø', 'Ù', 'Ú', 'Û', 'Ü', 'Ý', 'Þ', 'ß', 'à', 'á', 'â', 'ã', 'ä', 'å', 'è', 'é', 'ê', 'ë', 'ì', 'í', 'î', 'ï', 'ð', 'ñ', 'ò', 'ó', 'ô', 'õ', 'ö', 'ø', 'ù', 'ú', 'û', 'ü', 'ý', 'þ', 'ÿ', 'Ā' };


	//Eliminar los acentos

	public static char EliminaAcento(char caracter)
    {
        int pos = VocalesAcentuadas.IndexOf(caracter); //indice en las vocales acentuadas

        return pos >= 0 ? VocalesSinAcentos[pos] : caracter; //Si la vocal esta acentuada, la devuelve sin acento

    }
	//Entrada: palabra a normalizar
	//Salida: palabra normalizada
	public static string NormalizarPalabra(string palabra)
    {
        string resultado = string.Empty;

        foreach (char c in palabra)
            resultado = resultado + EliminaAcento(c); //Cada caracter de la palabra se le elimina el acento si lo tiene y se lleva a minúscula

        return resultado;
    }

	//Saber si un caracter es separador de palabras

    public static bool EsSeparadorPalabras(char caracter)
    {
        return Separadores.IndexOf(caracter) >= 0; //Si aparece en los separadores de palabra, devuelve verdadero, en caso contrario falso
    }

	//Saber si una palabra esta en las palabras vacías
    public static bool EsPalabraVacia(string palabra)
    {
        return PalabrasVacias.IndexOf(palabra) >= 0;//Si aparece en las palabras vacías, devuelve verdadero, en caso contrario falso
    }
    public static bool EsCaracterControl(char c)
    {
        return c >= 0 && c <= 31 || c >= 127 && c <= 160 || c >= 162 && c <= 190 ? true : false;
    }
	
	//Separa las palabras que contiene la cadena texto devolviendo una lista de palabras

    public static List<string> SeparaPalabras(string texto)
    {
        return SeparaPalabrasdeignorados(texto, new List<char>()); //devuelve el resultado de llamar a SeparaPalabrasdeignorados para que ignoro solo los vacios
    }

	//Separa las palabras  que contiene el texto de los ignorados almacenandolas en una lista de palabras.
	public static List<string> SeparaPalabrasdeignorados(string texto, List<char> ignorados)
    {
        List<string> palabras = new List<string>(); //lista temporal para poner las palabras

        string temp = string.Empty; //variable temporal
        
		foreach (char c in texto) //para cada caracter 
        {
            if (ignorados != null && ignorados.IndexOf(c) >= 0) //Si es ignorado se agrega
                temp = temp + c;
            else if (!EsSeparadorPalabras(c) && !EsCaracterControl(c)) //Si no es separador de palabras o caracter de control se agrega poniendola en minúscula
            {
                temp = temp + c;
            }
            else if (temp.Length !=0) //entra aquí si el caracter es separador de palabras o caracter de control, es el fin de la palabra
            {
                palabras.Add(temp); //adicionar la palabra
                temp = string.Empty; 
            }
        }
        if (temp.Length != 0) //Si al final queda alguna palabra en temp, se adiciona también
            palabras.Add(temp);

        return palabras; 
    }
	
	//Buscar los nombres de los archivos *.txt que se encuentran en el camino dado

    public static List<string> ListaArchivos(string camino)
    {
        return Directory.EnumerateFiles(camino, "*.txt", SearchOption.AllDirectories).ToList();
    }
	
	//Elimina un elemento de un arreglo
	//Entrada: arreglo del que se quiere eliminar el elemento y posicion del elemento
	//Salida: nuevo arreglo sin el elemento
    public static int[] EliminaElementoArreglo( int[] arreglo, int posicion)
	{	int[] resultado = arreglo;//por defecto se devuelve el mismo arreglo
	 
	 	if((arreglo.Length > 0) &&(posicion>0)&&(posicion<arreglo.Length)) { //Si cumple las condiciones
			int[] temporal = new int[arreglo.Length-1]; //Se crea arreglo temporal con dimension menor en uno al de entrada
			int pos = 0;//auxiliar para recorrer los elementos que se va a copiar
			
			for (int i = 0; i < arreglo.Length;i++) //Se copia de un arreglo al otro ingnorando el valor de posición
				if (i!= posicion){//si no se la posición que se va a borrar
					temporal[pos] = arreglo[i];//se copia el elemento
					pos++;//se incrementa la posición
				}
			resultado = temporal;//se asigna a resultado el arreglo sin el elemento		
			
		}
	 	return resultado;
	 
	}
	


	//Busca las cadenas de texto donde aparezcan la mayor cantidad de palabras 
	//Entrada: arreglo de palabras a buscar y texto donde se va a buscar
	//Salida: arreglo de los textos donde aparecen la mayor cantidad palabras
	public static string[] BuscarMayorCantidadPalabrasenTexto(string[] palabras, string[] texto)
	{	string[] resultado ={};//se crea variable del resultado
		int[] cantidadpalbaras = new int[texto.Length];//se crea un arreglo de enteros para guardar la cantidad de palabras que hay en cada texto de las palabras buscadas
	 	int maximo = 0;//se utiliza para guardar la máxima cantidad de palabra encontradas en un texto
	 	if(palabras.Length>0 && texto.Length>0) {//si se cumple la condicion
			for(int i = 0; i < texto.Length; i++){//para cada elemento de texto
				cantidadpalbaras[i]=0;//se inicializa a 0 la cantidad de palabras encontradas
				for(int j = 0; j < palabras.Length; j++) {//para cada una de las palabras a buscar
					if (texto[i].Contains(palabras[j]))//si el texto las contiene
						cantidadpalbaras[i]++;//se incrementa 
						if (maximo < cantidadpalbaras[i] )//si el máximo es menor a la cantidad de palabras que contiene
							maximo = cantidadpalbaras[i];//se le asigna nuevo valor máximo

				}
			}	
		}
	 
	 	List<string> listatextos = new List<string>();//como no sabemos cuantos texto con el máximo de palabras hay, utilizamos una lista para almacenarlos
	 	
	 	for(int i = 0; i < texto.Length; i++)//para cada uno de los elementos del texto	
			if (cantidadpalbaras[i] == maximo)//si contiene la cantidad maxima de palabras
				listatextos.Add(texto[i]);//se copia a la lista	
	 	if(listatextos.Count()>0)//si se encontraron elementos
	 		resultado = listatextos.ToArray();//convierto la lista en arreglo y la guardo en resultado
	 	return resultado;//devolvemos el resultado
	}
	//Calcula la distancia entre dos palabras según el algoritmo de Damerau-Levenshtein
	//es utilizado para buscar la sugerencia a una palabra que no aparezca por estar mal escrita en la consulta
	//codigo tomado de https://www.csharpstar.com/csharp-string-distance-algorithm/
	//Entrada:palabras que se quieren comparar
	//Salida:distancia entre las dos palabras
	public static int DistanciaDamerauLevenshtein(string s, string t)
	{
		var limites = new { alto = s.Length + 1, ancho = t.Length + 1 };//limites de la matriz que se va a crear

		int[,] matriz = new int[limites.alto, limites.ancho];//matriz para realizar los calculos

		//inicialización por defecto de la matriz
		for (int alto = 0; alto < limites.alto; alto++)
			matriz[alto, 0] = alto;
		for (int ancho = 0; ancho < limites.ancho; ancho++)
			matriz[0, ancho] = ancho;

		for (int alto = 1; alto < limites.alto; alto++)//para cada una de las filas de la matriz
		{
			for (int ancho = 1; ancho < limites.ancho; ancho++)//para cada una de las colunmas de la matriz
			{
				int costo = (s[alto - 1] == t[ancho - 1]) ? 0 : 1;//si coinciden las letras el costo es 0, si no 1
				int insercion = matriz[alto, ancho - 1] + 1;//calculala insercion de una letra
				int borrado = matriz[alto - 1, ancho] + 1;//calcula el borrado de una letra
				int sustitucion = matriz[alto - 1, ancho - 1] + costo;//calcula la sustitucion de una letra

				int distancia = Math.Min(insercion, Math.Min(borrado, sustitucion));//la distancia es el mínimo de los tres valores

				if (alto > 1 && ancho > 1 && s[alto - 1] == t[ancho - 2] && s[alto - 2] == t[ancho - 1])
					distancia = Math.Min(distancia, matriz[alto - 2, ancho - 2] + costo);

				matriz[alto, ancho] = distancia;//se almacena la distancia
			}
		}

		return matriz[limites.alto - 1, limites.ancho - 1];//se devuelve la distancia
	}
	/*public static bool ExisteTermino(string termino)
	{
		return Co;//Si la cantidad de veces que aparece el termino es mayor que 0 devuelve verdadero, falso en caso contrario 
	}*/
}