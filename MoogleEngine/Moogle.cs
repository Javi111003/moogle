namespace MoogleEngine;


public static class Moogle
{   private static Nucleo NucleoBuscador;//núcleo buscador
    static Moogle()
	{
		NucleoBuscador = new Nucleo();//inicializamos el núcleo
	}   

    public static SearchResult Query(string query) {
        
        return new SearchResult( NucleoBuscador.RealizarConsulta(query), NucleoBuscador.Sugerencia);//realizamos la consulta y devolvemos resultados y sugerencia
    }
}