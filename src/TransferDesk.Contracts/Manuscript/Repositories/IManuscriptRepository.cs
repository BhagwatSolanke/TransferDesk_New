using System;
using System.Collections.Generic;
using Entities= TransferDesk.Contracts.Manuscript.Entities;   

namespace TransferDesk.Contracts.Manuscript.Repositories
{
    public interface IManuscriptRepository:IDisposable 
    {
       
        //IEnumerable<Entities.Manuscript> GetManuscripts();
        //Entities.Manuscript GetManuscriptByID(int manuscriptID);
        int? AddManuscript(Entities.Manuscript manuscript);
        void UpdateManuscript(Entities.Manuscript manuscript);
        void SaveChanges();

        //List<Entities.Section> GetSectionMasterList(int journalID);

        //List<Entities.ArticleType> GetArticleTypeList(int journalID);
    }
}

//refered microsoft's pattern example below
//using System;
//using System.Collections.Generic;
//using ContosoUniversity.Models;

//namespace ContosoUniversity.DAL
//{
//    public interface IStudentRepository : IDisposable
//    {
//        IEnumerable<Student> GetStudents();
//        Student GetStudentByID(int studentId);
//        void InsertStudent(Student student);
//        void DeleteStudent(int studentID);
//        void UpdateStudent(Student student);
//        void Save();
//    }
//}

//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace ContosoUniversity.Models
//{
//    public abstract class Person
//    {
//        public int ID { get; set; }

//        [Required]
//        [StringLength(50)]
//        [Display(Name = "Last Name")]
//        public string LastName { get; set; }
//        [Required]
//        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
//        [Column("FirstName")]
//        [Display(Name = "First Name")]
//        public string FirstMidName { get; set; }

//        [Display(Name = "Full Name")]
//        public string FullName
//        {
//            get
//            {
//                return LastName + ", " + FirstMidName;
//            }
//        }
//    }
//}