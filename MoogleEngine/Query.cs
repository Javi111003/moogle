namespace MoogleEngine;

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

//Clase Consulta
//Esta clase es la encargada de gestionar todo lo relativo a las consultas
public class Query
{	private string Texto; //Texto original de la consulta
 	private Dictionary<string,int> lTerminos = new Dictionary<string,int>(); //Diccionario donde se almacenan los terminos que aparecen en la consulta y la cantidad de veces que aparece cada uno
 	private List<string> lTerminosObligatorios = new List<string>(); //Lista de terminos obligatorios
 	private List<string> lTerminosProhibidos = new List<string>();//Lista de terminos prohibidos
 	private Dictionary<string,int> lTerminosImportantes = new Dictionary<string,int>();//Diccionario donde se almacenan los terminos importantes que aparecen en la consulta y la cantidad de veces que apartece cada uno
	private List<Herramientas.ParesPalabras> lTerminosCercanos = new List<Herramientas.ParesPalabras>();//Lista de pares de terminos que se le calcula la distancia entre ellos
 
 	//Constructor de la clase
 	//Entrada: cadena que contiene el texto de la consulta
 	public Query(string texto)
	{
		this.Texto = texto;//guardamos el texto
		Procesar();//procesamos la consulta	
	}
 
	//Devuelve el texto de la consulta
 	public string Contenido
	{
		get{return Texto;}
	}
 	//Devuelve el diccionario donde se almacenan los terminos que aparecen en la consulta y la cantidad de veces que apartece cada uno
 	public Dictionary<string,int> Terminos
	{
		get{return this.lTerminos;}
	}
 	//Devuelve la lista de terminos obligatorios
 	public List<string> TerminosObligatorios
	{
		get{return this.lTerminosObligatorios;}
	}
	//Devuelve la lista de terminos prohibidos
 	public List<string> TerminosProhibidos
	{
		get{return this.lTerminosProhibidos;}
	}
 	//Devuelve el diccionario donde se almacenan los terminos importantes que aparecen en la consulta y la cantidad de veces que apartece cada uno
 	public Dictionary<string,int> TerminosImportantes
	{
		get{return this.lTerminosImportantes;}
	}
 	//Devuelve la lista de pares de terminos que se le calcula la distancia entre ellos
 	public List<Herramientas.ParesPalabras> TerminosCercanos
	{
		get{return this.lTerminosCercanos;}
	}
 	
 	//Método que realiza el procesamiento del texto de la consulta
	private void Procesar()

	{
		this.Texto = this.Texto.ToLower();
		List<string> palabras = new List<string>();//Lista temporal de palabras
		string temp;//valor temporal
		string ultimotermino = String.Empty;//ultimo termino procesado
	 	bool distanciaentrepalabras = false;//si se esta procesando el pedido de la disnacia entre palabras toma valor verdadero, en caso contrario falso
	 
	 	palabras = Herramientas.SeparaPalabrasdeignorados(this.Texto, Herramientas.OperadoresBusqueda); //Se crea una lista de las palabra que contiene la consulta respetando los operadores de busqueda
	 	foreach( string palabra in palabras ){//para cada palabra de la lista
			switch (palabra[0]){//se separan los casos por el primer caracter de la palabra
        		case '!'://caso de termino prohibido
					temp = palabra.Substring(1,palabra.Length-1);//se extrae la palabra
            		if (!Herramientas.EsPalabraVacia(temp)){//Si no es palabra vacía
						temp = Herramientas.NormalizarPalabra(temp);//Se normaliza la palabra
						if(!(lTerminosProhibidos.IndexOf(temp) >= 0))//Si no esta ya en los terminos prohibidos
							lTerminosProhibidos.Add(temp);//se agrega  a la lista de terminos prohibidos
						ultimotermino=temp;	//se guarda el último termino procesado para su uso posterior	
					}
            	break;

        		case '^'://caso de termino obligatorio
            		temp = palabra.Substring(1,palabra.Length-1);//se extrae la palabra
            		if (!Herramientas.EsPalabraVacia(temp)){//Si no es palabra vacía
						temp = Herramientas.NormalizarPalabra(temp);//Se normaliza la palabra
						if(!(lTerminosObligatorios.IndexOf(temp) >= 0))//Si no esta ya en los terminos obligatorios
							lTerminosObligatorios.Add(temp);//se agrega  a la lista de terminos obligatorios
						if (lTerminos.ContainsKey(temp))//Si esta en los terminos
                			lTerminos[temp]++;//Se incrementa la cantidad de apariciones
						else //Si no esta en los terminos
							lTerminos.Add(temp, 1); //se agrega  a los de terminos
						ultimotermino=temp;//se guarda el último termino procesado para su uso posterior	
					}
           		break;

        		case '~'://caso de distancia entre palabras
            		distanciaentrepalabras = true;//toma el valor verdadero porque estamos procesando una par de palabras donde se pide la distancia
            	break;
				
				case '*': //caso de termino importante
					int contador = 1;//contador de los * que aparecen
					do {
						temp = palabra.Substring(contador,palabra.Length-contador);//al finalizar el ciclo, temp contiene el termino
						contador++;
					} while (temp[0]== '*');
					          		
            		if (!Herramientas.EsPalabraVacia(temp)){//Si no es palabra vacía
						temp = Herramientas.NormalizarPalabra(temp);//Se normaliza la palabra
						if(lTerminosImportantes.ContainsKey(temp))//Si esta en los terminos imortantes
							lTerminosImportantes[temp] = lTerminosImportantes[temp]+contador;//Se incrementa la cantidad de apariciones, contador tiene la cantidad de * que aparecen antes del termino
						else//Si no esta en los terminos importantes
							lTerminosImportantes.Add(temp, contador); //se agrega  a los terminos importantes, contador tiene la cantidad de * que aparecen antes del termino
						if (lTerminos.ContainsKey(temp))//Si esta en los terminos
                			lTerminos[temp]=lTerminos[temp]+contador;//Se incrementa la cantidad de apariciones, contador tiene la cantidad de * que aparecen antes del termino
						else //Si no esta en los terminos
							lTerminos.Add(temp, contador); //se agrega  a los de terminos, contador tiene la cantidad de * que aparecen antes del termino
						ultimotermino=temp;//se guarda el último termino procesado para su uso posterior
					
					}
            	break;

        		default://procesamiento de los otros terminos que no son obligatorios, importantes o prohibidos
					
            		if (!Herramientas.EsPalabraVacia(palabra)) {//Si no es palabra vacía
						temp = Herramientas.NormalizarPalabra(palabra);//Se normaliza la palabra
						if (distanciaentrepalabras) {//si estamos procesando un par de palabras que se pide la distancia
							lTerminosCercanos.Add(new Herramientas.ParesPalabras(ultimotermino,temp));//se agrega  a los terminos cercanos el par de palabras ultimotermino y el termino actual
							distanciaentrepalabras = false;//toma el valor falso porque terminamos de procesar un par de palabras donde se pide la distancia
						}
						if(!(lTerminos.ContainsKey(temp)))//Si no esta en los terminos
							lTerminos.Add(temp, 1); //se agrega  a los de terminos
						else//Si esta en los terminos
							lTerminos[temp] = lTerminos[temp]+1;//Se incrementa la cantidad de apariciones
						ultimotermino=temp;//se guarda el último termino procesado para su uso posterior
						
					}
            	break;
    		}
			
		}

	}
 	
 	
}

