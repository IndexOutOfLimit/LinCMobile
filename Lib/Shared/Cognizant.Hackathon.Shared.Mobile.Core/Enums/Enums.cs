using System.ComponentModel;

namespace Cognizant.Hackathon.Shared.Mobile.Core.Enums
{
    public enum AppProperty
    {   
        Username,
        Password,
        State,
        CredentialProvider,
        MainViewModel,
        DeviceId,       
    }

    public enum LinCUserType
    {
        Supplier,
        Consumer,
        Volunteer
    }

    public enum LinCLogicalType
    {
        True,
        False
    }

    public enum LinCUserRegisterType
    {
        Individual,
        Org
    }

    public enum LinCServiceType
    {
        Grocery,
        Medical,
        Cloth,
        Food,
        Vegetable,
        Others
    }

    public enum LinCTrasactionStatus
    {
        Success,
        Failure
    }

    /// <summary>
    /// DataOperation Enum for CRUD operation status
    /// </summary>
    [DefaultValue(Nop)]
    public enum DataOperation : int
    {
        /// <summary>
        /// No data operation
        /// </summary>
        [Description("No Data Operation")] Nop,
        /// <summary>
        /// Data record not allowed to be updated
        /// </summary>
        [Description("Data record not allowed to be updated")] Unchanged,
        /// <summary>
        /// Data operation error
        /// </summary>
        [Description("Operation failed due to error")] OperationError,
        /// <summary>
        /// Data created successfully
        /// </summary>
        [Description("Data created successfully")] CreatedSuccessfully,
        /// <summary>
        /// Creating data failed
        /// </summary>
        [Description("Creating data failed")] CreatingFailed,
        /// <summary>
        /// Data updated successfully
        /// </summary>
        [Description("Data updated successfully")] UpdatedSuccessfully,
        /// <summary>
        /// Updating data failed
        /// </summary>
        [Description("Updating data failed")] UpdatingFailed,
        /// <summary>
        /// Data deleted successfully
        /// </summary>
        [Description("Data deleted successfully")] DeletedSuccessfully,
        /// <summary>
        /// Deleting failed
        /// </summary>
        [Description("Deleting data failed")] DeletingFailed
    }
}