using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace program.DataAccess
{
    public static class SqlDbContext
    {
        private static string ConnString => ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        // Hàm thực thi Stored Procedure trả về bảng dữ liệu (Dùng cho Login, Xem lớp)
        public static DataTable ExecuteQuery(string spName, SqlParameter[] parameters = null)
        {
            using var conn = new SqlConnection(ConnString);
            using var cmd = new SqlCommand(spName, conn) { CommandType = CommandType.StoredProcedure };
            if (parameters != null) cmd.Parameters.AddRange(parameters);

            var dt = new DataTable();
            using var adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);
            return dt;
        }

        public static int ExecuteNonQuery(string sql, SqlParameter[] parameters = null)
        {
            using var conn = new SqlConnection(ConnString);
            conn.Open();
            using var cmd = new SqlCommand(sql, conn);
            if (parameters != null) cmd.Parameters.AddRange(parameters);
            return cmd.ExecuteNonQuery();
        }
    }
}