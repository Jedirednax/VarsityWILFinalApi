--USE master;
--DROP DATABASE VCTicketTracker;
--CREATE DATABASE VCTicketTracker;
-- USE VCTicketTracker;
-- DELETE TABLE StaffModules;
-- DELETE TABLE StudentModules;
-- DELETE TABLE UserTokens;
-- DELETE TABLE UserRoles;
-- DELETE TABLE Logins;
-- -- DELETE TABLE EnumList;
-- DELETE TABLE Category;
-- DELETE TABLE Staff;
-- DELETE TABLE Students;
-- -- DELETE TABLE Course;
-- --DELETE TABLE Department;
-- --DELETE TABLE CourseModules;
-- --DELETE TABLE Modules;
-- DELETE TABLE FAQ;
-- DELETE TABLE Ticket;
-- DELETE TABLE TicketEvents;
-- DELETE TABLE Users;
-- --DELETE TABLE StaffTypes;



CREATE TABLE [dbo].[EnumList] (
    [ID]                     INT            PRIMARY KEY IDENTITY(1,1),  
    [EnumName]               VARCHAR(50)    NOT NULL,     
    [EnumID]                 INT            NOT NULL,               
    [Name]                   VARCHAR(100)   NOT NULL,        
    [Value]                  VARCHAR(100)   NOT NULL        
);

CREATE TABLE [dbo].[Department] (
    [DepartmentID]           INT            IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [DepartmentName]         VARCHAR(63)    NULL UNIQUE
);

CREATE TABLE [dbo].[Category] (
    [CategoryID]             INT            IDENTITY (1, 1) NOT NULL,
    [CategoryName]           VARCHAR(255)   NULL,
    [CategoryPriority]       INT            NULL,
    [CategoryDepartment]     INT            NULL,
    [SubType]                VARCHAR(63)    NULL,
    [Description]            VARCHAR(MAX)   NULL,
    [CategoryFields]         VARCHAR(MAX)   NULL,
    PRIMARY KEY CLUSTERED ([CategoryID] ASC),
    CONSTRAINT [FK_CategoryDepartment] FOREIGN KEY ([CategoryDepartment]) REFERENCES [dbo].[Department] ([DepartmentID])
);

CREATE TABLE [dbo].[StaffTypes] (
    [Id]                     BIGINT         IDENTITY(1,1),
    [Name]                   VARCHAR(64)    NOT NULL,
    [NormalizedName]         VARCHAR(64)    NOT NULL,
    [ConcurrencyStamp]       VARCHAR(256)   NULL,
    [LimitPriv]              VARCHAR(7)     NULL,
    PRIMARY KEY ([Id]),
    UNIQUE ([NormalizedName]) 
);

CREATE TABLE [dbo].[Course] (
    [CourseID]               INT            NOT NULL,
    [CourseCode]             VARCHAR(15)    NULL,
    [CourseName]             VARCHAR(127)   NOT NULL,
    [Years]                  INT            NULL,
    [Nqf]                    INT            NULL,
    [Credits]                INT            NULL,
    [School]                 VARCHAR(64)    NULL,
    PRIMARY KEY CLUSTERED ([CourseID] ASC)
);

CREATE TABLE [dbo].[Modules] (
    [ModuleCode]             VARCHAR(15)  NOT NULL,
    [ModuleName]             VARCHAR(255) NULL,
    PRIMARY KEY CLUSTERED ([ModuleCode] ASC)
);

CREATE TABLE [dbo].[CourseModules] (
    [cmID]                   INT            IDENTITY (1, 1) NOT NULL,
    [ModuleCode]             VARCHAR(15)    NULL,
    [CourseID]               INT            NULL,
    [NQFLevel]               INT            NULL,
    [ModuleCredits]          INT            NULL,
    [Semester]               INT            NULL,
    [Years]                  INT            NULL,
    PRIMARY KEY CLUSTERED ([cmID] ASC),
    CONSTRAINT [FK_CourseModules] FOREIGN KEY ([ModuleCode]) REFERENCES [dbo].[Modules] ([ModuleCode]),
    CONSTRAINT [FK_ModuleCourses] FOREIGN KEY ([CourseID]) REFERENCES [dbo].[Course] ([CourseID])
);

CREATE TABLE [dbo].[Users] (
    [UserID]                 BIGINT         IDENTITY (1, 1) NOT NULL,
    [FirstName]              VARCHAR(64)    NOT NULL,
    [LastName]               VARCHAR(64)    NOT NULL,
    [Username]               VARCHAR(64)    NOT NULL UNIQUE,
    [NormalizedUserName]     VARCHAR(64)    NOT NULL,			
    [Email]                  VARCHAR(256)   NOT NULL UNIQUE,	
    [NormalizedEmail]        VARCHAR(256)   NOT NULL,			
    [EmailConfirmed]         BIT            NOT NULL DEFAULT(0),         
    [PasswordHash]           VARCHAR(256)   NOT NULL,          
    [SecurityStamp]          VARCHAR(256)   NOT NULL,          
    [ConcurrencyStamp]       VARCHAR(256)   NULL,              
    [UserType]               VARCHAR(31)    NOT NULL,
    [AuthenticationType]     VARCHAR(64)    NULL,
    [IsAuthenticated]        BIT            NOT NULL DEFAULT(0),
    [FcmToken]               VARCHAR(256)   NOT NULL,			
    PRIMARY KEY CLUSTERED ([UserID] ASC)
);

CREATE TABLE [dbo].[Logins] (
    [LoginProvider]          VARCHAR(256)   NOT NULL,
    [ProviderKey]            VARCHAR(256)   NOT NULL,
    [UserId]                 BIGINT         NOT NULL,
    PRIMARY KEY ([LoginProvider], [ProviderKey]),
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([UserID]) ON DELETE CASCADE
);

CREATE TABLE [dbo].[UserTokens] (
    [TokenId]                BIGINT         NOT NULL,
    [UserId]                 BIGINT         NOT NULL,
    [TokenType]              VARCHAR(255)   NULL,
    [Token]                  VARCHAR(MAX)   NULL,
    [TokenActive]            BIT            NULL,
    PRIMARY KEY ([TokenId]),
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([UserID]) ON DELETE CASCADE
);

