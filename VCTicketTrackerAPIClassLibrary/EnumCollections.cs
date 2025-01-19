using System.Text.Json.Serialization;

namespace VCTicketTrackerAPIClassLibrary
{
    public class EnumList
    {

        public int ID { get; set; }
        public string EnumName { get; set; }

        public record EnumItem(int EnumID, string Name, string Value);
    }



    [JsonConverter(typeof(JsonStringEnumConverter<UserTypeAbv>))]
    public enum UserTypeAbv
    {
        // Order is also Priv Level
        SA,
        AD,
        SM,
        GU,
        LR,
        ST,
    }

    [JsonConverter(typeof(JsonStringEnumConverter<Status>))]
    public enum Status
    {
        Created,
        Unassgined,
        WaitingForFeedback,
        WaitingForStudent,
        WaitingForLecturer,
        WaitingForSupport,
        WithExternal,
        WatingForExternal,
        WithStudent,
        WithLecturer,
        WithSupport,
        Resolved,
    }
    public enum AssessmentSupportConcession
    {
        ChronicPhysicalMentalConditions,
        ComorbidityDuringPandemics,
        StandardizedExamConditions
    }

    public enum BlockedPortalHoldQuery
    {
        OutstandingFees,
        OutstandingDocuments
    }

    public enum FinanceQuery
    {
        AdhocFeeQuery,
        AmendmentToPaymentDetails,
        BillOfCostsInvoiceRequest,
        BursaryRequestOrQuery,
        DebitOrderQuery,
        OverdueAccountQuery,
        PaymentArrangementSpecialArrangement,
        Refund,
        StatementBalanceQuery,
        SuspensionHandoverAccountInArrearsQuery,
        SubmitProofOfPayment
    }

    public enum LibrarianSupport
    {
        ReferencingSupport,
        ResearchSupport,
        TextbookOrResourceQuery
    }

    public enum StudentSystemsQuery
    {
        AcademicMaterialQuery,
        AccessToMaterialAssessments,
        AzureLabServices,
        LearnQuery,
        PasswordQuery,
        StudentIntranetQuery,
        StudentPortalQuery,
        OnlineAssessmentPlatformQuery,
        InvigilatorAppQuery
    }

    public enum WritingCentreSupport
    {
        // No content provided, add values as needed
    }

    public enum AssessmentType
    {
        Exam,
        OralExaminations,
        PracticalExaminations
    }

    public enum AssessmentAppeal
    {
        AccessToExamScript,
        AssessmentRemark,
        AssessmentResults,
        DeniedConcession,
        UnfairAssessmentProcess
    }

    public enum UserAssessmentType
    {
        Assignment,
        EducationLectureAttendance,
        Exam,
        OralTests,
        OralExaminations,
        POE,
        PracticalAssignments,
        PracticalExaminations,
        Practicums,
        Presentations,
        Projects,
        Simulations,
        Tasks,
        TakeHomeAssessments,
        Test,
        TeachingExperience
    }

    public enum DisciplinaryAppeal
    {
        DisciplinaryAppeals,
        IntellectualIntegrityAppeal
    }

    public enum ModuleExemption
    {
        CurriculumChangeExemption,
        OtherModuleExemption
    }
    [JsonConverter(typeof(JsonStringEnumConverter<Departments>))]
    public enum Departments
    {
        Administration,
        HumanResources,
        Maintenance,
        Marketing,
        Security,
        Social,
        Admissions,
        Academics,
        Finances,
        Sports,

        SchoolofFinanceandAccounting,
        SchoolofEducation,
        SchoolofInformationTechnology,
        SchoolofLaw,
        SchoolofManagement,
        SchoolofHumanitiesandSocialSciences,
        SchoolofEngineering,
    }
    [JsonConverter(typeof(JsonStringEnumConverter<Categories>))]
    public enum Categories
    {
        AcademicInternalCreditQuery,
        AcademicReportRequest,
        AddorDropmodules,
        AssessmentQuery,
        AssessmentSupportforlearningconcessionsneeds,
        BlockedPortalorHoldQuery,
        CancellationsandAppeals,
        ContractRegistrationQuery,
        CounsellorStudentWellnessRequest,
        ExamCentreAmendmentQuery,
        FinanceQuery,
        GraduationQuery,
        LecturerQuery,
        LibrarianInformationSpecialistSupport,
        PolicyQuery,
        ProgrammeAssessmentScheduleQuery,
        RequestanAppointment,
        ResultsQuery,
        StudentSystemsQuery,
        StudentVisaStudyVisaLetterLetterofConduct,
        TimetableQuery,
        TransferrequestQuery,
        WizeBooksQuery,
        WritingCentreResearchSupport,
        ApplicationforaDiscontinuationAssessment,
        ApplicationforDeansexam,
        ApplicationforGraduationconfirmationorSyllabusRequest,
        AssessmentAppeals,
        CondonedabsenceforlectureattendancerequirementforEducationstudents,
        Deferredassessment,
        DisciplinaryAppeals,
        ExtensionofQualificationCompletionTime,
        ExternalCredit,
        ExternalDualRegistration,
        IncreasedCreditLoad,
        InternalCredits,
        ModuleExemption,
        RequestforCertificateReprint,
        RequestforUncollectedCertificates,
        Safety,
        StudentConductandDisciplinequeries,
        RequesttoRegisterforNonQualificationPurposeModule,
        TranscriptRequest
    }
    //         public static int[] Co = {71637,84706,88606,88609,90905,93729,94119,94696,96408,97235,97600,97601,98032,99284,101647,104532,105032,105123,109005,111287,112899,117788,117914};
}




