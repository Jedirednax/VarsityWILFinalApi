using System.Text.Json;

namespace VCTicketTrackerAPIClassLibrary.Tickets
{
    /// <summary>
    /// <example>
    /// <code>
    /// {
    ///  "id": 0,
    ///  "name": "string",
    ///  "description": "string",
    ///  "depart": 0,
    ///  "priority": 0,
    ///  "parentType": "string",
    ///  "fields": {
    ///    "additionalProp1": "string",
    ///    "additionalProp2": "string",
    ///    "additionalProp3": "string"
    ///  }
    ///}
    ///</code>
    /// </example>
    /// </summary>
    public class Category
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Depart { get; set; }
        public int Priority { get; set; }
        public string ParentType { get; set; }

        // String:Name, string:type
        public Dictionary<string, string> Fields { get; set; }


        public Category(string obj)
        {
            var y = JsonSerializer.Deserialize<Category>(obj);
            ID          =y.ID;
            Name        =y.Name;
            Description =y.Description;
            Depart      =y.Depart;
            Priority    =y.Priority;
            ParentType  =y.ParentType;
            Fields      =y.Fields;
        }
        public Category(int iD, string? name, string? description, int depart, int priority, string parentType, Dictionary<string, string> fields)
        {
            ID          = iD;
            Name        = name;
            Description = description;
            Depart      = depart;
            Priority    = priority;
            ParentType  = parentType;
            Fields      = fields;
        }
        public Category(int iD, string? name, string? description, int depart, int priority, string parentType)
        {
            ID          = iD;
            Name        = name;
            Description = description;
            Depart      = depart;
            Priority    = priority;
            ParentType  = parentType;
        }
        public Category(int iD, string? name, string? description, int depart, int priority, string parentType, string fields)
        {
            ID          = iD;
            Name        = name;
            Description = description;
            Depart      = depart;
            Priority    = priority;
            ParentType  = parentType;
            try
            {

                Fields     =JsonSerializer.Deserialize<Dictionary<string, string>>(fields);
            }
            catch { }
        }

