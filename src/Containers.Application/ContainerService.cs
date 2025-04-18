
using Containers.Models;
using Microsoft.Data.SqlClient;

namespace Containers.Application;

public class ContainerService : IConrainerService
{
    private string _connectionString;
    public ContainerService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IEnumerable<Container> GetAllContainers()
    {
        List<Container> containers = [];
        const string queryString = "SELECT * FROM Containers";

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            SqlCommand command = new SqlCommand(queryString, connection);
            connection.Open();

            SqlDataReader reader = command.ExecuteReader();
            try
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var containerRow = new Container
                        {
                            ID = reader.GetInt32(0),
                            ContainerTypeID = reader.GetInt32(1),
                            IsHazardous = reader.GetBoolean(2),
                            Name = reader.GetString(3),
                        };

                        containers.Add(containerRow);
                    }
                }
                
            }
            finally
            {
                reader.Close();
            }
        }
        return containers;
    }

    public bool AddContainer(Container container)
    {
        const string insertString =
            "INSERT INTO Containers(ContainerTypeId, IsHazardious, Name) VALUES(@ContainerTypeID, @IsHazardous, @Name)";
        int countRowsAdded = -1;
        
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            SqlCommand command = new SqlCommand(insertString, connection);
            
            command.Parameters.AddWithValue("@ContainerTypeID", container.ContainerTypeID);
            command.Parameters.AddWithValue("@IsHazardous", container.IsHazardous);
            command.Parameters.AddWithValue("@Name", container.Name);
            
            connection.Open();
            countRowsAdded = command.ExecuteNonQuery();
        }
        
        return countRowsAdded != -1;
    }
}