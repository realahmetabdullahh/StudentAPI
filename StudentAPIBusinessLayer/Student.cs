using System.Data;
using StudentDataAccessLayer;

namespace StudentAPIBusinessLayer
{

    public class Student
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public StudentDTO SDTO {
            get { return (new StudentDTO(this.ID, this.Name, this.Age, this.Grade)); }
        }

            public int ID { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
            public int Grade { get; set; }

            public Student(StudentDTO SDTO, enMode cMode =enMode.AddNew )

            {
                this.ID = SDTO.Id ;
                this.Name =SDTO.Name; 
                this.Age =SDTO.Age;
                this.Grade = SDTO.Grade;
             
                Mode = cMode;
            }

        private bool _AddNewStudent()
            {
                //call DataAccess Layer 

                this.ID = StudentData.AddStudent(SDTO);

                return (this.ID != -1);
            }

            private bool _UpdateStudent()
            {
                return StudentData.UpdateStudent(SDTO);
            }


            public static List<StudentDTO> GetAllStudents()
            {
                return StudentData.GetAllStudents();
            }


        public static List<StudentDTO> GetPassedStudents()
        {
            return StudentData.GetPassedStudents();
        }

        public static double GetAverageGrade()
        {
            return StudentData.GetAverageGrade();
        }

        public static Student Find(int ID)
            {

               StudentDTO SDTO = StudentData.GetStudentById(ID);

            if (SDTO != null)
            {
                
                return new Student(SDTO, enMode.Update);
            }
            else
                return null;
            }

            public bool Save()
            {
                switch (Mode)
                {
                    case enMode.AddNew:
                        if (_AddNewStudent())
                        {

                            Mode = enMode.Update;
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    case enMode.Update:

                        return _UpdateStudent();

                }

                return false;
            }

            public static bool DeleteStudent(int ID)
            {
                return StudentData.DeleteStudent(ID);
            }

        }
    }

