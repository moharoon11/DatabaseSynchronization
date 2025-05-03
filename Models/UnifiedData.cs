

namespace DbSync.Models {


    public class UnifiedData {


        public string SourceTable {get; set;}

        public int? EmployeeId {get; set;}

        public int? StudentId {get; set;}

        public string Name {get; set;}

        public string Department {get; set;}

        public string Course {get; set;}

        public DateTime SyncedAt {get; set;}
    }
}