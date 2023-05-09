namespace MoogleEngine;

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


//Clase Lematizador
//Esta clase es la encargada de sacar las raices de las palabras para buscar los lemas
//Es una variación del algoritmo de Porter publicada en jesusutrera.com/articles/article01.html
//La misma se a modificado para adaptarla a nuestro programa
//R1 es la región tras la que la primera letra no vocal sigue a una vocal, o es la región vacía al final de la palabra si no hay vocales.
//R2 es la región tras la que la primera letra no vocal sigue a una vocal en R1, o es la región vacía al final de la palabra si no hay vocales.
//RV se define como sigue: Si la segunda letra es consonante, RV es la región después de la cual le sigue una vocal, o si las dos primeras letras son vocales, 
//RV es la región tras la primera consonante y, en otro caso, (caso consonante-vocal) RV es la región después de la tercera letra. Final de la palabra si estas posiciones no han podido ser encontradas.
//Los pasos del algoritmo serían
//Paso 0: eliminación de sufijos de pronombres adjuntos
//Paso 1: Eliminación de sufijos
//Realizar el paso 2 si no se eliminó ninguna terminación en el paso 1.
//Paso 3 eliminar sufijos residuales
public static class Stemming
    {
    public static List<string> Paso_0 = new List<string>() {//Sufijos a eliminar paso 0
            "me", "se", "sela", "selo", "selas", "selos", "la", "le", "lo", "las", "les", "los", "nos"
        };

    public static List<string> DespuesPaso_0 = new List<string>() {//Se eliminan si vienen despues de...
            "iéndo", "ándo", "ár", "ér", "ír", "ando", "iendo", "ar", "er", "ir", "yendo"
        };

    public static List<string> Paso1_1 = new List<string>() {//Sufijos a eliminar paso 1.1
            "anza", "anzas", "ico", "ica", "icos", "icas", "ismo", "ismos", "able", "ables", "ible", "ibles", "ista", "istas", "oso", "osa", "osos", "osas", "amiento", "amientos", "imiento", "imientos"
        };

    public static List<string> Paso1_2 = new List<string>() {//Sufijos a eliminar paso 1.2
            "adora", "ador", "ación", "adoras", "adores", "aciones", "ante", "antes", "ancia", "ancias"
        };

    public static List<string> Paso1_3 = new List<string>() {//Sufijos a eliminar paso 1.3
            "logía", "logías"
        };

    public static List<string> Paso1_4 = new List<string>() {//Sufijos a eliminar paso 1.4
            "ución", "uciones"
        };

    public static List<string> Paso1_5 = new List<string>() {//Sufijos a eliminar paso 1.5
            "encia", "encias"
        };

    public static List<string> Paso1_6 = new List<string>() {//Sufijos a eliminar paso 1.6
            "amente"
        };

    public static List<string> Paso1_7 = new List<string>() {//Sufijos a eliminar paso 1.7
            "mente"
        };

    public static List<string> Paso1_8 = new List<string>() {//Sufijos a eliminar paso 1.8
            "idad", "idades"
        };

    public static List<string> Paso1_9 = new List<string>() {//Sufijos a eliminar paso 1.9
            "iva", "ivo", "ivas", "ivos"
        };

    public static List<string> Paso2_a = new List<string>() {//Sufijos a eliminar paso 2.a
            "yeron", "yendo", "yamos", "yais", "yan", "yen", "yas", "yes", "ya", "ye", "yo", "yó"
        };

    public static List<string> Paso2_b1 = new List<string>() {//Sufijos a eliminar paso 2.b1
            "en", "es", "éis", "emos"
        };

    public static List<string> Paso2_b2 = new List<string>() {//Sufijos a eliminar paso 2.b2
            "arían", "arías", "arán", "arás", "aríais", "aría", "aréis", "aríamos", "aremos", "ará",
            "aré", "erían", "erías", "erán", "erás", "eríais", "ería", "eréis", "eríamos", "eremos",
            "erá", "eré", "irían", "irías", "irán", "irás", "iríais", "iría", "iréis", "iríamos",
            "iremos", "irá", "iré", "aba", "ada", "ida", "ía", "ara", "iera", "ad", "ed", "id", "ase",
            "iese", "aste", "iste", "an", "aban", "ían", "aran", "ieran", "asen", "iesen", "aron",
            "ieron", "ado", "ido", "ando", "iendo", "ió", "ar", "er", "ir", "as", "abas", "adas",
            "idas", "ías", "aras", "ieras", "ases", "ieses", "ís", "áis", "abais", "íais", "arais",
            "ierais", "aseis", "ieseis", "asteis", "isteis", "ados", "idos", "amos", "ábamos", "íamos",
            "imos", "áramos", "iéramos", "iésemos", "ásemos"
        };

    public static List<string> Paso3_1 = new List<string>() {//Sufijos a eliminar paso 3.1
            "os", "a", "o", "á", "í", "ó"
        };

    public static List<string> Paso3_2 = new List<string>() {//Sufijos a eliminar paso 3.2
            "e", "é"
        };
    
	//Extrae la raíz de la palabra creando el lema
	//Entrada: palabra de la que se quiere obtener la raíz
	//Salida: raíz de la palabra(lema)
	public static string Stemmear(string palabra)
        {
            string result = palabra;//resultado por defecto es la misma palabra si ella en si misma es la raiz 

           
                if (palabra.Length >= 3)//Si la palabra tiene mas de tres caracteres
                {
                    StringBuilder sb = new StringBuilder(palabra.ToLower());//Creo el StringBuilder de la palabra llevandola a minúscula

                    if (sb[0] == '\'') sb.Remove(0, 1);//si comienza con ' se elimina

                    int r1 = 0, r2 = 0, rv = 0;
                    computarR1R2RV(sb, ref r1, ref r2, ref rv);//Calcular R1, R2 y RV

                    Paso0(sb, rv);//realizar el paso 0
                    int cont = sb.Length;//guardar la longitud de la palabra
                    Paso1(sb, r1, r2);// realizar el paso 1

                    if (sb.Length == cont)//si no se elimino el sufijo de la palabra
                    {
                        Paso2a(sb, rv);//realizar el paso 2.a
                        if (sb.Length == cont)//si no se elimino el sufijo de la palabra
                        {
                            Paso2b(sb, rv);//realizar el paso 2.b
                        }
                    }
                    Paso3(sb, rv);//realizar el paso 3
                    EliminaAcentos(sb);//eliminar los acentos

                    result = sb.ToString().ToLower();//convertir  de StringBuilder a String llevandola a minúscula
                }
         

            return result;//retorna el resultado de la lematización
        }
		//Calcular R1, R2 y RV. (posiciones donde comienzan)
		//Entrada: la palabra. Se pasan por referencia la variable de R1, R2 y RV para en el mismo metodo obtener los tres valores
        public static void computarR1R2RV(StringBuilder sb, ref int r1, ref int r2, ref int rv)
        {
            r1 = sb.Length;//se inicializan los valores a la longitud de la cadena
            r2 = sb.Length;
            rv = sb.Length;

            //R1 región tras la que la primera letra no vocal sigue a una vocal, o es la región vacía al final de la palabra si no hay vocales.
            for (int i = 1; i < sb.Length; ++i)//se recorren todas las letras
            {
                if ((!EsVocal(sb[i])) && (EsVocal(sb[i - 1])))//si letra no vocal sigue a una vocal
                {
                    r1 = i + 1;//r1 comienza en la siguiente posición
                    break;
                }
            }

            //R2 región tras la que la primera letra no vocal sigue a una vocal en R1, o es la región vacía al final de la palabra si no hay vocales.
            for (int i = r1 + 1; i < sb.Length; ++i)//se recorren todas las letras a partir de R1+1
            {
                if ((!EsVocal(sb[i])) && (EsVocal(sb[i - 1])))//si letra no vocal sigue a una vocal
                {
                    r2 = i + 1;//r2 comienza en la siguiente posición
                    break;
                }
            }

            //RV región tras la primera consonante y, en otro caso, (caso consonante-vocal) RV es la región después de la tercera letra. Final de la palabra si estas posiciones no han podido ser encontradas.
            if (sb.Length >= 2)//si la palabra tiene mas de dos letras
            {
                if (!EsVocal(sb[1]))//si la 2da letra no es vocal
                {
                    for (int i = 1; i < sb.Length; ++i)//se recorren todas las letras
                    {
                        if (EsVocal(sb[i]))//si la letra es vocal
                        {
                            rv = sb.Length > i ? i + 1 : sb.Length;//si no estoy al final de la palabra, seria la posición siguiente, si no, seria la longitud de la palabra
                            break;
                        }
                    }
                }
                else//si la 2da letra es vocal
                {
                    if (EsVocal(sb[0]) && EsVocal(sb[1]))//si la 1ra y 2da letras son vocales
                    {
                        for (int i = 1; i < sb.Length; ++i)//se recorren todas las letras
                        {
                            if (!EsVocal(sb[i]))//si la letra no es vocal
                            {
                                rv = sb.Length > i ? i + 1 : sb.Length;//si no estoy al final de la palabra, seria la posición siguiente, si no, seria la longitud de la palabra
                                break;
                            }
                        }
                    }
                    else//si no es ninguno de los casos anteriores
                    {
                        rv = sb.Length >= 3 ? 3 : sb.Length;//si la palabra tiene 3 o mas caracteres, sería 3, si no, seria la longitud de la palabra
                    }
                }
            }
        }
		//Saber si un caracter es caracter es vocal
		//Entrada: caracter
		//Salida: verdadero si lo es, falso si no lo es
        public static bool EsVocal(char c)
        {
            return Herramientas.Vocales.IndexOf(c) >= 0;
        }
		//Realizar el paso 0
		//Entrada: palabra y valor de RV
	    public static void Paso0(StringBuilder sb, int rv)
        {
            int index = -1;//inicalizar el indice

            for (int i = 5; i > 1 && index < 0; --i)
            {
                if (sb.Length >= i)// si el indice es menor o igaul que la longitud de la palabra
                {
                    
                    index = Paso_0.LastIndexOf(sb.ToString(sb.Length - i, i));//Busco el indice del sufijo

                   
                    if (index >= 0) //Si lo he encontrado...
                    {
                        string aux = Paso_0[index];//tomo el prefijo

                        //busco el indice de la palabra a la que debe preceder
                        int index_after = DespuesPaso_0.LastIndexOf(aux);

                        //Si encuentro la palabra a la que debe preceder...
                        if (index_after >= 0)
                        {
                            string palabra = DespuesPaso_0[index_after];//tomo el prefijo

                            //Compruebo si esa palabra precede, efectivamente, al sufijo
                            if (sb.ToString(0, index).Substring(0, index_after).Length + palabra.Length == sb.ToString(0, index).Length)
                            {
                                if (DespuesPaso_0[index_after] == "yendo" && sb[index_after - 1] == 'u' && index_after >= rv)//si es uno de estos
                                {
                                    sb.Remove(sb.Length - index, index);//se elimina
                                }
                                else//si no lo es
                                {
                                    sb.Remove(sb.Length - index, index);//se elimina
                                    for (int j = index_after; j < sb.Length; j++)//se eliminan los acentos
                                        sb[j] = Herramientas.EliminaAcento(sb[j]);
                                }
                            }
                        }
                    }
                }
            }
        }
		//Realizar el paso 1
		//Entrada: palabra y valores de R1 y R2
        public static void Paso1(StringBuilder sb, int r1, int r2)
        {
            int posicion = -1;//posicion donde se encuentra el sufijo
            int coleccion = -1;//en dependencia del valor de esta variable es lo que se bebehacer despues del analisis de la palabra
            string encontrada = "";
            string buscar = sb.ToString();//palabra sobre la que se va a buscar

            foreach (string s in Paso1_1)//para cada uno de los sufijos del paso
            {
                int index = buscar.LastIndexOf(s);//se busca la posición en la palabra
                if (index >= 0)//si se encontró
                {
                    string palabra = buscar.Substring(index);//se saca la subcadena
                    int aux = -1;

                    aux = Paso1_1.LastIndexOf(palabra);//indice en los sufijos
                    if (aux >= 0 && Paso1_1[aux].Length > encontrada.Length)//si existe el indice y la longitud del sufijo es mayor que la longitud del encontrado
                    {
                        encontrada =  Paso1_1[aux];//toma el valor del sufijo encontrado
                        posicion = index;//se guarda la posicion
                        coleccion = 1;//se define que hacer
                    }
                }
            }

            foreach (string s in Paso1_2)//para cada uno de los sufijos del paso
            {
                int index = buscar.LastIndexOf(s);//se busca la posición en la palabra
                if (index >= 0)//si se encontró
                {
                    string palabra = buscar.Substring(index);//se saca la subcadena
                    int aux = -1;

                    aux = Paso1_2.LastIndexOf(palabra);//indice en los sufijos
                    if (aux >= 0 && Paso1_2[aux].Length > encontrada.Length)//si existe el indice y la longitud del sufijo es mayor que la longitud del encontrado
                    {
                        encontrada = Paso1_2[aux];//toma el valor del sufijo encontrado
                        posicion = index;//se guarda la posicion
                        coleccion = 2;//se define que hacer
                    }
                }
            }

            foreach (string s in Paso1_3)//para cada uno de los sufijos del paso
            {
                int index = buscar.LastIndexOf(s);//se busca la posición en la palabra
                if (index >= 0)//si se encontró
                {
                    string palabra = buscar.Substring(index);//se saca la subcadena
                    int aux = -1;

                    aux = Paso1_3.LastIndexOf(palabra);//indice en los sufijos
                    if (aux >= 0 && Paso1_3[aux].Length > encontrada.Length)//si existe el indice y la longitud del sufijo es mayor que la longitud del encontrado
                    {
                        encontrada = Paso1_3[aux];//toma el valor del sufijo encontrado
                        posicion = index;//se guarda la posicion
                        coleccion = 3;//se define que hacer
                    }
                }
            }

            foreach (string s in Paso1_4)//para cada uno de los sufijos del paso
            {
                int index = buscar.LastIndexOf(s);//se busca la posición en la palabra
                if (index >= 0)//si se encontró
                {
                    string palabra = buscar.Substring(index);//se saca la subcadena
                    int aux = -1;

                    aux = Paso1_4.LastIndexOf(palabra);//indice en los sufijos
                    if (aux >= 0 && Paso1_4[aux].Length > encontrada.Length)//si existe el indice y la longitud del sufijo es mayor que la longitud del encontrado
                    {
                        encontrada = Paso1_4[aux];//toma el valor del sufijo encontrado
                        posicion = index;//se guarda la posicion
                        coleccion = 4;//se define que hacer
                    }
                }
            }

            foreach (string s in Paso1_5)//para cada uno de los sufijos del paso
            {
                int index = buscar.LastIndexOf(s);//se busca la posición en la palabra
                if (index >= 0)//si se encontró
                {
                    string palabra = buscar.Substring(index);//se saca la subcadena
                    int aux = -1;

                    aux = Paso1_5.LastIndexOf(palabra);//indice en los sufijos
                    if (aux >= 0 && Paso1_5[aux].Length > encontrada.Length)//si existe el indice y la longitud del sufijo es mayor que la longitud del encontrado
                    {
                        encontrada = Paso1_5[aux];//toma el valor del sufijo encontrado
                        posicion = index;//se guarda la posicion
                        coleccion = 5;//se define que hacer
                    }
                }
            }

            foreach (string s in Paso1_6)//para cada uno de los sufijos del paso
            {
                int index = buscar.LastIndexOf(s);//se busca la posición en la palabra
                if (index >= 0)//si se encontró
                {
                    string palabra = buscar.Substring(index);//se saca la subcadena
                    int aux = -1;

                    aux = Paso1_6.LastIndexOf(palabra);//indice en los sufijos
                    if (aux >= 0 && Paso1_6[aux].Length > encontrada.Length)//si existe el indice y la longitud del sufijo es mayor que la longitud del encontrado
                    {
                        encontrada = Paso1_6[aux];//toma el valor del sufijo encontrado
                        posicion = index;//se guarda la posicion
                        coleccion = 6;//se define que hacer
                    }
                }
            }

            foreach (string s in Paso1_7)//para cada uno de los sufijos del paso
            {
                int index = buscar.LastIndexOf(s);//se busca la posición en la palabra
                if (index >= 0)//si se encontró
                {
                    string palabra = buscar.Substring(index);//se saca la subcadena
                    int aux = -1;

                    aux = Paso1_7.LastIndexOf(palabra);//indice en los sufijos
                    if (aux >= 0 && Paso1_7[aux].Length > encontrada.Length)//si existe el indice y la longitud del sufijo es mayor que la longitud del encontrado
                    {
                        encontrada = Paso1_7[aux];//toma el valor del sufijo encontrado
                        posicion = index;//se guarda la posicion
                        coleccion = 7;//se define que hacer
                    }
                }
            }

            foreach (string s in Paso1_8)//para cada uno de los sufijos del paso
            {
                int index = buscar.LastIndexOf(s);//se busca la posición en la palabra
                if (index >= 0)//si se encontró
                {
                    string palabra = buscar.Substring(index);//se saca la subcadena
                    int aux = -1;

                    aux = Paso1_8.LastIndexOf(palabra);//indice en los sufijos
                    if (aux >= 0 && Paso1_8[aux].Length > encontrada.Length)//si existe el indice y la longitud del sufijo es mayor que la longitud del encontrado
                    {
                        encontrada = Paso1_8[aux];//toma el valor del sufijo encontrado
                        posicion = index;//se guarda la posicion
                        coleccion = 8;//se define que hacer
                    }
                }
            }

            foreach (string s in Paso1_9)//para cada uno de los sufijos del paso
            {
                int index = buscar.LastIndexOf(s);//se busca la posición en la palabra
                if (index >= 0)//si se encontró
                {
                    string palabra = buscar.Substring(index);//se saca la subcadena
                    int aux = -1;

                    aux = Paso1_9.LastIndexOf(palabra);//indice en los sufijos
                    if (aux >= 0 && Paso1_9[aux].Length > encontrada.Length)//si existe el indice y la longitud del sufijo es mayor que la longitud del encontrado
                    {
                        encontrada = Paso1_9[aux];//toma el valor del sufijo encontrado
                        posicion = index;//se guarda la posicion
                        coleccion = 9;//se define que hacer
                    }
                }
            }

            if (posicion >= 0)//si se encontró un sufijo
            {
                switch (coleccion)
                {
                    case 1:
                        if (posicion >= r2)//si esta en R2 o posterior
                            sb.Remove(posicion, sb.Length - posicion);//se elimina el sufijo
                        break;
                    case 2:
                        if (posicion >= r2)//si esta en R2 o posterior
                            sb.Remove(posicion, sb.Length - posicion);//se elimina el sufijo
                        break;
                    case 3:
                        if (posicion >= r2)//si esta en R2 o posterior
                        {
                            sb.Remove(posicion, sb.Length - posicion);//se elimina el sufijo
                            sb.Append("log");//se adiciona
                        }
                        break;
                    case 4:
                        if (posicion >= r2)//si esta en R2 o posterior
                        {
                            sb.Remove(posicion, sb.Length - posicion);//se elimina el sufijo
                            sb.Append("u");//se adiciona
                        }
                        break;
                    case 5:
                        if (posicion >= r2)//si esta en R2 o posterior
                        {
                            sb.Remove(posicion, sb.Length - posicion);//se elimina el sufijo
                            sb.Append("ente");//se adiciona
                        }
                        break;
                    case 6:
                        if (posicion >= r1)//si esta en R1 o posterior
                            sb.Remove(posicion, sb.Length - posicion);//se elimina el sufijo
                        else
                        {
                            string aux = sb.ToString(0, posicion);//se saca la subcadena del pricipio a la posicion
                            if (aux.Length>2 &&
                                aux.Substring(0, aux.Length - 2) == "iv" &&
                                aux.Substring(0, aux.Length - 2) == "oc" &&
                                aux.Substring(0, aux.Length - 2) == "ic" &&
                                aux.Substring(0, aux.Length - 2) == "ad" && posicion >= r2)//si una de estas y esta en R2 o posterior
                            {
                                sb.Remove(posicion, sb.Length - posicion);//se elimina el sufijo
                            }
                        }
                        break;
                    case 7:
                    case 8:
                    case 9:
                        if (posicion >= r2)//si esta en R2 o posterior
                        {
                            sb.Remove(posicion, sb.Length - posicion);//se elimina el sufijo
                        }
                        break;
                }
            }
        }
		//Realizar el paso 2a
		//Entrada: palabra y valor de RV
        public static void Paso2a(StringBuilder sb, int rv)
        {
            int index = -1;

            //Busco el indice del sufijo
            index = Paso2_a.IndexOf(sb.ToString());

            if (index >= rv && sb.ToString().Substring(sb.Length - index - 1, 1) == "u")//si se cumple
            {
                sb.Remove(sb.Length - index, index);//se elimina el sufijo
            }
        }
		//Realizar el paso 2b
		//Entrada: palabra y valor de RV
        public static void Paso2b(StringBuilder sb, int rv)
        {
            string seleccionado = "";
            int pos = -1;
            int index = -1;

            foreach(string s in Paso2_b1)
            {
                index = sb.ToString().LastIndexOf(s);
                if (index >= 0) 
                {
                    string palabra = sb.ToString().Substring(index);
                    int aux = index;

                    index = Paso2_b1.LastIndexOf(palabra);
                    if (index >= 0)
                    {
                        seleccionado = Paso2_b1[index];
                        pos = aux;
                    }
                }
            }

            if (pos >= rv && sb.ToString(sb.Length - pos - 2, pos) == "gu")
                sb.Remove(pos - 1, sb.Length - pos + 1);

            pos = -1;
            index = -1;
            seleccionado = "";

            foreach (string s in Paso2_b2)
            {
                index = sb.ToString().LastIndexOf(s);
                if (index >= 0)
                {
                    string palabra = sb.ToString().Substring(index);
                    int aux = index;

                    index = Paso2_b2.LastIndexOf(palabra);
                    if (index >= 0)
                    {
                        seleccionado = Paso2_b2[index];
                        pos = aux;
                    }
                }
            }

            if (pos >= rv)
                sb.Remove(pos, sb.Length - pos);
        }
		//Realizar el paso 3
		//Entrada: palabra y valor de RV
        public static void Paso3(StringBuilder sb, int rv)
        {
            string seleccionado = "";
            int pos = -1;
            int index = -1;

            foreach (string s in Paso3_1)
            {
                index = sb.ToString().LastIndexOf(s);
                if (index >= 0) 
                {
                    string palabra = sb.ToString().Substring(index);
                    int aux = index;

                    index = Paso3_1.LastIndexOf(palabra);
                    if (index >= 0)
                    {
                        seleccionado = Paso3_1[index];
                        pos = aux;
                    }
                }
            }

            if (pos >= rv)
                sb.Remove(pos, sb.Length - pos);

            pos = -1;
            index = -1;
            seleccionado = "";

            foreach (string s in Paso3_2)
            {
                index = sb.ToString().LastIndexOf(s);
                if (index >= 0)
                {
                    string palabra = sb.ToString().Substring(index);
                    int aux = index;

                    index = Paso3_2.LastIndexOf(palabra);
                    if (index >= 0)
                    {
                        seleccionado = Paso3_2[index];
                        pos = index;
                    }
                }
            }

            if (pos >= 0 && sb.ToString(sb.Length - pos - 2, pos) == "gu" && pos - 1 >= rv)
                sb.Remove(pos - 1, sb.Length - pos + 1);
        }
		//Elimina los acentos de las vocales acentuada en una palabra
        public static void EliminaAcentos(StringBuilder sb)
        {
            for(int i = 0; i < sb.Length; ++i)
            {
                char c = sb[i];
                sb[i] = Herramientas.EliminaAcento(c);
            }
        }
}
