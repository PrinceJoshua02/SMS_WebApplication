using SMS_WebApplication.Models;
using SMS_WebApplication.ViewModel;


namespace SMS_WebApplication.Repositories
{
    public interface IStudentRepository
    {
        // get
        List<Student> GetStudents(string token);

        // get by id
        Student GetStudentById(int id, string token);

        // add 
        StudentViewModel AddStudents(StudentViewModel newStudent, string token);

        // update 
        Task<Student> UpdateStudent(int id, Student newStudent, string token);

        // delete 
        Task DeleteStudent(int id, string token);
    }
}
