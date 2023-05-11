namespace MoogleEngine;

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

//Clase que se utiliza para buscar los sinónimos de una palabra
//el archivo sinónimos.txt continene en cada una de sus líneas la lista de palabras separadas por comas que son sinónomos entre sí
public class Sinonimos
{
	private string[] ListaSinonimos = {""};//listado de los sinónimos
	
		
	//Constructor de la clase
	//Entrada: Camino en disco del documento de los sinónimos
	public Sinonimos(string camino)
	{
		CargarArchivo(camino);//se cargan los sinónimos del archivo
	}
 	
	//Carga a memoria el contenido del documento donde están los sinónimos
	//Entrada: Camino en disco del documento
	//Salida: verdadero si lo cargó, falso en caso contrario
	private bool CargarArchivo(string camino)
	{	bool resultado = false;//por defecto el resultado es falso
	 	
	 	try
        {
	 		if (File.Exists(camino))//si existe el archivo
        	{
	 			ListaSinonimos = File.ReadAllLines(camino);//lee el archivo a la memoria
	 			resultado = true;//resultado verdadero
			}
	 					
		}
        catch (Exception e)
        {
            Console.WriteLine("Error: {0}", e.ToString());
        }
        finally {}
	 	return resultado;//devuelve el contenido del documento
	}
	
	//Busca los sinónimos de una palabra
	//Entrada: palabra a la que se le quiere buscar sus sinónimos
	//Salida: lista con los sinónomos de la palabra
	public List<string> BuscarSinonimos(string palabra)
	{
		
		string[] palabraabuscar = { palabra };//se crea un arreglo que solo contiene la palabra para poder utilizarlo con el método
		
		//se consulta en las lista de sinónimos aquellas en la que aparece la palabra a buscar
		//en resultadoconsulta se retornan las líneas donde aparece la palabra
        string[] resultadoconsulta = Herramientas.BuscarMayorCantidadPalabrasenTexto(palabraabuscar, ListaSinonimos);
				
		List<string> resultado = new List<string>();//se crea lista para poner los sinónimos de la palabra

        foreach (string str in resultadoconsulta) //para cada una de las cadenas 
        {  
			string[] temp = str.Split(',');//se separan las palabras
			for(int i = 0; i < temp.Count(); i++)//para cada una de las palabras
				if ((temp[i]!=palabra) && (!resultado.Contains(temp[i])))//si la palabra seleccionada es diferente a la palabra que se busca y no aparece todavía en la lista
            		resultado.Add(temp[i]); //se adiciona a la lista
        }  
		
		return resultado;//se devuelve la lista de los sinónimos
	}
}