CREATE TABLE [dbo].[UserRoles] (
    [UserId]                 BIGINT         NOT NULL,                  
    [RoleId]                 BIGINT         NOT NULL,                  
    CONSTRAINT [FK_UserRoles_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([UserID]),
    CONSTRAINT [FK_UserRoles_StaffTypes] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[StaffTypes]([Id]),
    PRIMARY KEY ([UserId], [RoleId])
);

CREATE TABLE [dbo].[Staff] (
    [StaffID]                BIGINT         NOT NULL,
    [UserID]                 BIGINT         NOT NULL,
    [DepartmentID]           INT            NULL, 
    [Campus]                 VARCHAR(31)    NULL,
    [StaffType]              VARCHAR(64)    NULL,
    [SerSys]                 VARCHAR(32)    NULL,
    [CourseID]               INT            NULL,
    [NumberOfTickets]        INT            NULL,
    PRIMARY KEY CLUSTERED ([StaffID] ASC),
    CONSTRAINT [FK_StaffDepartment] FOREIGN KEY ([DepartmentID]) REFERENCES [dbo].[Department] ([DepartmentID]),
    CONSTRAINT [FK_StaffTypes] FOREIGN KEY ([StaffType]) REFERENCES [dbo].[StaffTypes] ([NormalizedName]),
    CONSTRAINT [FK_StaffUser] FOREIGN KEY ([UserID]) REFERENCES [dbo].[Users] ([UserID])
);

CREATE TABLE [dbo].[Students] (
    [STNumber]               VARCHAR(15)    NOT NULL,
    [UserID]                 BIGINT         NULL,
    [CourseID]               INT            NULL,
    [ModeOfDelivery]         VARCHAR(64)    NULL,
    [Campus]                 VARCHAR(31)    NULL,
    [RegistrationType]       VARCHAR(128)   NULL,
    [Qualification]          VARCHAR(128)   NULL,
    [ModGroup]               INT            NULL,
    PRIMARY KEY CLUSTERED ([STNumber] ASC),
    CONSTRAINT [FK_StudentCourse] FOREIGN KEY ([CourseID]) REFERENCES [dbo].[Course] ([CourseID]),
    CONSTRAINT [FK_StudentUser] FOREIGN KEY ([UserID]) REFERENCES [dbo].[Users] ([UserID])
);

CREATE TABLE [dbo].[StaffModules] (
    [AssinedId]              BIGINT         IDENTITY (1, 1) NOT NULL,
    [StaffID]                BIGINT         NULL,
    [ModuleCode]             VARCHAR(15)    NULL,
    [ModGroup]               INT            NULL,
    PRIMARY KEY CLUSTERED ([AssinedId] ASC),
    CONSTRAINT [FK_ModulesStaff] FOREIGN KEY ([StaffID]) REFERENCES [dbo].[Staff] ([StaffID]),
    CONSTRAINT [FK_StaffModules] FOREIGN KEY ([ModuleCode]) REFERENCES [dbo].[Modules] ([ModuleCode])
);

CREATE TABLE [dbo].[StudentModules] (
    [AssinedId]              BIGINT         IDENTITY (1, 1) NOT NULL,
    [StudentID]              VARCHAR(15)    NULL,
    [ModuleCode]             VARCHAR(15)    NULL,
    [ModGroup]               INT            NULL,
    PRIMARY KEY CLUSTERED ([AssinedId] ASC),
    CONSTRAINT [FK_ModuleStudents] FOREIGN KEY ([StudentID]) REFERENCES [dbo].[Students] ([STNumber]),
    CONSTRAINT [FK_StudentModules] FOREIGN KEY ([ModuleCode]) REFERENCES [dbo].[Modules] ([ModuleCode])
);

CREATE TABLE [dbo].[Ticket] (
    [TicketID]               BIGINT         IDENTITY (1, 1) NOT NULL,
    [TicketCreatedBy]        INT            NULL,
    [AssignedTo]             BIGINT         NULL,
    [TicketTitle]            VARCHAR(63)    NULL,
    [TicketStatus]           INT            NULL,
    [TicketDescription]      VARCHAR(MAX)   NULL,
    [TicketDocumentURLS]     VARCHAR(MAX)   NULL,
    [TicketCreationDate]     DATETIME2(7)   NULL,
    [TicketResolvedDate]     DATETIME2(7)   NULL,
    [TicketCategoryID]       INT            NULL,
    [CategoryInformation]    VARCHAR(MAX)   NULL,
    PRIMARY KEY CLUSTERED ([TicketID] ASC),
    CONSTRAINT [FK_TicketCategory] FOREIGN KEY ([TicketCategoryID]) REFERENCES [dbo].[Category] ([CategoryID])
);

CREATE TABLE [dbo].[TicketEvents] (
    [TicketEventID]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [TicketID]               BIGINT         NOT NULL,
    [DateOfEvent]            DATETIME2(7)   NULL,
    [UpdatedBy]              INT            NULL,
    [StatusSetFrom]          INT            NULL,
    [StatusSetTo]            INT            NULL,
    [EventType]              VARCHAR(15)    NULL,
    [EventComments]          VARCHAR(MAX)   NULL,
    PRIMARY KEY CLUSTERED ([TicketEventID] ASC, [TicketID] ASC),
    CONSTRAINT [FK_TickteEvent] FOREIGN KEY ([TicketID]) REFERENCES [dbo].[Ticket] ([TicketID])
);

CREATE TABLE [dbo].[Faq] (
    [QuestionID]             BIGINT         NOT NULL Identity(1,1),
    [Question]               VARCHAR(255)   NULL,
    [Answer]                 VARCHAR(MAX)   NULL,
    [LinkedDocument]        VARCHAR(MAX)   NULL,
    [Topic]                  VARCHAR(255)   NULL,
    [Rating]                 TINYINT        NULL,
    PRIMARY KEY CLUSTERED ([QuestionID] ASC)
);
----------------------------------------------------------------------------------------------------
/* Example Faqs From chat */
INSERT INTO [dbo].[Faq] (Question, Answer, LinkedDocument, Topic, Rating) 
VALUES ('How do I reset my password?', 'Go to the login page and click "Forgot Password" to reset it.', 'path/to/reset-password', 'Account', 5);

INSERT INTO [dbo].[Faq] (Question, Answer, LinkedDocument, Topic, Rating) 
VALUES ('How can I contact customer support?', 'You can reach support via chat, email, or phone.', 'path/to/contact-support', 'Support', 4);

INSERT INTO [dbo].[Faq] (Question, Answer, LinkedDocument, Topic, Rating) 
VALUES ('How do I update my profile information?', 'Log in, go to "Account Settings," and select "Edit Profile."', 'path/to/update-profile', 'Account', 4);

INSERT INTO [dbo].[Faq] (Question, Answer, LinkedDocument, Topic, Rating) 
VALUES ('Why am I having trouble logging in?', 'Ensure you are entering the correct email and password or use "Forgot Password" to reset.', 'path/to/login-issues', 'Login', 3);

INSERT INTO [dbo].[Faq] (Question, Answer, LinkedDocument, Topic, Rating) 
VALUES ('How can I track my service request status?', 'After logging in, go to "My Requests" to check the status.', 'path/to/request-status', 'Service Requests', 4);

INSERT INTO [dbo].[Faq] (Question, Answer, LinkedDocument, Topic, Rating) 
VALUES ('What should I do if I encounter an error?', 'Take a screenshot of the error and contact support with details.', 'path/to/error-support', 'Support', 5);

INSERT INTO [dbo].[Faq] (Question, Answer, LinkedDocument, Topic, Rating) 
VALUES ('How do I access the user guides and tutorials?', 'Visit the "Help" section on the website for guides and tutorials.', 'path/to/guides', 'Guides', 3);

INSERT INTO [dbo].[Faq] (Question, Answer, LinkedDocument, Topic, Rating) 
VALUES ('How do I close my account?', 'Please contact support to assist with closing your account.', 'path/to/close-account', 'Account', 2);

----------------------------------------------------------------------------------------------------
/* Status */
INSERT INTO [dbo].[EnumList] ([EnumName], EnumID, Name, Value) VALUES 
/* 1   */ ('Status',                              1, 'Created',            'Created'),
/* 2   */ ('Status',                              2, 'Unassigned',         'Unassigned'),
/* 3   */ ('Status',                              3, 'WaitingForFeedback', 'Waiting For Feedback'),
/* 4   */ ('Status',                              4, 'WaitingForStudent',  'Waiting For Student'),
/* 5   */ ('Status',                              5, 'WaitingForLecturer', 'Waiting For Lecturer'),
/* 6   */ ('Status',                              6, 'WaitingForSupport',  'Waiting For Support'),
/* 7   */ ('Status',                              7, 'WithExternal',       'With External'),
/* 8   */ ('Status',                              8, 'WaitingForExternal', 'Waiting For External'),
/* 9   */ ('Status',                              9, 'WithStudent',        'With Student'),
/* 10  */ ('Status',                             10, 'WithLecturer',       'With Lecturer'),
/* 11  */ ('Status',                             11, 'WithSupport',        'With Support'),
/* 12  */ ('Status',                             12, 'Resolved',           'Resolved');
----------------------------------------------------------------------------------------------------
/* AssessmentSupportConcession */
INSERT INTO [dbo].[EnumList] (EnumName, EnumID, Name, Value) VALUES 
/* 1   */ ('AssessmentSupportConcession',       1, 'ChronicPhysicalMentalConditions', 'Chronic Physical Mental Conditions'),
/* 2   */ ('AssessmentSupportConcession',       2, 'ComorbidityDuringPandemics',      'Comorbidity During Pandemics'),
/* 3   */ ('AssessmentSupportConcession',       3, 'StandardizedExamConditions',      'Standardized Exam Conditions');
----------------------------------------------------------------------------------------------------
/* BlockedPortalHoldQuery */
INSERT INTO [dbo].[EnumList] (EnumName, EnumID, Name, Value) VALUES 
/* 1   */ ('BlockedPortalHoldQuery',            1, 'OutstandingFees',                 'Outstanding Fees'),
/* 2   */ ('BlockedPortalHoldQuery',            2, 'OutstandingDocuments',            'Outstanding Documents');
----------------------------------------------------------------------------------------------------
/* FinanceQuery */
INSERT INTO [dbo].[EnumList] (EnumName, EnumID, Name, Value) VALUES 
/* 1   */ ('FinanceQuery',                       1, 'AdhocFeeQuery',                           'Adhoc Fee Query'),
/* 2   */ ('FinanceQuery',                       2, 'AmendmentToPaymentDetails',               'Amendment To Payment Details'),
/* 3   */ ('FinanceQuery',                       3, 'BillOfCostsInvoiceRequest',               'Bill Of Costs Invoice Request'),
/* 4   */ ('FinanceQuery',                       4, 'BursaryRequestOrQuery',                   'Bursary Request Or Query'),
/* 5   */ ('FinanceQuery',                       5, 'DebitOrderQuery',                         'Debit Order Query'),
/* 6   */ ('FinanceQuery',                       6, 'OverdueAccountQuery',                     'Overdue Account Query'),
/* 7   */ ('FinanceQuery',                       7, 'PaymentArrangementSpecialArrangement',    'Payment Arrangement Special Arrangement'),
/* 8   */ ('FinanceQuery',                       8, 'Refund',                                  'Refund'),
/* 9   */ ('FinanceQuery',                       9, 'StatementBalanceQuery',                   'Statement Balance Query'),
/* 10  */ ('FinanceQuery',                      10, 'SuspensionHandoverAccountInArrearsQuery', 'Suspension Handover Account In Arrears Query'),
/* 11  */ ('FinanceQuery',                      11, 'SubmitProofOfPayment',                    'Submit Proof Of Payment');
----------------------------------------------------------------------------------------------------
/* LibrarianSupport */
INSERT INTO [dbo].[EnumList] (EnumName, EnumID, Name, Value) VALUES 
/* 1   */ ('LibrarianSupport',                   1, 'ReferencingSupport',           'Referencing Support'),
/* 2   */ ('LibrarianSupport',                   2, 'ResearchSupport',              'Research Support'),
/* 3   */ ('LibrarianSupport',                   3, 'TextbookOrResourceQuery',      'Textbook Or Resource Query');
----------------------------------------------------------------------------------------------------
/* StudentSystemsQuery */
INSERT INTO [dbo].[EnumList] (EnumName, EnumID, Name, Value) VALUES 
/* 1   */ ('StudentSystemsQuery',                1, 'AcademicMaterialQuery',         'Academic Material Query'),
/* 2   */ ('StudentSystemsQuery',                2, 'AccessToMaterialAssessments',   'Access To Material Assessments'),
/* 3   */ ('StudentSystemsQuery',                3, 'AzureLabServices',              'Azure Lab Services'),
/* 4   */ ('StudentSystemsQuery',                4, 'LearnQuery',                    'Learn Query'),
/* 5   */ ('StudentSystemsQuery',                5, 'PasswordQuery',                 'Password Query'),
/* 6   */ ('StudentSystemsQuery',                6, 'StudentIntranetQuery',          'Student Intranet Query'),
/* 7   */ ('StudentSystemsQuery',                7, 'StudentPortalQuery',            'Student Portal Query'),
/* 8   */ ('StudentSystemsQuery',                8, 'OnlineAssessmentPlatformQuery', 'Online Assessment Platform Query'),
/* 9   */ ('StudentSystemsQuery',                9, 'InvigilatorAppQuery',           'Invigilator App Query');
----------------------------------------------------------------------------------------------------
/* AssessmentType */
INSERT INTO [dbo].[EnumList] (EnumName, EnumID, Name, Value) VALUES 
/* 1   */ ('AssessmentType',                    1, 'Exam',                          'Exam'),
/* 2   */ ('AssessmentType',                    2, 'OralExaminations',              'Oral Examinations'),
/* 3   */ ('AssessmentType',                    3, 'PracticalExaminations',         'Practical Examinations');
----------------------------------------------------------------------------------------------------
/* AssessmentAppeal */
INSERT INTO [dbo].[EnumList] (EnumName, EnumID, Name, Value) VALUES 
/* 1   */ ('AssessmentAppeal',                  1, 'AccessToExamScript',            'Access To Exam Script'),
/* 2   */ ('AssessmentAppeal',                  2, 'AssessmentRemark',              'Assessment Remark'),
/* 3   */ ('AssessmentAppeal',                  3, 'AssessmentResults',             'Assessment Results'),
/* 4   */ ('AssessmentAppeal',                  4, 'DeniedConcession',              'Denied Concession'),
/* 5   */ ('AssessmentAppeal',                  5, 'UnfairAssessmentProcess',       'Unfair Assessment Process');
----------------------------------------------------------------------------------------------------
/* UserAssessmentType */
INSERT INTO [dbo].[EnumList] (EnumName, EnumID, Name, Value) VALUES 
/* 1   */ ('UserAssessmentType',                1, 'Assignment',                    'Assignment'),
/* 2   */ ('UserAssessmentType',                2, 'EducationLectureAttendance',    'Education Lecture Attendance'),
/* 3   */ ('UserAssessmentType',                3, 'Exam',                          'Exam'),
/* 4   */ ('UserAssessmentType',                4, 'OralTests',                     'Oral Tests'),
/* 5   */ ('UserAssessmentType',                5, 'OralExaminations',              'Oral Examinations'),
/* 6   */ ('UserAssessmentType',                6, 'POE',                           'POE'),
/* 7   */ ('UserAssessmentType',                7, 'PracticalAssignments',          'Practical Assignments'),
/* 8   */ ('UserAssessmentType',                8, 'PracticalExaminations',         'Practical Examinations'),
/* 9   */ ('UserAssessmentType',                9, 'Practicums',                    'Practicums'),
/* 10  */ ('UserAssessmentType',               10, 'Presentations',                 'Presentations'),
/* 11  */ ('UserAssessmentType',               11, 'Projects',                      'Projects'),
/* 12  */ ('UserAssessmentType',               12, 'Simulations',                   'Simulations'),
/* 13  */ ('UserAssessmentType',               13, 'Tasks',                         'Tasks'),
/* 14  */ ('UserAssessmentType',               14, 'TakeHomeAssessments',           'Take Home Assessments'),
/* 15  */ ('UserAssessmentType',               15, 'Test',                          'Test'),
/* 16  */ ('UserAssessmentType',               16, 'TeachingExperience',            'Teaching Experience');
----------------------------------------------------------------------------------------------------
/* DisciplinaryAppeal */
INSERT INTO [dbo].[EnumList] (EnumName, EnumID, Name, Value) VALUES 
/* 1   */ ('DisciplinaryAppeal',                1, 'DisciplinaryAppeals',           'Disciplinary Appeals'),
/* 2   */ ('DisciplinaryAppeal',                2, 'IntellectualIntegrityAppeal',   'Intellectual Integrity Appeal');
----------------------------------------------------------------------------------------------------
/* ModuleExemption */
INSERT INTO [dbo].[EnumList] (EnumName, EnumID, Name, Value) VALUES 
/* 1   */ ('ModuleExemption',                   1, 'CurriculumChangeExemption',     'Curriculum Change Exemption'),
/* 2   */ ('ModuleExemption',                   2, 'OtherModuleExemption',          'Other Module Exemption');
----------------------------------------------------------------------------------------------------
/* Staff Types */
INSERT INTO [dbo].[StaffTypes] ([Name], [NormalizedName], [LimitPriv])VALUES
/* 1   */('ServerAdmin',    'SERVERADMIN',     '0'),
/* 2   */('Administrator',  'ADMINISTRATOR',   '1'),
/* 3   */('SupportMember',  'SUPPORTMEMBER',   '2'),
/* 4   */('Lecturer',       'LECTURER',        '3'),
/* 5   */('Student',        'STUDENT',         '4'),
/* 6   */('Guest',          'GUEST',           '5');
----------------------------------------------------------------------------------------------------
/* Department */
INSERT INTO Department VALUES
/* 1   */('Administration'),
/* 2   */('Human resources'),
/* 3   */('Maintenance'),
/* 4   */('Marketing'),
/* 5   */('Security'),
/* 6   */('Social'),
/* 7   */('Admissions'),
/* 8   */('Academics'),
/* 9   */('Finances'),
/* 10  */('Sports'),
/* 11  */('School of Finance and Accounting'),
/* 12  */('School of Education'),
/* 13  */('School of Information Technology'),
/* 14  */('School of Law'),
/* 15  */('School of Management'),
/* 16  */('School of Humanities and Social Sciences'),
/* 17  */('School of Engineering');
----------------------------------------------------------------------------------------------------
/* Category */
INSERT INTO [dbo].[Category] (CategoryName, CategoryPriority, CategoryDepartment, SubType, Description, CategoryFields) VALUES 
/* 1   */('Academic Internal Credit Query', 4, 8, 'Campus', 
            'Click here to query an internal academic credit', 
            '{"SelectedModules": "List<String>"}'),
/* 2   */('Academic Report Request', 1, 8, 'Campus', 
            'Click here for an academic record', 
            '{"SelectedModules": "List<String>"}'),
/* 3   */('Add or Drop modules', 2, 8, 'Campus', 
            'Click here if you would like to amend your contract or modules', 
            '{"SelectedModules": "List<String>"}'),
/* 4   */('Assessment Query', 1, 8, 'Campus', 
            'Click here for an assessment query', 
            '{"SelectedModules": "List<String>"}'),
/* 5   */('Assessment Support for learning concessions & needs', 2, 8, 'Campus', 
            'Click here to apply for assessment support relating to learning concessions and needs', 
            '{"Concession": "String"}'),
/* 6   */('Blocked Portal or Hold Query', 2, 1, 'Campus', 
            'Click here if your student portal is blocked or you have a query on a financial hold', 
            '{"SelectedModules": "List<String>", "QueryType": "String"}'),
/* 7   */('Cancellations and Appeals', 3, 8, 'Campus', 
            'Click here should you need to request a cancellation or make a cancellation appeal', 
            '{"SelectedModules": "List<String>"}'),
/* 8   */('Contract / Registration Query', 3, 2, 'Campus', 
            'Click here if you would like to amend your registration contract or modules', 
            '{"SelectedModules": "List<String>"}'),
/* 9   */('Counsellor / Student Wellness Request', 1, 2, 'Campus', 
            'Click here to request an appointment with our registered Counsellor', 
            '{}'),
/* 10  */('Exam Centre Amendment / Query', 3, 8, 'Campus', 
            'Click here to make an exam centre request or amendment', 
            '{"SelectedModules": "List<String>"}'),
/* 11  */('Finance Query', 4, 9, 'Campus', 
            'Click here if you have a fee or finance query', 
            '{"SelectedModules": "List<String>", "QueryType": "String"}'),
/* 12  */('Graduation Query', 5, 8, 'Campus', 
            'Click here if you have a graduation query', 
            '{"SelectedModules": "List<String>"}'),
/* 13  */('Lecturer Query', 5, 8, 'Campus', 
            'Click here for a lecturer query', 
            '{"SelectedModules": "List<String>"}'),
/* 14  */('Librarian / Information Specialist Support', 4, 8, 'Campus', 
            'Click here for Librarian/Information Specialist support', 
            '{"SelectedModules": "List<String>", "QueryType": "String"}'),
/* 15  */('Policy Query', 4, 1, 'Campus', 
            'Click here if you have an IIE policy query', 
            '{"SelectedModules": "List<String>"}'),
/* 16  */('Programme Assessment Schedule (PAS) Query', 3, 8, 'Campus', 
            'Click here if you would like to query your Programme Assessment Schedule (PAS)', 
            '{"SelectedModules": "List<String>"}'),
/* 17  */('Request an Appointment', 3, 1, 'Campus', 
            'Click here to request an appointment with one of our support staff', 
            '{}'),
/* 18  */('Results Query', 2, 8, 'Campus', 
            'Click here if you have a results query', 
            '{"SelectedModules": "List<String>"}'),
/* 19  */('Student Systems Query', 3, 1, 'Campus', 
            'Click here if you have a student portal, Intranet, connect email address or general IT query', 
            '{"SelectedModules": "List<String>", "QueryType": "String"}'),
/* 20  */('Student Visa / Study Visa Letter / Letter of Conduct', 4, 1, 'Campus', 
            'Click here if you have a study visa query, require a study visa letter or require a Letter of Conduct', 
            '{"SelectedModules": "List<String>"}'),
/* 21  */('Timetable Query', 5, 8, 'Campus', 
            'Click here if you have a timetable query', 
            '{}'),
/* 22  */('Transfer request / Query', 5, 1, 'Campus', 
            'Click here if you need to request a brand or campus transfer or have a transfer query', 
            '{"SelectedModules": "List<String>"}'),
/* 23  */('Wize Books Query', 4, 8, 'Campus', 
            'Click here if you have a Wize Books query', 
            '{"SelectedModules": "List<String>"}'),
/* 24  */('Writing Centre / Research Support', 4, 8, 'Campus', 
            'Click here for Writing Centre/ Research support', 
            '{"SelectedModules": "List<String>", "QueryType": "String"}'),
/* 25  */('Application for a Discontinuation Assessment', 3, 8, 'Student', 
            'Click here if you would like to apply for a Discontinuation Assessment', 
            '{"LastRegistrationYear": "String", "AssessmentDate": "String", "AssessmentType": "String", "SelectedModules": "List<String>"}'),
/* 26  */('Application for Deans exam', 0, 8, 'Student', 
            'Click here if you would like to apply for a Deans exam', 
            '{"LastRegistrationYear": "String", "AssessmentDate": "String", "AssessmentType": "String", "SelectedModules": "List<String>"}'),
/* 27  */('Application for Graduation confirmation or Syllabus Request', 0, 8, 'Student', 
            'Click here if you would like to apply for qualification verification or request your syllabus', 
            '{}'),
/* 28  */('Assessment Appeals', 3, 8, 'Student', 
            'Click here if you would like to submit an application for an Assessment Appeal', 
            '{"AppealType": "String", "Reason": "String", "AssessmentDate": "String", "AssessmentType": "String", "AssessmentBeingAppealed": "String", "SelectedModules": "List<String>"}'),
/* 29  */('Condoned absence for lecture attendance requirement for Education students', 4, 8, 'Student', 
            'Click here if you would like to apply for condoned absence for lecture attendance (applicable to Education students only)', 
            '{"SelectedModules": "List<String>", "ReasonForAppeal": "String"}'),
/* 30  */('Deferred assessment', 4, 8, 'Student', 
            'Click here if you would like to apply for a Deferred assessment', 
            '{"ReasonForAppeal": "String", "PreviousReasonForAppeal": "String", "MissedDate": "String", "AssessmentType": "String", "ReplacedAssessment": "String", "ReplacementDate": "String", "SelectedModules": "List<String>"}'),
/* 31  */('Disciplinary Appeals', 2, 1, 'Student', 
            'Click here if you would like to submit an appeal to a disciplinary outcome', 
            '{"SelectedModules": "List<String>", "DisciplinaryAppealType": "String"}'),
/* 32  */('Extension of Qualification Completion Time', 3, 8, 'Student', 
            'Click here if you would like to apply for an extension of qualification completion time. This is not for extensions on assessment submissions', 
            '{"FirstYearOfRegistration": "String"}'),
/* 33  */('External Credit', 5, 8, 'Student', 
            'Click here if you would like to apply for external credits', 
            '{"Institution": "String", "SAQUID": 0, "YearCompleted": "String", "SelectedModules": "List<String>"}'),
/* 34  */('External Dual Registration', 5, 1, 'Student', 
            'Click here if you would like to apply for dual registration', 
            '{}'),
/* 35  */('Increased Credit Load', 5, 8, 'Student', 
            'Click here if you would like to apply for an increase in your credit load', 
            '{"SelectedModules": "List<String>"}'),
/* 36  */('Internal Credits', 5, 8, 'Student', 
            'Click here if you would like to apply for internal credits', 
            '{"YearCompleted": "String", "SelectedModules": "List<String>"}'),
/* 37  */('Module Exemption', 3, 8, 'Student', 
            'Click here if you would like to apply for an exemption of a module(s)', 
            '{"SelectedModules": "List<String>", "ExemptionType": "String", "CreditValue": 0, "QualificationValue": 0}'),
/* 38  */('Request for Certificate Reprint', 3, 1, 'Student', 
            'Click here if you would like to request a certificate reprint', 
            '{}'),
/* 39  */('Request for Uncollected Certificates', 3, 1, 'Student', 
            'Click here if you would like to request an uncollected certificate', 
            '{}'),
/* 40  */('Safety, Student Conduct and Discipline queries', 3, 1, 'Student', 
            'Click here if you have queries relating to safety, conduct or discipline', 
            '{}'),
/* 41  */('Study Route Request', 5, 1, 'Student', 
            'Click here if you would like to request a study route or exit route', 
            '{"SelectedModules": "List<String>"}'),
/* 42  */('Study Skills and Support', 4, 1, 'Student', 
            'Click here for study skills and support', 
            '{}'),
/* 43  */('Time Extension Request for Coursework', 3, 1, 'Student', 
            'Click here if you need to apply for a time extension on coursework', 
            '{}'),
/* 44  */('Transfer of Credits from other Qualifications', 5, 1, 'Student', 
            'Click here if you would like to apply for a transfer of credits from another qualification', 
            '{"SelectedModules": "List<String>"}'),
/* 45  */('Transfer to another Programme / Institution', 5, 1, 'Student', 
            'Click here if you need to apply for a transfer to another programme or institution', 
            '{}'),
/* 46  */('Vary Assessment Dates', 3, 8, 'Student', 
            'Click here if you need to apply for a variation of assessment dates', 
            '{"SelectedModules": "List<String>"}'),
/* 47  */('Withdraw / Cancel Registration', 1, 1, 'Student', 
            'Click here if you would like to withdraw or cancel your registration', 
            '{}');
--------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------
/* Courses */
INSERT INTO Course (School, CourseName,  Years, Nqf, Credits, CourseID) VALUES
/* 1   */('School of Finance and Accounting',    'IIE Bachelor of Accounting',                                                   3,    7, 422, 99284),
/* 2   */('School of Finance and Accounting',    'IIE Postgraduate Diploma in Accounting',                                       1,    8, 120, 88609),
/* 3   */('School of Education',                 'IIE Bachelor of Education in Foundation Phase Teaching',                       4,    7, 480, 96408),
/* 4   */('School of Education',                 'IIE Bachelor of Education in Intermediate Phase Teaching',                     4,    7, 511, 97235),
/* 5   */('School of Education',                 'IIE Higher Certificate in Early Childhood Care and Education',                 1,    5, 137, 104532),
/* 6   */('School of Information Technology',    'IIE Bachelor of Computer and Information Sciences in Application Development', 3,    7, 360, 97600),
/* 7   */('School of Information Technology',    'IIE Bachelor of Computer and Information Sciences Honours',                    1,    8, 120, 88606),
/* 8   */('School of Information Technology',    'IIE Higher Certificate in Mobile Application and Web Development',             1,    5, 120, 117914),
/* 9   */('School of Information Technology',    'IIE Postgraduate Diploma in Data Analytics',                                   1,    8, 120, 117788),
/* 10  */('School of Law',                       'IIE Bachelor of Arts in Law',                                                  3,    7, 360, 105123),
/* 11  */('School of Law',                       'IIE Bachelor of Commerce in Law',                                              3,    7, 360, 93729),
/* 12  */('School of Law',                       'IIE Bachelor of Laws',                                                         4,    8, 500, 101647),
/* 13  */('School of Law',                       'Higher Certificate in Legal Studies',                                          1,    5, 120, 94696),
/* 14  */('School of Management',                'IIE Bachelor of Commerce',                                                     3,    7, 360, 84706),
/* 15  */('School of Management',                'IIE Bachelor of Commerce in Entrepreneurship',                                 3,    7, 360, 111287),
/* 16  */('School of Management',                'IIE Bachelor of Commerce Honours in Management',                               1,    8, 120, 97601),
/* 17  */('School of Management',                'IIE Postgraduate Diploma in Management',                                       1,    8, 120, 109005),
/* 18  */('School of Management',                'IIE Higher Certificate in Business Principles and Practice',                   1,    5, 120, 71637),
/* 19  */('School of Humanities and Social Sciences', 'IIE Bachelor of Arts',                                                    3,    7, 360, 94119),
/* 20  */('School of Humanities and Social Sciences', 'IIE Bachelor of Arts Honours in Communication',                           1,    8, 120, 98032),
/* 21  */('School of Humanities and Social Sciences', 'IIE Bachelor of Arts Honours in Psychology',                              1,    8, 120, 105032),
/* 22  */('School of Humanities and Social Sciences', 'IIE Bachelor of Social Science',                                          3,    7, 360, 90905),
/* 23  */('School of Humanities and Social Sciences', 'Higher Certificate in Communication Practices',                           1,    5, 120, 112899);
--------------------------------------------------------------------------------------------------
/* Modules */
/* 1   */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('AAFM8411',    'Applied Accounting and Finance for Managers');
/* 2   */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('AATT6312',    'Accounting for Attorneys');
/* 3   */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('ACBP5121',    'Accounting 1A');
/* 4   */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('ACBP5122',    'Accounting 1B');
/* 5   */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('ACBP6221',    'Accounting 2A');
/* 6   */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('ACBP6222',    'Accounting 2B');
/* 7   */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('ACBP7321',    'Accounting 3A');
/* 8   */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('ACBP7322',    'Accounting 3B');
/* 9   */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('ACSO5112',    'Accounting Software');
/* 10  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('ADDB7311',    'Advanced Databases');
/* 11  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('ADTO8411',    'Advanced Topics in IS Research');
/* 12  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('AFMW7321',    'Africa in the Modern World');
/* 13  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('AFPC7312',    'Assessment in the FP Classroom');
/* 14  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('AINT8412',    'Artificial Intelligence');
/* 15  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('ALRE7311',    'Alternative Dispute Resolution');
/* 16  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('AORP6221',    'Organisational Psychology');
/* 17  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('APCT5121',    'Applied Communication Techniques');
/* 18  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('APDS7311',    'Application Development Security');
/* 19  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('APTC5121',    'Applied Communication Techniques');
/* 20  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('ASME7312',    'Assessment and Measurement');
/* 21  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('AUDI6211',    'Auditing 2A');
/* 22  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('AUDI7319',    'Auditing 3');
/* 23  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('AUDI8419',    'Auditing 4');
/* 24  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('BDMO5212',    'Business Decision Modelling');
/* 25  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('BERS7321',    'Beliefs, Religion and Spirituality');
/* 26  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('BMAD5112',    'Business Management & Administration B');
/* 27  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('BMAD5121',    'Business Management & Administration A');
/* 28  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('BMAN5121',    'Business Management 1');
/* 29  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('BMDE6211',    'Business Management: Digital Entrepreneurship');
/* 30  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('BMNG5121',    'Business Management 1A');
/* 31  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('BMNG5122',    'Business Management 1B');
/* 32  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('BMNG6221',    'Business Management 2A');
/* 33  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('BMNG6222',    'Business Management 2B');
/* 34  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('BMNG7321',    'Business Management 3A');
/* 35  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('BMNG7322',    'Business Management 3B');
/* 36  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('BSNC6222',    'Building Support Networks for Children and Families');
/* 37  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('BSTA6212',    'Business Statistics');
/* 38  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('BUEL6212',    'Business Enterprises Law');
/* 39  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('BUET6212',    'Business Ethics');
/* 40  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('BUSL6222',    'Business Law');
/* 41  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('CBSA7321',    'Criminal Behaviour in South Africa: A Psychosocial Approach');
/* 42  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('CCPR8412',    'Contemporary Communication Practices');
/* 43  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('CDLT6211',    'Child Development');
/* 44  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('CICO5122',    'Crime in Context');
/* 45  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('CIPR8411',    'Civil Procedure');
/* 46  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('CLDV6211',    'Cloud Development A');
/* 47  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('CLDV6212',    'Cloud Development B');
/* 48  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('CLIM7321',    'Climatology');
/* 49  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('COLA5112',    'Communicative Languages: Afrikaans');
/* 50  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('COLN5112',    'Communicative Languages: Northern Sotho');
/* 51  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('COLX5112',    'Communicative Languages: IsiXhosa');
/* 52  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('COLZ5112',    'Communicative Languages: IsiZulu');
/* 53  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('COMI8412',    'Contemporary Management and Innovation');
/* 54  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('COMT8411',    'Communication Theory');
/* 55  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('CONL5112',    'Constitutional Law and Human Rights');
/* 56  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('CONS5112',    'Communicative Languages: Northern Sotho');
/* 57  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('CONT8411',    'Contemporary Management Principles');
/* 58  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('COPO6222',    'Comparative Politics');
/* 59  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('COSC5121',    'Communication Science 1A');
/* 60  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('COSC5132',    'Communication Science 1B: Intercultural Communication');
/* 61  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('COSC6222',    'Communication Science 2B: Persuasive Communication');
/* 62  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('COSC6231',    'Communication Science 2A');
/* 63  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('COSC7321',    'Communication Science 3A');
/* 64  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('COSC7322',    'Communication Science 3B');
/* 65  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('COXH5112',    'Communicative Languages: IsiXhosa');
/* 66  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('COZU5112',    'Communicative Languages: IsiZulu');
/* 67  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('CPSY8111',    'Community Psychology');
/* 68  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('CRIM5111',    'Introduction to Criminology 1A');
/* 69  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('CRIM6211',    'Criminology 2A');
/* 70  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('CRIM7311',    'Criminology 3A');
/* 71  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('CRIM7312',    'Criminology 3B');
/* 72  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('CRMA7312',    'Compliance and Risk Management');
/* 73  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('CRST8412',    'Critical Studies');
/* 74  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('CRUN7112',    'Criminal Procedure');
/* 75  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('DANA8411',    'Data Analytics 1');
/* 76  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('DANA8412',    'Data Analytics 2');
/* 77  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('DASC8412',    'Data Science');
/* 78  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('DSAN6211',    'Systems Analysis and Design');
/* 79  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('DBAS6211',    'Advanced Databases');
/* 80  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('DCIR7322',    'Diplomacy in Contemporary International Relations');
/* 81  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('DEDW6221',    'Development and the Developing World');
/* 82  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('DGMT6212',    'Digital Marketing');
/* 83  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('DIAL5111',    'Digital and Academic Literacies');
/* 84  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('DIAL5112',    'Digital and Academic Literacies');
/* 85  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('DMET5112',    'Digital Marketing for Entrepreneurs');
/* 86  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('DPSY8111',    'Developmental Psychology');
/* 87  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('DTPS5111',    'Design Thinking and Problem-Solving');
/* 88  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('ECDX5112',    'ECD Experience (6 weeks of ECD based work integrated learning)');
/* 89  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('ECLA7321',    'E-Commerce Law');
/* 90  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('ECLD5111',    'Early Childhood Learning and Development');
/* 91  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('ECTL6111',    'Early Childhood Teaching & Learning Environment');
/* 92  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('EDMA7412',    'Educational Management');
/* 93  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('EFCL5112',    'English First & FAL 1A: Children?s Literature');
/* 94  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('EFEL5112',    'English First & FAL 1B: Emergent Language');
/* 95  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('EFFA6212',    'English First and First Additional Language 1');
/* 96  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('EFRP6211',    'English First & FAL 2: Reading and Phonics');
/* 97  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('EFWI7311',    'English First and FAL 3');
/* 98  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('ELEC8411',    'Elective (Choose from approved electives)');
/* 99  */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('EMOW6222',    'Ethics and the Modern World');
/* 100 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('EMTE8412',    'Emerging Technologies');
/* 101 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('ENAS7321',    'Environmental Impact Assessment');
/* 102 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('ENED6122',    'English for Education');
/* 103 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('ENFL6111',    'English for Law');
/* 104 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('ENGL5121',    'English 1A');
/* 105 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('ENGL6122',    'English 1B');
/* 106 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('ENGL6221',    'English 2A');
/* 107 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('ENGL6222',    'English 2B: Postcolonialism');
/* 108 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('ENGL7321',    'English 3A');
/* 109 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('ENGL7322',    'English 3B');
/* 110 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('ENLP5111',    'English Language Practice A');
/* 111 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('ENLP5112',    'English Language Practice B');
/* 112 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('ENRM7322',    'Enterprise Risk Management');
/* 113 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('ENTP6211',    'Entrepreneurship 2A: Ideation');
/* 114 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('ENTP6212',    'Entrepreneurship 2B: Small Business Management');
/* 115 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('ENTP7311',    'Entrepreneurship 3A');
/* 116 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('ENTP7312',    'Entrepreneurship 3B');
/* 117 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('ENTR5112',    'Entrepreneurship 1');
/* 118 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('EPRM6221',    'Environmental Policy and Resource Management');
/* 119 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('ERPR7412',    'Education Research Practice');
/* 120 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('FAFR6121',    'First Additional Language: Afrikaans');
/* 121 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('FALA6211',    'Law of Family');
/* 122 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('FALN6111',    'First Additional Language: Northern Sotho');
/* 123 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('FALX6121',    'First Additional Language: IsiXhosa');
/* 124 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('FALZ6121',    'First Additional Language: IsiZulu');
/* 125 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('FCVP7322',    'Forensic Criminology: Victim and Offender Profiling');
/* 126 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('FIAC5111',    'Financial Accounting 1A');
/* 127 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('FIAC5112',    'Financial Accounting 1B');
/* 128 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('FIAC6211',    'Financial Accounting 2A');
/* 129 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('FIAC6212',    'Financial Accounting 2B');
/* 130 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('FIAC7319',    'Financial Accounting 3');
/* 131 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('FIAC8419',    'Financial Accounting 4');
/* 132 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('FINE6211',    'Financial Management for Entrepreneurs');
/* 133 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('FINM6221',    'Financial Management 2A');
/* 134 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('FINM6222',    'Financial Management 2B');
/* 135 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('FINM7321',    'Financial Management 3A');
/* 136 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('FINM7322',    'Financial Management 3B');
/* 137 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('FMLP5112',    'Facilitating and Managing Literacy Programme');
/* 138 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('FMLS5112',    'Facilitating and Managing Life Skills Programme');
/* 139 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('FMNP5112',    'Facilitating and Managing Numeracy Programme');
/* 140 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('FOED7411',    'Foundations of Education');
/* 141 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('FOMS5112',    'Fundamentals of Media Studies');
/* 142 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('FPIN7321',    'Foreign Policy: Introduction');
/* 143 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('FUIR5122',    'Fundamentals of International Relations');
/* 144 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('FUPS5122',    'Fundamentals of Political Science');
/* 145 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('GBMP8411',    'Global Business Management and Practice');
/* 146 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('GEBV7312',    'Gender Based Violence');
/* 147 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('GECS6221',    'Gender, Culture and Society');
/* 148 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('GISF7222',    'Geographical Information Systems: FOSS Geo-Informatics');
/* 149 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('GISP7322',    'Geographical Information Systems: Planning and Decision Making');
/* 150 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('GOET7312',    'Governance and Ethics');
/* 151 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('GPCL6211',    'General Principles of Criminal Law');
/* 152 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('HAND6212',    'Handwriting');
/* 153 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('HSNU5111',    'Health, Safety and Nutrition');
/* 154 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('IBMA8411',    'International Business Management');
/* 155 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('ICJS5122',    'Introduction to South African Criminal Justice System');
/* 156 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('ICTC6212',    'ITC Integration into the Classroom');
/* 157 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('ICTD8411',    'ICT for Development');
/* 158 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('IDES5122',    'Introduction to Development Studies');
/* 159 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('IEMS5111',    'Introduction to EMS');
/* 160 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('IFAA6211',    'FAL: Afrikaans 1');
/* 161 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('IFAA7412',    'FAL: Afrikaans 3');
/* 162 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('IFAN6211',    'FAL: Northern Sotho 1');
/* 163 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('IFAN6311',    'FAL: Northern Sotho 2');
/* 164 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('IFAN7412',    'FAL: Northern Sotho 3');
/* 165 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('IFAX6211',    'FAL: IsiXhosa 1');
/* 166 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('IFAX6311',    'FAL: IsiXhosa 2');
/* 167 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('IFAX7412',    'FAL: IsiXhosa 3');
/* 168 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('IFAZ6211',    'FAL: IsiZulu 1');
/* 169 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('IFAZ6311',    'FAL: IsiZulu 2');
/* 170 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('IFAZ7412',    'FAL: IsiZulu 3');
/* 171 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('IFFA6311',    'FAL: Afrikaans 2');
/* 172 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('IFFA6312',    'English First and First Additional Language 2');
/* 173 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('IFFA7411',    'English First and First Additional Language 3');
/* 174 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('IHGE5122',    'Introduction to Human Geography');
/* 175 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('IINS5211',    'Introduction to Information Systems');
/* 176 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('IMAD5112',    'Introduction to Mobile Application Development');
/* 177 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('INAC5111',    'Introduction to Accounting 1A');
/* 178 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('INDI6111',    'Indigenous Law');
/* 179 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('INED7211',    'Inclusive Education A');
/* 180 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('INED7212',    'Inclusive Education B');
/* 181 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('INER7411',    'Introduction to Education Research');
/* 182 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('INFL5111',    'Introduction and Foundations to Law');
/* 183 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('INKM7322',    'Information and Knowledge Management');
/* 184 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('INPA6212',    'Instruments of Payment');
/* 185 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('INPCf110',    'Introduction to Personal Computing');
/* 186 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('INPL7322',    'Intellectual Property Law');
/* 187 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('INRL6221',    'Industrial Relations');
/* 188 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('INRP8419',    'Independent Research Project');
/* 189 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('INRS7321',    'Introduction to Research');
/* 190 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('INTA5121',    'International Studies 1');
/* 191 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('INTB5122',    'International Studies 2');
/* 192 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('IPGE5121',    'Introduction to Physical Geography');
/* 193 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('IPMA6212',    'IT Project Management');
/* 194 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('IPRG5111',    'Introduction to Programming Logic');
/* 195 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('IPTL6111',    'IP Teaching and Learning');
/* 196 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('IQTT5111',    'Introduction to Quantitative Thinking and Techniques');
/* 197 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('ISCI6212',    'Selected Contemporary Crime Issues');
/* 198 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('ITPP5112',    'IT Professional Practice');
/* 199 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('ITSA5111',    'Introduction to Scholarship A');
/* 200 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('JRNS6221',    'Journalism 1');
/* 201 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('JRNS7321',    'Journalism 2');
/* 202 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('JUJU8222',    'Jurisprudence and Ethics');
/* 203 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('LACO6221',    'Law of Contract');
/* 204 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('LADE7111',    'Law of Delict');
/* 205 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('LAES5111',    'Law of Enterprise Structures');
/* 206 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('LAEV8411',    'Law of Evidence and Litigation Techniques');
/* 207 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('LAIN7312',    'Law of Insolvency');
/* 208 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('LAPR7311',    'Law of Property');
/* 209 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('LEAP6211',    'Legal and Accounting Principles');
/* 210 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('LEIN5112',    'Legal Interpretation');
/* 211 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('LLAW7321',    'Labour Law');
/* 212 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('LOPF5131',    'Law of Persons');
/* 213 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('LOSA6212',    'Law of Succession');
/* 214 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('LSBK6211',    'Life Skills: Beginning Knowledge ? Natural Science & Technology');
/* 215 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('LSBK6212',    'Life Skills: Beginning Knowledge Social Sciences');
/* 216 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('LSCA5111',    'Life Skills: Early Childhood Art');
/* 217 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('LSMO5111',    'Life Skills: Movement Gr R');
/* 218 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('LSPD6411',    'Life Skills: Drama OR');
/* 219 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('LSPE6411',    'Life Skills: Physical Education');
/* 220 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('LSPM6411',    'Life Skills: Music');
/* 221 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('LSPS6412',    'Life Skills: Personal and Social well-being');
/* 222 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('MAKT5112',    'Introduction to Marketing Theory and Practice');
/* 223 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('MAKT6211',    'Marketing 2A');
/* 224 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('MAKT6212',    'Marketing 2B');
/* 225 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('MAKT7311',    'Marketing 3A');
/* 226 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('MAKT7312',    'Marketing 3B');
/* 227 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('MALE8411',    'Management and Leadership');
/* 228 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('MAPC5112',    'Mathematical Principles for Computer Science');
/* 229 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('MAST5112',    'Mobile App Scripting');
/* 230 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('MCED7311',    'Multicultural Education');
/* 231 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('MDPE5111',    'Management, Development and Professionalism in ECD');
/* 232 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('MEDS6222',    'Medical Sociology');
/* 233 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('MELE6221',    'Media Law and Ethics');
/* 234 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('MEST6222',    'Media Studies 1');
/* 235 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('MEST7322',    'Media Studies 2');
/* 236 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('MFAC6211',    'Management Accounting and Finance 2A');
/* 237 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('MFAC6212',    'Management Accounting and Finance 2B');
/* 238 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('MFAC7319',    'Management Accounting and Finance 3');
/* 239 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('MFAC8419',    'Management Accounting & Finance 4');
/* 240 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('MLET7311',    'Media Law and Ethics');
/* 241 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('NCTE6222',    'New Communication Technology');
/* 242 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('NWEG5111',    'Network Engineering 1A');
/* 243 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('NWEG5122',    'Network Engineering 1B');
/* 244 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('OPSC7311',    'Open Source Coding (Introduction)');
/* 245 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('IPMA7312',    'IT Project Management');
/* 246 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('OPSC7312',    'Open Source Coding (Intermediate)');
/* 247 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('PDAN8411',    'Programming for Data Analytics 1');
/* 248 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('PDAN8412',    'Programming for Data Analytics 2');
/* 249 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('PIXD5112',    'Principles of UI and UX Design');
/* 250 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('PMAC5112',    'Economics 1B');
/* 251 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('PMIC5111',    'Economics 1A');
/* 252 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('POID6221',    'Political Ideologies');
/* 253 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('POPO6222',    'Poverty and Power: The Uneven World');
/* 254 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('PPJU7322',    'Political Philosophy and Justice');
/* 255 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('PRAL6222',    'Administrative Law');
/* 256 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('PRCC7412',    'Problem Solving and Creativity');
/* 257 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('PRET7311',    'Professional Ethics');
/* 258 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('PRLD5121',    'Programming Logic and Design');
/* 259 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('PRMA5122',    'Project Management and Administration');
/* 260 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('PRMA6211',    'Project Management');
/* 261 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('PROG5121',    'Programming IA');
/* 262 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('PROG6112',    'Programming 1B');
/* 263 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('PROG6212',    'Programming 2B');
/* 264 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('PROG6221',    'Programming 2A');
/* 265 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('PROG7311',    'Programming 3A');
/* 266 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('PROG7312',    'Programming 3B');
/* 267 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('PRSE6212',    'Principles of Security');
/* 268 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('PSAS8111',    'Psychological Assessment');
/* 269 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('PSED5111',    'Psychology for Educators 1A');
/* 270 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('PSED5112',    'Psychology for Educators 1B');
/* 271 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('PSIN6221',    'Places and Spaces: International Migration in The Global Age');
/* 272 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('PSPA8112',    'Psychopathology');
/* 273 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('PSYC5121',    'Psychology 1A');
/* 274 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('PSYC5122',    'Psychology 1B: Introduction to Psychology');
/* 275 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('PSYC6221',    'Psychology 2A: Social Psychology');
/* 276 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('PSYC6222',    'Psychology 2B: Developmental Psychology');
/* 277 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('PSYC6224',    'Psychology 2C: Personality Psychology');
/* 278 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('PSYC7313',    'Psychology 3C: Community Psychology');
/* 279 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('PSYC7321',    'Psychology 3A: Cognitive Psychology');
/* 280 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('PSYC7322',    'Psychology 3B: Abnormal Psychology');
/* 281 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('PSYC7324',    'Psychology 3D: Research Psychology');
/* 282 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('PUBL8412',    'Public International Law');
/* 283 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('PUPM6222',    'Public Policymaking');
/* 284 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('QUAT6221',    'Quantitative Techniques');
/* 285 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('REBS8419',    'Research');
/* 286 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('REHS8419',    'Research for the Human Sciences');
/* 287 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('REIT8419',    'Research for ICT');
/* 288 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('REMS8411',    'Research Methodology and Statistics');
/* 289 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('REMW8419',    'Legal Research Methods & Writing');
/* 290 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('REMY8412',    'Research Methodology');
/* 291 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('REPR7312',    'Research Practice');
/* 292 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('RETM6222',    'Retail Management');
/* 293 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('RPDA8411',    'Research Proposal');
/* 294 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('RPDA8412',    'Research Project');
/* 295 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('SAAJ7322',    'South African and African Social Justice');
/* 296 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('SADD7322',    'South African Democracy and Development');
/* 297 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('SAND6221',    'Systems Analysis and Design');
/* 298 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('SMAA8411',    'Statistical and Mathematical Analysis');
/* 299 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('SOCE7312',    'Social Education');
/* 300 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('SOCI5121',    'Sociology 1A');
/* 301 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('SOCI5122',    'Sociology 1B');
/* 302 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('SOCL8411',    'Strategic Organisational Communication and Leadership');
/* 303 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('SODE6221',    'Sociology of Development');
/* 304 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('SOEN6222',    'Software Engineering');
/* 305 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('SPCM7321',    'Supply Chain Management');
/* 306 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('SPOF6212',    'Specific Offences');
/* 307 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('SPYC7321',    'Sociological Perspectives on Youth Culture and Social Change');
/* 308 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('STRM8411',    'Strategic Management and Leadership');
/* 309 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('SUSR8412',    'Sustainability and Social Responsibility');
/* 310 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('TALA6311',    'FAL: Afrikaans A');
/* 311 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('TALA7312',    'FAL: Afrikaans B');
/* 312 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('TALN6311',    'FAL: Northern Sotho A');
/* 313 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('TALN7312',    'FAL: Northern Sotho B');
/* 314 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('TALX6311',    'FAL: IsiXhosa A');
/* 315 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('TALX7312',    'FAL: IsiXhosa B');
/* 316 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('TALZ6311',    'FAL: IsiZulu A');
/* 317 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('TALZ7312',    'FAL: IsiZulu B');
/* 318 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('TAXA6212',    'Taxation 2A');
/* 319 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('TAXA7319',    'Taxation 3');
/* 320 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('TAXA8419',    'Taxation 4');
/* 321 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('TAXN7321',    'Taxation');
/* 322 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('TEIP5119',    'Teaching Experience 1');
/* 323 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('TEIP6219',    'Teaching Experience 2');
/* 324 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('TEIP7319',    'Teaching Experience 3');
/* 325 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('TEIP7419',    'Teaching Experience 4');
/* 326 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('TEXP5119',    'Teaching Experience 1');
/* 327 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('TEXP6219',    'Teaching Experience 2');
/* 328 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('TEXP7319',    'Teaching Experience 3');
/* 329 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('TEXP7419',    'Teaching Experience 4');
/* 330 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('THIN8112',    'Therapeutic Interventions');
/* 331 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('TIMA5111',    'Mathematics 1A');
/* 332 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('TIMA6211',    'Mathematics 2A');
/* 333 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('TIMA6212',    'Mathematics 2B');
/* 334 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('TIMA6311',    'Mathematics 3A');
/* 335 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('TIMA7312',    'Mathematics 3B');
/* 336 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('TIPM6112',    'Mathematics 1B');
/* 337 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('TISS6211',    'Social Sciences 2A');
/* 338 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('TISS6212',    'Social Sciences 2B');
/* 339 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('TISS7411',    'Social Sciences 3A');
/* 340 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('TISS7412',    'Social Sciences 3B');
/* 341 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('TMEM5112',    'Emergent Mathematics 1');
/* 342 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('TMMD7312',    'Mathematics 3B');
/* 343 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('TMNO6211',    'Mathematics 2A');
/* 344 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('TMPF6212',    'Mathematics 2B');
/* 345 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('TMSS6311',    'Mathematics 3A');
/* 346 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('TNST5112',    'Nat Sciences and Technology 1');
/* 347 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('TNST6311',    'Nat Sciences and Technology 2A');
/* 348 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('TNST6312',    'Nat Sciences and Technology 2B');
/* 349 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('TNST7411',    'Nat Sciences and Technology 3A');
/* 350 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('TNST7412',    'Nat Sciences and Technology 3B');
/* 351 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('TPPS6221',    'Theoretical Perspectives in Political and Social Thought');
/* 352 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('TRPR7411',    'The Reflective Practitioner A');
/* 353 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('TRPR7412',    'The Reflective Practitioner B');
/* 354 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('UIDU8412',    'User Interface Design and Usability');
/* 355 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('UPDS7322',    'Urban Planning, Development and Sustainability');
/* 356 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('VISA6221',    'Victimology in South Africa');
/* 357 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('WEBD5012',    'Web Development (Introduction)');
/* 358 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('WE-DE500',    'Web Development (Introduction)');
/* 359 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('WOCR7221',    'Women in Crime');
/* 360 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('WPCY7321',    'Management of Community Projects: Working Preventatively with Children, Youth and Families');
/* 361 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('WPIS7321',    'War, Peace and International Security');
/* 362 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('XBAL7329',    'Work Integrated Learning 3');
/* 363 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('XBCE7319',    'Work Integrated Learning');
/* 364 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('XBCM7329',    'Work Integrated Learning');
/* 365 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('XBPP5129',    'Work Integrated Learning');
/* 366 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('XCOP5112',    'Work Integrated Learning');
/* 367 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('XHAW5112',    'Work Integrated Learning 1');
/* 368 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('YCLA6222',    'Youth in Conflict with the Law');
/* 369 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('XBCAD7319',    'Work Integrated Learning 3');
/* 370 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('XBLAW5119',    'Work Integrated Learning 1');
/* 371 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('XBLAW7329',    'Work Integrated Learning 3');
/* 372 */INSERT INTO Modules (ModuleCode, ModuleName) VALUES('XBLAW8419',    'Work Integrated Learning 4');
--------------------------------------------------------------------------------------------------
/* Modules Per Course */
/* 1   */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (99284, 1, 1, 'APCT5121', 7, 15);
/* 2   */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (99284, 1, 1, 'BMNG5121', 7, 15);
/* 3   */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (99284, 1, 1, 'PMIC5111', 7, 15);
/* 4   */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (99284, 1, 1, 'QUAT6221', 7, 15);
/* 5   */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (99284, 1, 2, 'BUSL6222', 7, 15);
/* 6   */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (99284, 1, 2, 'BMNG5122', 7, 15);
/* 7   */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (99284, 1, 2, 'PMAC5112', 7, 15);
/* 8   */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 1, 1, 'INPCf110', 5, 5);
/* 9   */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 1, 1, 'FAFR6121', 6, 12);
/* 10  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 1, 1, 'FALX6121', 6, 12);
/* 11  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 1, 1, 'FALZ6121', 6, 12);
/* 12  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 1, 1, 'FALN6111', 6, 12);
/* 13  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 1, 1, 'COLA5112', 5, 12);
/* 14  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 1, 1, 'COLX5112', 5, 12);
/* 15  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 1, 1, 'COLZ5112', 5, 12);
/* 16  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 1, 1, 'COLN5112', 5, 12);
/* 17  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 1, 1, 'ITSA5111', 5, 8);
/* 18  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 1, 1, 'PSED5111', 6, 12);
/* 19  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 1, 1, 'PSED5112', 5, 12);
/* 20  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 2, 3, 'CDLT6211', 6, 12);
/* 21  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 2, 3, 'ICTC6212', 6, 12);
/* 22  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 2, 3, 'INED7211', 7, 12);
/* 23  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 2, 3, 'INED7212', 7, 12);
/* 24  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 3, 5, 'MCED7311', 7, 12);
/* 25  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 3, 5, 'PRET7311', 7, 12);
/* 26  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 3, 5, 'SOCE7312', 7, 12);
/* 27  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 4, 7, 'FOED7411', 7, 12);
/* 28  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 4, 7, 'INER7411', 7, 10);
/* 29  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 4, 7, 'ERPR7412', 7, 10);
/* 30  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 4, 7, 'EDMA7412', 7, 12);
/* 31  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 4, 7, 'TRPR7411', 7, 10);
/* 32  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 4, 7, 'TRPR7412', 7, 10);
/* 33  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 1, 1, 'INPCf110', 5, 5);
/* 34  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 1, 1, 'FAFR6121', 6, 12);
/* 35  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 1, 1, 'FALX6121', 6, 12);
/* 36  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 1, 1, 'FALZ6121', 6, 12);
/* 37  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 1, 1, 'FALN6111', 6, 12);
/* 38  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 1, 1, 'COLA5112', 5, 12);
/* 39  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 1, 1, 'COLX5112', 5, 12);
/* 40  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 1, 1, 'COLZ5112', 5, 12);
/* 41  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 1, 1, 'COLN5112', 5, 12);
/* 42  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 1, 1, 'ITSA5111', 5, 8);
/* 43  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 1, 1, 'PSED5111', 5, 12);
/* 44  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 1, 2, 'PSED5112', 5, 12);
/* 45  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 2, 3, 'CDLT6211', 6, 12);
/* 46  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 2, 3, 'ICTC6212', 6, 12);
/* 47  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 2, 3, 'INED7211', 7, 12);
/* 48  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 2, 3, 'INED7212', 7, 12);
/* 49  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 3, 5, 'MCED7311', 7, 12);
/* 50  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 3, 5, 'PRET7311', 7, 12);
/* 51  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 3, 5, 'SOCE7312', 7, 12);
/* 52  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 4, 7, 'FOED7411', 7, 12);
/* 53  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 4, 7, 'INER7411', 7, 10);
/* 54  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 4, 7, 'ERPR7412', 7, 10);
/* 55  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 4, 7, 'EDMA7412', 7, 12);
/* 56  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 4, 7, 'TRPR7411', 7, 10);
/* 57  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 4, 7, 'TRPR7412', 7, 10);
/* 58  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (104532, 1, 1, 'DIAL5111', 5, 15);
/* 59  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97600, 1, 2, 'ITPP5112', 6, 15);
/* 60  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97600, 1, 2, 'PRSE6212', 6, 15);
/* 61  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97600, 2, 3, 'CLDV6211', 6, 15);
/* 62  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97600, 2, 3, 'DBAS6211', 6, 15);
/* 63  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97600, 2, 3, 'PROG6221', 6, 15);
/* 64  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97600, 2, 3, 'SAND6221', 6, 15);
/* 65  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97600, 2, 4, 'CLDV6212', 6, 15);
/* 66  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97600, 2, 4, 'PROG6212', 6, 15);
/* 67  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97600, 2, 4, 'SOEN6222', 6, 15);
/* 68  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97600, 2, 4, 'ADDB7311', 7, 15);
/* 69  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97600, 3, 5, 'INRS7321', 7, 15);
/* 70  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97600, 3, 6, 'APDS7311', 7, 15);
/* 71  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97600, 3, 6, 'PROG7312', 7, 15);
/* 72  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97600, 3, 6, 'XBCAD7319', 7, 15);
/* 73  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (117914, 1, 1, 'IMAD5112', 5, 14);
/* 74  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (117914, 1, 1, 'IQTT5111', 5, 14);
/* 75  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (117914, 1, 1, 'IPRG5111', 5, 15);
/* 76  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (117914, 1, 2, 'MAST5112', 5, 15);
/* 77  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (117914, 1, 2, 'PIXD5112', 5, 15);
/* 78  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (117914, 1, 2, 'XHAW5112', 5, 15);
/* 79  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (105123, 1, 1, 'LOPF5131', 5, 15);
/* 80  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (105123, 1, 1, 'INFL5111', 5, 15);
/* 81  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (105123, 1, 1, 'ENFL6111', 6, 15);
/* 82  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (105123, 1, 2, 'LEIN5112', 5, 15);
/* 83  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (105123, 1, 2, 'CONL5112', 5, 15);
/* 84  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (105123, 2, 3, 'FALA6211', 6, 15);
/* 85  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (105123, 2, 3, 'GPCL6211', 6, 15);
/* 86  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (105123, 2, 4, 'SPOF6212', 6, 15);
/* 87  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (105123, 2, 4, 'PRAL6222', 6, 15);
/* 88  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (105123, 2, 0, 'XBLAW5119', 5, 15);
/* 89  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (105123, 3, 5, 'INRS7321', 7, 15);
/* 90  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (105123, 3, 5, 'REPR7312', 7, 15);
/* 91  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (93729, 1, 1, 'BMNG5121', 5, 15);
/* 92  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (93729, 1, 1, 'INFL5111', 5, 15);
/* 93  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (93729, 1, 1, 'ENFL6111', 6, 15);
/* 94  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (93729, 1, 1, 'ACBP5121', 5, 15);
/* 95  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (93729, 1, 2, 'BMNG5122', 5, 15);
/* 96  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (93729, 1, 2, 'CONL5112', 5, 15);
/* 97  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (93729, 1, 2, 'ACBP5122', 5, 15);
/* 98  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (93729, 2, 3, 'LACO6221', 6, 15);
/* 99  */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (93729, 2, 3, 'BMNG6221', 6, 15);
/* 100 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (93729, 2, 4, 'LEIN5112', 5, 15);
/* 101 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (93729, 2, 0, 'XBLAW5119', 5, 15);
/* 102 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (93729, 3, 5, 'INRS7321', 7, 15);
/* 103 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (93729, 3, 5, 'BMNG7321', 7, 15);
/* 104 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (93729, 3, 6, 'REPR7312', 7, 15);
/* 105 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (93729, 3, 6, 'BUEL6212', 6, 15);
/* 106 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (93729, 3, 6, 'BMNG7322', 7, 15);
/* 107 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (93729, 3, 6, 'LLAW7321', 7, 15);
/* 108 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (93729, 3, 6, 'INPA6212', 6, 15);
/* 109 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (101647, 1, 1, 'LOPF5131', 5, 15);
/* 110 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (101647, 1, 1, 'INFL5111', 5, 15);
/* 111 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (101647, 1, 1, 'ENFL6111', 6, 15);
/* 112 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (101647, 1, 2, 'CONL5112', 5, 15);
/* 113 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (101647, 1, 2, 'LEIN5112', 5, 15);
/* 114 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (101647, 1, 0, 'XBLAW5119', 5, 15);
/* 115 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (101647, 2, 3, 'GPCL6211', 6, 15);
/* 116 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (101647, 2, 3, 'LACO6221', 6, 15);
/* 117 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (101647, 2, 3, 'FALA6211', 6, 15);
/* 118 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (101647, 2, 4, 'PRAL6222', 6, 15);
/* 119 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (101647, 2, 4, 'SPOF6212', 6, 15);
/* 120 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (101647, 2, 4, 'BUEL6212', 6, 15);
/* 121 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (101647, 2, 4, 'INPA6212', 6, 15);
/* 122 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (101647, 3, 5, 'LLAW7321', 7, 15);
/* 123 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94696, 1, 1, 'IMAD5112', 5, 14);
/* 124 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94696, 1, 1, 'IQTT5111', 5, 14);
/* 125 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94696, 1, 1, 'IPRG5111', 5, 15);
/* 126 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94696, 1, 2, 'MAST5112', 5, 15);
/* 127 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94696, 1, 2, 'PIXD5112', 5, 15);
/* 128 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94696, 1, 2, 'XHAW5112', 5, 15);
/* 129 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94696, 2, 3, 'CLDV6211', 6, 15);
/* 130 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94696, 2, 3, 'DBAS6211', 6, 15);
/* 131 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94696, 2, 3, 'PROG6221', 6, 15);
/* 132 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94696, 2, 3, 'SAND6221', 6, 15);
/* 133 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94696, 2, 4, 'CLDV6212', 6, 15);
/* 134 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94696, 2, 4, 'SOEN6222', 6, 15);
/* 135 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94696, 2, 4, 'PROG6212', 6, 15);
/* 136 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94696, 2, 4, 'ADDB7311', 7, 15);
/* 137 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94696, 3, 5, 'ITPP5112', 6, 15);
/* 138 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94696, 3, 5, 'PRSE6212', 6, 15);
/* 139 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94696, 3, 6, 'APDS7311', 7, 15);
/* 140 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94696, 3, 6, 'OPSC7312', 7, 15);
/* 141 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94696, 3, 6, 'PROG7312', 7, 15);
/* 142 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94696, 3, 6, 'XBCAD7319', 7, 15);
/* 143 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (84706, 1, 1, 'ACBP5121', 5, 15);
/* 144 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (84706, 1, 1, 'APCT5121', 5, 15);
/* 145 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (84706, 1, 1, 'BMNG5121', 5, 15);
/* 146 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (84706, 1, 1, 'PMIC5111', 5, 15);
/* 147 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (84706, 1, 2, 'ACBP5122', 5, 15);
/* 148 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (84706, 1, 2, 'BMNG5122', 5, 15);
/* 149 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (84706, 1, 2, 'PMAC5112', 5, 15);
/* 150 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (84706, 2, 3, 'BMNG6221', 6, 15);
/* 151 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (84706, 2, 3, 'QUAT6221', 6, 15);
/* 152 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (84706, 2, 4, 'BUSL6222', 6, 15);
/* 153 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (84706, 2, 4, 'BMNG6222', 6, 15);
/* 154 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (84706, 2, 4, 'PRMA6211', 6, 15);
/* 155 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (84706, 3, 5, 'BMNG7321', 7, 15);
/* 156 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (84706, 3, 5, 'INRS7321', 7, 15);
/* 157 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (84706, 3, 6, 'BMNG7322', 7, 15);
/* 158 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (84706, 3, 6, 'REPR7312', 7, 15);
/* 159 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (84706, 3, 6, 'ENRM7322', 7, 15);
/* 160 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (111287, 1, 1, 'ACBP5121', 5, 15);
/* 161 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (111287, 1, 1, 'PMIC5111', 5, 15);
/* 162 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (111287, 1, 2, 'ACBP5122', 5, 15);
/* 163 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (111287, 1, 2, 'PMAC5112', 5, 15);
/* 164 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (111287, 2, 3, 'QUAT6221', 6, 15);
/* 165 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (111287, 2, 4, 'BMNG6222', 6, 15);
/* 166 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (111287, 2, 4, 'BUSL6222', 6, 15);
/* 167 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (111287, 2, 4, 'PRMA6211', 6, 15);
/* 168 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (111287, 3, 5, 'BMNG7321', 7, 15);
/* 169 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (111287, 3, 5, 'INRS7321', 7, 15);
/* 170 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (111287, 3, 6, 'BMNG7322', 7, 15);
/* 171 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (111287, 3, 6, 'REPR7312', 7, 15);
/* 172 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (111287, 3, 6, 'ENRM7322', 7, 15);
/* 173 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (71637, 1, 1, 'IQTT5111', 5, 15);
/* 174 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (71637, 1, 1, 'DIAL5111', 5, 15);
/* 175 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94119, 1, 1, 'COSC5121', 5, 15);
/* 176 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94119, 1, 1, 'PSYC5121', 5, 15);
/* 177 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94119, 1, 1, 'SOCI5121', 5, 15);
/* 178 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94119, 1, 2, 'COSC5132', 5, 15);
/* 179 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94119, 1, 2, 'PSYC5122', 5, 15);
/* 180 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94119, 1, 2, 'SOCI5122', 5, 15);
/* 181 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94119, 2, 3, 'PSYC6221', 6, 15);
/* 182 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94119, 2, 4, 'PSYC6222', 6, 15);
/* 183 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94119, 2, 4, 'PSYC6224', 6, 15);
/* 184 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94119, 3, 5, 'INRS7321', 7, 15);
/* 185 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94119, 3, 5, 'PSYC7321', 7, 15);
/* 186 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94119, 3, 5, 'PSYC7313', 7, 15);
/* 187 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94119, 3, 6, 'REPR7312', 7, 15);
/* 188 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94119, 3, 6, 'PSYC7322', 7, 15);
/* 189 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94119, 3, 6, 'PSYC7324', 7, 15);
/* 190 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 1, 1, 'COSC5121', 5, 15);
/* 191 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 1, 1, 'SOCI5121', 5, 15);
/* 192 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 1, 1, 'PSYC5121', 5, 15);
/* 193 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 1, 2, 'SOCI5122', 5, 15);
/* 194 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 1, 2, 'COSC5132', 5, 15);
/* 195 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 1, 2, 'PSYC5122', 5, 15);
/* 196 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 2, 3, 'PSYC6221', 6, 15);
/* 197 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 2, 4, 'PSYC6222', 6, 15);
/* 198 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 2, 4, 'PSYC6224', 6, 15);
/* 199 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 3, 5, 'INRS7321', 7, 15);
/* 200 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 3, 5, 'PSYC7321', 7, 15);
/* 201 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 3, 5, 'PSYC7313', 7, 15);
/* 202 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 3, 6, 'REPR7312', 7, 15);
/* 203 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 3, 6, 'PSYC7322', 7, 15);
/* 204 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 3, 6, 'PSYC7324', 7, 15);
/* 205 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (112899, 1, 1, 'APCT5121', 5, 15);
/* 206 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (112899, 1, 1, 'DIAL5111', 5, 15);
/* 207 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (112899, 1, 2, 'COSC5121', 5, 15);
/* 208 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 1, 0, 'TEXP5119', 5, 8);
/* 209 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 2, 3, 'TMNO6211', 6, 12);
/* 210 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 2, 3, 'TMPF6212', 6, 12);
/* 211 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 2, 0, 'TEXP6219', 6, 12);
/* 212 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 3, 5, 'TMMD7312', 7, 12);
/* 213 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 3, 5, 'TMSS6311', 6, 12);
/* 214 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 3, 0, 'TEXP7319', 7, 12);
/* 215 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 4, 0, 'TEXP7419', 7, 12);
/* 216 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 1, 0, 'TEIP5119', 5, 6);
/* 217 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 2, 4, 'TIMA6211', 6, 8);
/* 218 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 2, 4, 'TIMA6212', 6, 8);
/* 219 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 2, 0, 'TEIP6219', 6, 8);
/* 220 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 3, 6, 'TIMA6311', 6, 8);
/* 221 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 3, 6, 'TIMA7312', 7, 15);
/* 222 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 3, 0, 'TEIP7319', 7, 8);
/* 223 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 4, 0, 'TEIP7419', 7, 8);
/* 224 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (104532, 1, 1, 'COXH5112', 5, 15);
/* 225 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (104532, 1, 1, 'COZU5112', 5, 15);
/* 226 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (104532, 1, 1, 'CONS5112', 5, 15);
/* 227 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97600, 3, 5, 'IPMA6212', 7, 15);
/* 228 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (117914, 1, 1, 'DIAL5112', 5, 14);
/* 229 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (117914, 1, 2, 'WEBD5012', 5, 15);
/* 230 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (105123, 3, 5, 'MLET7311', 7, 15);
/* 231 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (105123, 3, 0, 'XBAL7329', 7, 15);
/* 232 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (101647, 3, 0, 'XBLAW7329', 7, 15);
/* 233 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94696, 1, 2, 'WE-DE500', 5, 15);
/* 234 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (84706, 3, 5, 'XBCM7329', 7, 15);
/* 235 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (111287, 1, 1, 'APTC5121', 5, 15);
/* 236 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (111287, 3, 0, 'XBCE7319', 7, 15);
/* 237 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (71637, 1, 2, 'XBPP5129', 5, 15);
/* 238 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94119, 2, 3, 'MELE6221', 6, 15);
/* 239 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (112899, 1, 2, 'XCOP5112', 5, 15);
/* 240 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97600, 1, 2, 'NWEG5122', 6, 15);
/* 241 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97600, 1, 2, 'PROG6112', 6, 15);
/* 242 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97600, 3, 6, 'OPSC7312', 7, 15);
/* 243 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (105123, 1, 1, 'ENGL5121', 5, 15);
/* 244 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (105123, 1, 2, 'PSYC5121', 5, 15);
/* 245 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (105123, 1, 2, 'ENGL6122', 6, 15);
/* 246 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (105123, 1, 2, 'COSC5121', 5, 15);
/* 247 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (105123, 2, 4, 'COSC6231', 6, 15);
/* 248 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (105123, 2, 4, 'ENGL6221', 6, 15);
/* 249 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (105123, 3, 6, 'COSC7321', 7, 15);
/* 250 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (105123, 3, 6, 'ENGL7321', 7, 15);
/* 251 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (105123, 3, 6, 'COSC7322', 7, 15);
/* 252 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (105123, 3, 6, 'ENGL7322', 7, 15);
/* 253 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (93729, 1, 2, 'PMIC5111', 5, 15);
/* 254 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (93729, 2, 3, 'PMAC5112', 5, 15);
/* 255 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94696, 3, 5, 'NWEG5122', 6, 15);
/* 256 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94696, 3, 5, 'PROG6112', 6, 15);
/* 257 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94119, 1, 1, 'ENGL5121', 5, 15);
/* 258 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94119, 1, 1, 'ENGL6122', 6, 15);
/* 259 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94119, 2, 1, 'ENGL6221', 6, 15);
/* 260 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94119, 2, 1, 'COSC6231', 6, 15);
/* 261 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94119, 3, 1, 'ENGL7321', 7, 15);
/* 262 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94119, 3, 1, 'COSC7321', 7, 15);
/* 263 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94119, 3, 1, 'ENGL7322', 7, 15);
/* 264 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94119, 3, 1, 'COSC7322', 7, 15);
/* 265 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 1, 1, 'DIAL5111', 5, 15);
/* 266 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (99284, 1, 1, 'FIAC5111', 7, 18);
/* 267 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (99284, 1, 2, 'FIAC5112', 7, 18);
/* 268 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (99284, 2, 3, 'AUDI6211', 7, 12);
/* 269 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (99284, 2, 3, 'IINS5211', 7, 15);
/* 270 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (99284, 2, 3, 'LAES5111', 7, 12);
/* 271 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (99284, 2, 3, 'FIAC6211', 7, 18);
/* 272 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (99284, 2, 3, 'MFAC6211', 7, 18);
/* 273 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (99284, 2, 4, 'BDMO5212', 7, 15);
/* 274 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (99284, 2, 4, 'BUET6212', 7, 12);
/* 275 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (99284, 2, 4, 'TAXA6212', 7, 12);
/* 276 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (99284, 2, 4, 'FIAC6212', 7, 18);
/* 277 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (99284, 2, 4, 'MFAC6212', 7, 18);
/* 278 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (99284, 3, 0, 'AUDI7319', 7, 30);
/* 279 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (99284, 3, 0, 'FIAC7319', 7, 30);
/* 280 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (99284, 3, 0, 'MFAC7319', 7, 30);
/* 281 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (99284, 3, 0, 'TAXA7319', 7, 30);
/* 282 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (88609, 1, 1, 'FIAC8419', 8, 30);
/* 283 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (88609, 1, 1, 'TAXA8419', 8, 30);
/* 284 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (88609, 1, 1, 'MFAC8419', 8, 30);
/* 285 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (88609, 1, 1, 'AUDI8419', 8, 30);
/* 286 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 1, 1, 'ECTL6111', 6, 12);
/* 287 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 1, 1, 'LSCA5111', 5, 6);
/* 288 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 1, 1, 'EFEL5112', 5, 6);
/* 289 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 1, 1, 'LSMO5111', 5, 6);
/* 290 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 1, 1, 'TMEM5112', 5, 6);
/* 291 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 1, 1, 'EFCL5112', 5, 6);
/* 292 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 2, 3, 'EFRP6211', 6, 12);
/* 293 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 2, 3, 'HAND6212', 6, 6);
/* 294 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 2, 3, 'LSBK6211', 6, 10);
/* 295 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 2, 3, 'LSBK6212', 6, 10);
/* 296 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 3, 5, 'AFPC7312', 7, 12);
/* 297 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 3, 5, 'EFWI7311', 7, 12);
/* 298 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 3, 5, 'TALA7312', 7, 12);
/* 299 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 3, 5, 'TALX7312', 7, 12);
/* 300 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 3, 5, 'TALZ7312', 7, 12);
/* 301 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 3, 5, 'TALN7312', 7, 12);
/* 302 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 3, 5, 'TALA6311', 6, 12);
/* 303 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 3, 5, 'TALX6311', 6, 12);
/* 304 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 3, 5, 'TALZ6311', 6, 12);
/* 305 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 3, 5, 'TALN6311', 6, 12);
/* 306 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 4, 8, 'LSPD6411', 6, 12);
/* 307 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 4, 8, 'LSPM6411', 6, 12);
/* 308 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 4, 8, 'LSPS6412', 6, 10);
/* 309 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 4, 8, 'LSPE6411', 6, 10);
/* 310 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (96408, 4, 8, 'PRCC7412', 7, 12);
/* 311 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 1, 1, 'IPTL6111', 6, 7);
/* 312 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 1, 2, 'ENED6122', 6, 12);
/* 313 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 1, 2, 'IEMS5111', 5, 5);
/* 314 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 1, 2, 'TIPM6112', 6, 8);
/* 315 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 1, 2, 'TIMA5111', 5, 8);
/* 316 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 1, 2, 'TNST5112', 5, 8);
/* 317 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 2, 4, 'IFAA6211', 6, 12);
/* 318 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 2, 4, 'IFAX6211', 6, 12);
/* 319 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 2, 4, 'IFAZ6211', 6, 12);
/* 320 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 2, 4, 'IFAN6211', 6, 12);
/* 321 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 2, 4, 'EFFA6212', 6, 12);
/* 322 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 2, 4, 'TISS6211', 6, 15);
/* 323 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 2, 4, 'TISS6212', 6, 15);
/* 324 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 3, 5, 'ASME7312', 7, 10);
/* 325 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 3, 6, 'IFFA6311', 6, 12);
/* 326 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 3, 6, 'IFAX6311', 6, 12);
/* 327 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 3, 6, 'IFAZ6311', 6, 12);
/* 328 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 3, 6, 'IFAN6311', 6, 12);
/* 329 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 3, 6, 'IFFA6312', 6, 12);
/* 330 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 3, 6, 'TNST6311', 6, 15);
/* 331 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 3, 6, 'TNST6312', 6, 15);
/* 332 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 4, 8, 'IFFA7411', 7, 15);
/* 333 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 4, 8, 'IFAA7412', 7, 15);
/* 334 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 4, 8, 'IFAX7412', 7, 15);
/* 335 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 4, 8, 'IFAZ7412', 7, 15);
/* 336 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 4, 8, 'IFAN7412', 7, 15);
/* 337 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 4, 8, 'TNST7411', 7, 8);
/* 338 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 4, 8, 'TNST7412', 7, 8);
/* 339 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 4, 8, 'TISS7411', 7, 8);
/* 340 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97235, 4, 8, 'TISS7412', 7, 8);
/* 341 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (104532, 1, 1, 'HSNU5111', 5, 15);
/* 342 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (104532, 1, 1, 'ECLD5111', 5, 15);
/* 343 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (104532, 1, 1, 'MDPE5111', 5, 15);
/* 344 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (104532, 1, 2, 'FMLP5112', 5, 15);
/* 345 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (104532, 1, 2, 'FMNP5112', 5, 15);
/* 346 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (104532, 1, 2, 'FMLS5112', 5, 15);
/* 347 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (104532, 1, 2, 'ECDX5112', 5, 17);
/* 348 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (88606, 1, 1, 'ADTO8411', 8, 20);
/* 349 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (88606, 1, 1, 'ICTD8411', 8, 20);
/* 350 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (88606, 1, 2, 'EMTE8412', 8, 20);
/* 351 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (88606, 1, 2, 'UIDU8412', 8, 20);
/* 352 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (88606, 1, 2, 'AINT8412', 8, 20);
/* 353 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (88606, 1, 0, 'REIT8419', 8, 40);
/* 354 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97600, 1, 1, 'MAPC5112', 5, 15);
/* 355 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97600, 1, 1, 'NWEG5111', 5, 15);
/* 356 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97600, 1, 1, 'PROG5121', 5, 15);
/* 357 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97600, 1, 1, 'PRLD5121', 5, 15);
/* 358 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97600, 3, 5, 'OPSC7311', 7, 15);
/* 359 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97600, 3, 5, 'PROG7311', 7, 15);
/* 360 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (117788, 1, 1, 'DANA8411', 8, 15);
/* 361 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (117788, 1, 1, 'PDAN8411', 8, 15);
/* 362 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (117788, 1, 1, 'RPDA8411', 8, 15);
/* 363 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (117788, 1, 1, 'SMAA8411', 8, 15);
/* 364 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (117788, 1, 2, 'DANA8412', 8, 15);
/* 365 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (117788, 1, 2, 'PDAN8412', 8, 15);
/* 366 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (117788, 1, 2, 'RPDA8412', 8, 15);
/* 367 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (117788, 1, 2, 'DASC8412', 8, 15);
/* 368 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (105123, 1, 2, 'CRIM5111', 5, 15);
/* 369 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (105123, 2, 3, 'AORP6221', 6, 15);
/* 370 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (105123, 2, 4, 'CRIM6211', 6, 15);
/* 371 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (105123, 3, 5, 'ALRE7311', 7, 15);
/* 372 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (105123, 3, 6, 'GEBV7312', 7, 15);
/* 373 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (105123, 3, 6, 'CRIM7311', 7, 15);
/* 374 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (105123, 3, 6, 'CRIM7312', 7, 15);
/* 375 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (93729, 2, 3, 'LEAP6211', 6, 15);
/* 376 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (93729, 2, 4, 'BSTA6212', 6, 15);
/* 377 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (93729, 3, 5, 'ECLA7321', 7, 15);
/* 378 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (93729, 3, 5, 'CRMA7312', 7, 15);
/* 379 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (93729, 3, 6, 'GOET7312', 7, 15);
/* 380 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (101647, 2, 3, 'INDI6111', 6, 15);
/* 381 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (101647, 2, 4, 'LOSA6212', 6, 15);
/* 382 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (101647, 3, 5, 'CRUN7112', 7, 15);
/* 383 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (101647, 3, 5, 'LADE7111', 7, 15);
/* 384 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (101647, 3, 5, 'LAPR7311', 7, 15);
/* 385 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (101647, 3, 6, 'AATT6312', 6, 15);
/* 386 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (101647, 3, 6, 'LAIN7312', 7, 15);
/* 387 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (101647, 3, 6, 'INPL7322', 7, 15);
/* 388 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (101647, 4, 7, 'CIPR8411', 8, 15);
/* 389 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (101647, 4, 7, 'LAEV8411', 8, 15);
/* 390 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (101647, 4, 8, 'PUBL8412', 8, 15);
/* 391 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (101647, 4, 0, 'JUJU8222', 8, 15);
/* 392 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (101647, 4, 0, 'REMW8419', 8, 20);
/* 393 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (101647, 4, 0, 'XBLAW8419', 8, 15);
/* 394 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94696, 1, 1, 'DIAL5111', 5, 15);
/* 395 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (84706, 1, 2, 'MAKT5112', 5, 15);
/* 396 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (84706, 2, 3, 'ACBP6221', 6, 15);
/* 397 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (84706, 2, 3, 'FINM6221', 6, 15);
/* 398 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (84706, 2, 3, 'INRL6221', 6, 15);
/* 399 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (84706, 2, 3, 'MAKT6211', 6, 15);
/* 400 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (84706, 2, 4, 'ACBP6222', 6, 15);
/* 401 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (84706, 2, 4, 'FINM6222', 6, 15);
/* 402 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (84706, 2, 4, 'DGMT6212', 6, 15);
/* 403 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (84706, 2, 4, 'MAKT6212', 6, 15);
/* 404 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (84706, 2, 4, 'RETM6222', 6, 15);
/* 405 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (84706, 3, 5, 'ACBP7321', 7, 15);
/* 406 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (84706, 3, 5, 'FINM7321', 7, 15);
/* 407 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (84706, 3, 5, 'MAKT7311', 7, 15);
/* 408 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (84706, 3, 5, 'SPCM7321', 7, 15);
/* 409 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (84706, 3, 6, 'ACBP7322', 7, 15);
/* 410 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (84706, 3, 6, 'FINM7322', 7, 15);
/* 411 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (84706, 3, 6, 'TAXN7321', 7, 15);
/* 412 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (84706, 3, 6, 'INKM7322', 7, 15);
/* 413 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (84706, 3, 6, 'MAKT7312', 7, 15);
/* 414 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97601, 1, 0, 'REBS8419', 8, 40);
/* 415 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97601, 1, 1, 'MALE8411', 8, 20);
/* 416 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97601, 1, 1, 'IBMA8411', 8, 20);
/* 417 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97601, 1, 2, 'SUSR8412', 8, 20);
/* 418 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (97601, 1, 2, 'COMI8412', 8, 20);
/* 419 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (111287, 1, 1, 'BMAN5121', 5, 15);
/* 420 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (111287, 1, 2, 'ENTR5112', 5, 15);
/* 421 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (111287, 1, 2, 'DMET5112', 5, 15);
/* 422 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (111287, 2, 3, 'ENTP6211', 6, 15);
/* 423 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (111287, 2, 3, 'BMDE6211', 6, 15);
/* 424 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (111287, 2, 3, 'FINE6211', 6, 15);
/* 425 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (111287, 2, 4, 'ENTP6212', 6, 15);
/* 426 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (111287, 3, 5, 'ENTP7311', 7, 15);
/* 427 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (111287, 3, 6, 'ENTP7312', 7, 15);
/* 428 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (71637, 1, 1, 'INAC5111', 5, 15);
/* 429 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (71637, 1, 1, 'BMAD5121', 5, 15);
/* 430 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (71637, 1, 2, 'PRMA5122', 5, 15);
/* 431 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (71637, 1, 2, 'ACSO5112', 5, 15);
/* 432 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (71637, 1, 2, 'BMAD5112', 5, 15);
/* 433 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (109005, 1, 1, 'CONT8411', 8, 20);
/* 434 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (109005, 1, 1, 'AAFM8411', 8, 20);
/* 435 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (109005, 1, 1, 'STRM8411', 8, 20);
/* 436 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (109005, 1, 2, 'GBMP8411', 8, 20);
/* 437 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (109005, 1, 2, 'REMY8412', 8, 20);
/* 438 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (109005, 1, 2, 'ELEC8411', 8, 20);
/* 439 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94119, 2, 3, 'JRNS6221', 6, 15);
/* 440 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94119, 2, 4, 'ENGL6222', 6, 15);
/* 441 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94119, 2, 4, 'COSC6222', 6, 15);
/* 442 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94119, 2, 4, 'MEST6222', 6, 15);
/* 443 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94119, 2, 4, 'NCTE6222', 6, 15);
/* 444 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94119, 3, 5, 'JRNS7321', 7, 15);
/* 445 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (94119, 3, 6, 'MEST7322', 7, 15);
/* 446 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (98032, 1, 1, 'COMT8411', 8, 20);
/* 447 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (98032, 1, 1, 'SOCL8411', 8, 20);
/* 448 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (98032, 1, 2, 'CRST8412', 8, 20);
/* 449 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (98032, 1, 2, 'CCPR8412', 8, 20);
/* 450 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (98032, 1, 0, 'REHS8419', 8, 40);
/* 451 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (105032, 1, 1, 'PSAS8111', 8, 20);
/* 452 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (105032, 1, 1, 'DPSY8111', 8, 20);
/* 453 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (105032, 1, 2, 'THIN8112', 8, 20);
/* 454 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (105032, 1, 2, 'PSPA8112', 8, 20);
/* 455 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (105032, 1, 2, 'CPSY8111', 8, 20);
/* 456 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (105032, 1, 0, 'REMS8411', 8, 20);
/* 457 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (105032, 1, 0, 'INRP8419', 8, 30);
/* 458 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 1, 1, 'INTA5121', 5, 15);
/* 459 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 1, 1, 'IPGE5121', 5, 15);
/* 460 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 1, 2, 'INTB5122', 5, 15);
/* 461 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 1, 2, 'FUPS5122', 5, 15);
/* 462 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 1, 2, 'FUIR5122', 5, 15);
/* 463 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 1, 2, 'IHGE5122', 5, 15);
/* 464 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 1, 2, 'IDES5122', 5, 15);
/* 465 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 1, 2, 'CICO5122', 5, 15);
/* 466 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 1, 2, 'ICJS5122', 5, 15);
/* 467 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 2, 3, 'PSIN6221', 6, 15);
/* 468 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 2, 3, 'DEDW6221', 6, 15);
/* 469 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 2, 3, 'POID6221', 6, 15);
/* 470 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 2, 3, 'TPPS6221', 6, 15);
/* 471 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 2, 3, 'EPRM6221', 6, 15);
/* 472 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 2, 3, 'SODE6221', 6, 15);
/* 473 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 2, 3, 'WOCR7221', 7, 15);
/* 474 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 2, 3, 'VISA6221', 6, 15);
/* 475 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 2, 3, 'GECS6221', 6, 15);
/* 476 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 2, 4, 'POPO6222', 6, 15);
/* 477 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 2, 4, 'EMOW6222', 6, 15);
/* 478 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 2, 4, 'COPO6222', 6, 15);
/* 479 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 2, 4, 'PUPM6222', 6, 15);
/* 480 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 2, 4, 'GISF7222', 7, 15);
/* 481 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 2, 4, 'BSNC6222', 6, 15);
/* 482 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 2, 4, 'YCLA6222', 6, 15);
/* 483 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 2, 4, 'ISCI6212', 6, 15);
/* 484 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 2, 4, 'MEDS6222', 6, 15);
/* 485 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 3, 5, 'FPIN7321', 7, 15);
/* 486 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 3, 5, 'WPIS7321', 7, 15);
/* 487 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 3, 5, 'AFMW7321', 7, 15);
/* 488 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 3, 5, 'CLIM7321', 7, 15);
/* 489 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 3, 5, 'ENAS7321', 7, 15);
/* 490 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 3, 5, 'WPCY7321', 7, 15);
/* 491 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 3, 5, 'CBSA7321', 7, 15);
/* 492 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 3, 5, 'SPYC7321', 7, 15);
/* 493 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 3, 5, 'BERS7321', 7, 15);
/* 494 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 3, 6, 'SADD7322', 7, 15);
/* 495 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 3, 6, 'DCIR7322', 7, 15);
/* 496 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 3, 6, 'PPJU7322', 7, 15);
/* 497 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 3, 6, 'GISP7322', 7, 15);
/* 498 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 3, 6, 'UPDS7322', 7, 15);
/* 499 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 3, 6, 'FCVP7322', 7, 16);
/* 500 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (90905, 3, 6, 'SAAJ7322', 7, 15);
/* 501 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (112899, 1, 1, 'ENLP5111', 5, 15);
/* 502 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (112899, 1, 1, 'DTPS5111', 5, 15);
/* 503 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (112899, 1, 2, 'ENLP5112', 5, 15);
/* 504 */INSERT INTO CourseModules (CourseID, Years, Semester, ModuleCode, NQFLevel, ModuleCredits) VALUES (112899, 1, 2, 'FOMS5112', 5, 15);