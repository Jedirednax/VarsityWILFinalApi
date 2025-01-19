namespace VCTicketTrackerAPIClassLibrary.Orginisation
{
    /// <summary>
    /// <example>
    /// <code>
    /// 
    ///  "moduleCode": "string",
    ///  "moduleName": "string",
    ///  "moduleCredits": 0,
    ///  "years": 0,
    ///  "semester": 0,
    ///  "nqfLevel": 0
    ///}
    /// </code>
    /// </example>
    /// </summary>
    public class StudentModule : Module, IModule
    {
        public int ModuleCredits { get; set; }
        public int Years { get; set; }
        public int Semester { get; set; }
        public int NQFLevel { get; set; }

        public StudentModule(string? moduleCode, string? moduleName, int moduleCredits, int years, int semester, int nQFLevel) : base(moduleCode, moduleName)
        {
            ModuleCode = moduleCode;
            ModuleName = moduleName;
            ModuleCredits = moduleCredits;
            Years = years;
            Semester = semester;
            NQFLevel = nQFLevel;
        }

        public StudentModule(string? moduleCode, string? moduleName) : base(moduleCode, moduleName)
        {
        }

        public override string? ToString()
        {
            return
                $"|Name: {ModuleName}\n\t" +
                $"|Code: {ModuleCode,-10}" +
                $"|Cred: {ModuleCredits,-10}" +
                $"|Year: {Years,-10}" +
                $"|Sems: {Semester,-10}" +
                $"|NQFL: {NQFLevel,-10}";
        }
    }
}
