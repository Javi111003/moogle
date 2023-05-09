namespace MoogleEngine;

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

//Clase Documento
//Esta clase es la encargada de gestionar todo lo relativo a los documentos sobre los que se realizaran las consultas
public class Documento
{	private string Ruta;//Camino en disco del documento
	private string Texto;//Contenido completo del texto del documento
 	private List<string> lTerminos = new List<string>();//Lista de terminos que componen el documento.
	private List<string> lexemas = new List<string>();//Lista de lemas de los terminos que componen el documento.
 	
 	//Constructor de la clase(solo necesita la direccion de los archivos)
 	public Documento(string camino)
	{   this.Ruta = string.Empty; //Se inicializa a vacio el camino del archivo
        this.Texto = String.Empty; //Se inicializa a vacio el texto

		if(camino.Length > 0){//Si camino no es vacio
			this.Ruta = camino; //Se guarda el camino del archivo
       		CargarArchivo();//Se carga el archivo del disco
			Procesar();	//Se procesa el archivo
		}
	}
 
 	//Para su uso posterior(Extraer el nombre del txt)
 	public string Nombre
	{
		get{return Ruta;}
	}
 
 	//Devuelve contenido del documento
 	public string Contenido
	{
		get{return Texto;}
	}
 
 
 	public List<string> Terminos
	{
		get{return this.lTerminos;}
	}
 
 	//Devuelve una lista de las posiciones donde aparece un termino en el documento(uso para la distancia)

 	public List<int> PosicionesTermino(string termino)
	{	List<int> resultado = new List<int>();//Se inicializa la lista vacía
	 	
	 	for (int i=0; i<lTerminos.Count;i++)//Se recorren todos los terminos
			if( lTerminos[i] == termino)//Si coincide con el termino del documento
				resultado.Add(i);//Se adiciona a la lista la posición del termino
			
	 	return resultado;
	}
 
 	//(Devuelve la frecuencia de un termino en el documento que se analiza)
 	public int CuentaTermino(string termino)
 	{	
		return PosicionesTermino(termino).Count;//devolver la cantidad de posiciones en la que aparece el termino
 	}
 
 	//Devuelve si existe un termino en el documento
 
	public bool ExisteTermino(string termino)
 	{		 
	 	return (CuentaTermino(termino)>0);//Si la cantidad de veces que aparece el termino es mayor que 0 devuelve verdadero, falso en caso contrario 
 	}
 
 
	public bool ExistenTerminos(string[] terminos)
 	{	
		foreach ( string termino in terminos)//Para cada uno de los terminos 	
			if(CuentaTermino(termino)>0) //Si la cantidad de veces que aparece mayor que 0 devuelve verdadero
				return true;
	 	return false;//caso contrario 
 	}
 	
 	//BUsqueda de la distancia mínima entre dos terminos en el documento
 	public int DistanciaMinTerminos(string termino1, string termino2)
	{	int resultado = lTerminos.Count;//Por defecto la distancia mínima entre los dor terminos es la distancia maxima que puede haber, que es la cantidad de terminos del documento
		
		List<int> ltermino1 = PosicionesTermino(termino1);//lista de las posiciones del primer termino
		List<int> ltermino2 = PosicionesTermino(termino2);//lista de las posiciones del segundo termino
	 	if ((ltermino1.Count>0) && (ltermino2.Count>0)){//si los dos terminos existen, tienen como minimo una posición
			resultado = Math.Abs(ltermino1[0]-ltermino2[0]);//se inicializa la distancia minima como la distancia entre las primeras posiciones de los terminos
			for(int i=0; i<ltermino1.Count;i++)//se recorren todas las posiciones del primer termino
				for(int j=0; j<ltermino2.Count;j++)	//se recorren todas las posiciones del segundo termino
					if (Math.Abs(ltermino1[i]-ltermino2[j]) < resultado )//si la distancia es menor que teníamos
						resultado = Math.Abs(ltermino1[i]-ltermino2[j]);//guardamos la nueva distancia
		}
		return resultado;//Devuelve la distancia mínima entre dos terminos en el documento
	}
 	
 	//Palabras del documento , lematizadas 
 	public List<string> Lemas
	{
		get{return this.lexemas;}
	}
 
	//Devuelve una lista de las posiciones donde aparece un lema en el documento

 	public List<int> PosicionesLema(string lema)
	{	List<int> resultado = new List<int>();//Se inicializa la lista vacía
	 	
	 	for (int i=0; i<lexemas.Count;i++)//Se recorren todos los lemas
			if( lexemas[i] == lema)//Si coincide con el lema del documento
				resultado.Add(i);//Se adiciona a la lista la posición del lema
			
	 	return resultado;//Devuelve una lista de las posiciones donde aparece un lema en el documento
	}
 
 //FRecuencia de dicho lema en el Documento
 	public int CuentaLema(string lema)
 	{	
		return PosicionesLema(lema).Count;//devolver la cantidad de posiciones en la que aparece el lema
 	}
 
 	
 	//Extraer del texto del documento una oración con la mayor cantidad de palabras posibles

	public string ExtraerOracion(string[] palabras)
 	{	
	 	string[] oraciones = this.Texto.Split(new char[] { '.', '¿', '?', '!', '¡' });//dividimos el texto por oraciones, teniendo en cuenta los caracteres que delimitan las mismas
			
        string[] resultado = Herramientas.BuscarMayorCantidadPalabrasenTexto(palabras, oraciones);//buscamos las oraciones donde aparezcan la mayor cantidad de palabras

        return resultado[0];//devolvemos el arreglo de oraciones que contienen las palabras
 	}
 
 	//Realiza la carga del texto del documento en el disco
 	private bool CargarArchivo()
	{	bool resultado = false;//resultado falso por defecto, carga insatisfactoria
	 	
	 	try
        {
	 		if (File.Exists(this.Ruta))//si existe el documento
        	{
	 			this.Texto = File.ReadAllText(this.Ruta);//cargar el texto
				this.Texto = this.Texto.ToLower();//lo llevamos a minuscula
				
				resultado = true;//carga satisfactoria
			}
	 					
		}
        catch (Exception e)//capturamos la excepción
        {
            Console.WriteLine("Error: {0}", e.ToString());//mostramos el error
        }
        finally {}

	 	return resultado;//retormanos resultado de la carga
	}
 
 	//Procesar el documento
	private void Procesar()
	{   List<string> palabras = new List<string>();//lista temporal que va a contener las palabras del texto
		string temppalabra;//temporal para contener todas las palabras del texto
		palabras = Herramientas.SeparaPalabras(this.Texto);//crear lista con las palabras del texto
	 	foreach( string palabra in palabras ){//para cada una de las palabras
			if (!Herramientas.EsPalabraVacia(palabra)){//si no es palabra vacía
				temppalabra = Herramientas.NormalizarPalabra(palabra);//se normaliza la palabra
				lTerminos.Add(temppalabra);//se adiciona a los terminos
				lexemas.Add(Stemming.Stemmear(temppalabra));//se lematiza la palabras y se adiciona a los lemas
			}
		}

	} 	
}
