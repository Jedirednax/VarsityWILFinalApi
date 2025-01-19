namespace VCTicketTrackerAPIClassLibrary.Orginisation
{

    /// <summary>
    /// Module
    /// <example>
    /// <code>
    /// {
    ///  "moduleCode": "string",
    ///  "moduleName": "string"
    ///}
    /// </code>
    /// </example>
    /// </summary>
    public class Module : IModule
    {
        public string? ModuleCode { get; set; }
        public string? ModuleName { get; set; }

        public Module(string? moduleCode, string? moduleName)
        {
            ModuleCode = moduleCode;
            ModuleName = moduleName;
        }

        public override string? ToString()
        {
            return
                $"|Name: {ModuleName}\n\t" +
                $"|Code: {ModuleCode,-10}";
        }

    }
}
