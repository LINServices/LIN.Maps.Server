namespace LIN.Identity.Data;


public class Profiles
{


    #region Abstracciones



    /// <summary>
    /// Crear nuevo perfil
    /// </summary>
    /// <param name="data">Modelo</param>
    public async static Task<CreateResponse> Create(LIN.Types.Maps.Models.ProfileModel data)
    {

        // Obtiene la conexión
        (Conexión context, string connectionKey) = Conexión.GetOneConnection();

        var res = await Create(data, context);
        context.CloseActions(connectionKey);
        return res;
    }



    /// <summary>
    /// Obtiene un perfil por medio del ID
    /// </summary>
    /// <param name="id">ID del profile</param>
    /// <returns></returns>
    public async static Task<ReadOneResponse<LIN.Types.Maps.Models.ProfileModel>> Read(int id)
    {

        // Obtiene la conexión
        (Conexión context, string connectionKey) = Conexión.GetOneConnection();

        var res = await Read(id, context);
        context.CloseActions(connectionKey);
        return res;

    }



    /// <summary>
    /// Obtiene un perfil por medio del usuario LIN
    /// </summary>
    /// <param name="id">ID de LIN</param>
    public async static Task<ReadOneResponse<LIN.Types.Maps.Models.ProfileModel>> ReadByAccount(int id)
    {

        // Obtiene la conexión
        (Conexión context, string connectionKey) = Conexión.GetOneConnection();

        var res = await ReadByAccount(id, context);
        context.CloseActions(connectionKey);
        return res;

    }





    #endregion



    /// <summary>
    /// Crea un nuevo profile
    /// </summary>
    /// <param name="data">Modelo del enlace</param>
    /// <param name="context">Contexto de conexión</param>
    public async static Task<CreateResponse> Create(LIN.Types.Maps.Models.ProfileModel data, Conexión context)
    {
        // ID en 0
        data.ID = 0;

        // Ejecución
        try
        {
            var res = context.DataBase.Profiles.Add(data);
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
    /// Obtiene un perfil por medio del ID
    /// </summary>
    /// <param name="id">ID del perfil</param>
    /// <param name="context">Contexto de conexión</param>
    public async static Task<ReadOneResponse<LIN.Types.Maps.Models.ProfileModel>> Read(int id, Conexión context)
    {

        // Ejecución
        try
        {

            var now = DateTime.Now;

            var activo = await (from P in context.DataBase.Profiles
                                where P.ID == id
                                select P).FirstOrDefaultAsync();

            if (activo == null)
                return new(Responses.NotExistProfile);


            return new(Responses.Success, activo);
        }
        catch
        {
        }
        return new();

    }



    /// <summary>
    /// Obtiene un perfil por medio del ID del usuario LIN
    /// </summary>
    /// <param name="id">ID del Usuario LIN</param>
    /// <param name="context">Contexto de conexión</param>
    public async static Task<ReadOneResponse<LIN.Types.Maps.Models.ProfileModel>> ReadByAccount(int id, Conexión context)
    {

        // Ejecución
        try
        {

            var now = DateTime.Now;

            var activo = await (from P in context.DataBase.Profiles
                                where P.AccountID == id
                                select P).FirstOrDefaultAsync();

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