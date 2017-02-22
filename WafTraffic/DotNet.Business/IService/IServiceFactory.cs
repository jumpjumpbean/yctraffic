namespace DotNet.Business
{
    using System;

    public interface IServiceFactory
    {
        IDbHelperService CreateBusinessDbHelperService();
        IExceptionService CreateExceptionService();
        IFileService CreateFileService();
        IFolderService CreateFolderService();
        IItemDetailsService CreateItemDetailsService();
        IItemsService CreateItemsService();
        ILogOnService CreateLogOnService();
        ILogService CreateLogService();
        IMessageService CreateMessageService();
        IModuleService CreateModuleService();
        IOrganizeService CreateOrganizeService();
        IParameterService CreateParameterService();
        IPermissionItemService CreatePermissionItemService();
        IPermissionService CreatePermissionService();
        IRoleService CreateRoleService();
        ISequenceService CreateSequenceService();
        IStaffService CreateStaffService();
        ITableColumnsService CreateTableColumnsService();
        IDbHelperService CreateUserCenterDbHelperService();
        IUserService CreateUserService();
        IWorkFlowActivityAdminService CreateWorkFlowActivityAdminService();
        IWorkFlowCurrentService CreateWorkFlowCurrentService();
        IWorkFlowProcessAdminService CreateWorkFlowProcessAdminService();
        void InitService();
    }
}

