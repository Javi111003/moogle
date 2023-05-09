# Moogle!

![](moogle.png)

> Proyecto de Programación I.
> Facultad de Matemática y Computación - Universidad de La Habana.
> Curso 2023.

Javier González Díaz C-122
Moogle! es una aplicación *totalmente original* cuyo propósito es buscar inteligentemente un texto en un conjunto de documentos.

Es una aplicación web, desarrollada con tecnología .NET Core 6.0, específicamente usando Blazor como *framework* web para la interfaz gráfica, y en el lenguaje C#.
La aplicación está dividida en dos componentes fundamentales:

- `MoogleServer` es un servidor web que renderiza la interfaz gráfica y sirve los resultados.
- `MoogleEngine` es una biblioteca de clases donde está... ehem... casi implementada la lógica del algoritmo de búsqueda.


## Solucion aplicada 

La búsqueda en nuestro proyecto es lo más inteligente posible. No nos limitamos a los documentos donde aparece exactamente la frase introducida por el usuario, sino que el usuario puede buscar no solo una palabra sino en general una frase cualquiera.

Si no aparecen todas las palabras de la frase en un documento, pero al menos aparecen algunas, este documento también es devuelto, pero con un peso menor mientras menos palabras aparezcan.

El orden en que aparezcan en el documento los términos de la consulta en general no importa, ni siquiera que aparezcan en lugares totalmente diferentes del documento.

Si en diferentes documentos aparecen la misma cantidad de palabras de la consulta, pero uno de ellos contiene una palabra más rara porque aparece en menos documentos, el documento con palabras más raras tiene un peso más alto, porque es una respuesta más específica.
De la misma forma, si un documento tiene más términos de la consulta que otro, tiene un peso más alto, a menos que sean términos menos relevantes.
Las palabras excesivamente comunes como las preposiciones, conjunciones, etc. y aquellas que se repiten muchas veces, son ignoradas por completo ya que aparecerán en la inmensa mayoría de los documentos , todo esto para mejorar la exactiud de los documentos mas relevantes para el usuario.

Ademas de implementar las funcionalidades adicionales para una mejor experiencia , todo esto logrado con la construccion de clases especificas bien definidas que confluyen en un inicializador denominado Nucleo , el cual en la descripcion del proyecto se abordan los elementos que lo componen y que hacen posible la de bolucion de los resultados de busqueda ordenados de mayor a menor segun la evaluacion del score calculado con nuestro modelo vectorial .


### Otras funcionalidades que mejoran la experiencia

Operadores:

El proyecto cuenta con varios operadores para mejorar la búsqueda del usuario:

•	Exclusión, identificado con un ! delante de una palabra, indica que la palabra no debe aparecer en ningún documento devuelto.
•	Inclusión, identificado con un ^ delante de una palabra, indica que la palabra debe aparecer en todos los documentos devueltos.
•	Mayor Relevancia, identificado por varios * delante de una palabra, indica que la palabra es más relevante que las demás palabras de la consulta tantas veces como * tenga delante de ella.
•	Cercanía identificado con un ~ entre las palabras indica que los documentos en los que aparecen estas dos palabras más cerca tendrán mayor peso.

Sugerencia:

Para brindar una mayor exactitud en la búsqueda, el proyecto cuenta con un corrector de palabras, el cual se encarga de dar una sugerencia al usuario en caso de que la búsqueda no coincida con los datos almacenados.

Steamming:

Con el fin de que  nuestra aplicación sea mucho mejor, implementamos en la búsqueda que si las palabras exactas no aparecen, pero aparecen palabras derivadas de la misma raíz, también devuelve esos documentos, pero con un peso menor.

## Requisitos

.NET Core 6.0 

##LINUX

```bash
make dev
```


##Si estás en Windows;

```bash
dotnet watch run --project MoogleServer
```
