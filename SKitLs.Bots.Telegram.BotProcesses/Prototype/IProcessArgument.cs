namespace SKitLs.Bots.Telegram.BotProcesses.Prototype
{
    /// <summary>
    /// The interface representing the arguments required to execute a bot process.
    /// </summary>
    public interface IProcessArgument
    {
        /// <summary>
        /// Represents status of process completion.
        /// </summary>
        public ProcessCompleteStatus CompleteStatus { get; set; }

        // This interface serves as a marker interface, indicating that classes implementing it will provide specific arguments 
        // needed to execute various bot processes. The primary purpose of this interface is to facilitate a unified approach 
        // for passing arguments to different bot processes within the application. By having a common interface, it becomes 
        // easier for the Process Manager and other components to interact with and execute different bot processes in a 
        // consistent manner.

        // For example, a specific bot process might require arguments related to user input, while another process might 
        // need configuration settings or data from external sources. By adhering to the IProcessArguments interface, these 
        // processes can be managed and executed by the Process Manager in a flexible and maintainable manner.

        // Implementers of this interface should ensure that the properties and members defined within their classes 
        // appropriately represent the necessary data for the respective bot process. Additionally, it is essential to 
        // properly document the purpose and usage of each argument to promote code readability and collaboration among 
        // developers.

        // Example usage of IProcessArguments interface:
        // public class UserInputArguments : IProcessArguments
        // {
        //    public string UserInput { get; set; }
        //    public int UserAge { get; set; }
        //    // Other properties specific to the user input bot process
        // }
        // public class ConfigurationArguments : IProcessArguments
        // {
        //    public string ConfigSetting1 { get; set; }
        //    public bool IsEnabled { get; set; }
        //    // Other properties specific to the configuration bot process
        // }
    }
}