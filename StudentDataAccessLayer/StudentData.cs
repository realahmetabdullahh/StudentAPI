
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace StudentDataAccessLayer
{
    public class StudentDTO
    {
        public StudentDTO(int id, string name, int age, int grade)
        {
            this.Id = id;
            this.Name = name;
            this.Age = age;
            this.Grade = grade;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Grade { get; set; }
    }

    public class StudentData
    {
        static string _connectionString = "Server=localhost;Database=StudentsDB;User Id=sa;Password=sa123456;Encrypt=False;TrustServerCertificate=True;Connection Timeout=30;";

        public static List<StudentDTO> GetAllStudents()
        {
            var studentsList = new List<StudentDTO>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SP_GetAllStudents", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            studentsList.Add(new StudentDTO(
                                reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32(reader.GetOrdinal("Id")),
                                reader.IsDBNull(reader.GetOrdinal("Name")) ? string.Empty : reader.GetString(reader.GetOrdinal("Name")),
                                reader.IsDBNull(reader.GetOrdinal("Age")) ? 0 : reader.GetInt32(reader.GetOrdinal("Age")),
                                reader.IsDBNull(reader.GetOrdinal("Grade")) ? 0 : reader.GetInt32(reader.GetOrdinal("Grade"))
                            ));
                        }
                    }
                }
            }
            return studentsList;
        }

        public static List<StudentDTO> GetPassedStudents()
        {
            var passedStudentsList = new List<StudentDTO>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetPassedStudents", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            passedStudentsList.Add(new StudentDTO(
                                reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32(reader.GetOrdinal("Id")),
                                reader.IsDBNull(reader.GetOrdinal("Name")) ? string.Empty : reader.GetString(reader.GetOrdinal("Name")),
                                reader.IsDBNull(reader.GetOrdinal("Age")) ? 0 : reader.GetInt32(reader.GetOrdinal("Age")),
                                reader.IsDBNull(reader.GetOrdinal("Grade")) ? 0 : reader.GetInt32(reader.GetOrdinal("Grade"))
                            ));
                        }
                    }
                }
            }
            return passedStudentsList;
        }

        public static double GetAverageGrade()
        {
            double averageGrade;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetAverageGrade", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    object result = command.ExecuteScalar();
                    averageGrade = result != DBNull.Value ? Convert.ToDouble(result) : 0.0;
                }
            }
            return averageGrade;
        }

        public static StudentDTO GetStudentById(int studentId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = new SqlCommand("SP_GetStudentById", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@studentId", studentId);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new StudentDTO(
                            reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32(reader.GetOrdinal("Id")),
                            reader.IsDBNull(reader.GetOrdinal("Name")) ? string.Empty : reader.GetString(reader.GetOrdinal("Name")),
                            reader.IsDBNull(reader.GetOrdinal("Age")) ? 0 : reader.GetInt32(reader.GetOrdinal("Age")),
                            reader.IsDBNull(reader.GetOrdinal("Grade")) ? 0 : reader.GetInt32(reader.GetOrdinal("Grade"))
                        );
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public static int AddStudent(StudentDTO studentDTO)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = new SqlCommand("SP_AddStudent", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Name", studentDTO.Name);
                command.Parameters.AddWithValue("@Age", studentDTO.Age);
                command.Parameters.AddWithValue("@Grade", studentDTO.Grade);

                SqlParameter outputIdParam = new SqlParameter("@NewStudentId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(outputIdParam);

                connection.Open();
                command.ExecuteNonQuery();

                return (int)outputIdParam.Value;
            }
        }

        public static bool UpdateStudent(StudentDTO studentDTO)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = new SqlCommand("SP_UpdateStudent", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", studentDTO.Id);
                command.Parameters.AddWithValue("@Name", studentDTO.Name);
                command.Parameters.AddWithValue("@Age", studentDTO.Age);
                command.Parameters.AddWithValue("@Grade", studentDTO.Grade);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public static bool DeleteStudent(int studentId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = new SqlCommand("SP_DeleteStudent", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@studentId", studentId);
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected == 1;
            }
        }
    }
}
