using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoogleEngine
{
    public class Matrix
    {
        public static double[,] Multiplicar(double[,] matriz1, double[,] matriz2)
        {
            
            double[,] sol = new double[matriz1.GetLength(0), matriz2.GetLength(1)];
            for (int i = 0; i < sol.GetLength(0) - 1; i++)
            {
                for (int j = 0; j < sol.GetLength(1) - 1; j++)
                {
                    for (int k = 0; k < sol.GetLength(1); k++)
                    {
                        sol[i, j] += matriz1[i, k] * matriz2[k, j];
                    }
                }
            }
            return sol;
        }
        static public double[,] Sumar(double[,] matriz1, double[,] matriz2)
        {
            double[,] sol = new double[matriz1.GetLength(0), matriz1.GetLength(1)];
            for (int i = 0; i < sol.GetLength(0) - 1; i++)
            {
                for (int j = 0; j < sol.GetLength(1) - 1; j++)
                {
                    sol[i, j] = matriz1[i, j] + matriz2[i, j];
                }

            }
            return sol;
        }
        public static double[,] Rotar(double[,] matriz, int CantidadRotaciones)
        {
            int k = CantidadRotaciones;
            if (k >= 4)
            {
                k -= 4;
                return Rotar(matriz, k);
            }
            else if (k == 0)
            {
                return matriz;
            }
            else if (k == 1 || k == -3)
            {
                double[,] sol = new double[matriz.GetLength(1), matriz.GetLength(0)];

                for (int i = 0; i < sol.GetLength(0); i++)
                {
                    int c = sol.GetLength(1) - 1;
                    for (int j = 0; j < sol.GetLength(1); j++)
                    {

                        sol[i, j] = matriz[c, i]; c--;

                    }
                }
                return sol;

            }
            else if (k == 2 || k == -2)
            {
                double[,] sol = new double[matriz.GetLength(1), matriz.GetLength(0)];

                for (int i = 0; i < sol.GetLength(0); i++)
                {
                    int c = sol.GetLength(1) - 1;
                    for (int j = 0; j < sol.GetLength(1); j++)
                    {

                        sol[i, j] = matriz[c, i]; c--;

                    }
                }

                return Rotar(sol, 1);
            }
            else if (k == 3 || k == -1)
            {
                double[,] sol = new double[matriz.GetLength(1), matriz.GetLength(0)];

                for (int i = 0; i < sol.GetLength(0); i++)
                {
                    int c = sol.GetLength(1) - 1;
                    for (int j = 0; j < sol.GetLength(1); j++)
                    {

                        sol[i, j] = matriz[c, i]; c--;

                    }
                }

                return Rotar(sol, 2);
            }
            else
            {
                k += 4;
                return Rotar(matriz, k);
            }

        }
        //Elimina una columna de una matriz o arreglo bidimensional
        public static int[,] EliminaColumnaMatriz(int[,] matriz, int posicion)
        {
            int[,] resultado = matriz;//por defecto se devuelve la misma matriz

            if ((matriz.GetLength(0) > 0) && (matriz.GetLength(1) > 0) && (posicion > 0) && (posicion < matriz.GetLength(1)))
            {//Si cumple las condiciones

                int[,] temporal = new int[matriz.GetLength(0), matriz.GetLength(1) - 1];//Se crea matriz temporal con dimensión menor en una columna a la de entrada
                int pos = 0;//temporal para recorrer los elementos a copiar

                for (int i = 0; i < matriz.GetLength(1); i++)//Se copia de una matriz a otra ingnorando los valores de la posición
                    if (i != posicion)
                    {//si no es la posicion a borrara
                        for (int j = 0; j < matriz.GetLength(0); j++)//se copia la columna
                            temporal[j, pos] = matriz[j, i];
                        pos++;//se incrementa la posición
                    }
                resultado = temporal;//se asigna el resultado

            }
            return resultado;//se devuelve el resultado

        }

        //Elimina varias columnas de una matriz o arreglo bidimensional
        //Entrada: matriz de la que se quiere eliminar las columnas y posiciones de las columnas
        //Salida: nueva matriz sin las columnas
        public static int[,] RemoveAllColumnas(int[,] matriz, List<int> posiciones)
        {
            int[,] resultado = matriz;//por defecto se devuelve la misma matriz

            if ((matriz.GetLength(0) > 0) && (matriz.GetLength(1) > 0) && (posiciones.Count > 0))
            {//Si cumple las condiciones
                posiciones.Sort();//Se ordenan las posiciones
                int[,] tempmatriz = matriz;//arreglo temporal
                for (int i = 0; i < posiciones.Count; i++)//Se eliminan una por una las columnas de la matriz
                    if ((posiciones[i] - i) < tempmatriz.GetLength(1))//si la posicion que se quería borrar menos la cantidad de posiciones borradas es menor que la longitud de la matriz actual
                        tempmatriz = EliminaColumnaMatriz(tempmatriz, posiciones[i] - i);//se elimina la columna
                resultado = tempmatriz; //se asigna el resultado

            }
            return resultado;//se devuelve el resultado
        }
        //Elimina varias celdas de una fila en la matriz
        public static int[] EliminaCeldas(int[] arreglo, List<int> posiciones)
        {
            int[] resultado = arreglo;//por defecto se devuelve el mismo arreglo

            if ((arreglo.Length > 0) && (posiciones.Count > 0))
            {//Si cumple las condiciones
                posiciones.Sort();//Se ordenan las posiciones
                int[] temparreglo = arreglo;//arreglo temporal
                for (int i = 0; i < posiciones.Count; i++) //Se eliminan una por una las posiciones
                    if ((posiciones[i] - i) < temparreglo.Length)//si la posición que se quería borrar menos la cantidad de posiciones borradas es menor que la longitud del arreglo actual
                        temparreglo = Herramientas.EliminaElementoArreglo(temparreglo, posiciones[i] - i);//se elimina el elemento
                resultado = temparreglo;//se asigna el resultado	

            }
            return resultado;//se devuelve el resultado
        }

    }
}

