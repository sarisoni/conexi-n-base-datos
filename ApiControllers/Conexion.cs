using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using MongoDB.Driver;

[ApiController]
[Route("conexion")]
public class Conexion : Controller {
    [HttpGet("sql")]
    public IActionResult ListarCarrerasSql(){
        List<CarreraSQL> lista = new List<CarreraSQL>();
       
        SqlConnection conn = new SqlConnection(CadenasConexion.SQL_SERVER);
        SqlCommand cmd = new SqlCommand("select IdCarrera, Carrera from Carreras");
        cmd.Connection = conn;
        cmd.CommandType = System.Data.CommandType.Text;
        cmd.Connection.Open();

        SqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read()) {
            CarreraSQL carrera = new CarreraSQL();
            carrera.IdCarrera = reader.GetInt16("IdCarrera");
            carrera.Carrera = reader.GetString("Carrera");

            lista.Add(carrera);
        }

        reader.Close();
        conn.Close();

        return Ok(lista);

    }

    [HttpGet("mongo")] 
    public IActionResult ListarSalonesMongoDb(){
        MongoClient client = new MongoClient(CadenasConexion.MONGO_DB);
        var db = client.GetDatabase("Practica2_Ivan");
        var collection = db.GetCollection<SalonMongo>("Salones");

        var lista = collection.Find(FilterDefinition<SalonMongo>.Empty).ToList();

        return Ok(lista);
    }
}