using System;
using System.Data.SqlClient;

namespace Images.Data
{
    public class ImagesRepository

    {
        private string _connectionString;
        public ImagesRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public int AddImage(string imageName, string password)
        {
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT INTO Images Values(@password, @imageName, 0 )Select Scope_Identity()";

            cmd.Parameters.AddWithValue("@password", password);
            cmd.Parameters.AddWithValue("@imageName", imageName);
            conn.Open();
            return (int)(decimal)cmd.ExecuteScalar();
        }
        public Image GetImage(int imageId)
        {
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"Select * FROM Images WHERE Id=@id";
            cmd.Parameters.AddWithValue("@id", imageId);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            return new Image
            {
                Id = (int)reader["Id"],
                Password = (string)reader["Password"],
                ImagePath = (string)reader["ImageName"],
                NumberOfViews = (int)reader["NumberOfViews"]
            };



        }
        public void UpdateViews(int imageId)
        {
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"UPDATE Images Set NumberOfViews=NumberOfViews+1   WHERE Id=@id";

            cmd.Parameters.AddWithValue("@id", imageId);
            conn.Open();
            cmd.ExecuteNonQuery();
        }



    }
}
