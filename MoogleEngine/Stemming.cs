namespace MoogleEngine;

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SharpNL.Stemmer.Snowball;


//Implemento el stemmer del español perteneciente al nuget Knuppe.MA.SharpNL una reimplementacion de OpenNLP
//Funcion:Extraer la raiz de las palabras , implementadp en la busqueda de sugerencia e influyente en una parte del calculo de los pesos segun la query
    public static class Stemming
    {

        //Extrae la raíz de la palabra 
        //Entrada: palabra de la que se quiere obtener la raíz
        //Salida: raíz de la palabra
        public static string Stemmear(string palabra)
        {
            SpanishStemmer stemer = SpanishStemmer.Instance; //instancia del stemmer

            string raiz = stemer.Stem(palabra);
            return raiz;
        }


    }
