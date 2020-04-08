using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

using cw3.Models;
using Microsoft.AspNetCore.Mvc;

namespace cw3.Controllers
{   
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {

        [HttpGet]
        public IActionResult GetStudents()
        {
            var list = new List<Student>();
            using(SqlConnection con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s18687;Integrated Security=True"))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "select FirstName, LastName, BirthDate, Studies.Name, Semester from Student, Studies, Enrollment where Enrollment.IdEnrollment=Student.IdEnrollment and Studies.IdStudy=Enrollment.IdStudy;";

                con.Open();
                SqlDataReader dr = com.ExecuteReader();
                while(dr.Read())
                {
                    var st = new Student();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.BirthDate = dr["BirthDate"].ToString();
                    st.Study = dr["Name"].ToString();
                    st.Semester = dr["Semester"].ToString();
              

                    list.Add(st);
                }
            }


               // con.Dispose();
            return Ok(list);
        }


        [HttpGet("{IndexNumber}")]
        public IActionResult GetStudent(string IndexNumber)
        {
            using (SqlConnection con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s18687;Integrated Security=True"))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "select * from Student where IndexNumber=@index";

                SqlParameter par = new SqlParameter();
                par.Value = IndexNumber;
                par.ParameterName = "index";
                com.Parameters.Add(par);


                con.Open();
                var dr = com.ExecuteReader();
                if(dr.Read())
                {
                    var st = new Student();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.BirthDate = dr["BirthDate"].ToString();
                    st.Study = dr["Name"].ToString();
                    st.Semester = dr["Semester"].ToString();

                    return Ok(st);
                }
            }
                return NotFound();
        }

    }
}