namespace LIN.Identity.Data;


public class Points
{


    #region Abstracciones



    /// <summary>
    /// Crear nuevo punto
    /// </summary>
    /// <param name="data">Modelo</param>
    public async static Task<CreateResponse> Create(LIN.Types.Maps.Models.PlacePoint data)
    {

        // Obtiene la conexión
        (Conexión context, string connectionKey) = Conexión.GetOneConnection();

        var res = await Create(data, context);
        context.CloseActions(connectionKey);
        return res;
    }



    /// <summary>
    /// Obtiene la lista de puntos
    /// </summary>
    /// <param name="id">ID del profile</param>
    public async static Task<ReadAllResponse<LIN.Types.Maps.Models.PlacePoint>> Read(int id)
    {

        // Obtiene la conexión
        (Conexión context, string connectionKey) = Conexión.GetOneConnection();

        var res = await ReadAll(id, context);
        context.CloseActions(connectionKey);
        return res;

    }


    #endregion



    /// <summary>
    /// Crea un nuevo punto
    /// </summary>
    /// <param name="data">Modelo del enlace</param>
    /// <param name="context">Contexto de conexión</param>
    public async static Task<CreateResponse> Create(LIN.Types.Maps.Models.PlacePoint data, Conexión context)
    {
        // ID en 0
        data.ID = 0;

        // Ejecución
        try
        {
            context.DataBase.Attach(data.Profile);
            var res = context.DataBase.Points.Add(data);
            await context.DataBase.SaveChangesAsync();
            return new(Responses.Success, data.ID);
        }
        catch
        {
            context.DataBase.Remove(data);
        }
        return new();
    }



    /// <summary>
    /// Obtiene la lista de puntos
    /// </summary>
    /// <param name="id">ID del perfil</param>
    /// <param name="context">Contexto de conexión</param>
    public async static Task<ReadAllResponse<LIN.Types.Maps.Models.PlacePoint>> ReadAll(int id, Conexión context)
    {

        // Ejecución
        try
        {

            var now = DateTime.Now;

            var activo = await (from P in context.DataBase.Points
                                where P.Profile.ID== id
                                select P).ToListAsync();

            if (activo == null)
                return new(Responses.NotExistProfile);


            return new(Responses.Success, activo);
        }
        catch
        {
        }
        return new();

    }


}