namespace VCTicketTrackerAPIClassLibrary.Orginisation
{
    /// <summary>
    /// 
    /// <example>
    /// <code>
    /// {
    ///  "moduleCode": "string",
    ///  "moduleName": "string",
    ///  "modGroup": 0,
    ///  "staffID": 0
    ///}
    /// </code>
    /// </example>
    /// </summary>
    public class StaffModule : Module
    {
        public string ModuleCode { get; set; }
        public int ModGroup { get; set; }
        public long StaffID { get; set; }

        public StaffModule(string? moduleCode, string? moduleName, int modGroup, long staffID) : base(moduleCode, moduleName)
        {
            ModuleCode = moduleCode;
            ModuleName = moduleName;
            ModGroup=modGroup;
            StaffID=staffID;
        }


        public override string? ToString()
        {
            return
                $"|Name: {ModuleName}\n\t" +
                $"|Code: {ModuleCode,-10}";
        }
    }
}