        public Category()
        {
        }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions {
                WriteIndented = true // Optional: makes the JSON pretty-printed
            });
        }
    }

}
/*
[
  {
    "id": 1,
    "name": "Academic Internal Credit Query",
    "description": "Click here to query an internal academic credit",
    "depart": 8,
    "priority": 0,
    "parentType": "Campus",
    "fields": {
      "SelectedModules": "List<String>"
    }
  },
  {
    "id": 2,
    "name": "Academic Report Request",
    "description": "Click here for an academic record",
    "depart": 8,
    "priority": 0,
    "parentType": "Campus",
    "fields": {
      "SelectedModules": "List<String>"
    }
  },
  {
    "id": 3,
    "name": "Add or Drop modules",
    "description": "Click here if you would like to amend your contract or modules",
    "depart": 8,
    "priority": 0,
    "parentType": "Campus",
    "fields": {
      "SelectedModules": "List<String>"
    }
  },
  {
    "id": 4,
    "name": "Assessment Query",
    "description": "Click here for an assessment query",
    "depart": 8,
    "priority": 0,
    "parentType": "Campus",
    "fields": {
      "SelectedModules": "List<String>"
    }
  },
  {
    "id": 5,
    "name": "Assessment Support for learning concessions & needs",
    "description": "Click here to apply for assessment support relating to learning concessions and needs",
    "depart": 8,
    "priority": 0,
    "parentType": "Campus",
    "fields": {
      "Concession": "String"
    }
  },
  {
    "id": 6,
    "name": "Blocked Portal or Hold Query",
    "description": "Click here if your student portal is blocked or you have a query on a financial hold",
    "depart": 1,
    "priority": 0,
    "parentType": "Campus",
    "fields": {
      "SelectedModules": "List<String>",
      "QueryType": "String"
    }
  },
  {
    "id": 7,
    "name": "Cancellations and Appeals",
    "description": "Click here should you need to request a cancellation or make a cancellation appeal",
    "depart": 8,
    "priority": 0,
    "parentType": "Campus",
    "fields": {
      "SelectedModules": "List<String>"
    }
  },
  {
    "id": 8,
    "name": "Contract / Registration Query",
    "description": "Click here if you would like to amend your registration contract or modules",
    "depart": 2,
    "priority": 0,
    "parentType": "Campus",
    "fields": {
      "SelectedModules": "List<String>"
    }
  },
  {
    "id": 9,
    "name": "Counsellor / Student Wellness Request",
    "description": "Click here to request an appointment with our registered Counsellor",
    "depart": 2,
    "priority": 0,
    "parentType": "Campus",
    "fields": {}
  },
  {
    "id": 10,
    "name": "Exam Centre Amendment / Query",
    "description": "Click here to make an exam centre request or amendment",
    "depart": 8,
    "priority": 0,
    "parentType": "Campus",
    "fields": {
      "SelectedModules": "List<String>"
    }
  },
  {
    "id": 11,
    "name": "Finance Query",
    "description": "Click here if you have a fee or finance query",
    "depart": 9,
    "priority": 0,
    "parentType": "Campus",
    "fields": {
      "SelectedModules": "List<String>",
      "QueryType": "String"
    }
  },
  {
    "id": 12,
    "name": "Graduation Query",
    "description": "Click here if you have a graduation query",
    "depart": 8,
    "priority": 0,
    "parentType": "Campus",
    "fields": {
      "SelectedModules": "List<String>"
    }
  },
  {
    "id": 13,
    "name": "Lecturer Query",
    "description": "Click here for a lecturer query",
    "depart": 8,
    "priority": 0,
    "parentType": "Campus",
    "fields": {
      "SelectedModules": "List<String>"
    }
  },
  {
    "id": 14,
    "name": "Librarian / Information Specialist Support",
    "description": "Click here for Librarian/Information Specialist support",
    "depart": 8,
    "priority": 0,
    "parentType": "Campus",
    "fields": {
      "SelectedModules": "List<String>",
      "QueryType": "String"
    }
  },
  {
    "id": 15,
    "name": "Policy Query",
    "description": "Click here if you have an IIE policy query",
    "depart": 1,
    "priority": 0,
    "parentType": "Campus",
    "fields": {
      "SelectedModules": "List<String>"
    }
  },
  {
    "id": 16,
    "name": "Programme Assessment Schedule (PAS) Query",
    "description": "Click here if you would like to query your Programme Assessment Schedule (PAS)",
    "depart": 8,
    "priority": 0,
    "parentType": "Campus",
    "fields": {
      "SelectedModules": "List<String>"
    }
  },
  {
    "id": 17,
    "name": "Request an Appointment",
    "description": "Click here to request an appointment with one of our support staff",
    "depart": 1,
    "priority": 0,
    "parentType": "Campus",
    "fields": {}
  },
  {
    "id": 18,
    "name": "Results Query",
    "description": "Click here if you have a results query",
    "depart": 8,
    "priority": 0,
    "parentType": "Campus",
    "fields": {
      "SelectedModules": "List<String>"
    }
  },
  {
    "id": 19,
    "name": "Student Systems Query",
    "description": "Click here if you have a student portal, Intranet, connect email address or general IT query",
    "depart": 1,
    "priority": 0,
    "parentType": "Campus",
    "fields": {
      "SelectedModules": "List<String>",
      "QueryType": "String"
    }
  },
  {
    "id": 20,
    "name": "Student Visa / Study Visa Letter / Letter of Conduct",
    "description": "Click here if you have a study visa query, require a study visa letter or require a Letter of Conduct",
    "depart": 1,
    "priority": 0,
    "parentType": "Campus",
    "fields": {
      "SelectedModules": "List<String>"
    }
  },
  {
    "id": 21,
    "name": "Timetable Query",
    "description": "Click here if you have a timetable query",
    "depart": 8,
    "priority": 0,
    "parentType": "Campus",
    "fields": {}
  },
  {
    "id": 22,
    "name": "Transfer request / Query",
    "description": "Click here if you need to request a brand or campus transfer or have a transfer query",
    "depart": 1,
    "priority": 0,
    "parentType": "Campus",
    "fields": {
      "SelectedModules": "List<String>"
    }
  },
  {
    "id": 23,
    "name": "Wize Books Query",
    "description": "Click here if you have a Wize Books query",
    "depart": 8,
    "priority": 0,
    "parentType": "Campus",
    "fields": {
      "SelectedModules": "List<String>"
    }
  },
  {
    "id": 24,
    "name": "Writing Centre / Research Support",
    "description": "Click here for Writing Centre/ Research support",
    "depart": 8,
    "priority": 0,
    "parentType": "Campus",
    "fields": {
      "SelectedModules": "List<String>",
      "QueryType": "String"
    }
  },
  {
    "id": 25,
    "name": "Application for a Discontinuation Assessment",
    "description": "Click here if you would like to apply for a Discontinuation Assessment",
    "depart": 8,
    "priority": 0,
    "parentType": "Student",
    "fields": {
      "LastRegistrationYear": "String",
      "AssessmentDate": "String",
      "AssessmentType": "String",
      "SelectedModules": "List<String>"
    }
  },
  {
    "id": 26,
    "name": "Application for Deans exam",
    "description": "Click here if you would like to apply for a Deans exam",
    "depart": 8,
    "priority": 0,
    "parentType": "Student",
    "fields": {
      "LastRegistrationYear": "String",
      "AssessmentDate": "String",
      "AssessmentType": "String",
      "SelectedModules": "List<String>"
    }
  },
  {
    "id": 27,
    "name": "Application for Graduation confirmation or Syllabus Request",
    "description": "Click here if you would like to apply for qualification verification or request your syllabus",
    "depart": 8,
    "priority": 0,
    "parentType": "Student",
    "fields": {}
  },
  {
    "id": 28,
    "name": "Assessment Appeals",
    "description": "Click here if you would like to submit an application for an Assessment Appeal",
    "depart": 8,
    "priority": 0,
    "parentType": "Student",
    "fields": {
      "AppealType": "String",
      "Reason": "String",
      "AssessmentDate": "String",
      "AssessmentType": "String",
      "AssessmentBeingAppealed": "String",
      "SelectedModules": "List<String>"
    }
  },
  {
    "id": 29,
    "name": "Condoned absence for lecture attendance requirement for Education students",
    "description": "Click here if you would like to apply for condoned absence for lecture attendance (applicable to Education students only)",
    "depart": 8,
    "priority": 0,
    "parentType": "Student",
    "fields": {
      "SelectedModules": "List<String>",
      "ReasonForAppeal": "String"
    }
  },
  {
    "id": 30,
    "name": "Deferred assessment",
    "description": "Click here if you would like to apply for a Deferred assessment",
    "depart": 8,
    "priority": 0,
    "parentType": "Student",
    "fields": {
      "ReasonForAppeal": "String",
      "PreviousReasonForAppeal": "String",
      "MissedDate": "String",
      "AssessmentType": "String",
      "ReplacedAssessment": "String",
      "ReplacementDate": "String",
      "SelectedModules": "List<String>"
    }
  },
  {
    "id": 31,
    "name": "Disciplinary Appeals",
    "description": "Click here if you would like to submit an appeal to a disciplinary outcome",
    "depart": 1,
    "priority": 0,
    "parentType": "Student",
    "fields": {
      "SelectedModules": "List<String>",
      "DisciplinaryAppealType": "String"
    }
  },
  {
    "id": 32,
    "name": "Extension of Qualification Completion Time",
    "description": "Click here if you would like to apply for an extension of qualification completion time. This is not for extensions on assessment submissions",
    "depart": 8,
    "priority": 0,
    "parentType": "Student",
    "fields": {
      "FirstYearOfRegistration": "String"
    }
  },
  {
    "id": 33,
    "name": "External Credit",
    "description": "Click here if you would like to apply for external credits",
    "depart": 8,
    "priority": 0,
    "parentType": "Student"
  },
  {
    "id": 34,
    "name": "External Dual Registration",
    "description": "Click here if you would like to apply for dual registration",
    "depart": 1,
    "priority": 0,
    "parentType": "Student",
    "fields": {}
  },
  {
    "id": 35,
    "name": "Increased Credit Load",
    "description": "Click here if you would like to apply for an increase in your credit load",
    "depart": 8,
    "priority": 0,
    "parentType": "Student",
    "fields": {
      "SelectedModules": "List<String>"
    }
  },
  {
    "id": 36,
    "name": "Internal Credits",
    "description": "Click here if you would like to apply for internal credits",
    "depart": 8,
    "priority": 0,
    "parentType": "Student",
    "fields": {
      "YearCompleted": "String",
      "SelectedModules": "List<String>"
    }
  },
  {
    "id": 37,
    "name": "Module Exemption",
    "description": "Click here if you would like to apply for an exemption of a module(s)",
    "depart": 8,
    "priority": 0,
    "parentType": "Student"
  },
  {
    "id": 38,
    "name": "Request for Certificate Reprint",
    "description": "Click here if you would like to request a certificate reprint",
    "depart": 1,
    "priority": 0,
    "parentType": "Student",
    "fields": {}
  },
  {
    "id": 39,
    "name": "Request for Uncollected Certificates",
    "description": "Click here if you would like to request an uncollected certificate",
    "depart": 1,
    "priority": 0,
    "parentType": "Student",
    "fields": {}
  },
  {
    "id": 40,
    "name": "Safety, Student Conduct and Discipline queries",
    "description": "Click here if you have queries relating to safety, conduct or discipline",
    "depart": 1,
    "priority": 0,
    "parentType": "Student",
    "fields": {}
  },
  {
    "id": 41,
    "name": "Study Route Request",
    "description": "Click here if you would like to request a study route or exit route",
    "depart": 1,
    "priority": 0,
    "parentType": "Student",
    "fields": {
      "SelectedModules": "List<String>"
    }
  },
  {
    "id": 42,
    "name": "Study Skills and Support",
    "description": "Click here for study skills and support",
    "depart": 1,
    "priority": 0,
    "parentType": "Student",
    "fields": {}
  },
  {
    "id": 43,
    "name": "Time Extension Request for Coursework",
    "description": "Click here if you need to apply for a time extension on coursework",
    "depart": 1,
    "priority": 0,
    "parentType": "Student",
    "fields": {}
  },
  {
    "id": 44,
    "name": "Transfer of Credits from other Qualifications",
    "description": "Click here if you would like to apply for a transfer of credits from another qualification",
    "depart": 1,
    "priority": 0,
    "parentType": "Student",
    "fields": {
      "SelectedModules": "List<String>"
    }
  },
  {
    "id": 45,
    "name": "Transfer to another Programme / Institution",
    "description": "Click here if you need to apply for a transfer to another programme or institution",
    "depart": 1,
    "priority": 0,
    "parentType": "Student",
    "fields": {}
  },
  {
    "id": 46,
    "name": "Vary Assessment Dates",
    "description": "Click here if you need to apply for a variation of assessment dates",
    "depart": 8,
    "priority": 0,
    "parentType": "Student",
    "fields": {
      "SelectedModules": "List<String>"
    }
  },
  {
    "id": 47,
    "name": "Withdraw / Cancel Registration",
    "description": "Click here if you would like to withdraw or cancel your registration",
    "depart": 1,
    "priority": 0,
    "parentType": "Student",
    "fields": {}
  }
]
*/